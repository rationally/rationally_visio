using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.View
{
    /// <summary>
    /// View for the Rationally application.
    /// </summary>
    public class RationallyView : RationallyContainer
    {
        public RationallyView(Page page) : base(page)
        {

        }



        
        
        public override bool ExistsInTree(Shape s)
        {
            return Children.Exists(x => x.ExistsInTree(s));
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (AlternativesContainer.IsAlternativesContainer(s.Name))
            {
                if (Children.Exists(x => AlternativesContainer.IsAlternativesContainer(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show("Only one instance of the alternatives container is allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Only one instance of the related documents container is allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Only one instance of the forces container is allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Only one instance of the information container is allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    InformationContainer informationContainer = new InformationContainer(Page, s);
                    Children.Add(informationContainer);
                    RepaintHandler.Repaint(informationContainer);
                }
            }
            else if (TitleLabel.IsTitleLabel(s.Name))
            {
                if (Children.Exists(x => TitleLabel.IsTitleLabel(x.Name)))
                {
                    if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
                    {
                        MessageBox.Show("Only one instance of the title box is allowed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        s.DeleteEx((short)VisDeleteFlags.visDeleteNormal);
                    }
                }
                else
                {
                    TitleLabel titleLabel = new TitleLabel(Page, s);
                    Children.Add(titleLabel);
                    titleLabel.Repaint();
                }
            }
            else if(allowAddOfSubpart)
            {
                Children.ForEach(r => r.AddToTree(s, true));
            }
        }

        public override RationallyComponent GetComponentByShape(Shape s)
        {
            return Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
        }
    }
}
