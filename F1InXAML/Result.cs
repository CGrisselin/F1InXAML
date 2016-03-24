using System;

namespace F1InXAML
{
    public class Result
    {
        public Result(int number, int position, string positionText, double points, Driver driver, Constructor constructor, int grid, int laps, TimeSpan time, FastestLap fastestLap)
        {
            Number = number;
            Position = position;
            PositionText = positionText;
            Points = points;
            Driver = driver;
            Constructor = constructor;
            Grid = grid;
            Laps = laps;
            Time = time;
            FastestLap = fastestLap;
        }

        public int Number { get; }

        public int Position { get; }

        public string PositionText { get; }

        public double Points { get; }

        public Driver Driver { get; }

        public Constructor Constructor { get; }

        public int Grid { get; }

        public int Laps { get; }

        public TimeSpan Time { get; }

        public FastestLap FastestLap { get; }

    }
}