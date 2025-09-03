# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **SolApp**, a .NET 8.0 console application that calculates and displays the current solar time as a percentage through the day or night period for a given location (default: Provo, Utah).

## Essential Commands

**Development:**
```bash
dotnet restore  # Restore NuGet packages
dotnet build    # Build the project
dotnet run      # Run the application
```

**Publishing:**
```bash
# Self-contained executable for Linux
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true
```

**Development Environment:**
```bash
nix develop  # Enter Nix development shell with .NET SDK 8
```

## Architecture

- **Single-file application**: All logic is contained in `Program.cs` using C# top-level program features
- **Core component**: `SolarTimeCalculator` static class handles all astronomical calculations
- **Data structure**: `SolarEvents` record holds sunrise/sunset times
- **External dependency**: Uses `SunCalcNet v1.2.3` NuGet package for solar calculations

## Key Technical Details

- **Location configuration**: Hardcoded coordinates in `DefaultLatitude` and `DefaultLongitude` constants in `Program.cs:9-10`
- **Timezone handling**: Application converts UTC times to local time; recent fixes addressed UTC timezone issues
- **Output format**: Returns percentage followed by 'd' (day) or 'n' (night)
- **No testing framework**: This is a simple utility without formal unit tests

## Application Logic Flow

1. Gets current date/time
2. Calculates sunrise/sunset for current day and adjacent days using SunCalcNet
3. Determines if current time falls during day or night period
4. Computes percentage progress through that solar period
5. Outputs result in format: `{percentage}d` or `{percentage}n`

## Configuration

To change the location, edit the constants in `Program.cs`:
```csharp
const double DefaultLatitude = 40.2338;  // Provo, Utah
const double DefaultLongitude = -111.6585;
```