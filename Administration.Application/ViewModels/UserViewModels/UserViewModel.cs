using Administration.Domain.Entities.Users;
using System;
using System.Collections.Generic;


namespace Administration.Application.ViewModels
{
    /// <summary>
    /// User view model
    /// </summary>
    public class UserViewModel 
        //: BaseModel 
        //, IMapFrom<ApplicationUser>
    {
        /// <summary>
        /// Creates a new instance of the <see cref="ApplicationUser"/> object.
        /// </summary>
        public UserViewModel()
        {
            //FirstName = string.Empty;
            //LastName = string.Empty;
        }

        public UserViewModel(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.Name;
            UserName = user.UserName;
            Email = user.Email;
            CreatedOnUtc = user.CreatedOnUtc;
            UpdatedOnUtc = user.UpdatedOnUtc;
            IsActive = user.IsActive;
            Password = user.PasswordHash;
            //Roleid = user.User;
        }

        /// <summary>
        /// User id
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        ///  First name of the user
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// User's username (used for logging in)
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// User's email adress
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the UTC date time when the entity was first created.
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// Gets or sets the UTC date time when the entity was last updated.
        /// </summary>
        public DateTime? UpdatedOnUtc { get; set; }
        /// <summary>
        /// Shows if the entity is active or not
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Shows the user's password
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Unique identifier of the user's role
        /// </summary>
        public Guid? Roleid { get; set; }

        /// <summary>
        /// Name of the role
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// The identifier of the user that created the user.
        /// </summary>
        public Guid CreatedById { get; set; }
        /// <summary>
        /// The full name of the user that created the user.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// The identifier of the user that last updated the user.
        /// </summary>
        public Guid? UpdatedById { get; set; }
        /// <summary>
        /// The full name of the user that last updated the user.
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}