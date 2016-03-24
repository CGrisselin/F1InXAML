namespace F1InXAML
{
    public class Status
    {
        public Status(int id, string description)
        {
            Id = id;
            Description = description;
        }

        public int Id { get; }

        public string Description { get; }        
    }
}