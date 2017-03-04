using System;
using System.Reflection;
using log4net;
using Newtonsoft.Json;

namespace Rationally.Visio.Model
{
    public class Stakeholder
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static int highestId = -1;
        public string Name { get; set; }
        public string Role { get; set; }

        public int Id { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of the stakeholder

        [JsonConstructor]
        private Stakeholder()
        {
        }



        public Stakeholder(string name, string role)
        {
            Name = name;
            Role = role;
            Id = ++highestId;
        }

        public Stakeholder(string name, string role, int id)
        {
            Name = name;
            Role = role;
            Id = id;
            if (id > highestId)
            {
                highestId = id;
            }
        }
    }
}
