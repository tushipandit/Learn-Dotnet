const state = {
  projects: [],
  selectedId: null,
  activeGroup: null,
  runningOnly: false,
  jobs: new Map(),
  latestJobId: null,
  pollTimer: null,
  toastTimer: null
};

const elements = {
  grid: document.querySelector("#project-grid"),
  groups: document.querySelector("#group-list"),
  selected: document.querySelector("#selected-project"),
  actionArea: document.querySelector("#action-area"),
  output: document.querySelector("#console-output"),
  meta: document.querySelector("#console-meta"),
  status: document.querySelector("#job-status"),
  liveDot: document.querySelector("#console-live-dot"),
  stop: document.querySelector("#stop-job"),
  empty: document.querySelector("#empty-state"),
  search: document.querySelector("#search"),
  typeFilter: document.querySelector("#type-filter"),
  filterTag: document.querySelector("#active-filter"),
  toast: document.querySelector("#toast"),
  panel: document.querySelector("#detail-panel")
};

async function loadProjects() {
  elements.grid.innerHTML = '<div class="loading-state"><span class="loader"></span> Discovering projects...</div>';
  try {
    const response = await fetch("/api/projects");
    if (!response.ok) throw new Error(await readError(response));
    state.projects = await response.json();
    renderGroups();
    renderStats();
    renderProjects();
    if (state.selectedId) selectProject(state.selectedId, false);
  } catch (error) {
    elements.grid.innerHTML = "";
    showToast(`Could not load projects: ${error.message}`, true);
  }
}

function renderGroups() {
  const counts = new Map();
  for (const project of state.projects) counts.set(project.group, (counts.get(project.group) || 0) + 1);
  elements.groups.innerHTML = [...counts].map(([group, count]) => `
    <button class="group-link ${state.activeGroup === group ? "selected" : ""}" data-group="${escapeHtml(group)}">
      <span class="group-marker"></span><span>${escapeHtml(group)}</span><span class="group-count">${count}</span>
    </button>`).join("");
  document.querySelector("#sidebar-total").textContent = state.projects.length;
}

function renderStats() {
  document.querySelector("#stat-projects").textContent = state.projects.length;
  document.querySelector("#stat-runnable").textContent = state.projects.filter(project => project.canRun).length;
  document.querySelector("#stat-frameworks").textContent = new Set(state.projects.map(project => project.framework)).size;
  updateRunningCount();
}

function visibleProjects() {
  const term = elements.search.value.trim().toLowerCase();
  const kind = elements.typeFilter.value;
  return state.projects.filter(project => {
    const job = state.jobs.get(project.id);
    const matchesText = !term || `${project.name} ${project.group} ${project.framework} ${project.relativePath}`.toLowerCase().includes(term);
    const matchesKind = kind === "all" || project.kind === kind;
    const matchesGroup = !state.activeGroup || project.group === state.activeGroup;
    const matchesRunning = !state.runningOnly || (job && isActive(job.status));
    return matchesText && matchesKind && matchesGroup && matchesRunning;
  });
}

function renderProjects() {
  const visible = visibleProjects();
  elements.grid.innerHTML = visible.map(project => {
    const job = state.jobs.get(project.id);
    const status = job ? job.status.toLowerCase() : "";
    const statusText = job ? (isActive(job.status) ? job.status : job.status) : project.canRun ? "Ready" : "Buildable";
    return `
      <article class="project-card ${state.selectedId === project.id ? "selected" : ""}" data-project="${escapeHtml(project.id)}" tabindex="0" role="button" aria-label="Open ${escapeHtml(project.name)} project console">
        <div class="card-top"><span class="project-mark">${project.canRun ? "C#" : "◇"}</span><span class="project-kind">${escapeHtml(project.kind)}</span></div>
        <h3 class="project-name" title="${escapeHtml(project.name)}">${escapeHtml(project.name)}</h3>
        <div class="project-path" title="${escapeHtml(project.relativePath)}">${escapeHtml(project.relativePath)}</div>
        <div class="card-bottom"><span class="framework-tag">${escapeHtml(project.framework)}</span><span class="card-status ${status}"><i class="status-dot"></i>${escapeHtml(statusText)}</span></div>
      </article>`;
  }).join("");
  elements.empty.hidden = visible.length > 0;
  elements.grid.hidden = visible.length === 0;
  const filter = elements.filterTag;
  const label = state.activeGroup || (state.runningOnly ? "Running now" : "");
  filter.hidden = !label;
  filter.innerHTML = label ? `${escapeHtml(label)} <button aria-label="Clear filter">×</button>` : "";
  document.querySelector("#breadcrumb-current").textContent = state.activeGroup || (state.runningOnly ? "Running now" : "All projects");
}

