namespace PrefixClassName.MsTest;

/// <summary>
/// Test class attribute that outputs the class name as part of the test name.
/// </summary>
public class PrefixTestClassAttribute : TestClassAttribute
{
    /// <summary>
    /// Gets a test method attribute that outputs the class name as part of the test name.
    /// </summary>
    public override TestMethodAttribute? GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
    {
        var attribute = base.GetTestMethodAttribute(testMethodAttribute);

#pragma warning disable MSTEST0056 // TestMethodAttribute should set DisplayName correctly
        return attribute is null ? null : new PrefixTestMethodAttribute(attribute, attribute.DeclaringFilePath, attribute.DeclaringLineNumber ?? -1);
#pragma warning restore MSTEST0056
    }
}