using Administration.Application.Interfaces.Projects;
using Administration.Application.Interfaces.UserInterface;
using Administration.Application.Results;
using Administration.Application.ViewModels.ProjectViewModels;
using Administration.Domain.Entities.Projects;
using Administration.Domain.Entities.Users;
using Administration.Infra.Data.Repositories;
using Administration.Infra.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.Application.Services.Projects
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> _projectRepository;
        private readonly IRepository<ProjectTask> _projectTaskRepo;
        private readonly IRepository<ApplicationUser> _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;
        private readonly IRepository<ProjectUser> _projectUserRepository;

        /// <summary>
        /// Constructoor
        /// </summary>
        /// <param name="projectRepository"></param>
        public ProjectService(IRepository<Project> projectRepository,
            IRepository<ProjectTask> projectTaskRepo, UserManager<ApplicationUser> userManager
            , IUserService userService, IRepository<ApplicationUser> userRepository,
            IRepository<ProjectUser> projectUserRepository)
        {
            _projectRepository = projectRepository;
            _projectTaskRepo = projectTaskRepo;
            _userManager = userManager;
            _userService = userService;
            _userRepository = userRepository;
            _projectUserRepository = projectUserRepository;
        }

        /// <summary>
        /// List Project for User Login 
        /// </summary>
        /// <returns></returns>
        public HashSet<ProjectViewModel> GetProjects()
        {
            var admin = _userService.GetCurrentUserId();
            //var projects = _projectUserRepository.TableNoTracking.Where(c => c.UserId == admin.Id.);
            //var project = _projectRepository.TableNoTracking
            //    .Include(x=> x.ProjectUsers)
            //    .Where(x => !x.IsDeleted &&  x.ProjectUsers.FirstOrDefault().UserId == admin.Result.Id)
            //    .Select(x => new ProjectViewModel()
            //    {
            //        Id = x.Id,
            //        ProjectName = x.ProjectName,
            //        StartDate = x.StartDate,
            //        EndDate = x.EndDate,
            //        CreatedOnUtc = x.CreatedOnUtc,
            //        CreatedById = x.CreatedById,
            //        UpdatedOnUtc = x.UpdatedOnUtc,
            //        UpdatedById = x.UpdatedById
            //    });

            var projectIdList = _projectUserRepository.TableNoTracking.Where(x => x.UserId.Equals(admin))
                            .Select(x => x.ProjectId);
            List<ProjectViewModel> list = new List<ProjectViewModel>();
            foreach (var item in projectIdList)
            {
                var projectDetails = _projectRepository.TableNoTracking
                    .Where(x => x.Id == item).FirstOrDefault();
                list.Add(new ProjectViewModel()
                {
                    Id = projectDetails.Id,
                    ProjectName = projectDetails.ProjectName,
                    StartDate = projectDetails.StartDate,
                    EndDate = projectDetails.EndDate,
                    CreatedOnUtc = projectDetails.CreatedOnUtc,
                    CreatedById = projectDetails.CreatedById,
                    UpdatedOnUtc = projectDetails.UpdatedOnUtc,
                    UpdatedById = projectDetails.UpdatedById
                });
            }

            return list.ToHashSet();
        }

        /// <summary>
        /// Create new Project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result> CreateProject(CreateProjectViewModel project, CancellationToken cancellationToken = default)
        {
            var admin = await _userService.GetCurrentUserDetailsAsync();
            var existingProject = _projectRepository.TableNoTracking.Any(x => x.ProjectName.ToLower() == project.ProjectName.ToLower() && !x.IsDeleted);
            if (existingProject)
            {
                return Result.Fail("Një Project me të njëjtin emër ekziston.");
            }
            try
            {
                Project entity = new()
                {
                    ProjectName = project.ProjectName,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    IsDeleted = false,
                    CreatedOnUtc = DateTime.Now,
                    //CreatedById = project.CreatedById
                    CreatedById = admin.Id,

                };
                await _projectRepository.InsertAsync(entity, cancellationToken);
                await AssaignUser(entity.Id, admin.Id, cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("Ndodhi një gabim. Ju lutem provoni përsëri.");
            }
        }

        public async Task<Result> UpdateProjectAsync(UpdateProjectViewModel project, CancellationToken cancellationToken = default)
        {
            var admin = await _userService.GetCurrentUserDetailsAsync();
            var model = _projectRepository.Table.FirstOrDefault(t => t.Id == project.Id && !t.IsDeleted);
            if (model == null)
            {
                return Result.Fail("Tema nuk u gjet.");
            }
            var existingproject = _projectRepository.TableNoTracking.Any(x => x.ProjectName.ToLower() == project.ProjectName.ToLower() && !x.IsDeleted && x.Id != model.Id);
            if (existingproject)
            {
                return Result.Fail("Një projekt me të njëjtin emër ekziston.");
            }
            try
            {
                model.ProjectName = project.ProjectName;
                model.EndDate = project.EndDate;
                model.UpdatedOnUtc = DateTime.Now;
                model.UpdatedById = admin.Id;
                await _projectRepository.UpdateAsync(model, cancellationToken);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail("Ndodhi një gabim. Ju lutem provoni përsëri.");
            }
        }

        public async Task<Result> DeleteProjectAsync(Guid id, CancellationToken cancellation = default)
        {
            try
            {
                //soft deleted
                var project = await _projectRepository.GetByIdAsync(id, cancellation);

                if (project == null)
                    return Result.Fail("Project with this Id was not found");
                project.IsDeleted = true;

                await _projectRepository.UpdateAsync(project, cancellation);
            }
            catch
            {
                return Result.Fail("An error occurred while trying to delete this Project");
            }
            return Result.Ok();
        }

        public async Task<Result> AssaignUser(Guid projectId, Guid userId, CancellationToken cancellation = default)
        {
            var admin = await _userService.GetCurrentUserDetailsAsync();
            var existingProject = _projectRepository.TableNoTracking.Any(x => x.Id == projectId && !x.IsDeleted);
            var existingUser = _userRepository.TableNoTracking.Any(x => x.Id == userId && x.IsActive);

            if (existingProject && existingUser)
            {
                try
                {
                    ProjectUser entity = new()
                    {
                        ProjectId = projectId,
                        UserId = userId,
                    };
                    await _projectUserRepository.InsertAsync(entity, cancellation);
                    return Result.Ok();
                }
                catch (Exception ex)
                {
                    return Result.Fail("Ndodhi një gabim. Ju lutem provoni përsëri.");
                }
            }

            return Result.Fail("Ndodhi një gabim. Useri ose projekti nuk ekziston");
        }

    }
}
