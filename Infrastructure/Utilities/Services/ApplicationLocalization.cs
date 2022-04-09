using Application.Contracts;
using Helpers.Resources;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Utilities
{
    public class ApplicationLocalization : IApplicationLocalization
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ApplicationLocalization(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }
        public string Get(string key, params string[] placeholderValues)
        {
            return _localizer[key, placeholderValues].Value;
        }

        public Dictionary<string, string> GetAll()
        {
            return _localizer.GetAllStrings().ToDictionary(x => x.Name, x => x.Value);
        }

        public string CurrentLang => string.IsNullOrWhiteSpace(Thread.CurrentThread.CurrentUICulture.Parent.Name) ?
           Thread.CurrentThread.CurrentUICulture.Name : Thread.CurrentThread.CurrentUICulture.Parent.Name;


        public string CurrentLangWithCountry => Thread.CurrentThread.CurrentUICulture.Name;
    }
}
