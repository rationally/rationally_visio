using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization.Formatters.Binary;
using log4net;
using Newtonsoft.Json;
using Rationally.Visio.RationallyConstants;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Rationally.Visio.View.Planning;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.Model
{
    /// <summary>
    /// Model for the Rationally application.
    /// </summary>

    public class RationallyModel 
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<Alternative> Alternatives { get; }
        public List<RelatedDocument> Documents { get; }

        public List<Stakeholder> Stakeholders { get; } 
        public Dictionary<string, Color> AlternativeStateColors { get; set; }

        public Dictionary<string, Color> DefaultStates { get; } = new Dictionary<string, Color> { {"Accepted", Color.FromArgb(0, 175, 0) }, { "Rejected", Color.FromArgb(153, 12, 0) }, {"Proposed", Color.FromArgb(96, 182, 215) }, {"Challenged", Color.FromArgb(255, 173, 21) },{ "Discarded", Color.FromArgb(155, 155, 155) } };

        public string Author { get; set; }
        public string DecisionName { get; set; }
        public string DateString { get; set; }
        public string Version { get; set; }
        public List<Force> Forces { get; set; }

        public List<PlanningItem> PlanningItems { get; set; }

        public RationallyModel()
        {
            Author = "";
            DecisionName = "";
            DateString = "";
            Version = "";
            Alternatives = new List<Alternative>();
            Documents = new List<RelatedDocument>();
            Forces = new List<Force>();
            Stakeholders = new List<Stakeholder>();
            PlanningItems = new List<PlanningItem>();

            ResetAlternativeStateColors();
            AlternativeStateColorsFromFile.ToList().Select(rawState => (AlternativeState)rawState.Value).ToList().ForEach(stateColor => AlternativeStateColors.Add(stateColor.State,stateColor.Color));
            //AlternativeStates = new List<string> {"Accepted", "Challenged", "Discarded", "Proposed", "Rejected"}; //Currently hardcoded, could be user setting in future product.
        }
        public RationallyModel DeepCopy()
        {
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<RationallyModel>(json);
        }
        public void RegenerateAlternativeIdentifiers()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Don't update the view during an undo, since the undo does that for us
            {
                int i = 0;
                AlternativesContainer alternativesContainer = (AlternativesContainer) Globals.RationallyAddIn.View.Children.First(c => c is AlternativesContainer);
                alternativesContainer.Children.Where(c => c is AlternativeContainer).ToList().ForEach(c => ((AlternativeContainer) c).SetAlternativeIdentifier(i++));
            }
            int j = 0;
            foreach (Alternative a in Alternatives)
            {
                a.GenerateIdentifier(j); 
                j++;
            }
        }

        public void RegeneratePlanningIdentifiers()
        {
            int i = 0;
            PlanningContainer planningContainer = (PlanningContainer)Globals.RationallyAddIn.View.Children.First(c => c is PlanningContainer);
            planningContainer.Children.Where(c => c is PlanningItemComponent).ToList().ForEach(c => ((PlanningItemComponent)c).SetPlanningItemIndex(i++));
        }

        public void RegenerateDocumentIdentifiers()
        {
            int i = 0;
            RelatedDocumentsContainer docsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            docsContainer.Children.Where(c => c is RelatedDocumentContainer).ToList().ForEach(c => ((RelatedDocumentContainer)c).SetDocumentIdentifier(i++));
        }

        public void RegenerateForceIdentifiers()
        {
            int i = 0;
            ForcesContainer forcesContaineresContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
            forcesContaineresContainer.Children.Where(c => c is ForceContainer).ToList().ForEach(c => ((ForceContainer)c).SetForceIdentifier(i++));
        }

        public void RegenerateStakeholderIdentifiers()
        {
            int i = 0;
            StakeholdersContainer stakeholdersContainer = (StakeholdersContainer)Globals.RationallyAddIn.View.Children.First(c => c is StakeholdersContainer);
            stakeholdersContainer.Children.Where(c => c is StakeholderContainer).ToList().ForEach(c => ((StakeholderContainer)c).SetStakeholderIndex(i++));
        }

        /// <summary>
        /// Reduces the current list of alternative states (and their color) to only the "No State" element.
        /// </summary>
        public void ResetAlternativeStateColors() => AlternativeStateColors = new Dictionary<string, Color> {{"No State", Color.DimGray}};

        internal IEnumerable<DictionaryEntry> AlternativeStateColorsFromFile
        {
            get
            {
                if (File.Exists(Constants.StateResourceFile))
                {
                    using (ResXResourceReader resxReader = new ResXResourceReader(Constants.StateResourceFile))
                    {
                        //FOR EACH KV pair that represents an alternative state + color DO:
                        foreach (DictionaryEntry entry in resxReader.Cast<DictionaryEntry>().Where(entry => ((string) entry.Key).StartsWith("alternativeState")))
                        {
                            yield return entry;
                        }
                    }
                }
                else
                {
                    int i = 0;
                    foreach (AlternativeState state in DefaultStates.Select(kvp => new AlternativeState
                    {
                        Color = kvp.Value,
                        State = kvp.Key
                    }))
                    {
                        yield return new DictionaryEntry("alternativeState" + i, state);
                        i++;
                    }
                }
            }
        }
    }
}
