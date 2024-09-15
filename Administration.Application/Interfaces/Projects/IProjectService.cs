using Administration.Application.Results;
using Administration.Application.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Application.Interfaces.Projects
{
    public interface IProjectService
    {
        /// <summary>
        /// IQueryable projects
        /// </summary>
        /// <returns></returns>
        HashSet<ProjectViewModel> GetProjects();

        /// <summary>
        /// Create project and insert it in db.
        /// </summary>
        /// <param name="project">.</param>
        /// <param name="cancellationToken">.</param>
        /// <returns>.</returns>
        Task<Result> CreateProject(CreateProjectViewModel project, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update project.
        /// </summary>
        /// <param name="project">.</param>
        /// <param name="cancellationToken">.</param>
        /// <returns>.</returns>
        Task<Result> UpdateProjectAsync(UpdateProjectViewModel project, CancellationToken cancellationToken = default);
        Task<Result> DeleteProjectAsync(Guid Id, CancellationToken cancellationToken = default);
        Task<Result> AssaignUser(Guid projectId, Guid userId, CancellationToken cancellation = default);
    }
}
