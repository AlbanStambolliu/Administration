using Administration.Application.Interfaces.Projects;
using Administration.Application.ViewModels.ProjectTaskViewModels;
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
    public class ProjectTaskController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        private readonly IProjectTaskService _taskService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="projectService"></param>
        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// ProjectList
        /// </summary>
        /// <returns></returns>
        [HttpGet("List")]
        public IActionResult GetProjects()
        {
            return Json(_taskService.GetTask());
        }

        /// <summary>
        /// Create project
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("Create/Task")]
        public async Task<IActionResult> SaveProject([FromBody] CreateProjectTaskViewModel model)
        {
            var result = await _taskService.CreateTask(model);
            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateTopic(
            [FromBody] ProjectTaskViewModel topic)
        {
            var result = await _taskService.UpdateTaskAsync(topic);
            if (!result.Success)
            {
                ModelState.AddModelErrorFromResults(result);
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpPost("Delete")]
        //[HttpGet]
        public async Task<IActionResult> DeleteProjectAsync(Guid Id, CancellationToken cancellation)
        {
            //return Json(new { success = true });
            return Ok(await _taskService.DeleteTaskAsync(Id, cancellation));
        }
    }
}
