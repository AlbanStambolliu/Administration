using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.ViewModels.ProjectViewModels
{
    public class UpdateProjectViewModel : BaseModel
    {
        // <summary>
        /// Property to keep project name 
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// gets or sets started date 
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// gets or sets end project date 
        /// </summary>
        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }

        /// <summary>
        /// gets or sets user id updated
        /// </summary>
        public Guid UpdatedById { get; set; }

        public Guid CreatedById { get; set; }
    }
}
