using System;
using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class Alternative
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestUniqueIdentifier = -1;
        //All properties must be public and have a getter and setter;
        public string Status { get; set; }

        public string Title { get; set; }

        public string IdentifierString { get; set; }

        public int UniqueIdentifier { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of alternatives


        [JsonConstructor]
        private Alternative()
        {
        }


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
