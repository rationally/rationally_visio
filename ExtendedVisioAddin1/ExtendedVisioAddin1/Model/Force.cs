namespace ExtendedVisioAddin1.Model
{
    public class Force
    {
        public string Concern { get; set; }
        public string Description { get; set; }

        public Force(string concern, string description)
        {
            Concern = concern;
            Description = description;
        }
    }
}
