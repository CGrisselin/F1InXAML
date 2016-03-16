using System.Collections.Generic;
using System.Linq;

namespace F1InXAML
{
    public class Season
    {
        public Season(int year, IReadOnlyCollection<DriverStanding> driverStandings, IReadOnlyCollection<ConstructorStanding> constructorStandings)
        {
            Year = year;
            DriverStandings = driverStandings;
            ConstructorStandings = constructorStandings;
        }

        public int Year { get; }

        public IReadOnlyCollection<DriverStanding> DriverStandings { get; }

        public IReadOnlyCollection<ConstructorStanding> ConstructorStandings { get; }

        public DriverStanding WinningDriver => DriverStandings.First();

        public ConstructorStanding WinningConstructor => ConstructorStandings.First();
    }
}