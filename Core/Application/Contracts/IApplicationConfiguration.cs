using Helpers.BaseModels;
using System.Collections.Generic;

namespace Application.Contracts;

public interface IApplicationConfiguration
{
    AppSettings GetAppSettings();
    IEnumerable<ApiClient> GetApiClients();
    JWTSettings GetJwtSettings();
    Firebase_Settings GetFirebaseSettings();
    AmazonSettings GetAmazonSettings();
}