using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public class ConversionService : IConversionService
{
    private readonly Dictionary<string, (string Category, double ToBase)> _units = new(StringComparer.OrdinalIgnoreCase)
    {
        // Length — base: meter
        { "meter",      ("length", 1.0) },
        { "meters",     ("length", 1.0) },
        { "m",          ("length", 1.0) },
        { "kilometer",  ("length", 1000.0) },
        { "kilometers", ("length", 1000.0) },
        { "km",         ("length", 1000.0) },
        { "centimeter", ("length", 0.01) },
        { "centimeters",("length", 0.01) },
        { "cm",         ("length", 0.01) },
        { "millimeter", ("length", 0.001) },
        { "millimeters",("length", 0.001) },
        { "mm",         ("length", 0.001) },
        { "mile",       ("length", 1609.344) },
        { "miles",      ("length", 1609.344) },
        { "mi",         ("length", 1609.344) },
        { "foot",       ("length", 0.3048) },
        { "feet",       ("length", 0.3048) },
        { "ft",         ("length", 0.3048) },
        { "inch",       ("length", 0.0254) },
        { "inches",     ("length", 0.0254) },
        { "in",         ("length", 0.0254) },
        { "yard",       ("length", 0.9144) },
        { "yards",      ("length", 0.9144) },
        { "yd",         ("length", 0.9144) },

        // Weight — base: kilogram
        { "kilogram",   ("weight", 1.0) },
        { "kilograms",  ("weight", 1.0) },
        { "kg",         ("weight", 1.0) },
        { "gram",       ("weight", 0.001) },
        { "grams",      ("weight", 0.001) },
        { "g",          ("weight", 0.001) },
        { "milligram",  ("weight", 0.000001) },
        { "milligrams", ("weight", 0.000001) },
        { "mg",         ("weight", 0.000001) },
        { "pound",      ("weight", 0.453592) },
        { "pounds",     ("weight", 0.453592) },
        { "lb",         ("weight", 0.453592) },
        { "lbs",        ("weight", 0.453592) },
        { "ounce",      ("weight", 0.0283495) },
        { "ounces",     ("weight", 0.0283495) },
        { "oz",         ("weight", 0.0283495) },
        { "ton",        ("weight", 1000.0) },
        { "tons",       ("weight", 1000.0) },
        { "tonne",      ("weight", 1000.0) },
        { "tonnes",     ("weight", 1000.0) },

        // Temperature — handled by formula, ToBase unused
        { "celsius",    ("temperature", 0) },
        { "c",          ("temperature", 0) },
        { "fahrenheit", ("temperature", 0) },
        { "f",          ("temperature", 0) },
        { "kelvin",     ("temperature", 0) },
        { "k",          ("temperature", 0) },

        // Speed — base: km/h
        { "km/h",       ("speed", 1.0) },
        { "kmh",        ("speed", 1.0) },
        { "kph",        ("speed", 1.0) },
        { "mph",        ("speed", 1.60934) },
        { "m/s",        ("speed", 3.6) },
        { "ms",         ("speed", 3.6) },
        { "knot",       ("speed", 1.852) },
        { "knots",      ("speed", 1.852) },

        // Area — base: square meter
        { "sqm",        ("area", 1.0) },
        { "m2",         ("area", 1.0) },
        { "sqkm",       ("area", 1_000_000.0) },
        { "km2",        ("area", 1_000_000.0) },
        { "sqft",       ("area", 0.092903) },
        { "ft2",        ("area", 0.092903) },
        { "sqmi",       ("area", 2_589_988.0) },
        { "mi2",        ("area", 2_589_988.0) },
        { "acre",       ("area", 4046.86) },
        { "acres",      ("area", 4046.86) },
        { "hectare",    ("area", 10_000.0) },
        { "hectares",   ("area", 10_000.0) },
        { "ha",         ("area", 10_000.0) },
    };

    public ConversionResult Convert(ConversionRequest request)
    {
        var fromKey = request.From.Trim().ToLower();
        var toKey   = request.To.Trim().ToLower();

        if (!_units.TryGetValue(fromKey, out var fromInfo))
            throw new ArgumentException($"Unknown unit: '{request.From}'. Use GET /api/convert/units to see supported units.");

        if (!_units.TryGetValue(toKey, out var toInfo))
            throw new ArgumentException($"Unknown unit: '{request.To}'. Use GET /api/convert/units to see supported units.");

        if (fromInfo.Category != toInfo.Category)
            throw new InvalidOperationException(
                $"Cannot convert between '{request.From}' ({fromInfo.Category}) and '{request.To}' ({toInfo.Category}).");

        double outputValue = fromInfo.Category == "temperature"
            ? ConvertTemperature(request.Value, fromKey, toKey)
            : request.Value * fromInfo.ToBase / toInfo.ToBase;

        return new ConversionResult
        {
            From        = request.From,
            To          = request.To,
            InputValue  = request.Value,
            OutputValue = Math.Round(outputValue, 6),
            Category    = fromInfo.Category
        };
    }

    public IEnumerable<string> GetSupportedUnits()
    {
        return _units.Keys.Order();
    }

    private static double ConvertTemperature(double value, string from, string to)
    {
        double celsius = from switch
        {
            "celsius" or "c"    => value,
            "fahrenheit" or "f" => (value - 32) * 5.0 / 9.0,
            "kelvin" or "k"     => value - 273.15,
            _ => throw new ArgumentException($"Unknown temperature unit: {from}")
        };

        return to switch
        {
            "celsius" or "c"    => celsius,
            "fahrenheit" or "f" => celsius * 9.0 / 5.0 + 32,
            "kelvin" or "k"     => celsius + 273.15,
            _ => throw new ArgumentException($"Unknown temperature unit: {to}")
        };
    }
}