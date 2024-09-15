using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.ViewModels.ProjectTaskViewModels
{
    public class ProjectTaskViewModel : BaseModel
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

        public bool IsCompleted { get; set; }

        public Guid ProjectId { get; set; }

        public Guid AssaignedUserId { get; set; }

        /// <summary>
        /// Get or set Date created
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        ///  Get or set Date updated
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        public Guid CreatedById { get; set; }

        /// <summary>
        /// gets or sets user id updated
        /// </summary>
        public Guid? UpdatedById { get; set; }
    }
}
