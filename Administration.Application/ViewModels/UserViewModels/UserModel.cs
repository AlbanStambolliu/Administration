using Microsoft.AspNetCore.Http;
using System;

namespace Administration.Application.ViewModels.UserViewModels
{
    public  class UserModel:BaseModel
    {
        /// <summary>
        ///  First name of the user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's username 
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
        /// The identifier of the user that created the user.
        /// </summary>
        public Guid CreatedById { get; set; }

        /// <summary>
        /// The identifier of the user that last updated the user.
        /// </summary>
        public Guid? UpdatedById { get; set; }

        /// <summary>
        /// Gets or sets Image Path 
        /// </summary>
        public IFormFile FilePath { get; set; }

        /// <summary>
        /// Gets and sets file name 
        /// </summary>
        public string FileName { get; set; }
    }
}
