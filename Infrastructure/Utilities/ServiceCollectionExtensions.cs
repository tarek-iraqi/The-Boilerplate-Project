using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Application.Interfaces;
using FileSignatures;
using Helpers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Utilities.Extenstions;
using Utilities.Services;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUtilitiesServices(this IServiceCollection services, IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("System:EmailSettings").Get<EmailSettings>();
            var amazonSettings = configuration.GetSection("AmazonSettings").Get<AmazonSettings>();

            services.Configure<EmailSettings>(configuration.GetSection("System:EmailSettings"));
            services.Configure<ApiClient>(configuration.GetSection("System:ApiClients"));
            services.Configure<AppSettings>(configuration.GetSection("System"));
            services.Configure<Firebase_Settings>(configuration.GetSection("Firbase_Settings"));
            services.Configure<AmazonSettings>(configuration.GetSection("AmazonSettings"));

            services
                .AddFluentEmail(emailSettings.FromEmail)
                .AddRazorRenderer()
                .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.FromEmail, emailSettings.Password);

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddSingleton<IApplicationConfiguration, ApplicationConfiguration>();
            services.AddScoped<IPhoneValidator, PhoneValidator>();
            services.AddScoped<IApplicationLocalization, ApplicationLocalization>();
            services.AddScoped<IFirebaseMessageSender, FirebaseMessageSender>();
            services.AddScoped<IFileValidator, FileValidator>();
            services.AddScoped<IUpload, LocalServerUpload>();

            var assembly = typeof(VideoFormatFile).GetTypeInfo().Assembly;
            var allFormats = FileFormatLocator.GetFormats(assembly, true);
            var inspector = new FileFormatInspector(allFormats);
            services.AddSingleton<IFileFormatInspector>(inspector);

            var awsCredentials = new BasicAWSCredentials(amazonSettings.AWS_ACCESS_KEY_ID, amazonSettings.AWS_SECRET_ACCESS_KEY);
                        
            AWSOptions options = new AWSOptions { Credentials = awsCredentials, Region = RegionEndpoint.EUCentral1 };

            services.AddAWSService<IAmazonS3>(options);
        }
    }
}
