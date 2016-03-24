using System;
using System.Collections.Generic;
using System.Linq;

namespace F1InXAML
{
    public class Season
    {
        public Season(int year, string url, 
            IReadOnlyCollection<Race> calendar, 
            IReadOnlyCollection<DriverStanding> driverStandings,
            IReadOnlyCollection<ConstructorStanding> constructorStandings)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            Year = year;
            Url = url;
            DriverStandings = driverStandings;
            ConstructorStandings = constructorStandings;
            Calendar = calendar;
            IsInProgress = year == DateTime.Now.Year &&
                        Calendar.Select(r => r.Date).Union(new[] { DateTime.MinValue }).Max(dt => dt) >= DateTime.Now.Date;
            PreviousRace = Calendar.LastOrDefault(r => r.Date < DateTime.Now.Date);
            NextRace = Calendar.FirstOrDefault(r => r.Date >= DateTime.Now.Date);
        }

        public int Year { get; }

        public string Url { get; }

        public IReadOnlyCollection<DriverStanding> DriverStandings { get; }

        public IReadOnlyCollection<ConstructorStanding> ConstructorStandings { get; }

        public IReadOnlyCollection<Race> Calendar { get;  }

        public DriverStanding WinningDriver => DriverStandings.FirstOrDefault();

        public ConstructorStanding WinningConstructor => ConstructorStandings.FirstOrDefault();

        public bool IsInProgress { get; }

        public Race PreviousRace { get; }

        public Race NextRace { get; }
    }
}