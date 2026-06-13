# Unit Conversion API

I built this as a simple API to convert between different units of measurement. You send it a value, tell it what unit you are converting from and to, and it gives you the result. That is really all it does.

For example you can ask it to convert 10 meters to feet, or 100 degrees Celsius to Fahrenheit, or 70 kilograms to pounds.

## How to Run It

You need .NET SDK installed first. Then:

1. Clone this repo
2. Open a terminal and go into the UnitConversionApi folder
3. Type this and hit Enter:

dotnet run

4. Once it starts you will see a localhost address in the terminal. Mine runs on http://localhost:5259

## Trying It Out

Just open your browser and paste one of these:

http://localhost:5259/api/convert?from=meters&to=feet&value=10

http://localhost:5259/api/convert?from=celsius&to=fahrenheit&value=100

http://localhost:5259/api/convert?from=kg&to=lbs&value=70

## What Units Are Supported

Length: meters, km, feet, miles, inches, cm, yards, mm
Temperature: celsius, fahrenheit, kelvin
Weight: kg, pounds, grams, ounces, tonnes
Speed: km/h, mph, m/s, knots
Area: sqm, sqft, acres, hectares

## How the Code is Organised

Controllers - this is where requests come in and responses go out
Models - just the data structures
Services - all the actual conversion logic lives here
Program.cs just wires it all together when the app starts

## A Couple of Things Worth Mentioning

Adding a new unit is really straightforward. You just add one line to the dictionary in ConversionService.cs with the unit name and its conversion factor.

Temperature works a bit differently from the other units. Because Celsius, Fahrenheit and Kelvin all have different zero points you cannot just multiply by a factor, so I handle those with proper formulas instead.
