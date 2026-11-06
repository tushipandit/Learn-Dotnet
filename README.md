# DotNetApplications

Building Dotnet Applications.

## Local project dashboard

This repository includes a local web dashboard for discovering, building, testing,
and running its .NET projects. It uses the installed .NET SDK and streams command
output in the browser; it does not upload project files or launch projects until
you select an action.

1. Install the .NET 10 SDK.
2. From the repository root, run:

   ```powershell
   dotnet run --project .\ProjectDashboard\ProjectDashboard.csproj
   ```

3. Open [http://127.0.0.1:5080](http://127.0.0.1:5080).
4. Select a project card, then choose **Build**, **Test**, or **Run**. Use **Stop
   process** to stop a running app. Build and test use the nearest solution when
   one is available; otherwise, they target that project file. **Test** is
   enabled only for projects whose files identify a test framework.

The dashboard only binds to `127.0.0.1`. It discovers `.csproj` files and permits
only the fixed `dotnet build`, `dotnet test`, and `dotnet run` actions. Projects
that need external services, secrets, a database, or an older .NET runtime may
need additional local setup. The current repository does not include detected
test projects, so its sample projects can be built and run, but there are no
automated test suites to execute yet. Classic .NET Framework web projects can be
built or tested where supported by your installed tools, but need Visual Studio /
IIS Express rather than `dotnet run`.
