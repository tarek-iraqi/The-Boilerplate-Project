using BoilerPlate.Testing._shared;
using Xunit;

namespace BoilerPlate.Testing.Application
{
    [Collection("AppCollection")]
    public class UserFeaturesTest
    {
        private readonly ApplicationLocalizationFixture _localizationFixture;

        public UserFeaturesTest(ApplicationLocalizationFixture localizationFixture)
        {
            _localizationFixture = localizationFixture;
        }

        //[Fact]
        //public void IsValidUserDataTest()
        //{
        //    Mock<IPhoneValidator> phoneValidator = new Mock<IPhoneValidator>();

        //    Register.CommandValidator validator = new Register.CommandValidator(_localizationFixture.localizer,
        //        phoneValidator.Object);

        //    Register.Command command = new Register.Command
        //    {
        //        first_name = "Tarek",
        //        last_name = "Iraqi",
        //        email = "tarek.iraqi@gmail.com",
        //        password = "zaq12wsx",
        //        password_confirmation = "zaq12wsx"
        //    };

        //    var result = validator.TestValidate(command);

        //    result.IsValid.ShouldBe(true);
        //}

        //[Fact]
        //public void IsNotValidUserEmailTest()
        //{
        //    Mock<IPhoneValidator> phoneValidator = new Mock<IPhoneValidator>();

        //    Register.CommandValidator validator = new Register.CommandValidator(_localizationFixture.localizer,
        //        phoneValidator.Object);

        //    Register.Command command = new Register.Command
        //    {
        //        first_name = "Tarek",
        //        last_name = "Iraqi",
        //        email = "tarek.iraqigmail.com",
        //        password = "zaq12wsx",
        //        password_confirmation = "zaq12wsx"
        //    };

        //    var result = validator.TestValidate(command);

        //    result.ShouldHaveValidationErrorFor(p => p.email);
        //}

        //[Fact]
        //public void NotMatchPaswwordTest()
        //{
        //    Mock<IPhoneValidator> phoneValidator = new Mock<IPhoneValidator>();

        //    Register.CommandValidator validator = new Register.CommandValidator(_localizationFixture.localizer,
        //        phoneValidator.Object);

        //    Register.Command command = new Register.Command
        //    {
        //        first_name = "Tarek",
        //        last_name = "Iraqi",
        //        email = "tarek.iraqi@gmail.com",
        //        password = "zaq12wsx",
        //        password_confirmation = "zaq12wsxc"
        //    };

        //    var result = validator.TestValidate(command);

        //    result.ShouldHaveValidationErrorFor(p => p.password_confirmation);
        //}
    }
}
