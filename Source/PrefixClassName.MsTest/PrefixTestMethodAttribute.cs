using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrefixClassName.MsTest;

internal class PrefixTestMethodAttribute : TestMethodAttribute
{
    private readonly TestMethodAttribute _testMethodAttribute;

    internal PrefixTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        _testMethodAttribute = testMethodAttribute;
    }

    public override TestResult[] Execute(ITestMethod testMethod)
    {
        var results = _testMethodAttribute.Execute(testMethod);

        if (TestInfo.RunningInTestExplorer)
            return results;

        foreach (var result in results)
        {
            string className = testMethod.TestClassName;
            string commonPrefix = TestInfo.GetCommonPrefix(testMethod.MethodInfo.DeclaringType!.Assembly);

            if (className.StartsWith(commonPrefix))
                className = className[commonPrefix.Length..];

            result.DisplayName = $"[{className}] {result.DisplayName ?? testMethod.TestMethodName}";
        }

        return results;
    }
}