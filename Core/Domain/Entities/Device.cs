using Domain.Common;
using System;

namespace Domain.Entities
{
    public class Device : Entity<long>
    {
        public string Model { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string DeviceLanguage { get; set; }
        public virtual AppUser User { get; set; }
    }
}
