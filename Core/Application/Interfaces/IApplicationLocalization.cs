using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationLocalization
    {
        string Get(string key, params string[] placeholderValues);
        Dictionary<string, string> GetAll();
        string CurrentLang { get; }
        string CurrentLangWithCountry { get; }
    }
}
