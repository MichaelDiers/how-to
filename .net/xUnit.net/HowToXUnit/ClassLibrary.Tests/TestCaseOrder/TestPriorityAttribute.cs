//
// example by xunit: https://github.com/xunit/samples.xunit/blob/main/v3/TestOrderExamples/TestCaseOrdering/TestPriorityAttribute.cs
//

namespace ClassLibrary.Tests.TestCaseOrder;

[AttributeUsage(AttributeTargets.Method)]
public class TestPriorityAttribute(int priority) : Attribute
{
    public int Priority { get; private set; } = priority;
}
