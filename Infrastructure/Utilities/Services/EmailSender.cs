using Application.Interfaces;
using FluentEmail.Core;
using Helpers.Constants;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly IFluentEmail _email;
        private readonly IFluentEmailFactory _emailFactory;
        private readonly IApplicationLocalization _localization;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IFluentEmail email,
            IFluentEmailFactory emailFactory,
            IApplicationLocalization localization,
            ILogger<EmailSender> logger)
        {
            _email = email;
            _emailFactory = emailFactory;
            _localization = localization;
            _logger = logger;
        }
        public async Task SendSingleEmail(string to, string subject, string message, string from = null)
        {
            var email = _email.To(to).Subject(subject).Body(message);

            if (!string.IsNullOrWhiteSpace(from))
                email.SetFrom(from);

            try
            {
                await email.SendAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

        public Task SendSingleEmail(string to, string subject, string templateFileName, object model, string from = null)
        {
            var templateFile = $"{Directory.GetCurrentDirectory()}" +
                $"/{KeyValueConstants.EmailTemplatesPath}" +
                $"/{_localization.CurrentLang}" +
                $"/{templateFileName}";

            var email = _email.To(to).Subject(subject)
                .UsingTemplateFromFile(templateFile, model);

            if (!string.IsNullOrWhiteSpace(from))
                email.SetFrom(from);

            try
            {
                email.SendAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Task.CompletedTask;
        }

        public Task SendMultipleEmails(string[] to, string subject, string message, string from = null)
        {
            foreach (var emailTo in to)
            {
                var email = _emailFactory.Create()
                    .To(emailTo).Subject(subject).Body(message);

                if (!string.IsNullOrWhiteSpace(from))
                    email.SetFrom(from);

                try
                {
                    email.SendAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return Task.CompletedTask;
        }

        public Task SendMultipleEmails(string[] to, string subject, string templateFileName, object model, string from = null)
        {
            var templateFile = $"{Directory.GetCurrentDirectory()}" +
                $"/{KeyValueConstants.EmailTemplatesPath}" +
                $"/{_localization.CurrentLang}" +
                $"/{templateFileName}";

            foreach (var emailTo in to)
            {

                var email = _emailFactory.Create()
                    .To(emailTo).Subject(subject)
                    .UsingTemplateFromFile(templateFile, model);

                if (!string.IsNullOrWhiteSpace(from))
                    email.SetFrom(from);

                try
                {
                    email.SendAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }

            return Task.CompletedTask;
        }
    }
}
