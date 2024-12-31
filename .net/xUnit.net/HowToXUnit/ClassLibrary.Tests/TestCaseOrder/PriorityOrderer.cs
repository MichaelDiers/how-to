//
// example by xunit: https://github.com/xunit/samples.xunit/blob/main/v3/TestOrderExamples/TestCaseOrdering/PriorityOrderer.cs
//

namespace ClassLibrary.Tests.TestCaseOrder;

using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;

public class PriorityOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var result = new List<TTestCase>();
        var sortedMethods = new SortedDictionary<int, List<IXunitTestCase>>();

        foreach (IXunitTestCase testCase in testCases)
        {
            var priority = 0;
            var attr = testCase.TestMethod.Method.GetCustomAttributes<TestPriorityAttribute>().FirstOrDefault();
            if (attr is not null)
            {
                priority = attr.Priority;
            }

            PriorityOrderer.GetOrCreate(
                    sortedMethods,
                    priority)
                .Add(testCase);
        }

        foreach (var list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
        {
            list.Sort(
                (x, y) => StringComparer.OrdinalIgnoreCase.Compare(
                    x.TestMethod.Method.Name,
                    y.TestMethod.Method.Name));
            foreach (var xunitTestCase in list)
            {
                if (xunitTestCase is TTestCase tTestCase)
                {
                    result.Add(tTestCase);
                }
            }
        }

        return result;
    }

    private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
    {
        if (dictionary.TryGetValue(
                key,
                out var result))
        {
            return result;
        }

        result = new TValue();
        dictionary[key] = result;

        return result;
    }
}
