using Helpers.Localization;
using Microsoft.AspNetCore.Identity;

namespace Utilities.Services
{
    public class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(email),
                Description = LocalizationKeys.DuplicateEmail
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(userName),
                Description = LocalizationKeys.DuplicateUserName
            };
        }

        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = nameof(email),
                Description = LocalizationKeys.InvalidEmail
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = LocalizationKeys.DuplicateRoleName
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = LocalizationKeys.InvalidRoleName
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = "token",
                Description = LocalizationKeys.InvalidToken
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = nameof(userName),
                Description = LocalizationKeys.InvalidUserName
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = "login",
                Description = LocalizationKeys.LoginAlreadyAssociated
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordMismatch
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordRequiresDigit
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordRequiresLower
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordRequiresNonAlphanumeric
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordRequiresUniqueChars
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordRequiresUpper
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.PasswordTooShort
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = "password",
                Description = LocalizationKeys.UserAlreadyHasPassword
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = LocalizationKeys.UserAlreadyInRole
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = nameof(role),
                Description = LocalizationKeys.UserNotInRole
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = "user",
                Description = LocalizationKeys.UserLockoutNotEnabled
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = "recovery_code",
                Description = LocalizationKeys.RecoveryCodeRedemptionFailed
            };
        }

        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = "concurrency",
                Description = LocalizationKeys.ConcurrencyFailure
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = "error",
                Description = LocalizationKeys.DefaultIdentityError
            };
        }
    }
}
