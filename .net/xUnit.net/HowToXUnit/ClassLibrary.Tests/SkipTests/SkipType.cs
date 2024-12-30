namespace ClassLibrary.Tests.SkipTests;

public partial class UsingSkip
{
    public class SkipType
    {
        public static bool SkipTypeConditionIsFalse => false;

        public static bool SkipTypeConditionIsTrue => true;
    }
}
