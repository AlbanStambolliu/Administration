using System;
using System.ComponentModel.DataAnnotations;


namespace Administration.Application.ViewModels.AuthenticationViewModels
{
    public class CreateUserViewModel
    {
        /// <summary>
        /// Property to keep the user's first name 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Property to keep the user's last name
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Property to keep user email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Property to keep the user's password
        /// </summary>
        [Required(ErrorMessage = "Vendosni një fjalëkalim!")]
        public string Password { get; set; }
        /// <summary>
        /// Foreign key used to assign to a user a role 
        /// </summary>
        //public Guid RoleId { get; set; }
    }
}
