namespace UnitConversionApi.Models;

public class ConversionRequest
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public double Value { get; set; }
}

public class ConversionResult
{
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public double InputValue { get; set; }
    public double OutputValue { get; set; }
    public string Category { get; set; } = string.Empty;
}

public class ErrorResponse
{
    public string Error { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
}