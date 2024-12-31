namespace ClassLibrary.Tests.SharedContext;

/// <summary>
///     The <see cref="ICollectionFixture{TFixture}" /> of <see cref="ClassFixture" /> is available in all tests within the
///     same test collection with the name <see cref="CollectionName" />.
/// </summary>
[CollectionDefinition(CustomCollectionDefinition.CollectionName)]
public class CustomCollectionDefinition : ICollectionFixture<ClassFixture>
{
    public const string CollectionName = nameof(CustomCollectionDefinition.CollectionName);
}
