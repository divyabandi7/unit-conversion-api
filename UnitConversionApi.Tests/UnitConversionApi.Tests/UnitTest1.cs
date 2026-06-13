using FluentAssertions;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Tests;

public class ConversionServiceTests
{
    private readonly ConversionService _sut = new();

    [Fact]
    public void Convert_MetersToFeet_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "meters", To = "feet", Value = 1 });
        result.OutputValue.Should().BeApproximately(3.28084, precision: 0.0001);
    }

    [Fact]
    public void Convert_CelsiusToFahrenheit_BoilingPoint()
    {
        var result = _sut.Convert(new ConversionRequest { From = "celsius", To = "fahrenheit", Value = 100 });
        result.OutputValue.Should().BeApproximately(212, precision: 0.001);
    }

    [Fact]
    public void Convert_CelsiusToFahrenheit_FreezingPoint()
    {
        var result = _sut.Convert(new ConversionRequest { From = "c", To = "f", Value = 0 });
        result.OutputValue.Should().BeApproximately(32, precision: 0.001);
    }

    [Fact]
    public void Convert_KilogramsToPounds_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "kg", To = "lbs", Value = 1 });
        result.OutputValue.Should().BeApproximately(2.20462, precision: 0.0001);
    }

    [Fact]
    public void Convert_KilometersToMiles_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "km", To = "miles", Value = 1 });
        result.OutputValue.Should().BeApproximately(0.621371, precision: 0.0001);
    }

    [Fact]
    public void Convert_UnknownUnit_ThrowsArgumentException()
    {
        var act = () => _sut.Convert(new ConversionRequest { From = "lightyears", To = "meters", Value = 1 });
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Convert_IncompatibleCategories_ThrowsInvalidOperationException()
    {
        var act = () => _sut.Convert(new ConversionRequest { From = "kg", To = "meters", Value = 5 });
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Convert_Result_IncludesCorrectCategory()
    {
        var result = _sut.Convert(new ConversionRequest { From = "kg", To = "lbs", Value = 1 });
        result.Category.Should().Be("weight");
    }
}