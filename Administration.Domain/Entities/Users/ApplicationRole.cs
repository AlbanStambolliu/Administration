using Administration.Domain.Entities.Generics;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;


namespace Administration.Domain.Entities.Users
{
    public class ApplicationRole : IdentityRole<Guid>, IAuditable
    {
        /// <summary>
        /// Collection of users containing this role
        /// </summary>
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        #region Implementation of IAuditable

        /// <summary>
        /// Gets or sets the UTC date time when the entity was first created.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC date time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the Id of the user that created this entity.
        /// </summary>
        public Guid CreatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that created this entity.
        /// </summary>
        public virtual ApplicationUser CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Id of the user that last updated this entity.
        /// </summary>
        public Guid? UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that last updated this entity.
        /// </summary>
        public virtual ApplicationUser? UpdatedBy { get; set; }

        /// <summary>
        /// Shows if the entity is active or not
        /// </summary>
        public bool IsActive { get; set; }
        #endregion

       
    }
}
