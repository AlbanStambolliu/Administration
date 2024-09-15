using Microsoft.AspNetCore.Identity;
using System;


namespace Administration.Domain.Entities.Users
{
    public class ApplicationUserRole:IdentityUserRole<Guid>
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
