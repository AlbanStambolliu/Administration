using Administration.Domain.Entities.Generics;
using Administration.Domain.Entities.Users;
using System;


namespace Administration.Domain.Entities.Projects
{
    public class ProjectTask: AuditableEntity
    {
        /// <summary>
        /// Get or set Task name
        /// </summary>
        public string TaskName { get; set; }

        // <summary>
        /// Get or set Discription
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set the start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Get or set the end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the task is completed.
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// gets or set if is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public Guid AssaignedUserId { get; set; }

        /// <summary>
        /// Gets or sets the user that created this entity.
        /// </summary>
        public virtual ApplicationUser AssaignedUser { get; set; } = null!;
    }

}
