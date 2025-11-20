# PrefixClassName.MsTest

[![Chat on Discord](https://img.shields.io/discord/906246067773923490)](https://discord.gg/EkQhJFsBu6)
[![View nuget packages](https://img.shields.io/nuget/v/PrefixClassName.MsTest.svg)](https://www.nuget.org/packages/PrefixClassName.MsTest/)

**PrefixClassName.MsTest** provides a `[PrefixTestClass]` attribute that can be used in place of `[TestClass]` to prefix the test name with the class name in MSTest v2+ for easier debugging when reading console or CI test results.

If the test classes are all in the same namespace then the namespace is omitted. If the test classes are nested in different namespaces, it will output only the part of the namespace that differs between the classes.

When tests are run from Visual Studio Test Explorer it does not modify the test name since Test Explorer can group the tests by namespace and class name. Running the tests with `dotnet test` or `vstest.console` shows the class name prefixes in the console output.

### About Singulink

We are a small team of engineers and designers dedicated to building beautiful, functional, and well-engineered software solutions. We offer very competitive rates as well as fixed-price contracts and welcome inquiries to discuss any custom development / project support needs you may have.

Visit https://github.com/Singulink to see our full list of publicly available libraries and other open-source projects.

## Version Info

PrefixTestClass v2 is compatible with MSTest v4+. Use PrefixTestClass v1 for earlier versions of MSTest.

## Installation

The package is available on NuGet - simply install the `PrefixClassName.MsTest` package.

**Supported Runtimes**: Anywhere .NET Standard 2.0+ is supported, including:
- .NET
- .NET Framework
- Mono
- Xamarin

## Usage

Replace all usages of `[TestClass]` with `[PrefixTestClass]` in your MSTest test classes:

```cs
[PrefixTestClass]
public class MyTestClass
{
    [TestMethod]
    public void MyTestMethod()
    {
        // Test code here
    }
}
```

You can ban usage of `[TestClass]` to prevent accidental usage of the non-prefixed attribute by adding [`BannedSymbols.txt`](Resources/BannedSymbols.txt) to the test project and the following to the `.csproj` file:

```xml
<ItemGroup>
  <PackageReference Include="Microsoft.CodeAnalysis.BannedApiAnalyzers" PrivateAssets="All" />
  <AdditionalFiles Include="BannedSymbols.txt" />
</ItemGroup>
```

See [Banned API Analyzers](https://github.com/dotnet/roslyn/blob/main/src/RoslynAnalyzers/Microsoft.CodeAnalysis.BannedApiAnalyzers/BannedApiAnalyzers.Help.md) documentation for more details.


## Sample Output

```
  Passed [MoneySetTests.TryGetValueTests] GetByCurrency_CurrencyDisallowed_ThrowsArgumentException [< 1 ms]
  Passed [MoneySetTests.TryGetValueTests] GetByCurrencyCode_CurrencyExists_ReturnsTrueAndOutputsValue [< 1 ms]
  Passed [MoneySetTests.TryGetValueTests] GetByCurrencyCode_ValueDoesNotExist_ReturnsFalse [< 1 ms]
  Passed [MoneySetTests.TryGetValueTests] GetByCurrencyCode_CurrencyDisallowed_ThrowsArgumentException [< 1 ms]
  Passed [MoneyTests.CompareToTests] LessThan_MinusOneResult [< 1 ms]
  Passed [MoneyTests.CompareToTests] GreaterThan_PlusOneResult [< 1 ms]
  Passed [MoneyTests.CompareToTests] Equal_ZeroResult [< 1 ms]
  Passed [MoneyTests.OperatorTests] Equal_EqualValues_ReturnsTrue [< 1 ms]
  Passed [MoneyTests.OperatorTests] Equal_DifferentValues_ReturnsFalse [< 1 ms]
  Passed [ImmutableMoneySetTests.AddTests] AddMoney_CurrencyExists_UpdatesValue [2 ms]
  Passed [ImmutableMoneySetTests.AddTests] AddMoney_NewCurrency_AddsValue [< 1 ms]
  Passed [ImmutableMoneySetTests.AddTests] AddMoney_DefaultValue_NoChange [< 1 ms]
```