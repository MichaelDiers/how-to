namespace ClassLibrary.Tests;

public class UsingSkip(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void Skip()
    {
        // do some tests

        Assert.Skip("Test is skipped here");

        Assert.Fail("Test should not reach");
    }

    [Fact]
    public void SkipUnless()
    {
        Assert.SkipUnless(
            true,
            "Test continues");

        testOutputHelper.WriteLine("test should pass here");

        Assert.SkipUnless(
            false,
            "Test is skipped here");

        Assert.Fail("Test should not reach");
    }

    [Fact]
    public void SkipWhen()
    {
        Assert.SkipWhen(
            false,
            "Test continues");

        Assert.SkipWhen(
            true,
            "Test is skipped here");

        Assert.Fail("Test should not reach");
    }
}
