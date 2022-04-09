using Application.Contracts;
using PhoneNumbers;

namespace Utilities
{
    public class PhoneValidator : IPhoneValidator
    {
        public string GetNationalPhoneNumberFormat(string phone, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            PhoneNumber parsedPhoneNumber;

            try
            {
                parsedPhoneNumber = phoneNumberUtil.Parse(phone, countryCode);
            }
            catch
            {
                return null;
            }

            var nationalFormat = phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.NATIONAL);

            return nationalFormat.Replace(" ", "");
        }

        public string GetInternationalPhoneNumberFormat(string phone, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            PhoneNumber parsedPhoneNumber;

            try
            {
                parsedPhoneNumber = phoneNumberUtil.Parse(phone, countryCode);
            }
            catch
            {
                return null;
            }

            var internationalFormat = phoneNumberUtil.Format(parsedPhoneNumber, PhoneNumberFormat.E164);

            return internationalFormat;
        }

        public bool IsValidPhoneNumber(string phone, string countryCode)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();

            PhoneNumber parsedPhoneNumber;

            try
            {
                parsedPhoneNumber = phoneNumberUtil.Parse(phone, countryCode);
            }
            catch
            {
                return false;
            }

            var isValidNumberForRegion = phoneNumberUtil.IsValidNumberForRegion(parsedPhoneNumber, countryCode);
            var numberType = phoneNumberUtil.GetNumberType(parsedPhoneNumber);

            return isValidNumberForRegion &&
                   (numberType == PhoneNumberType.FIXED_LINE_OR_MOBILE ||
                    numberType == PhoneNumberType.MOBILE ||
                    numberType == PhoneNumberType.FIXED_LINE);
        }
    }
}
