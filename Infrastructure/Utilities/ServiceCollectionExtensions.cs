using Application.Interfaces;
using Helpers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Mail;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void  AddUtilitiesServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("System:EmailSettings").Get<EmailSettings>();

            services.Configure<EmailSettings>(configuration.GetSection("System:EmailSettings"));
            services.Configure<ApiClient>(configuration.GetSection("System:ApiClients"));
            services.Configure<AppSettings>(configuration.GetSection("System"));
            services.Configure<Firebase_Settings>(configuration.GetSection("Firbase_Settings"));

            services
                .AddFluentEmail(emailSettings.FromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.FromEmail, emailSettings.Password);

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddScoped<IPhoneValidator, PhoneValidator>();
            services.AddScoped<IApplicationLocalization, ApplicationLocalization>();
            services.AddScoped<IFirebaseMessageSender, FirebaseMessageSender>();
        }
    }
}
