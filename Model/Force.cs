using System.Collections.Generic;
using System.Reflection;
using log4net;
using Newtonsoft.Json;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.Model
{
    public class Force
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string Concern { get; set; }
        public string Description { get; set; }
        public int Id { get; set; } //Id that exists independent of the order of the elements. Allows for the identifying of forces

        private static int highestId = -1;

        public Dictionary<int, string> ForceValueDictionary { get; set; } //key is the unique identifier for a alternative
        [JsonConstructor]
        private Force(){ }

        public Force(string concern, string description)
        {
            Concern = concern;
            Description = description;
            ForceValueDictionary = new Dictionary<int, string>();
            Id = ++highestId;
        }

        public Force(string concern, string description, Dictionary<int, string> forceValues, int id )
        {
            Concern = concern;
            Description = description;
            ForceValueDictionary = forceValues;
            if (id > highestId)
            {
                highestId = id;
            }
        }
    }
}
