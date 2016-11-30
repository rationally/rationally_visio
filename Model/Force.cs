using System.Collections.Generic;
using Rationally.Visio.View.Forces;

namespace Rationally.Visio.Model
{
    public class Force
    {
        public string Concern { get; set; }
        public string Description { get; set; }

        public Dictionary<int, string> ForceValueDictionary { get; set; } //key is the unique identifier for a alternative

        public Force() : this(ForceConcernComponent.DefaultConcern, ForceDescriptionComponent.DefaultDescription) { }

        public Force(string concern, string description)
        {
            Concern = concern;
            Description = description;
            ForceValueDictionary = new Dictionary<int, string>();
        }

        public Force(string concern, string description, Dictionary<int, string> forceValues )
        {
            Concern = concern;
            Description = description;
            ForceValueDictionary = forceValues;
        }
    }
}
