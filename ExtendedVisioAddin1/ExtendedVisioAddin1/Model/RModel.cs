using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    public class RModel
    {
        public Document RationallyDocument { get; }
        public List<Alternative> Alternatives { get; set; }
        public List<string> AlternativeStates { get; set; } 

        public string Author { get; set; }

        public string DecisionName { get; set; }

        public string Date { get; set; }

        public string Version { get; set; }

        public RModel()
        {
            Alternatives = new List<Alternative>();
            AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"};
            string docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\DecisionsStencil.vssx";
            this.RationallyDocument = Globals.ThisAddIn.Application.Documents.OpenEx(docPath,
((short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visAddHidden)); //todo: handling for file is open
        }
    }
}
