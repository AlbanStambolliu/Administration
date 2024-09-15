

using System.ComponentModel.DataAnnotations;

namespace Administration.Application.ViewModels.UserViewModels
{
    /// <summary>
    /// The model to be used for the Log In / Authorization 
    /// </summary>
    public class LogInViewModel
    {
        /// <summary>
        /// Username property to keep the value of username 
        /// </summary>
        //[Required(ErrorMessage ="UserName is required")]
        public string Username { get; set; }
        /// <summary>
        /// Password Value to be used for authentication 
        /// </summary>
        //[Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
