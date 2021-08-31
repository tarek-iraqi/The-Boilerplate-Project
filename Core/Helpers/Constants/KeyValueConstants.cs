using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Constants
{
    public static class KeyValueConstants
    {
        public const string ArabicLanguage = "ar-SA";
        public const string EnglishLanguage = "en-US";
        public const string EmailTemplatesPath = "wwwroot/emailTemplates";
        public const string AllowedCrosOrigins = "AllowedCrosOrigins";
        public const string Issuer = "WebApi";
        public const string Audience = "WebApi.Clients";
        public const string AccessToken = "access_token";
        public const string SignalREndPoint = "/broadcast";
        public const string SecretHashKey = "System:JWTSettings:SecretHashKey";
        public const string Role = "Role";
        public const string IP = "ip";
        public const string TokenType = "Bearer";
        public const string GeneralError = "error";
        public const string FirebaseApp = "FIREBASE_APP";
        public const string DbConnection = "DbConnection";
        public const string UsernameClaimType = "username";
        public const string StageLogsPath = "wwwroot/logs/staging";
        public const string DemoLogsPath = "wwwroot/logs/demo";

        #region Email Templates
        public const string AccountVerificationEmailTemplate = "AccountVerification.cshtml";
        public const string ResetPasswordEmailTemplate = "ResetPassword.cshtml";
        #endregion
    }
}
