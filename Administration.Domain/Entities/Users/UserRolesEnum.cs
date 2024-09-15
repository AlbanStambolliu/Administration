using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Domain.Entities.Users
{
    public enum UserRolesEnum
    {
        /// <summary>
        /// Administrator.
        /// </summary>
        [Display(Name = "Administrator")]
        Administrator = 1,
        /// <summary>
        /// Editor.
        /// </summary>
        [Display(Name = "Employee")]
        User = 2,
    }
}
