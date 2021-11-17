using Domain.Common;
using Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class AppUser : IdentityUser<Guid>, IBaseEntity
    {
        protected AppUser()
        {
            Claims = new HashSet<AppUserClaim>();
            Logins = new HashSet<AppUserLogin>();
            Tokens = new HashSet<AppUserToken>();
            UserRoles = new HashSet<AppUserRole>();
            Devices = new HashSet<Device>();
        }

        public AppUser(Name name, string user_name, string email,
            string phone_number = null, string profile_picture = null, bool is_active = true)
        {
            Name = name;
            UserName = user_name;
            Email = email;
            PhoneNumber = phone_number;
            ProfilePicture = profile_picture;
            IsActive = is_active;
        }

        public void EditData(Name name, string email, string phoneNumber, string profilePicture)
        {
            Name = name == null ? Name : name;
            Email = string.IsNullOrWhiteSpace(email) ? Email : email;
            PhoneNumber = string.IsNullOrWhiteSpace(phoneNumber) ? PhoneNumber : phoneNumber;
            ProfilePicture = string.IsNullOrWhiteSpace(profilePicture) ? ProfilePicture : profilePicture;
        }

        public void ChangeProfilePicture(string picture)
        {
            ProfilePicture = string.IsNullOrEmpty(picture) ? ProfilePicture : picture;
        }

        public Name Name { get; private set; }
        public string ProfilePicture { get; private set; }
        public bool IsActive { get; private set; }

        public virtual ICollection<AppUserClaim> Claims { get; set; }
        public virtual ICollection<AppUserLogin> Logins { get; set; }
        public virtual ICollection<AppUserToken> Tokens { get; set; }
        public virtual ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<Device> Devices { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
