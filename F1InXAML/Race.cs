using System;
using System.Net;
using System.Runtime.CompilerServices;

namespace F1InXAML
{
    public class Race
    {
        public Race(int season, int round, string url, string name, Circuit circuit, DateTime date)
        {
            Season = season;
            Round = round;
            Url = url;
            Name = name;
            Circuit = circuit;
            Date = date;
        }

        public int Season { get; }
        public int Round { get; }
        public string Url { get; }
        public string Name { get; }
        public Circuit Circuit { get; }
        public DateTime Date { get; }        
    }
}