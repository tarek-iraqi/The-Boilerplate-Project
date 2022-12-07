using Xunit;

namespace BoilerPlate.Testing._shared;

[CollectionDefinition("AppCollection")]
public class AppCollection : ICollectionFixture<ApplicationLocalizationFixture>
{
}