function selectProject(id, openPanel = true) {
  const project = state.projects.find(candidate => candidate.id === id);
  if (!project) return;
  state.selectedId = id;
  renderProjects();
  elements.selected.innerHTML = `
    <div class="selected-project-header"><span class="selected-project-icon">${project.canRun ? "C#" : "◇"}</span><div style="min-width:0"><div class="selected-project-title" title="${escapeHtml(project.name)}">${escapeHtml(project.name)}</div><div class="selected-project-group">${escapeHtml(project.group)}</div></div></div>
    <div class="selected-path">${escapeHtml(project.relativePath)}</div>
    <div class="selected-tags"><span class="selected-tag">${escapeHtml(project.framework)}</span><span class="selected-tag">${escapeHtml(project.kind)}</span></div>`;
  elements.actionArea.hidden = false;
  document.querySelector("#run-warning").hidden = project.canRun;
  document.querySelector("#test-warning").hidden = project.canTest;
  document.querySelector('[data-action="run"]').disabled = !project.canRun || hasActiveJob(id);
  document.querySelector('[data-action="test"]').disabled = !project.canTest || hasActiveJob(id);
  for (const button of document.querySelectorAll(".action-button")) {
    if (button.dataset.action !== "run" && button.dataset.action !== "test") button.disabled = hasActiveJob(id);
  }
  const job = state.jobs.get(id);
  if (job) showJob(job);
  else clearConsole();
  if (openPanel && window.innerWidth <= 1120) elements.panel.classList.add("open");
}

function clearConsole() {
  elements.output.innerHTML = '<span class="console-empty">Your build and run output will<br>appear here.</span>';
  elements.meta.textContent = "Select an action to get started";
  elements.status.innerHTML = '<span class="idle-dot"></span> IDLE';
  elements.liveDot.classList.remove("live");
  elements.stop.disabled = true;
}

function showJob(job) {
  elements.output.textContent = job.output || `> ${job.command}\n\nWaiting for output...`;
  elements.output.scrollTop = elements.output.scrollHeight;
  elements.meta.textContent = `${job.action.toUpperCase()} · ${new Date(job.startedAt).toLocaleTimeString()}`;
  const active = isActive(job.status);
  elements.status.innerHTML = `<span class="${active ? "footer-dot" : job.status === "Succeeded" ? "footer-dot" : "idle-dot"}"></span> ${escapeHtml(job.status.toUpperCase())}${job.exitCode == null ? "" : ` · EXIT ${job.exitCode}`}`;
  elements.liveDot.classList.toggle("live", active);
  elements.stop.disabled = !active;
  document.querySelector('[data-action="run"]').disabled = !state.projects.find(project => project.id === job.projectId)?.canRun || active;
  for (const button of document.querySelectorAll(".action-button")) {
    if (button.dataset.action === "build") button.disabled = active;
    if (button.dataset.action === "test") {
      button.disabled = !state.projects.find(project => project.id === job.projectId)?.canTest || active;
    }
  }
}

async function startAction(action) {
  if (!state.selectedId) return;
  try {
    const response = await fetch(`/api/projects/${encodeURIComponent(state.selectedId)}/actions/${action}`, { method: "POST" });
    if (!response.ok) throw new Error(await readError(response));
    const job = await response.json();
    state.jobs.set(job.projectId, job);
    state.latestJobId = job.id;
    showJob(job);
    renderProjects();
    updateRunningCount();
    clearInterval(state.pollTimer);
    state.pollTimer = setInterval(() => pollJob(job.id), 700);
  } catch (error) {
    showToast(error.message, true);
  }
}

async function pollJob(id) {
  try {
    const response = await fetch(`/api/jobs/${encodeURIComponent(id)}`);
    if (!response.ok) throw new Error(await readError(response));
    const job = await response.json();
    state.jobs.set(job.projectId, job);
    if (job.id === state.latestJobId) showJob(job);
    renderProjects();
    updateRunningCount();
    if (!isActive(job.status)) {
      clearInterval(state.pollTimer);
      state.pollTimer = null;
      if (job.status === "Failed") showToast(`${job.projectName} ${job.action} failed. See output for details.`, true);
    }
  } catch (error) {
    clearInterval(state.pollTimer);
    state.pollTimer = null;
    showToast(`Could not read process output: ${error.message}`, true);
  }
}

