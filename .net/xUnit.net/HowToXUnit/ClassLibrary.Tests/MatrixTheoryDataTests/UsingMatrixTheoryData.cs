namespace ClassLibrary.Tests.MatrixTheoryDataTests;

public class UsingMatrixTheoryData
{
    // generates 3 * 5 * 2 = 24 different test data sets
    public static TheoryData<int, string, Data> TestData = new MatrixTheoryData<int, string, Data>(
        [1, 2, 3],
        ["foo", "bar", "baz", "foobar"],
        [new Data("name1"), new Data("name2")]);

    [Theory]
    [MemberData(nameof(UsingMatrixTheoryData.TestData))]
    public void Test(int value, string message, Data data)
    {
        Assert.True(value > 0);
        Assert.NotNull(message);
        Assert.NotNull(data.Name);
    }
}
