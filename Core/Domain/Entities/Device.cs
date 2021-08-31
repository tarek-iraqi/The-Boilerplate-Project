using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities 
{
    public class Device : Entity<long>
    {
        public string Model { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
