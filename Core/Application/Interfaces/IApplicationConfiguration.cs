using Helpers.Models;
using System.Collections.Generic;

namespace Application.Interfaces
{
    public interface IApplicationConfiguration
    {
        AppSettings GetAppSettings();
        IEnumerable<ApiClient> GetApiClients();
        JWTSettings GetJwtSettings();
        Firebase_Settings GetFirebaseSettings();
    }
}
