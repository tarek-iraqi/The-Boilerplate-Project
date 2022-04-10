using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Application.Contracts;
using DinkToPdf;
using DinkToPdf.Contracts;
using FileSignatures;
using Helpers.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using Utilities.Extenstions;
using Utilities.Services;

namespace Utilities
{
    public static class ServiceCollectionExtensions
    {
        public static void AddUtilitiesServices(this IServiceCollection services, IConfiguration configuration,
            IWebHostEnvironment env)
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
            services.AddScoped<IExcelOperations, ExcelOperations>();
            services.AddScoped<IPDFOperations, PDFOperations>();
            services.AddScoped<IImageOperations, ImageOperations>();

            var assembly = typeof(VideoFormatFile).GetTypeInfo().Assembly;
            var allFormats = FileFormatLocator.GetFormats(assembly, true);
            var inspector = new FileFormatInspector(allFormats);
            services.AddSingleton<IFileFormatInspector>(inspector);

            var awsCredentials = new BasicAWSCredentials(amazonSettings.AWS_ACCESS_KEY_ID, amazonSettings.AWS_SECRET_ACCESS_KEY);

            AWSOptions options = new AWSOptions { Credentials = awsCredentials, Region = RegionEndpoint.EUCentral1 };

            services.AddAWSService<IAmazonS3>(options);

            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";
            var wkHtmlToPdfPath = Path.Combine(env.ContentRootPath, $"webkitengine/v0.12.4/{architectureFolder}/libwkhtmltox");
            CustomAssemblyLoadContext context = new CustomAssemblyLoadContext();
            context.LoadUnmanagedLibrary(wkHtmlToPdfPath);
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }
    }
}
