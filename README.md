# Unit Conversion API

I built this as a simple API to convert between different units of measurement. You send it a value, tell it what unit you are converting from and to, and it gives you the result.

## How to Run

You will need .NET SDK installed on your machine first. Then open your terminal and run:

git clone https://github.com/divyabandi7/unit-conversion-api.git

cd unit-conversion-api

dotnet run --no-launch-profile

Once it starts you will see: Now listening on http://localhost:5000

## Try It Out

Open your browser and paste any of these:

http://localhost:5000/api/convert?from=meters&to=feet&value=10

http://localhost:5000/api/convert?from=celsius&to=fahrenheit&value=100

http://localhost:5000/api/convert?from=kg&to=lbs&value=70

http://localhost:5000/api/convert?from=km/h&to=mph&value=100

http://localhost:5000/api/convert?from=acres&to=hectares&value=5

http://localhost:5000/api/convert/units

## Expected Results

meters to feet = 32.8084
celsius to fahrenheit = 212
kg to lbs = 154.3234

## Supported Units

Length: meters, km, feet, miles, inches, cm, yards, mm
Temperature: celsius, fahrenheit, kelvin
Weight: kg, pounds, grams, ounces, tonnes
Speed: km/h, mph, m/s, knots
Area: sqm, sqft, acres, hectares

## Project Structure

Controllers - handles incoming requests
Models - defines request and response structure
Services - contains all conversion logic
Program.cs - wires everything together

## Notes

Adding a new unit is easy. Just add one line to the dictionary in ConversionService.cs with the unit name and conversion factor.

Temperature uses formula based conversion because Celsius, Fahrenheit and Kelvin have different zero points.