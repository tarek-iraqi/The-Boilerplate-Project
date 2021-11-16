using Domain.Entities;
using Helpers.Exceptions;
using Shouldly;
using Xunit;

namespace BoilerPlate.Testing.Domain
{
    public class ProductTest
    {
        [Fact]
        public void CreateNewProductTest()
        {
            var product = new Product();
            product.Create("product1", "description", "123456", 1);

            Assert.NotNull(product.Name);
            Assert.NotNull(product.Description);
            Assert.NotNull(product.Barcode);
            Assert.NotEqual(0, product.Rate);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(11)]
        public void ProductRateNotValidTest(decimal rate)
        {
            var product = new Product();

            Assert.Throws<AppCustomException>(() => product.Create("product1", "description", "123456", rate));
        }

        [Fact]
        public void UpdateProductTest()
        {
            var product = new Product();
            product.Create("product1", "description", "123456", 1);

            product.Update("product2", "description22");

            product.Name.ShouldBe("product2");
            product.Description.ShouldBe("description22");
        }
    }
}
