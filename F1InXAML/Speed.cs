namespace F1InXAML
{
    public class Speed
    {
        public Speed(string unit, double value)
        {
            Unit = unit;
            Value = value;
        }

        public string Unit { get; }

        public double Value { get; }
    }
}