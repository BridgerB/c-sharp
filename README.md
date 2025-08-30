# Solar Time Calculator

A simple CLI tool that outputs the current solar time as a percentage through the day or night period.

## Output Format

- `{percentage}d` - Percentage through the day (sunrise to sunset)
- `{percentage}n` - Percentage through the night (sunset to sunrise)
issue
## Usage

```bash
dotnet run
# Output: 94d (94% through the day)
```

## Build

```bash
# Development build
dotnet build

# Self-contained executable
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true
./bin/Release/net8.0/linux-x64/publish/SolApp
```

## Dependencies

- .NET 8.0
- SunCalcNet - Solar calculation library

## Location

Currently configured for Provo, Utah (40.2338°N, -111.6585°W). Edit the constants in `Program.cs` to change location.