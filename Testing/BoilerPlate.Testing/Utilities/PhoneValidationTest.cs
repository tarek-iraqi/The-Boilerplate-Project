using Shouldly;
using Utilities;
using Xunit;

namespace BoilerPlate.Testing.Utilities;

public class PhoneValidationTest
{
    [Theory]
    [InlineData("01003776205", "EG")]
    [InlineData("1003776205", "EG")]
    [InlineData("+201003776205", "EG")]
    [InlineData("00201003776205", "EG")]
    public void IsValidPhoneNumberTest(string phone, string countryCode)
    {
        PhoneValidator phoneValidator = new PhoneValidator();
        var result = phoneValidator.IsValidPhoneNumber(phone, countryCode);
        result.ShouldBe(true);
    }

    [Theory]
    [InlineData("01003776205", "SA")]
    [InlineData("+21003776205", "EG")]
    [InlineData("0021003776205", "EG")]
    public void IsNotValidPhoneNumberTest(string phone, string countryCode)
    {
        PhoneValidator phoneValidator = new PhoneValidator();
        var result = phoneValidator.IsValidPhoneNumber(phone, countryCode);
        result.ShouldBe(false);
    }
}