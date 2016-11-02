namespace Rationally.Visio.Model
{
    public class Alternative
    {
        private static int highestUniqueIdentifier = -1;

        public string Status { get; set; }

        public string Title { get; set; }

        public string IdentifierString { get; private set; }

        public int UniqueIdentifier { get;} //Id that exists independent of the order of the elements. Allows for the identifying of alternatives
        
        public Alternative(string title, string status)
        {
            Title = title;
            Status = status;
            UniqueIdentifier = ++highestUniqueIdentifier;
        }
        public Alternative(string title, string status, int uniqueIdentifier)
        {
            Title = title;
            Status = status;
            UniqueIdentifier = uniqueIdentifier;
            if (uniqueIdentifier > highestUniqueIdentifier)
            {
                highestUniqueIdentifier = uniqueIdentifier;
            }
        }

        public void GenerateIdentifier(int identNumber)
        {
            char identChar = (char)(65 + identNumber); //convert to corresponding letter
            IdentifierString = identChar + ":";
        }
    }
}
