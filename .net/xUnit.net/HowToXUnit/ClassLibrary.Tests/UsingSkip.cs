namespace ClassLibrary.Tests;

public class UsingSkip(ITestOutputHelper testOutputHelper)
{
    public static bool ConditionIsFalse => false;

    public static bool ConditionIsTrue => true;

    [Fact]
    public void Skip()
    {
        // do some tests

        Assert.Skip("Test should be skipped here.");

        Assert.Fail("Test should be skipped before.");
    }

    [Fact]
    public void SkipUnless()
    {
        Assert.SkipUnless(
            true,
            "Test should continue.");

        testOutputHelper.WriteLine("Test should pass here.");

        Assert.SkipUnless(
            false,
            "Test should be skipped here.");

        Assert.Fail("Test should be skipped before.");
    }

    [Fact(
        SkipUnless = nameof(UsingSkip.ConditionIsFalse),
        Skip = "Test should be skipped.")]
    public void SkipUnlessConditionIsFalseThereforeTestIsSkipped()
    {
        Assert.Fail("Test should be skipped before.");
    }

    [Fact(
        SkipUnless = nameof(UsingSkip.ConditionIsTrue),
        Skip = "Test should run.")]
    public void SkipUnlessConditionIsTrueThereforeTestRuns()
    {
        // do some tests
    }

    [Fact(
        SkipUnless = nameof(SkipType.SkipTypeConditionIsFalse),
        SkipType = typeof(SkipType),
        Skip = "Test should be skipped.")]
    public void SkipUnlessUsingSkipTypeConditionIsFalseThereforeTestIsSkipped()
    {
        Assert.Fail("Test should be skipped before.");
    }

    [Fact(
        SkipUnless = nameof(SkipType.SkipTypeConditionIsTrue),
        SkipType = typeof(SkipType),
        Skip = "Test should run.")]
    public void SkipUnlessUsingSkipTypeConditionIsTrueThereforeTestRuns()
    {
        // do some tests
    }

    [Fact]
    public void SkipWhen()
    {
        Assert.SkipWhen(
            false,
            "Test should continue.");

        testOutputHelper.WriteLine("Test should pass here.");

        Assert.SkipWhen(
            true,
            "Test should be skipped here.");

        Assert.Fail("Test should be skipped before.");
    }

    [Fact(
        SkipWhen = nameof(UsingSkip.ConditionIsFalse),
        Skip = "Test should run.")]
    public void SkipWhenConditionIsFalseThereforeTestRuns()
    {
        // do some tests
    }

    [Fact(
        SkipWhen = nameof(UsingSkip.ConditionIsTrue),
        Skip = "Test should be skipped.")]
    public void SkipWhenConditionIsTrueThereforeTestIsSkipped()
    {
        Assert.Fail("Test should be skipped before.");
    }

    [Fact(
        SkipWhen = nameof(SkipType.SkipTypeConditionIsFalse),
        SkipType = typeof(SkipType),
        Skip = "Test should run.")]
    public void SkipWhenUsingSkipTypeConditionIsFalseThereforeTestRuns()
    {
        // do some tests
    }

    [Fact(
        SkipWhen = nameof(SkipType.SkipTypeConditionIsTrue),
        SkipType = typeof(SkipType),
        Skip = "Test should be skipped.")]
    public void SkipWhenUsingSkipTypeConditionIsTrueThereforeTestIsSkipped()
    {
        Assert.Fail("Test should be skipped before.");
    }

    public class SkipType
    {
        public static bool SkipTypeConditionIsFalse => false;

        public static bool SkipTypeConditionIsTrue => true;
    }
}
