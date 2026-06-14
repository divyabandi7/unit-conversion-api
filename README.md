# Unit Conversion API

A REST API built with ASP.NET Core that converts numerical values between different units of measurement — e.g., meters to feet, Celsius to Fahrenheit, kilograms to pounds.

---

## How to Run

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
git clone https://github.com/dk18234/unit-conversion-api.git
cd unit-conversion-api
dotnet run
```

The API starts at `http://localhost:5259`.

Swagger UI (interactive docs) is available at: `http://localhost:5259/swagger`

---

## Endpoints

| Method | Path | Description |
|--------|------|-------------|
| GET | `/api/convert?from=X&to=Y&value=N` | Convert a value via query string |
| POST | `/api/convert` | Convert a value via JSON body |
| GET | `/api/convert/units` | List all supported unit identifiers |

**GET example:**
```
GET /api/convert?from=meters&to=feet&value=10
```

**POST example:**
```json
POST /api/convert
{ "from": "kg", "to": "lbs", "value": 70 }
```

**Successful response:**
```json
{
  "from": "meters",
  "to": "feet",
  "inputValue": 10,
  "outputValue": 32.808399,
  "category": "length"
}
```

---

## Supported Categories

| Category | Example units |
|----------|---------------|
| Length | `m`, `km`, `cm`, `mm`, `ft`, `in`, `yd`, `mi` |
| Temperature | `celsius`, `fahrenheit`, `kelvin` (also `c`, `f`, `k`) |
| Weight | `kg`, `g`, `mg`, `lb`, `lbs`, `oz`, `ton` |
| Speed | `km/h`, `mph`, `m/s`, `knots` |
| Area | `sqm`, `sqkm`, `sqft`, `acre`, `hectare` |

Run `GET /api/convert/units` for the full list of accepted identifiers.

---

## Project Structure

```
Controllers/   → HTTP request handling
Models/        → Request/response shapes
Services/      → Conversion logic (interface + implementation)
Program.cs     → App bootstrap and DI registration
```

---

## Design Decisions & Trade-offs

### Dictionary-based conversion with a single base unit per category
Each unit is stored as a `(category, toBase)` pair in a case-insensitive dictionary. To convert, the value is multiplied by the source's `toBase` factor and divided by the target's — one formula covers all linear units. This makes adding a new unit a one-line change and keeps the logic free of per-unit branching.

**Trade-off:** the dictionary lives in-memory and is hardcoded. For a system with hundreds of units updated at runtime, this data would move to a database with the same conversion math unchanged — only the data source changes, not the formula.

### Temperature as a special case
Celsius, Fahrenheit, and Kelvin have offset scales, so a single linear factor does not work. These are routed to a two-step formula (→ Celsius → target) instead. The dictionary still holds them so that category validation and "unsupported unit" errors remain consistent; the `ToBase` field is unused for this category.

### GET and POST both supported
A GET with query parameters is the most natural fit for a conversion — it is a read operation, bookmarkable, and works directly from a browser. POST is added for clients that prefer a JSON body. Both share the same service call with no duplicated logic.

### Singular and plural aliases
`meter`/`meters`, `foot`/`feet`, `pound`/`pounds`, etc., are registered as separate keys pointing to the same factor. This avoids forcing callers to know the canonical form and makes the API more forgiving.

### Stateless singleton service
`ConversionService` holds no mutable state — only the unit dictionary — so it is safe to register as a singleton, avoiding per-request allocation on every API call.
