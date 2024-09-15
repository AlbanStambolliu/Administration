using Administration.Application.Interfaces;
using Administration.Application.Interfaces.Projects;
using Administration.Application.ViewModels.ProjectViewModels;
using Administration.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Administration.WebApi.Controllers
{
    /// <summary>
    /// Defines the <see cref="ProjectController" />.
    /// </summary>
    [ApiController]
    [Route("[Controller]")]
    [Authorize]
    //[Authorize("Administrator")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectService"></param>
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        /// <summary>
        /// ProjectList
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public IActionResult GetProjects()
        {
            return Json(_projectService.GetProjects());
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create/Project")]
        public async Task<IActionResult> SaveProject([FromBody] CreateProjectViewModel model)
        {
            var result = await _projectService.CreateProject(model);
            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        [HttpPost("Update")]
        public async Task<IActionResult> UpdateProject(
            [FromBody] UpdateProjectViewModel project)
        {
            var result = await _projectService.UpdateProjectAsync(project);
            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        /// <summary>
        /// Softt delete project
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [HttpPost("Delete")]
        //[HttpGet]
        public async Task<IActionResult> DeleteProjectAsync( Guid Id, CancellationToken cancellation)
        {
            //return Json(new { success = true });
            return Ok(await _projectService.DeleteProjectAsync(Id, cancellation));
        }

        [HttpPost]
        public async Task<IActionResult> AssaignUser(Guid projectId, Guid userId, CancellationToken cancellation)
        {
            var result = await _projectService.AssaignUser(projectId, userId);
            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return BadRequest(ModelState);
            }
            return Ok();
        }
    }
}

