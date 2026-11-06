using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var repositoryRoot = FindRepositoryRoot(builder.Environment.ContentRootPath);
var projects = ProjectCatalog.Discover(repositoryRoot);
var jobs = new JobManager();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    if (HttpMethods.IsPost(context.Request.Method))
    {
        var origin = context.Request.Headers.Origin.ToString();
        if (!string.IsNullOrEmpty(origin) &&
            !string.Equals(origin, "http://127.0.0.1:5080", StringComparison.OrdinalIgnoreCase))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsJsonAsync(new { error = "Cross-origin actions are not allowed." });
            return;
        }
    }

    await next();
});

app.MapGet("/api/projects", () => projects);

app.MapPost("/api/projects/{id}/actions/{action}", async (string id, string action) =>
{
    var project = projects.FirstOrDefault(candidate =>
        string.Equals(candidate.Id, id, StringComparison.Ordinal));
    if (project is null)
    {
        return Results.NotFound(new { error = "Project not found." });
    }

    if (action is not ("build" or "test" or "run"))
    {
        return Results.BadRequest(new { error = "Supported actions are build, test, and run." });
    }

    if (action == "run" && !project.CanRun)
    {
        return Results.Conflict(new { error = "This project is a classic .NET Framework web application and cannot be started with dotnet run." });
    }

    if (action == "test" && !project.CanTest)
    {
        return Results.Conflict(new { error = "No test framework or test project is configured for this project." });
    }

    try
    {
        var job = await jobs.StartAsync(project, action);
        return Results.Accepted($"/api/jobs/{job.Id}", job);
    }
    catch (InvalidOperationException exception)
    {
        return Results.Conflict(new { error = exception.Message });
    }
});

app.MapGet("/api/jobs/{id:guid}", (Guid id) =>
    jobs.TryGet(id, out var job) ? Results.Ok(job) : Results.NotFound(new { error = "Job not found." }));

app.MapPost("/api/jobs/{id:guid}/stop", (Guid id) =>
{
    if (!jobs.TryGet(id, out var job))
    {
        return Results.NotFound(new { error = "Job not found." });
    }

    if (!jobs.Stop(id))
    {
        return Results.Conflict(new { error = "This job has already finished." });
    }

    return Results.Ok(job);
});

app.Run("http://127.0.0.1:5080");

static string FindRepositoryRoot(string start)
{
    var directory = new DirectoryInfo(start);
    while (directory is not null)
    {
        if (File.Exists(Path.Combine(directory.FullName, "README.md")) &&
            Directory.Exists(Path.Combine(directory.FullName, "RabbitMQ")))
        {
            return directory.FullName;
        }

        directory = directory.Parent;
    }

    throw new DirectoryNotFoundException("Could not locate the repository root (expected README.md and RabbitMQ/).");
}

internal sealed record ProjectInfo(
    string Id,
    string Name,
    string Group,
    string Framework,
    string Kind,
    string RelativePath,
    string TargetPath,
    bool CanRun,
    bool CanTest);

internal static class ProjectCatalog
{
    public static IReadOnlyList<ProjectInfo> Discover(string root)
    {
        var ignoredFolders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".git", ".vs", "bin", "obj", "packages", "node_modules", "ProjectDashboard"
        };

        var projectFiles = new List<string>();
        var pendingDirectories = new Stack<string>();
        pendingDirectories.Push(root);
        while (pendingDirectories.TryPop(out var directory))
        {
            projectFiles.AddRange(Directory.EnumerateFiles(directory, "*.csproj", SearchOption.TopDirectoryOnly));
            foreach (var child in Directory.EnumerateDirectories(directory, "*", SearchOption.TopDirectoryOnly))
            {
                var name = Path.GetFileName(child);
                if (!ignoredFolders.Contains(name) &&
                    (File.GetAttributes(child) & FileAttributes.ReparsePoint) == 0)
                {
                    pendingDirectories.Push(child);
                }
            }
        }

