using Helpers.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(email),
                Description = ResourceKeys.DuplicateEmail
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(userName),
                Description = ResourceKeys.DuplicateUserName
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(email),
                Description = ResourceKeys.InvalidEmail
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = ResourceKeys.DuplicateRoleName
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = ResourceKeys.InvalidRoleName
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = "token",
                Description = ResourceKeys.InvalidToken
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(userName),
                Description = ResourceKeys.InvalidUserName
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = "login",
                Description = ResourceKeys.LoginAlreadyAssociated
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordMismatch
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordRequiresDigit
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordRequiresLower
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordRequiresNonAlphanumeric
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordRequiresUniqueChars
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordRequiresUpper
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.PasswordTooShort
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = "password",
                Description = ResourceKeys.UserAlreadyHasPassword
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = ResourceKeys.UserAlreadyInRole
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = ResourceKeys.UserNotInRole
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = "user",
                Description = ResourceKeys.UserLockoutNotEnabled
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = "recovery_code",
                Description = ResourceKeys.RecoveryCodeRedemptionFailed
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = "concurrency",
                Description = ResourceKeys.ConcurrencyFailure
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = "error",
                Description = ResourceKeys.DefaultIdentityError
            };
        }
    }
}
