using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.ViewModels.ProjectTaskViewModels
{
    public class CreateProjectTaskViewModel
    {
        /// <summary>
        /// Property to keep project name 
        /// </summary>
        public string TaskName { get; set; }
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
        /// <summary>
        /// gets or sets user id updated
        /// </summary>
        //public Guid UpdatedById { get; set; }

        public Guid AssaignedUserId { get; set; }
        public Guid CreatedById { get; set; }
    }
}
