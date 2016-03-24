using System;

namespace F1InXAML
{
    public class ConstructorStanding
    {
        public ConstructorStanding(Constructor constructor, int position, double points, int wins)
        {
            if (constructor == null) throw new ArgumentNullException(nameof(constructor));

            Constructor = constructor;
            Position = position;
            Points = points;
            Wins = wins;
        }

        public Constructor Constructor { get; }
        public int Position { get; }
        public double Points { get; }
        public int Wins { get; }
    }
}