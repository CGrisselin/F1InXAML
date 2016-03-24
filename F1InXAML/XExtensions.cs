using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf.Transitions;

namespace F1InXAML
{
    internal static class XExtensions
    {
        public static string ElementValueOrNull(this XElement xElement, XName name)
        {
            var element = xElement.Element(name);
            return element?.Value;
        }

        public static XElement ElementOrThrow(this XElement xElement, XName name)
        {
            var element = xElement.Element(name);
            if (element == null) throw new ApplicationException($"No such element {name} found in {xElement}.");
            return element;
        }

        public static string ElementValueOrThrow(this XElement xElement, XName name)
        {
            var element = xElement.Element(name);
            if (element == null) throw new ApplicationException($"No such element {name} found in {xElement}.");
            return element.Value;
        }

        public static TResult ElementValueOrThrow<TResult>(this XElement xElement, XName name, Func<string, TResult> parse)
        {
            var element = xElement.Element(name);
            if (element == null) throw new ApplicationException($"No such element {name} found in {xElement}.");
            try
            {
                return parse(element.Value);
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Cannot parse {element.Value} into {typeof(TResult).FullName}.", exc);
            }
        }

        public static string AttributeValueOrNull(this XElement xElement, XName name)
        {
            var element = xElement.Attribute(name);
            return element?.Value;
        }

        public static string AttributeValueOrThrow(this XElement xElement, XName name)
        {
            var attr = xElement.Attribute(name);
            if (attr == null)
                throw new ApplicationException($"No such attribute {name} on element {xElement.Name}.");
            return attr.Value;
        }

        public static TResult AttributeValueOrThrow<TResult>(this XElement xElement, XName name, Func<string, TResult> parse)
        {
            var attr = xElement.Attribute(name);
            if (attr == null)
                throw new ApplicationException($"No such attribute {name} on element {xElement.Name}.");

            try
            {
                return parse(attr.Value);
            }
            catch (Exception exc)
            {
                throw new ApplicationException($"Cannot parse {attr.Value} into {typeof(TResult).FullName}.", exc);                
            }
        }

        public static SeasonHeader ToSeasonHeader(this XElement seasonElement)
        {
            return new SeasonHeader(Int32.Parse(seasonElement.Value), seasonElement.Attribute("url").Value);
        }

        public static Driver ToDriver(this XElement driverElement, XNamespace ns)
        {
            return new Driver(
                driverElement.Attribute("driverId").Value,
                driverElement.AttributeValueOrNull("code"),
                driverElement.AttributeValueOrNull("url"),
                driverElement.ElementValueOrNull(ns + "PermanentNumber"),
                driverElement.ElementValueOrNull(ns + "GivenName"),
                driverElement.ElementValueOrNull(ns + "FamilyName"),
                driverElement.ElementValueOrNull(ns + "DateOfBirth"),
                driverElement.ElementValueOrNull(ns + "Nationality"));
        }

        public static Constructor ToConstructor(this XElement driverElement, XNamespace ns)
        {
            return new Constructor(
                driverElement.Attribute("constructorId").Value,
                driverElement.AttributeValueOrNull("url"),
                driverElement.ElementValueOrNull(ns + "Name"),
                driverElement.ElementValueOrNull(ns + "Nationality"));
        }

        public static RaceResults ToRaceResults(this XElement raceElement, XNamespace ns)
        {
            if (raceElement == null) throw new ArgumentNullException(nameof(raceElement));
            if (ns == null) throw new ArgumentNullException(nameof(ns));

            var race = raceElement.ToRace(ns);
            var resultsListElement = raceElement.Element(ns + "ResultsList");
            var results = resultsListElement?
                .Elements(ns + "Result")
                .Select(resultElement => resultElement.ToResult(ns))
                .ToArray() ?? new Result[0];

            return new RaceResults(race, results);
        }

