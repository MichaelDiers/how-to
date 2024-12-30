namespace ClassLibrary.Tests;

public class UsingExplicit
{
    [Fact(Explicit = true)]
    public void ExplicitTestShouldNotRun()
    {
        // the test explorer starts this test if only this test is selected
        // cli flag exists
        Assert.Fail("Should not run");
    }

    [Fact(Explicit = false)]
    public void NonExplicitTestShouldRun()
    {
        // do some tests
    }
}
