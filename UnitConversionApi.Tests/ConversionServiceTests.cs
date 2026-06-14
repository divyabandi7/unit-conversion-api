using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Tests;

public class ConversionServiceTests
{
    private readonly ConversionService _sut = new();

    // ── Length ────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_MetersToFeet_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "meters", To = "feet", Value = 1 });
        Assert.Equal(3.28084, result.OutputValue, precision: 4);
        Assert.Equal("length", result.Category);
    }

    [Fact]
    public void Convert_KilometersToMiles_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "km", To = "mi", Value = 1 });
        Assert.Equal(0.621371, result.OutputValue, precision: 4);
    }

    // ── Weight ────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_KilogramsToPounds_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "kg", To = "lbs", Value = 1 });
        Assert.Equal(2.204624, result.OutputValue, precision: 4);
        Assert.Equal("weight", result.Category);
    }

    [Fact]
    public void Convert_GramsToOunces_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "g", To = "oz", Value = 100 });
        Assert.Equal(3.527399, result.OutputValue, precision: 3);
    }

    // ── Temperature ───────────────────────────────────────────────────────────

    [Fact]
    public void Convert_CelsiusToFahrenheit_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "celsius", To = "fahrenheit", Value = 0 });
        Assert.Equal(32, result.OutputValue);
        Assert.Equal("temperature", result.Category);
    }

    [Fact]
    public void Convert_FahrenheitToCelsius_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "f", To = "c", Value = 212 });
        Assert.Equal(100, result.OutputValue);
    }

    [Fact]
    public void Convert_CelsiusToKelvin_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "c", To = "k", Value = 0 });
        Assert.Equal(273.15, result.OutputValue);
    }

    [Fact]
    public void Convert_KelvinToCelsius_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "kelvin", To = "celsius", Value = 273.15 });
        Assert.Equal(0, result.OutputValue);
    }

    // ── Speed ─────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_KphToMph_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "km/h", To = "mph", Value = 100 });
        Assert.Equal(62.137, result.OutputValue, precision: 2);
    }

    // ── Area ──────────────────────────────────────────────────────────────────

    [Fact]
    public void Convert_HectaresToAcres_ReturnsCorrectValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "hectare", To = "acre", Value = 1 });
        Assert.Equal(2.471054, result.OutputValue, precision: 3);
    }

    // ── Aliases & case-insensitivity ──────────────────────────────────────────

    [Fact]
    public void Convert_UpperCaseUnitNames_AreAccepted()
    {
        var result = _sut.Convert(new ConversionRequest { From = "KG", To = "LBS", Value = 1 });
        Assert.Equal(2.204624, result.OutputValue, precision: 4);
    }

    [Fact]
    public void Convert_SameUnit_ReturnsSameValue()
    {
        var result = _sut.Convert(new ConversionRequest { From = "meters", To = "m", Value = 42 });
        Assert.Equal(42, result.OutputValue);
    }

    // ── Error cases ───────────────────────────────────────────────────────────

    [Fact]
    public void Convert_UnknownFromUnit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _sut.Convert(new ConversionRequest { From = "parsec", To = "meters", Value = 1 }));
    }

    [Fact]
    public void Convert_UnknownToUnit_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _sut.Convert(new ConversionRequest { From = "meters", To = "parsec", Value = 1 }));
    }

    [Fact]
    public void Convert_IncompatibleCategories_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() =>
            _sut.Convert(new ConversionRequest { From = "meters", To = "kg", Value = 1 }));
    }

    // ── GetSupportedUnits ─────────────────────────────────────────────────────

    [Fact]
    public void GetSupportedUnits_ReturnsNonEmptyList()
    {
        var units = _sut.GetSupportedUnits().ToList();
        Assert.NotEmpty(units);
    }

    [Fact]
    public void GetSupportedUnits_ContainsExpectedUnits()
    {
        var units = _sut.GetSupportedUnits().ToHashSet(StringComparer.OrdinalIgnoreCase);
        Assert.Contains("meters", units);
        Assert.Contains("celsius", units);
        Assert.Contains("kg", units);
    }
}
