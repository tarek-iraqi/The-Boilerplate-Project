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
        public ApplicationConfiguration(IOptions<AppSettings> options,
            IOptions<Firebase_Settings> fbOptions)
        {
            _appSettings = options.Value;
            _fbOptions = fbOptions.Value;
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
    }
}
