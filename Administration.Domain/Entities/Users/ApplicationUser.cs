using Administration.Domain.Entities.Generics;
using Administration.Domain.Entities.Projects;
using Administration.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Administration.Domain.Entities.Users

{
    public class ApplicationUser : IdentityUser<Guid>,IAuditable
    {
        public ApplicationUser()
        {
            Name = string.Empty;
            Password = string.Empty;
        }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }


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
       // public virtual ApplicationUser CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Id of the user that last updated this entity.
        /// </summary>
        public Guid? UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets the user that last updated this entity.
        /// </summary>
        //public virtual ApplicationUser? UpdatedBy { get; set; }

        /// <summary>
        /// Shows if the entity is active or not
        /// </summary>
        public bool IsActive { get; set; }
        #endregion
        /// <summary>
        /// gets or set if is deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public string PicturePath { get; set; }

        //public ICollection<ProjectTask> Tasks { get; set; } = null!;
        public ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;
        public ICollection<ProjectUser> ProjectUsers { get; set; } = null!;


    }
}
