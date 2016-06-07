using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    public class RModel
    {
        public List<Alternative> Alternatives { get; set; }

        public List<RelatedDocument> Documents { get; set; }
        public List<string> AlternativeStates { get; set; }

        public string Author { get; set; }

        public string DecisionName { get; set; }

        public string Date { get; set; }

        public string Version { get; set; }

        public List<Force> Forces { get; set; }

        public RModel()
        {
            Alternatives = new List<Alternative>();
            Documents = new List<RelatedDocument>();
            Forces = new List<Force>();
            AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"};
        }

        
    }
}
