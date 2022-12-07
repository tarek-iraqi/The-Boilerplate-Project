using System.Collections.Generic;

namespace Application.Contracts;

public interface IApplicationLocalization
{
    string Get(string key, params string[] placeholderValues);
    Dictionary<string, string> GetAll();
    string CurrentLang { get; }
    string CurrentLangWithCountry { get; }
}