using Application.DTOs;
using Application.Contracts;
using Helpers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities.Services;

namespace WebApi.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("test")]
    [AllowAnonymous]
    public class TestController : BaseApiController
    {
        private readonly IEmailSender _emailSender;
        private readonly IPhoneValidator _phoneValidator;
        private readonly IApplicationLocalization _localizer;
        private readonly IFirebaseMessageSender _firebaseMessageSender;
        private readonly IApplicationConfiguration _configuration;
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        //private readonly IFirebaseCloudFirestore _firebaseCloudFirestore;

        public TestController(IEmailSender emailSender,
            IPhoneValidator phoneValidator,
            IApplicationLocalization localizer,
            IFirebaseMessageSender firebaseMessageSende,
            IApplicationConfiguration configuration,
            IConfiguration config,
            IWebHostEnvironment env)
            //IFirebaseCloudFirestore firebaseCloudFirestore)
        {
            _emailSender = emailSender;
            _phoneValidator = phoneValidator;
            _localizer = localizer;
            _firebaseMessageSender = firebaseMessageSende;
            _configuration = configuration;
            _config = config;
            _env = env;
        }

        //[HttpGet("firestore")]
        //public async Task<IActionResult> firestore()
        //{
        //   var result = await  _firebaseCloudFirestore.Get<notification>("notifications", "08d9ed98-6b93-4c5c-80b1-3e90b96b48dd");

        //    return Ok(new { user_id = result.user_id, has_unseen_notifications = result.has_unseen_notifications });
        //}

        [HttpGet("send_text_email")]
        public async Task<IActionResult> SendTextEmail()
        {
            await _emailSender.SendSingleEmail("tarek.iraqi@gmail.com",
                "Test send Single Email", "Test message");

            return Ok();
        }

        [HttpGet("send_temp_email")]
        public async Task<IActionResult> SendTempEmail()
        {
            await _emailSender.SendSingleEmail("tarek.iraqi@gmail.com",
                "Test send Single Email", KeyValueConstants.AccountVerificationEmailTemplate,
                new AccountVerificationEmailDTO { name = "Tarel", token = "Token" });

            return Ok();
        }


        [HttpPost("is_valid_phone")]
        public IActionResult IsValidPhone(PhoneNumber model)
        {
            return Ok(_phoneValidator.IsValidPhoneNumber(model.Phone, model.CountryCode));
        }

        [HttpPost("phone_format")]
        public IActionResult GetPhoneFormat(PhoneNumber model)
        {
            var national = _phoneValidator.GetNationalPhoneNumberFormat(model.Phone, model.CountryCode);
            var international = _phoneValidator.GetInternationalPhoneNumberFormat(model.Phone, model.CountryCode);
            return Ok(new
            {
                NationalFormat = national,
                InternationalFormat = international
            });
        }

        [HttpGet("local")]
        public IActionResult Local()
        {
            return Ok(new
            {
                Test = _localizer.Get("TEST"),
                culture = _localizer.CurrentLangWithCountry,
                lang = _localizer.CurrentLang
            });
        }

        [HttpGet("local_values")]
        public ActionResult List()
        {
            Dictionary<string, string> translations = _localizer.GetAll();
            return new JsonResult(translations);
        }

        [HttpGet("test_fb")]
        [AllowAnonymous]
        public async Task<ActionResult> TestFirebase()
        {
            var result = await _firebaseMessageSender.SendToTopic("Test", "Test", "test_topic",
                null, true);
            return new JsonResult(result);
        }

        [HttpGet("settings")]
        [AllowAnonymous]
        public ActionResult TestSettings()
        {
            var result = new
            {
                settings = _configuration.GetAppSettings(),
                connectionString = _config[KeyValueConstants.DbConnection]
            };
            return new JsonResult(result);
        }

        [HttpGet("check_file")]
        public async Task<ActionResult> CheckFile()
        {
            var templateFile = $"{_env.WebRootPath}" +
               $"/{KeyValueConstants.EmailTemplatesPath}" +
               $"/{_localizer.CurrentLang}" +
               $"/AccountVerification.cshtml";

            var file = await System.IO.File.ReadAllLinesAsync(templateFile);

            return Ok(file);
        }
    }

    public class PhoneNumber
    {
        public string Phone { get; set; }
        public string CountryCode { get; set; }
        public string CountryCodeNumber { get; set; }
    }

    public class LogObject
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string MessageTemplate { get; set; }
        public string Exception { get; set; }
        public LogProperties Properties { get; set; }
    }

    public class LogProperties
    {
        public string SourceContext { get; set; }
        public string ActionId { get; set; }
        public string ActionName { get; set; }
        public string RequestId { get; set; }
        public string RequestPath { get; set; }
        public string ConnectionId { get; set; }
        public string ApplicationName { get; set; }
    }

}
