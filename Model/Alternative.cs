using System;
using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class Alternative
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestId = -1;
        //All properties must be public and have a getter and setter;
        public string Status { get; set; }

        public string Title { get; set; }

        public string IdentifierString { get; set; }

        public int Id { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of alternatives


        [JsonConstructor]
        private Alternative()
        {
        }


        public Alternative(string title, string status)
        {
            Title = title;
            Status = status;
            Id = ++highestId;
        }
        public Alternative(string title, string status, int id)
        {
            Title = title;
            Status = status;
            Id = id;
            if (id > highestId)
            {
                highestId = id;
            }
        }

        public void GenerateIdentifier(int identNumber)
        {
            char identChar = (char)(65 + identNumber); //convert to corresponding letter
            IdentifierString = identChar + ":";
        }
    }
}
