using System;
using System.Collections.Generic;

namespace F1InXAML
{
    public class RaceResults
    {
        public RaceResults(Race race, IReadOnlyCollection<Result> results)
        {
            if (race == null) throw new ArgumentNullException(nameof(race));
            if (results == null) throw new ArgumentNullException(nameof(results));

            Race = race;
            Results = results;
        }

        public Race Race { get;  }

        public IReadOnlyCollection<Result> Results { get; }
    }
}