        return projectFiles
            .Select(path => CreateProject(root, path))
            .OrderBy(project => project.Group, StringComparer.OrdinalIgnoreCase)
            .ThenBy(project => project.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static ProjectInfo CreateProject(string root, string projectPath)
    {
        var document = XDocument.Load(projectPath);
        var projectElement = document.Root
            ?? throw new InvalidDataException($"Project file has no root element: {projectPath}");
        var framework = projectElement.Descendants()
            .FirstOrDefault(element => element.Name.LocalName is "TargetFramework" or "TargetFrameworks")
            ?.Value.Trim();
        if (string.IsNullOrWhiteSpace(framework))
        {
            framework = projectElement.Descendants()
                .FirstOrDefault(element => element.Name.LocalName == "TargetFrameworkVersion")
                ?.Value.Trim() ?? "unspecified";
        }

        var sdk = (string?)projectElement.Attribute("Sdk") ?? string.Empty;
        var outputType = projectElement.Descendants()
            .FirstOrDefault(element => element.Name.LocalName == "OutputType")
            ?.Value.Trim();
        var canRun = sdk.Contains("Microsoft.NET.Sdk.Web", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(outputType, "Exe", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(outputType, "WinExe", StringComparison.OrdinalIgnoreCase);
        var canTest = projectElement.Descendants().Any(element =>
            element.Name.LocalName == "IsTestProject" &&
            bool.TryParse(element.Value, out var isTestProject) &&
            isTestProject);
        canTest |= projectElement.Descendants()
            .Where(element => element.Name.LocalName == "PackageReference")
            .Select(element => (string?)element.Attribute("Include"))
            .Any(package => package is not null &&
                (package.Equals("Microsoft.NET.Test.Sdk", StringComparison.OrdinalIgnoreCase) ||
                 package.StartsWith("xunit", StringComparison.OrdinalIgnoreCase) ||
                 package.StartsWith("nunit", StringComparison.OrdinalIgnoreCase) ||
                 package.StartsWith("MSTest.", StringComparison.OrdinalIgnoreCase)));
        var relativePath = Path.GetRelativePath(root, projectPath);
        var group = relativePath.Split(Path.DirectorySeparatorChar)[0];
        var kind = sdk.Contains("Microsoft.NET.Sdk.Web", StringComparison.OrdinalIgnoreCase)
            ? "Web app"
            : canRun ? "Console app" : "Library / classic web";

        return new ProjectInfo(
            Id: Convert.ToBase64String(Encoding.UTF8.GetBytes(relativePath))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_'),
            Name: Path.GetFileNameWithoutExtension(projectPath),
            Group: group,
            Framework: framework,
            Kind: kind,
            RelativePath: relativePath.Replace(Path.DirectorySeparatorChar, '/'),
            TargetPath: projectPath,
            CanRun: canRun,
            CanTest: canTest);
    }
}

internal sealed record JobInfo(
    Guid Id,
    string ProjectId,
    string ProjectName,
    string Action,
    string Status,
    string Command,
    string Output,
    int? ExitCode,
    DateTimeOffset StartedAt,
    DateTimeOffset? FinishedAt);

internal sealed class JobManager
{
    private const int MaxOutputCharacters = 100_000;
    private readonly ConcurrentDictionary<Guid, Job> _jobs = new();
    private readonly object _sync = new();

    public Task<JobInfo> StartAsync(ProjectInfo project, string action)
    {
        Job job;
        lock (_sync)
        {
            if (_jobs.Values.Count(existing => existing.IsActive) >= 4)
            {
                throw new InvalidOperationException("Four project processes are already active. Stop one before starting another.");
            }

            if (_jobs.Values.Any(existing => existing.IsActive &&
                                             string.Equals(existing.Project.Id, project.Id, StringComparison.Ordinal)))
            {
                throw new InvalidOperationException("An action is already running for this project.");
            }

            job = new Job(project, action);
            _jobs[job.Info.Id] = job;
        }

        _ = job.ExecuteAsync();
        return Task.FromResult(job.Info);
    }

    public bool TryGet(Guid id, out JobInfo info)
    {
        if (_jobs.TryGetValue(id, out var job))
        {
            info = job.Info;
            return true;
        }

        info = null!;
        return false;
    }

    public bool Stop(Guid id) =>
        _jobs.TryGetValue(id, out var job) && job.Stop();

    private sealed class Job
    {
        private readonly object _sync = new();
        private readonly StringBuilder _output = new();
        private readonly Process _process;
        private string _status = "Starting";
        private int? _exitCode;
        private DateTimeOffset? _finishedAt;
        private bool _wasStopped;

        public ProjectInfo Project { get; }
        public bool IsActive
        {
            get
            {
                lock (_sync)
                {
                    return _finishedAt is null;
                }
            }
        }

        public JobInfo Info
        {
            get
            {
                lock (_sync)
                {
                    return new JobInfo(
                        _id,
                        Project.Id,
                        Project.Name,
                        Action,
                        _status,
                        Command,
                        _output.ToString(),
                        _exitCode,
                        StartedAt,
                        _finishedAt);
                }
            }
        }

        private readonly Guid _id = Guid.NewGuid();
        private string Action { get; }
        private string Command { get; }
        private DateTimeOffset StartedAt { get; } = DateTimeOffset.Now;

        public Job(ProjectInfo project, string action)
        {
            Project = project;
            Action = action;

            var startInfo = new ProcessStartInfo("dotnet")
            {
                WorkingDirectory = Path.GetDirectoryName(project.TargetPath)!,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            startInfo.Environment["DOTNET_NOLOGO"] = "1";
            startInfo.Environment["DOTNET_CLI_TELEMETRY_OPTOUT"] = "1";

            if (action == "run")
            {
                startInfo.ArgumentList.Add("run");
                startInfo.ArgumentList.Add("--project");
                startInfo.ArgumentList.Add(project.TargetPath);
                startInfo.ArgumentList.Add("--no-launch-profile");
            }
            else
            {
                var target = FindSolution(project.TargetPath) ?? project.TargetPath;
                startInfo.ArgumentList.Add(action);
                startInfo.ArgumentList.Add(target);
            }

            Command = $"dotnet {string.Join(" ", startInfo.ArgumentList.Select(QuoteArgument))}";
            _process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };
        }

        public async Task ExecuteAsync()
        {
            try
            {
                _process.OutputDataReceived += (_, eventArgs) => AppendLine(eventArgs.Data);
                _process.ErrorDataReceived += (_, eventArgs) => AppendLine(eventArgs.Data);
                if (!_process.Start())
                {
                    throw new InvalidOperationException("The dotnet process could not be started.");
                }

                lock (_sync)
                {
                    _status = "Running";
                    _output.AppendLine($"> {Command}");
                }

                _process.BeginOutputReadLine();
                _process.BeginErrorReadLine();
                await _process.WaitForExitAsync();
                _process.WaitForExit();

                lock (_sync)
                {
                    _exitCode = _process.ExitCode;
                    _status = _wasStopped ? "Stopped" : _process.ExitCode == 0 ? "Succeeded" : "Failed";
                    _finishedAt = DateTimeOffset.Now;
                }
            }
            catch (Exception exception)
            {
                lock (_sync)
                {
                    _status = "Failed";
                    _finishedAt = DateTimeOffset.Now;
                    _output.AppendLine(exception.Message);
                }
            }
        }

        public bool Stop()
        {
            lock (_sync)
            {
                if (_finishedAt is not null)
                {
                    return false;
                }
            }

            try
            {
                _process.Kill(entireProcessTree: true);
                lock (_sync)
                {
                    _wasStopped = true;
                    _status = "Stopped";
                    _finishedAt = DateTimeOffset.Now;
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        private void AppendLine(string? line)
        {
            if (line is null)
            {
                return;
            }

            lock (_sync)
            {
                _output.AppendLine(line);
                if (_output.Length > MaxOutputCharacters)
                {
                    _output.Remove(0, _output.Length - MaxOutputCharacters);
                    _output.Insert(0, "[Earlier output trimmed]\n");
                }
            }
        }

        private static string? FindSolution(string projectPath)
        {
            var directory = new DirectoryInfo(Path.GetDirectoryName(projectPath)!);
            while (directory is not null)
            {
                var solution = directory.EnumerateFiles("*.sln", SearchOption.TopDirectoryOnly)
                    .OrderBy(file => file.Name, StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();
                if (solution is not null)
                {
                    return solution.FullName;
                }

                directory = directory.Parent;
            }

            return null;
        }

        private static string QuoteArgument(string value) =>
            value.Contains(' ') ? $"\"{value}\"" : value;
    }
}
