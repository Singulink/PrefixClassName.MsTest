# PrefixClassName.MsTest

[![Chat on Discord](https://img.shields.io/discord/906246067773923490)](https://discord.gg/EkQhJFsBu6)
[![View nuget packages](https://img.shields.io/nuget/v/PrefixClassName.MsTest.svg)](https://www.nuget.org/packages/PrefixClassName.MsTest/)

**PrefixClassName.MsTest** provides a `[PrefixTestClass]` attribute that can be used in place of `[TestClass]` to prefix the test name with the class name in MSTest v2+ for easier debugging when reading console or CI test results.

If the test classes are all in the same namespace then the namespace is ommited. If the test classes are nested in different namespaces, it will output only the part of the namespace that differs between the classes.

When tests are run from Visual Studio Test Explorer it does not modify the test name since Test Explorer can group the tests by namespace and class name.

### About Singulink

We are a small team of engineers and designers dedicated to building beautiful, functional, and well-engineered software solutions. We offer very competitive rates as well as fixed-price contracts and welcome inquiries to discuss any custom development / project support needs you may have.

This package is part of our **Singulink Libraries** collection. Visit https://github.com/Singulink to see our full list of publicly available libraries and other open-source projects.

## Installation

The package is available on NuGet - simply install the `Singulink.Reflection.ObjectFactory` package.

**Supported Runtimes**: Anywhere .NET Standard 2.0+ is supported, including:
- .NET
- .NET Framework
- Mono
- Xamarin

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

## API

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