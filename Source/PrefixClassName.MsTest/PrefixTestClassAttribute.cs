using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PrefixClassName.MsTest;

/// <summary>
/// Test class attribute that outputs the class name as part of the test name.
/// </summary>
public class PrefixTestClassAttribute : TestClassAttribute
{
    /// <summary>
    /// Gets a test method attribute that outputs the class name as part of the test name.
    /// </summary>
    public override TestMethodAttribute? GetTestMethodAttribute(TestMethodAttribute? testMethodAttribute)
    {
        var attribute = base.GetTestMethodAttribute(testMethodAttribute);
        return attribute == null ? null : new PrefixTestMethodAttribute(attribute);
    }
}