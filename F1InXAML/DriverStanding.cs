using System;

namespace F1InXAML
{
    public class DriverStanding
    {        
        public DriverStanding(Driver driver, int position, int points, int wins)
        {
            if (driver == null) throw new ArgumentNullException(nameof(driver));            

            Driver = driver;
            Position = position;
            Points = points;
            Wins = wins;
        }

        public Driver Driver { get; }
        public int Position { get; }
        public int Points { get; }
        public int Wins { get; }
    }
}