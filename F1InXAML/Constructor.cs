using System;

namespace F1InXAML
{
    public class Constructor
    {
        public Constructor(string id, string url, string name, string nationality)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));

            Id = id;
            Url = url;
            Name = name;
            Nationality = nationality;
        }

        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
    }
}