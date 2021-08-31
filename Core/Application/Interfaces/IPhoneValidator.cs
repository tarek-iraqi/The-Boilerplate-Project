using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IPhoneValidator
    {
        bool IsValidPhoneNumber(string phone, string countryCode);
        string GetNationalPhoneNumberFormat(string phone, string countryCode);
        string GetInternationalPhoneNumberFormat(string phone, string countryCode);
    }
}
