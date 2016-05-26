namespace ExtendedVisioAddin1.Model
{
    public class Alternative
    {
        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string Identifier { get; set; }

        public Alternative(string title, string status, string description, string identifier)
        {
            Title = title;
            Status = status;
            Description = description;
            Identifier = identifier;
        }
    }
}
