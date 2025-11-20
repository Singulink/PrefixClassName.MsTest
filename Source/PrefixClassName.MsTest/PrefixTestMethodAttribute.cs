namespace PrefixClassName.MsTest;

internal class PrefixTestMethodAttribute : TestMethodAttribute
{
    private readonly TestMethodAttribute _testMethodAttribute;

#pragma warning disable MSTEST0057 // TestMethodAttribute derived class should propagate source information

    internal PrefixTestMethodAttribute(TestMethodAttribute testMethodAttribute, string callerFilePath, int callerLineNumber)
        : base(callerFilePath, callerLineNumber)
    {
        _testMethodAttribute = testMethodAttribute;
    }

#pragma warning restore MSTEST0057

    public override async Task<TestResult[]> ExecuteAsync(ITestMethod testMethod)
    {
        var results = await _testMethodAttribute.ExecuteAsync(testMethod);

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