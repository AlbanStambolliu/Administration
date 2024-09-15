using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Application.ViewModels
{
    /// <summary>
    /// ChangePasswordViewModel
    /// </summary>
    public class ChangePictureViewModel
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// OldPassword
        /// </summary>
        public string OldPicturePath { get; set; }
        /// <summary>
        /// NewPassword
        /// </summary>
        public string NewPicturePath { get; set; }
        
    }
}
