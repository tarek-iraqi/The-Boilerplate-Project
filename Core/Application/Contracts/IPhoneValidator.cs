namespace Application.Contracts
{
    public interface IPhoneValidator
    {
        bool IsValidPhoneNumber(string phone, string countryCode);
        string GetNationalPhoneNumberFormat(string phone, string countryCode);
        string GetInternationalPhoneNumberFormat(string phone, string countryCode);
    }
}
