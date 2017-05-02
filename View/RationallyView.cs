using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Rationally.Visio.View.Information;
using Rationally.Visio.View.Planning;
using Rationally.Visio.View.Stakeholders;

namespace Rationally.Visio.View
{
    /// <summary>
    /// View for the Rationally application.
    /// </summary>
    public class RationallyView : RationallyContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public RationallyView(Page page) : base(page)
        {

        }



        
        
        public override bool ExistsInTree(Shape s) => Children.Exists(x => x.ExistsInTree(s));

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (AlternativesContainer.IsAlternativesContainer(s.Name))
            {
                if (Children.Exists(x => AlternativesContainer.IsAlternativesContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneAlternativesContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    Children.Add(new AlternativesContainer(Page, s));
                }
            }
            else if (RelatedDocumentsContainer.IsRelatedDocumentsContainer(s.Name))
            {
                if (Children.Exists(x => RelatedDocumentsContainer.IsRelatedDocumentsContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneRelatedDocumentsContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    RelatedDocumentsContainer rdc = new RelatedDocumentsContainer(Page, s);
                    Children.Add(rdc);
                }
            }
            else if (ForcesContainer.IsForcesContainer(s.Name))
            {
                if (Children.Exists(x => ForcesContainer.IsForcesContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneForcesContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    ForcesContainer forcesContainer = new ForcesContainer(Page, s);
                    Children.Add(forcesContainer);
                }
            }
            else if (InformationContainer.IsInformationContainer(s.Name))
            {
                if (Children.Exists(x => InformationContainer.IsInformationContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneInformationContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    InformationContainer informationContainer = new InformationContainer(Page, s);
                    Children.Add(informationContainer);
                }
            }
            else if (TitleLabel.IsTitleLabel(s.Name))
            {
                if (Children.Exists(x => TitleLabel.IsTitleLabel(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneTitleAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    TitleLabel titleLabel = new TitleLabel(Page, s);
                    Children.Add(titleLabel);
                }
            }
            else if (StakeholdersContainer.IsStakeholdersContainer(s.Name))
            {
                if (Children.Exists(x => StakeholdersContainer.IsStakeholdersContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OneStakeholdersContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short) VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    StakeholdersContainer stakeholdersContainer = new StakeholdersContainer(Page, s);
                    Children.Add(stakeholdersContainer);
                }
            }
            else if (PlanningContainer.IsPlanningContainer(s.Name))
            {
                if (Children.Exists(x => PlanningContainer.IsPlanningContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show(Messages.OnePlanningContainerAllowed, Messages.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    PlanningContainer planningContainer = new PlanningContainer(Page, s);
                    Children.Add(planningContainer);
                }
            }
            else if (allowAddOfSubpart)
            {
                Children.ForEach(r => r.AddToTree(s, true));
            }
        }

        public override VisioShape GetComponentByShape(Shape s) => Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
    }
}
