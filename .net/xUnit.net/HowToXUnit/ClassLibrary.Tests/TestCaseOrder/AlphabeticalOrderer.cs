//
// Example by xunit: https://github.com/xunit/samples.xunit/blob/main/v3/TestOrderExamples/TestCaseOrdering/AlphabeticalOrderer.cs
//

namespace ClassLibrary.Tests.TestCaseOrder;

using Xunit.Sdk;
using Xunit.v3;

public class AlphabeticalOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var result = testCases.Cast<IXunitTestCase>().ToList();
        result.Sort(
            (x, y) => StringComparer.OrdinalIgnoreCase.Compare(
                x.TestMethod.Method.Name,
                y.TestMethod.Method.Name));
        return result.Cast<TTestCase>().ToArray();
    }
}