async function stopSelectedJob() {
  const job = state.selectedId && state.jobs.get(state.selectedId);
  if (!job || !isActive(job.status)) return;
  try {
    const response = await fetch(`/api/jobs/${encodeURIComponent(job.id)}/stop`, { method: "POST" });
    if (!response.ok) throw new Error(await readError(response));
    await pollJob(job.id);
  } catch (error) {
    showToast(error.message, true);
  }
}

function updateRunningCount() {
  const count = [...state.jobs.values()].filter(job => isActive(job.status)).length;
  document.querySelector("#stat-running").textContent = count;
  document.querySelector("#sidebar-running").textContent = count;
}

function hasActiveJob(id) {
  const job = state.jobs.get(id);
  return !!job && isActive(job.status);
}

function isActive(status) {
  return status === "Starting" || status === "Running";
}

async function readError(response) {
  try {
    const data = await response.json();
    return data.error || `Request failed (${response.status})`;
  } catch {
    return `Request failed (${response.status})`;
  }
}

function showToast(message, error = false) {
  elements.toast.textContent = message;
  elements.toast.classList.toggle("error", error);
  elements.toast.classList.add("show");
  clearTimeout(state.toastTimer);
  state.toastTimer = setTimeout(() => elements.toast.classList.remove("show"), 4200);
}

function escapeHtml(value) {
  return String(value).replace(/[&<>"']/g, character => ({
    "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;"
  })[character]);
}

document.querySelector("#group-list").addEventListener("click", event => {
  const button = event.target.closest("[data-group]");
  if (!button) return;
  state.activeGroup = state.activeGroup === button.dataset.group ? null : button.dataset.group;
  state.runningOnly = false;
  document.querySelector("#nav-all").classList.remove("active");
  document.querySelector("#nav-running").classList.remove("active");
  renderGroups();
  renderProjects();
});

elements.grid.addEventListener("click", event => {
  const card = event.target.closest("[data-project]");
  if (card) selectProject(card.dataset.project);
});

elements.grid.addEventListener("keydown", event => {
  if (event.key !== "Enter" && event.key !== " ") return;
  const card = event.target.closest("[data-project]");
  if (card) {
    event.preventDefault();
    selectProject(card.dataset.project);
  }
});

for (const button of document.querySelectorAll(".action-button")) {
  button.addEventListener("click", () => startAction(button.dataset.action));
}

elements.search.addEventListener("input", renderProjects);
elements.typeFilter.addEventListener("change", renderProjects);
document.querySelector("#refresh").addEventListener("click", loadProjects);
document.querySelector("#close-detail").addEventListener("click", () => elements.panel.classList.remove("open"));
document.querySelector("#stop-job").addEventListener("click", stopSelectedJob);
document.querySelector("#clear-output").addEventListener("click", clearConsole);
elements.filterTag.addEventListener("click", () => {
  state.activeGroup = null;
  state.runningOnly = false;
  document.querySelector("#nav-all").classList.add("active");
  document.querySelector("#nav-running").classList.remove("active");
  renderGroups();
  renderProjects();
});

document.querySelector("#nav-all").addEventListener("click", () => {
  state.activeGroup = null;
  state.runningOnly = false;
  document.querySelector("#nav-all").classList.add("active");
  document.querySelector("#nav-running").classList.remove("active");
  renderGroups();
  renderProjects();
});

document.querySelector("#nav-running").addEventListener("click", () => {
  state.activeGroup = null;
  state.runningOnly = !state.runningOnly;
  document.querySelector("#nav-running").classList.toggle("active", state.runningOnly);
  document.querySelector("#nav-all").classList.toggle("active", !state.runningOnly && !state.activeGroup);
  renderGroups();
  renderProjects();
});

document.addEventListener("keydown", event => {
  if ((event.ctrlKey || event.metaKey) && event.key.toLowerCase() === "k") {
    event.preventDefault();
    elements.search.focus();
  }
  if (event.key === "Escape") elements.panel.classList.remove("open");
});

loadProjects();
