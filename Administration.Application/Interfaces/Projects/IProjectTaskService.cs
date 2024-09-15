using Administration.Application.Results;
using Administration.Application.ViewModels.ProjectTaskViewModels;
using Administration.Application.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Application.Interfaces.Projects
{
    public interface IProjectTaskService
    {
        /// <summary>
        /// IQueryable projects
        /// </summary>
        /// <returns></returns>
        HashSet<ProjectViewModel> GetTask();

        /// <summary>
        /// Create project and insert it in db.
        /// </summary>
        /// <param name="project">.</param>
        /// <param name="cancellationToken">.</param>
        /// <returns>.</returns>
        Task<Result> CreateTask(CreateProjectTaskViewModel task, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update project.
        /// </summary>
        /// <param name="project">.</param>
        /// <param name="cancellationToken">.</param>
        /// <returns>.</returns>
        Task<Result> UpdateTaskAsync(ProjectTaskViewModel task, CancellationToken cancellationToken = default);
        Task<Result> DeleteTaskAsync(Guid Id, CancellationToken cancellationToken = default);
    }
}
