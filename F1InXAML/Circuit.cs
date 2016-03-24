using System;

namespace F1InXAML
{
    public class Circuit
    {
        public Circuit(string id, string url, string name, Location location)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            Id = id;
            Url = url;
            Name = name;
            Location = location;
        }

        public string Id { get; }
        public string Url { get; }
        public string Name { get; }
        public Location Location { get; }
        
    }
}