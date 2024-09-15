using Administration.Application.ViewModels.ProjectTaskViewModels;
using System;
using System.Collections.Generic;

namespace Administration.Application.ViewModels.ProjectViewModels
{
    public class ProjectViewModel : BaseModel
    {
        /// <summary>
        ///  Project name
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Get or set the Start date of project
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Get or set the end date of project
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Get or set Date created
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///  Get or set Date updated
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        ///  Get or set user id created
        /// </summary>
        public Guid CreatedById { get; set; }

        /// <summary>
        /// user id updated
        /// </summary>
        public Guid? UpdatedById { get; set; }

        public List<ProjectTaskModelList> ProjectTaskList { get; set; } = new List<ProjectTaskModelList>();

    }

    public class ProjectTaskModelList
    {
        /// <summary>
        /// Property to keep project name 
        /// </summary>
        public string TaskName { get; set; }
        /// <summary>
        /// Get or set d
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// gets or sets started date 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// gets or sets end project date 
        /// </summary>
        public DateTime EndDate { get; set; }
    }

    public class AllProjectTask
    {
        public List<ProjectViewModel> AllProjectTaskList { get; set; } = new List<ProjectViewModel>();
    }
}
