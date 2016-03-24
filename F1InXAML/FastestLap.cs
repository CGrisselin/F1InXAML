using System;

namespace F1InXAML
{
    public class FastestLap
    {
        public FastestLap(int rank, int lap, TimeSpan time, Speed averageSpeed)
        {
            Rank = rank;
            Lap = lap;
            Time = time;
            AverageSpeed = averageSpeed;
        }

        public int Rank { get; }
        public int Lap { get; }
        public TimeSpan Time { get; }
        public Speed AverageSpeed { get; }
    }
}