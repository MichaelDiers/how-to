//
// example by xunit: https://github.com/xunit/samples.xunit/blob/main/v3/TestOrderExamples/TestCaseOrdering/PriorityOrderExamples.cs
//

namespace ClassLibrary.Tests.TestCaseOrder;

[TestCaseOrderer(typeof(PriorityOrderer))]
public class PriorityOrdererTests
{
    public static bool TestACalled;
    public static bool TestBCalled;
    public static bool TestCCalled;
    public static bool TestDCalled;

    [Fact]
    [TestPriority(4)]
    public void TestA()
    {
        PriorityOrdererTests.TestACalled = true;

        Assert.True(PriorityOrdererTests.TestBCalled);
        Assert.True(PriorityOrdererTests.TestCCalled);
        Assert.True(PriorityOrdererTests.TestDCalled);
    }

    [Fact]
    [TestPriority(1)]
    public void TestB()
    {
        PriorityOrdererTests.TestBCalled = true;

        Assert.False(PriorityOrdererTests.TestACalled);
        Assert.False(PriorityOrdererTests.TestCCalled);
        Assert.False(PriorityOrdererTests.TestDCalled);
    }

    [Fact]
    [TestPriority(3)]
    public void TestC()
    {
        PriorityOrdererTests.TestCCalled = true;

        Assert.False(PriorityOrdererTests.TestACalled);
        Assert.True(PriorityOrdererTests.TestBCalled);
        Assert.True(PriorityOrdererTests.TestDCalled);
    }

    [Fact]
    [TestPriority(2)]
    public void TestD()
    {
        PriorityOrdererTests.TestDCalled = true;

        Assert.False(PriorityOrdererTests.TestACalled);
        Assert.True(PriorityOrdererTests.TestBCalled);
        Assert.False(PriorityOrdererTests.TestCCalled);
    }
}
