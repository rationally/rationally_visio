using System.Linq;
using System.Windows.Forms;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Model;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Documents;
using Rationally.Visio.View.Forces;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.View
{
    /// <summary>
    /// View for the Rationally application. Name is a shorthand for Rationally View.
    /// </summary>
    public class RView : RContainer
    {
        public RView(Page page) : base(page)
        {

        }

        public void AddAlternative(Alternative alternative)
        {
            AlternativesContainer container = (AlternativesContainer) Globals.RationallyAddIn.View.Children.First(ch => ch is AlternativesContainer);
            container.Children.Add(new AlternativeContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Alternatives.Count - 1, alternative));
            RepaintHandler.Repaint();
        }

        public void AddRelatedDocument(RelatedDocument document)
        {
            //container of all related documents:
            RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)Globals.RationallyAddIn.View.Children.First(c => c is RelatedDocumentsContainer);
            //create a container that wraps the new document
            RelatedDocumentContainer relatedDocumentContainer = new RelatedDocumentContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Documents.Count - 1, document);
            relatedDocumentsContainer.Children.Add(relatedDocumentContainer);

            RepaintHandler.Repaint(relatedDocumentsContainer);
        }

        /// <summary>
        /// Deletes an alternative container from the view.
        /// </summary>
        /// <param name="index">identifier of the alternative.</param>
        /// <param name="deleteShape">Shape was already deleted</param>
        public void DeleteAlternative(int index, bool deleteShape)
        {
            AlternativesContainer alternativesContainer = (AlternativesContainer)Children.FirstOrDefault(c => c is AlternativesContainer); //May have been deleted by the user
            if (alternativesContainer == null)
            {
                RepaintHandler.Repaint();
                return;
            }
            AlternativeContainer alternative = (AlternativeContainer)alternativesContainer.Children.FirstOrDefault(x => x.AlternativeIndex == index && x is AlternativeContainer); //Return null if no container or no alternative with index
            if (alternative != null)
            {
                alternativesContainer.Children.Remove(alternative);
                if (deleteShape)
                {
                    alternative.RShape.DeleteEx((short)VisDeleteFlags.visDeleteNormal); //deletes the alternative, and it's child components. This should not be done when the shape is already gone, such as when the user has deleted it himself.
                }
                RepaintHandler.Repaint();
            }
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
                    informationContainer.Repaint();
                }
            }
            else if(allowAddOfSubpart)
            {
                Children.ForEach(r => r.AddToTree(s, true));
            }
        }

        public override RComponent GetComponentByShape(Shape s)
        {
            return Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
        }
    }
}
