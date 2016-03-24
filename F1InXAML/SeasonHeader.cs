namespace F1InXAML
{
    public class SeasonHeader
    {
        public int Year { get; }
        public string Url { get; }

        public SeasonHeader(int year, string url)
        {
            Year = year;
            Url = url;
        }
    }
}