using Microsoft.AspNetCore.Mvc;
using UnitConversionApi.Models;
using UnitConversionApi.Services;

namespace UnitConversionApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConvertController : ControllerBase
{
    private readonly IConversionService _conversionService;
    private readonly ILogger<ConvertController> _logger;

    public ConvertController(IConversionService conversionService, ILogger<ConvertController> logger)
    {
        _conversionService = conversionService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Convert([FromQuery] string from, [FromQuery] string to, [FromQuery] double? value)
    {
        if (string.IsNullOrWhiteSpace(from))
            return BadRequest(new ErrorResponse { Error = "Missing parameter", Details = "'from' is required. Example: from=meters" });

        if (string.IsNullOrWhiteSpace(to))
            return BadRequest(new ErrorResponse { Error = "Missing parameter", Details = "'to' is required. Example: to=feet" });

        if (value is null)
            return BadRequest(new ErrorResponse { Error = "Missing parameter", Details = "'value' is required. Example: value=10" });

        try
        {
            var request = new ConversionRequest { From = from, To = to, Value = value.Value };
            var result = _conversionService.Convert(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid unit: {Message}", ex.Message);
            return BadRequest(new ErrorResponse { Error = "Invalid unit", Details = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Incompatible units: {Message}", ex.Message);
            return BadRequest(new ErrorResponse { Error = "Incompatible units", Details = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            return StatusCode(500, new ErrorResponse { Error = "Server error", Details = "An unexpected error occurred." });
        }
    }

    [HttpPost]
    public IActionResult ConvertPost([FromBody] ConversionRequest request)
    {
        if (request is null)
            return BadRequest(new ErrorResponse { Error = "Invalid body", Details = "Request body is required." });

        if (string.IsNullOrWhiteSpace(request.From))
            return BadRequest(new ErrorResponse { Error = "Missing field", Details = "'from' is required." });

        if (string.IsNullOrWhiteSpace(request.To))
            return BadRequest(new ErrorResponse { Error = "Missing field", Details = "'to' is required." });

        try
        {
            var result = _conversionService.Convert(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse { Error = "Invalid unit", Details = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ErrorResponse { Error = "Incompatible units", Details = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            return StatusCode(500, new ErrorResponse { Error = "Server error", Details = "An unexpected error occurred." });
        }
    }

    [HttpGet("units")]
    public IActionResult GetSupportedUnits()
    {
        return Ok(_conversionService.GetSupportedUnits());
    }
}