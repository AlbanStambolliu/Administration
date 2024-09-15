using System.ComponentModel.DataAnnotations;

namespace Administration.Application.ViewModels.AuthenticationViewModels
{
    /// <summary>
    /// User login model
    /// Not used
    /// </summary>
    public class UserLoginModel
    {
        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to remember user
        /// </summary>
        public bool RememberMe { get; set; }
    }
}