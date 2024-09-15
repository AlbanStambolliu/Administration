using Administration.Domain.Entities.Generics;
using System;
using System.Collections.Generic;

namespace Administration.Domain.Entities.Projects
{
    public class Project : AuditableEntity
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gests or sets the start date
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gests or sets the end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// gets or set if is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public ICollection<ProjectUser> ProjectUsers { get; set; } = null!;
        public ICollection<ProjectTask> ProjectTask { get; set; } = null!;
    }
}
