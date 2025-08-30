using System;
using System.Linq;
using SunCalcNet;

const double DefaultLatitude = 40.2338;
const double DefaultLongitude = -111.6585;

var result = SolarTimeCalculator.GetCurrentSolarTime(DefaultLatitude, DefaultLongitude);
Console.WriteLine(result);

public static class SolarTimeCalculator
{
    public static string GetCurrentSolarTime(double latitude, double longitude)
    {
        return CalculateSolarTime(DateTime.Now, latitude, longitude);
    }

    private static string CalculateSolarTime(DateTime currentTime, double latitude, double longitude)
    {
        var solarEvents = GetSolarEvents(currentTime, latitude, longitude);
        var currentTimeMs = new DateTimeOffset(currentTime).ToUnixTimeMilliseconds();
        
        var (percentage, period) = DetermineSolarPeriod(currentTimeMs, solarEvents);
        
        return $"{Math.Floor(percentage)}{period}";
    }

    private static SolarEvents GetSolarEvents(DateTime date, double latitude, double longitude)
    {
        var today = date.ToUniversalTime().Date;
        var tomorrow = today.AddDays(1);

        var todayPhases = SunCalc.GetSunPhases(today, latitude, longitude, 0).ToList();
        var tomorrowPhases = SunCalc.GetSunPhases(tomorrow, latitude, longitude, 0).ToList();

        var sunrise = GetPhaseTime(todayPhases, "Sunrise");
        var sunset = GetPhaseTime(todayPhases, "Sunset");
        var yesterdayPhases = SunCalc.GetSunPhases(today.AddDays(-1), latitude, longitude, 0).ToList();
        var previousSunset = GetPhaseTime(yesterdayPhases, "Sunset");
        var nextSunrise = GetPhaseTime(tomorrowPhases, "Sunrise");

        return new SolarEvents(sunrise, sunset, previousSunset, nextSunrise);
    }

    private static DateTime GetPhaseTime(System.Collections.Generic.List<SunCalcNet.Model.SunPhase> phases, string phaseName)
    {
        var phase = phases.First(p => p.Name.ToString() == phaseName).PhaseTime;
        return phase.Kind == DateTimeKind.Utc ? phase.ToLocalTime() : phase;
    }

    private static (double percentage, char period) DetermineSolarPeriod(long currentTimeMs, SolarEvents events)
    {
        var sunriseMs = new DateTimeOffset(events.Sunrise).ToUnixTimeMilliseconds();
        var sunsetMs = new DateTimeOffset(events.Sunset).ToUnixTimeMilliseconds();
        var previousSunsetMs = new DateTimeOffset(events.PreviousSunset).ToUnixTimeMilliseconds();
        var nextSunriseMs = new DateTimeOffset(events.NextSunrise).ToUnixTimeMilliseconds();

        if (currentTimeMs < sunriseMs)
        {
            return (CalculatePercentage(currentTimeMs, previousSunsetMs, sunriseMs), 'n');
        }
        
        if (currentTimeMs < sunsetMs)
        {
            return (CalculatePercentage(currentTimeMs, sunriseMs, sunsetMs), 'd');
        }
        
        return (CalculatePercentage(currentTimeMs, sunsetMs, nextSunriseMs), 'n');
    }

    private static double CalculatePercentage(long current, long start, long end)
    {
        var duration = (double)(end - start);
        var elapsed = (double)(current - start);
        return (elapsed / duration * 100.0) % 100.0;
    }

    private record SolarEvents(DateTime Sunrise, DateTime Sunset, DateTime PreviousSunset, DateTime NextSunrise);
}
