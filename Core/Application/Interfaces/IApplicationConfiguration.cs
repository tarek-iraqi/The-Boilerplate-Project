using Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
