using Administration.Application.Interfaces.Projects;
using Administration.Application.Interfaces.UserInterface;
using Administration.Application.Results;
using Administration.Application.ViewModels.ProjectTaskViewModels;
using Administration.Application.ViewModels.ProjectViewModels;
using Administration.Domain.Entities.Projects;
using Administration.Domain.Entities.Users;
using Administration.Infra.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Application.Services.Projects
{
    public class ProjectTaskService : IProjectTaskService
    {
        //Dependencies
        private readonly IRepository<ProjectTask> _projectTaskRepo;
        private readonly IRepository<Project> _projectRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRepository<ProjectUser> _projectUserRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="taskRepository"></param>
        /// <param name="projectRepo"></param>
        /// <param name="userManager"></param>
        /// <param name="userService"></param>
        /// <param name="projectUserRepository"></param>
        /// <param name="httpContextAccessor"></param>
        public ProjectTaskService(IRepository<ProjectTask> taskRepository, IRepository<Project> projectRepo,
            UserManager<ApplicationUser> userManager, IUserService userService,
            IRepository<ProjectUser> projectUserRepository, IHttpContextAccessor httpContextAccessor)
        {
            _projectTaskRepo = taskRepository;
            _userManager = userManager;
            _projectRepo = projectRepo;
            _userService = userService;
            _projectUserRepository = projectUserRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public HashSet<ProjectViewModel> GetTask()
        {
            //return only task project
            //var onlyTask = _projectTaskRepo.TableNoTracking
            //                .Where(t => t.AssaignedUserId.Equals(admin))
            //                .Select(t => new ProjectTaskModelList()
            //                {
            //                    TaskName = t.TaskName,
            //                    Description = t.Description,
            //                    EndDate = t.EndDate,
            //                    StartDate = t.StartDate
            //                });

            var admin = _userService.GetCurrentUserId();
            var projectId = _projectUserRepository.TableNoTracking
                .Where(x => x.UserId.Equals(admin)).Select(x=> x.ProjectId);
            List<ProjectViewModel> list = new List<ProjectViewModel>();
            foreach (var item in projectId)
            {
                var projectTask = _projectRepo.TableNoTracking
                    .Include(x => x.ProjectTask)
                    .Where(x => !x.IsDeleted && x.Id.Equals(item)).FirstOrDefault();
                list.Add(new ProjectViewModel()
                {
                    Id = projectTask.Id,
                    ProjectName = projectTask.ProjectName,
                    StartDate = projectTask.StartDate,
                    EndDate = projectTask.EndDate,
                    CreatedOnUtc = projectTask.CreatedOnUtc,
                    CreatedById = projectTask.CreatedById,
                    UpdatedOnUtc = projectTask.UpdatedOnUtc,
                    UpdatedById = projectTask.UpdatedById,
                    ProjectTaskList = projectTask.ProjectTask
                    .Where(x => x.AssaignedUserId.Equals(admin) && x.ProjectId.Equals(item))
                    .Select(x => new ProjectTaskModelList()
                    {
                        TaskName = x.TaskName,
                        Description = x.Description,
                        EndDate = x.EndDate,
                        StartDate = x.StartDate
                    }).ToList()
                });
            }
            return list.ToHashSet();
        }

        public async Task<Result> CreateTask(CreateProjectTaskViewModel task, CancellationToken cancellationToken = default)
        {
            var adminId = _userService.GetCurrentUserId();
            var existingTask = _projectTaskRepo.TableNoTracking.Any(x => x.TaskName.ToLower() == task.TaskName.ToLower() && !x.IsDeleted);
            if (existingTask)
            {
                return Result.Fail("Një Task me të njëjtin emër ekziston.");
            }
            var partOfProject = _projectUserRepository.TableNoTracking.Where(x => x.ProjectId == task.ProjectId).
                Any(p => p.UserId == adminId  /*& p.UserId == task.AssaignedUserId*/);

            var partOfProject2 = _projectUserRepository.TableNoTracking.Where(x => x.ProjectId == task.ProjectId).
                Any(p => p.UserId == task.AssaignedUserId);

            if (partOfProject && partOfProject2)
            {
                try
                {
                    ProjectTask entity = new()
                    {
                        TaskName = task.TaskName,
                        Description = task.Description,
                        StartDate = task.StartDate,
                        EndDate = task.EndDate,
                        IsCompleted = false,
                        IsDeleted = false,
                        ProjectId = task.ProjectId,
                        AssaignedUserId = task.AssaignedUserId,
                        CreatedOnUtc = DateTime.Now,
                        CreatedById = adminId,


                    };
                    await _projectTaskRepo.InsertAsync(entity, cancellationToken);
                    return Result.Ok();
                }
                catch (Exception ex)
                {
                    return Result.Fail("Ndodhi një gabim. Ju lutem provoni përsëri.");
                }

            }
            else
            {
                return Result.Fail("Ju ose personi te cilit po i beni assaign nuk jane pjese e ketij projekti ");
            }

        }

        public async Task<Result> UpdateTaskAsync(ProjectTaskViewModel task, CancellationToken cancellationToken = default)
        {
            var model = _projectTaskRepo.Table.FirstOrDefault(t => t.Id == task.Id && !t.IsDeleted);
            if (model == null)
            {
                return Result.Fail("Tasku nuk u gjet.");
            }
            var existingtask = _projectTaskRepo.TableNoTracking.Any(x => x.TaskName.ToLower() == task.TaskName.ToLower() && !x.IsDeleted /*&& x.Id != model.Id*/);
            if (existingtask)
            {
                return Result.Fail("Një task me të njëjtin emër ekziston.");
            }
            try
            {
                model.TaskName = task.TaskName;
                model.Description = task.Description;
                model.StartDate = task.StartDate;
                model.EndDate = task.EndDate;
                model.IsCompleted = task.IsCompleted;
                model.AssaignedUserId = task.AssaignedUserId;
                model.UpdatedOnUtc = DateTime.Now;
                model.UpdatedById = task.UpdatedById;
                await _projectTaskRepo.UpdateAsync(model, cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("Ndodhi një gabim. Ju lutem provoni përsëri.");
            }
        }

        public async Task<Result> DeleteTaskAsync(Guid id, CancellationToken cancellation = default)
        {
            try
            {
                //soft deleted
                var project = await _projectTaskRepo.GetByIdAsync(id, cancellation);

                if (project == null)
                    return Result.Fail("Task with this Id was not found");
                project.IsDeleted = true;

                await _projectTaskRepo.UpdateAsync(project, cancellation);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to delete this task");
            }
            return Result.Ok();
        }

    }
}