        public static Result ToResult(this XElement resultElement, XNamespace ns)
        {
            if (resultElement == null) throw new ArgumentNullException(nameof(resultElement));
            if (ns == null) throw new ArgumentNullException(nameof(ns));

            var fastestLapElement = resultElement.Element(ns + "FastestLap");
            var fastestLap = fastestLapElement?.ToFastestLap(ns);

            return new Result(
                resultElement.AttributeValueOrThrow("number", int.Parse),
                resultElement.AttributeValueOrThrow("position", int.Parse),
                resultElement.AttributeValueOrNull("positionText"),
                resultElement.AttributeValueOrThrow("points", double.Parse),
                resultElement.ElementOrThrow(ns + "Driver").ToDriver(ns),
                resultElement.ElementOrThrow(ns + "Constructor").ToConstructor(ns),
                resultElement.ElementValueOrThrow(ns + "Grid", int.Parse),
                resultElement.ElementValueOrThrow(ns + "Laps", int.Parse),
                resultElement.Element(ns+"Time").ToTimeSpan(),
                fastestLap);
        }

        public static FastestLap ToFastestLap(this XElement fastestLapElement, XNamespace ns)
        {
            if (fastestLapElement == null) throw new ArgumentNullException(nameof(fastestLapElement));
            if (ns == null) throw new ArgumentNullException(nameof(ns));
            //TimeSpan.TryParseExact("12:26.666", @"m\:ss\.fff", null, out ts);
            return new FastestLap(
                fastestLapElement.AttributeValueOrThrow("rank", int.Parse),
                fastestLapElement.AttributeValueOrThrow("lap", int.Parse),
                fastestLapElement.ElementOrThrow(ns + "Time").ToTimeSpan(),
                fastestLapElement.ElementOrThrow(ns + "AverageSpeed").ToSpeed(ns)
                );
        }

        public static TimeSpan ToTimeSpan(this XElement timeElement)
        {
            if (timeElement == null) return TimeSpan.Zero;

            var milliString = timeElement.AttributeValueOrNull("millis");
            if (milliString != null)
            {
                return TimeSpan.FromMilliseconds(double.Parse(milliString));
            }
            return TimeSpan.ParseExact(timeElement.Value, @"m\:ss\.fff", null);
        }

        public static Speed ToSpeed(this XElement speedElement, XNamespace ns)
        {
            if (speedElement == null) throw new ArgumentNullException(nameof(speedElement));
            if (ns == null) throw new ArgumentNullException(nameof(ns));

            return new Speed(
                speedElement.AttributeValueOrThrow("units"),
                double.Parse(speedElement.Value));
        }

        public static Race ToRace(this XElement raceElement, XNamespace ns)
        {
            return new Race(
                Int32.Parse(raceElement.Attribute("season").Value),
                Int32.Parse(raceElement.Attribute("round").Value),
                raceElement.Attribute("url").Value,
                raceElement.Element(ns + "RaceName").Value, 
                ToCircuit(raceElement.Element(ns + "Circuit"), ns), 
                ExtractDateTime(raceElement, ns));
        }

        public static DateTime ExtractDateTime(this XElement element, XNamespace ns)
        {
            var dateStr = element.Element(ns + "Date").Value;
            var timeStr = element.ElementValueOrNull(ns + "Time") ?? "00:00:00";

            return DateTime.Parse($"{dateStr}T{timeStr}");
        }

        public static Circuit ToCircuit(this XElement circuitElement, XNamespace ns)
        {
            return new Circuit(
                circuitElement.Attribute("circuitId").Value,
                circuitElement.Attribute("url").Value,
                circuitElement.Element(ns + "CircuitName").Value, ToLocation(circuitElement.Element(ns + "Location"), ns)
                );
        }

        public static Location ToLocation(this XElement locationElement, XNamespace ns)
        {
            return new Location(
                Double.Parse(locationElement.Attribute("lat").Value),
                Double.Parse(locationElement.Attribute("long").Value),
                locationElement.ElementValueOrNull(ns + "Locality"),
                locationElement.ElementValueOrNull(ns + "Country")
                );
        }
    }
}