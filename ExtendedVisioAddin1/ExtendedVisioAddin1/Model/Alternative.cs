namespace ExtendedVisioAddin1.Model
{
    public class Alternative
    {
        public static int HighestTimelessId = -1;

        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string Identifier { get; set; }

        public int TimelessId { get; set; }

        public Alternative(string title, string status, string description, string identifier, int timelessId)
        {
            Title = title;
            Status = status;
            Description = description;
            Identifier = identifier;
            TimelessId = timelessId;
            if (timelessId > HighestTimelessId)
            {
                HighestTimelessId = timelessId;
            }
        }
    }
}
