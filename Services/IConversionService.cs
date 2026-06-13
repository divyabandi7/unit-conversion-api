using UnitConversionApi.Models;

namespace UnitConversionApi.Services;

public interface IConversionService
{
    ConversionResult Convert(ConversionRequest request);
    IEnumerable<string> GetSupportedUnits();
}