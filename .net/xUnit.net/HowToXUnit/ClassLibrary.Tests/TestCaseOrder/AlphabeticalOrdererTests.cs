//
// Example by xunit: https://github.com/xunit/samples.xunit/blob/main/v3/TestOrderExamples/TestCaseOrdering/AlphabeticalOrderExample.cs
//

namespace ClassLibrary.Tests.TestCaseOrder;

[TestCaseOrderer(typeof(AlphabeticalOrderer))]
public class AlphabeticalOrdererTests
{
    private static bool test1Called;
    private static bool test2Called;
    private static bool test3Called;

    [Fact]
    public void Test1()
    {
        AlphabeticalOrdererTests.test1Called = true;

        Assert.False(AlphabeticalOrdererTests.test2Called);
        Assert.False(AlphabeticalOrdererTests.test3Called);
    }

    [Fact]
    public void Test2()
    {
        AlphabeticalOrdererTests.test2Called = true;

        Assert.True(AlphabeticalOrdererTests.test1Called);
        Assert.False(AlphabeticalOrdererTests.test3Called);
    }

    [Fact]
    public void Test3()
    {
        AlphabeticalOrdererTests.test3Called = true;

        Assert.True(AlphabeticalOrdererTests.test1Called);
        Assert.True(AlphabeticalOrdererTests.test2Called);
    }
}
