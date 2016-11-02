namespace Rationally.Visio.Model
{
    public class Alternative
    {
        public static int HighestUniqueIdentifier = -1;

        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public string IdentifierString { get; set; }

        public int UniqueIdentifier { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of alternatives

        public Alternative(string title, string status, string description, string identifierString, int uniqueIdentifier)
        {
            Title = title;
            Status = status;
            Description = description;
            IdentifierString = identifierString;
            UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier > HighestUniqueIdentifier)
            {
                HighestUniqueIdentifier = uniqueIdentifier;
            }
        }

        public void GenerateIdentifier(int identNumber)
        {
            char identChar = (char)(65 + identNumber); //convert to corresponding letter
            IdentifierString = identChar + ":";
        }
    }
}
