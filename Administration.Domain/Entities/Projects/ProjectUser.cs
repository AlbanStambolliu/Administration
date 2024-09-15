using Administration.Domain.Entities.Generics;
using Administration.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Administration.Domain.Entities.Projects
{
    public class ProjectUser 
    {
        public Guid UserId { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;

        public Guid ProjectId { get; set; }
        public virtual Project Project { get; set; } = null!;

    }
}
