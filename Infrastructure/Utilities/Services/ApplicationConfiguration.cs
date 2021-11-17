using Application.Interfaces;
using Helpers.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Utilities
{
    public class ApplicationConfiguration : IApplicationConfiguration
    {
        private readonly AppSettings _appSettings;
        private readonly Firebase_Settings _fbOptions;
        private readonly IOptions<AmazonSettings> _amazonOptions;

        public ApplicationConfiguration(IOptions<AppSettings> options,
            IOptions<Firebase_Settings> fbOptions,
            IOptions<AmazonSettings> amazonOptions)
        {
            _appSettings = options.Value;
            _fbOptions = fbOptions.Value;
            _amazonOptions = amazonOptions;
        }
        public AppSettings GetAppSettings()
        {
            return _appSettings;
        }

        public IEnumerable<ApiClient> GetApiClients()
        {
            return _appSettings.ApiClients;
        }

        public JWTSettings GetJwtSettings()
        {
            return _appSettings.JWTSettings;
        }

        public Firebase_Settings GetFirebaseSettings()
        {
            return _fbOptions;
        }

        public AmazonSettings GetAmazonSettings()
        {
            return _amazonOptions.Value;
        }
    }
}
