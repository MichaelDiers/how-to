namespace ClassLibrary.Tests.ClassDataTests;

public class UsingClassData
{
    [Theory]
    [ClassData(typeof(DataProviderArrayOfObject))]
    public void ArrayOfObject(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data);
    }

    [Theory]
    [ClassData(typeof(DataProviderArrayOfObjectAsync))]
    public void ArrayOfObjectAsync(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data);
    }

    [Theory]
    [ClassData(typeof(DataProviderITheoryDataRow))]
    public void TheoryDataRow(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data);
    }

    [Theory]
    [ClassData(typeof(DataProviderITheoryDataRowAsync))]
    public void TheoryDataRowAsync(string message, int value, Data data)
    {
        Assert.NotNull(message);
        Assert.True(value > 0);
        Assert.NotNull(data);
    }
}
