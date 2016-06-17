using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Visio;
using rationally_visio;
using Shape = Microsoft.Office.Interop.Visio.Shape;

namespace ExtendedVisioAddin1
{
    public partial class ThisAddIn
    {
        //TODO: application static kan mss mooier
        public RModel Model { get; set; }
        public RView View { get; set; }
        private bool DocumentCreation { get; set; }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            Model = new RModel();
            View = new RView(Application.ActivePage);
            DocumentCreation = false;
            Application.MarkerEvent += Application_MarkerEvent;
            Application.TemplatePaths = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += DelegateCreateDocumentEvent;
            Application.ShapeAdded += Application_ShapeAddedEvent;
            Application.QueryCancelSelectionDelete += Application_QueryCancelSelectionDelete;
            Application.BeforeShapeDelete += Application_DeleteShapeEvent;
            Application.CellChanged += Application_CellChangedEvent;
            Application.TextChanged += Application_TextChangedEvent;

            Application.BeforePageDelete += Application_BeforePageDeleteEvent;
            Application.WindowActivated += Application_WindowActivatedEvent;
            RegisterEventHandlers();
        }

        private void Application_TextChangedEvent(Shape shape)
        {
            if (shape.Document.Template.ToLower().Contains("rationally") && ForceValueComponent.IsForceValue(shape.Name))
            {
                ForcesContainer forcesContainer = (ForcesContainer)View.Children.First(c => c is ForcesContainer);
                ForceValueComponent forceValue = (ForceValueComponent)View.GetComponentByShape(shape);
                new RepaintHandler(forceValue);//repaint the force value, for coloring
                ForceTotalsRow forceTotalsRow = forcesContainer.Children.First(c => c is ForceTotalsRow) as ForceTotalsRow;
                if (forceTotalsRow != null) new RepaintHandler(forceTotalsRow.Children.Where(c => c is ForceTotalComponent).First(c => c.AlternativeIdentifier == forceValue.AlternativeIdentifier));
            }
        }

        private void Application_WindowActivatedEvent(Window w)
        {
            if (w.Type == 1 && w.Document.Template.ToLower().Contains("rationally"))
            {
                View.Page = Application.ActivePage;
                RebuildTree(w.Document);
                if (DocumentCreation)
                {
                    DocumentCreation = false;

                    Globals.ThisAddIn.Application.PurgeUndo(); //On day 7 he said: Don't allow undoing of creation. 
                }
            }
        }

        private static void RegisterEventHandlers()
        {
            MarkerEventHandlerRegistry registry = MarkerEventHandlerRegistry.Instance;
            registry.Register("alternatives.add", new AddAlternativeEventHandler());
            registry.Register("relatedDocuments.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedDocuments.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("relatedDocumentContainer.moveUp", new MoveUpDocumentHandler());
            registry.Register("relatedDocumentContainer.moveDown", new MoveDownDocumentHandler());
            registry.Register("alternative.delete", new RemoveAlternativeEventHandler());
            registry.Register("alternativeState.change", new EditAlternativeStateEventHandler());
            registry.Register("relatedFile.edit", new EditRelatedFileHandler());
            registry.Register("alternative.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternative.moveDown", new MoveDownAlternativeHandler());

            registry.Register("forces.add", new AddForceHandler());
            registry.Register("forceContainer.add", new AddForceHandler());
            registry.Register("forceConcern.add", new AddForceHandler());
            registry.Register("forceValue.add", new AddForceHandler());
            registry.Register("forceDescription.add", new AddForceHandler());

            registry.Register("forceContainer.delete", new RemoveForceHandler());
            registry.Register("forceConcern.delete", new RemoveForceHandler());
            registry.Register("forceValue.delete", new RemoveForceHandler());
            registry.Register("forceDescription.delete", new RemoveForceHandler());

            registry.Register("forceContainer.moveUp", new MoveUpForceHandler());
            registry.Register("forceConcern.moveUp", new MoveUpForceHandler());
            registry.Register("forceValue.moveUp", new MoveUpForceHandler());
            registry.Register("forceDescription.moveUp", new MoveUpForceHandler());

            registry.Register("forceContainer.moveDown", new MoveDownForceHandler());
            registry.Register("forceConcern.moveDown", new MoveDownForceHandler());
            registry.Register("forceValue.moveDown", new MoveDownForceHandler());
            registry.Register("forceDescription.moveDown", new MoveDownForceHandler());
        }


        protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new RationallyRibbon();
        }

        private void Application_MarkerEvent(Application application, int sequence, string context)
        {
            if (application.ActiveDocument.Template.ToLower().Contains("rationally"))
            {
                Selection selection = Application.ActiveWindow.Selection; //event must originate from selected element
                //for (int i = 0; i < selection.Count; i++) 
                foreach (Shape s in selection)
                {
                    if (s.CellExistsU["User.rationallyType", 0] != 0)
                    {
                        string identifier = context;
                        if (context.Contains("."))
                        {
                            identifier = context.Split('.')[1];
                            context = context.Split('.')[0];
                        }

                        MarkerEventHandlerRegistry.Instance.HandleEvent(s.CellsU["User.rationallyType"].ResultStr["Value"] + "." + context, Model, s, identifier);
                    }
                }
            }
        }

        private void Application_CellChangedEvent(Cell cell)
        {
            Shape changedShape = cell.Shape;
            if (changedShape == null || !changedShape.Document.Template.ToLower().Contains("rationally") || changedShape.CellExistsU["User.rationallyType", 0] == 0)
            {
                return;
            }
            if (RelatedUrlComponent.IsRelatedUrlComponent(changedShape.Name) && cell.LocalName.Equals("Hyperlink.Row_1.Address"))
            {
                //find the container that holds all Related Documents
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)View.Children.First(c => c is RelatedDocumentsContainer);
                //find the related document holding the changed shape (one of his children has RShape equal to changedShape)
                RelatedDocumentContainer relatedDocumentContainer = relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(dc => dc.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0);
                //update the text of the URL display component to the new url
                RelatedURLURLComponent relatedURLURLComponent = (RelatedURLURLComponent)relatedDocumentContainer.Children.First(c => c is RelatedURLURLComponent);
                relatedURLURLComponent.Text = changedShape.Hyperlink.Address;
                //new RepaintHandler();
            }
            else if (Application.IsUndoingOrRedoing && ForceContainer.IsForceContainer(changedShape.Name) && cell.LocalName.Equals("User.forceIndex")) //No need to rebuild tree outside an undo
            {
                RComponent forcesComponent = View.Children.FirstOrDefault(x => x is ForcesContainer);
                if (forcesComponent != null)
                {
                    Model.Forces.Clear();
                    View.Children.Remove(forcesComponent);
                    Shape temp = forcesComponent.RShape;
                    View.Children.Add(new ForcesContainer(temp.ContainingPage, temp));
                }
            }
            else if (Application.IsUndoingOrRedoing && AlternativeContainer.IsAlternativeContainer(changedShape.Name) && cell.LocalName.Equals("User.alternativeIndex"))
            {
                RComponent alternativeComponent = View.Children.FirstOrDefault(x => x is AlternativesContainer);
                if (alternativeComponent != null)
                {
                    Model.Alternatives.Clear();
                    View.Children.Remove(alternativeComponent);
                    Shape temp = alternativeComponent.RShape;
                    View.Children.Add(new AlternativesContainer(temp.ContainingPage, temp));
                }
            }
            else if (Application.IsUndoingOrRedoing && RelatedDocumentContainer.IsRelatedDocumentContainer(changedShape.Name) && cell.LocalName.Equals("User.documentIndex"))
            {
                RComponent docComponent = View.Children.FirstOrDefault(x => x is RelatedDocumentsContainer);
                if (docComponent != null)
                {
                    Model.Documents.Clear();
                    View.Children.Remove(docComponent);
                    Shape temp = docComponent.RShape;
                    View.Children.Add(new RelatedDocumentsContainer(temp.ContainingPage, temp));
                }
            }
        }

        private void RebuildTree(IVDocument d)
        {
            View.Children.Clear();
            Model.Alternatives.Clear();
            Model.Documents.Clear();
            Model.Forces.Clear();
            foreach (Page page in d.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    View.AddToTree(shape, false);
                }
            }
        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            if (s.CellExistsU["User.rationallyType", 0] != 0 && !View.ExistsInTree(s))
            {
                View.AddToTree(s, true);
            }
        }

        private bool Application_QueryCancelSelectionDelete(Selection e)
        {
            foreach (Shape s in e)
            {
                string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];
                if (rationallyType == "alternativeTitle")
                {
                    int id = Application.BeginUndoScope("scope");
                    AlternativesContainer cont = (AlternativesContainer) View.Children.First(x => x is AlternativesContainer);
                    foreach (AlternativeContainer alternativeContainer in cont.Children.Where(c => c is AlternativeContainer).Cast<AlternativeContainer>().ToList())
                    {
                        if (alternativeContainer.Children.Where(c => c.RShape.Equals(s)).ToList().Count > 0) //check if this alternative contains the to be deleted component
                        {
                            if (!alternativeContainer.Deleted)
                            {
                                alternativeContainer.RShape.Delete(); //delete the parent wrapper of s
                                cont.Children.Remove(alternativeContainer); //remove the alternative from the view tree
                            }
                            alternativeContainer.Children.Where(c => !c.Deleted && c.RShape!=s).ToList().ForEach(c => c.RShape.Delete()); //Delete the children of the parent.
                        }
                    }
                    Application.EndUndoScope( id, true);
                }
            }
            return false;
        }

        private void Application_BeforePageDeleteEvent(Page p)
        {
            if (p.Document.Template.ToLower().Contains("rationally"))
            {
                foreach (Shape shape in p.Shapes)
                {
                    View.DeleteFromTree(shape);
                }
            }
        }

        private void Application_DeleteShapeEvent(Shape s)
        {
            
            if (s.Document.Template.ToLower().Contains("rationally"))
            {
                if (s.CellExistsU["User.isStub", 0] != 0)
                {
                    return;
                }

                if (s.CellExistsU["User.rationallyType", 0] != 0)
                {
                    string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];

                    //mark the deleted shape as 'deleted' in the view tree
                    RComponent deleted = View.GetComponentByShape(s);
                    if (deleted != null)
                    {
                        deleted.Deleted = true;
                    }

                    RelatedDocumentsContainer relatedDocumentsContainer = View.Children.FirstOrDefault(c => c is RelatedDocumentsContainer) as RelatedDocumentsContainer; //todo NullReferenceMeuk
                    switch (rationallyType)
                    {
                        case "relatedDocumentContainer":
                            RComponent relDoc = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };
                            int docIndex = relDoc.DocumentIndex;
                            Model.Documents.RemoveAt(docIndex);
                            //for each container, remove the children of which the shape equals the to be deleted shape
                            relatedDocumentsContainer.Children = relatedDocumentsContainer.Children.Where(c => !c.RShape.Equals(s)).ToList();
                            new RepaintHandler(relatedDocumentsContainer);
                            break;
                        case "relatedUrl":
                        case "relatedFile":
                        case "relatedDocumentTitle":
                            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                            {
                                if (relatedDocumentContainer.Children.Where(c => c.RShape.Equals(s)).ToList().Count > 0) //check if this related document contains the to be deleted component
                                {
                                    if (!relatedDocumentContainer.Deleted)
                                    {
                                        relatedDocumentContainer.RShape.Delete(); //delete the parent wrapper of s
                                        relatedDocumentsContainer.Children.Remove(relatedDocumentContainer); //remove the related document from the view tree
                                    }
                                    relatedDocumentContainer.Children.Where(c => !c.Deleted).ToList().ForEach(c => c.RShape.Delete());//Delete the children of the parent.
                                }
                            }

                            break;
                        case "relatedUrlUrl":
                            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                            {
                                relatedDocumentContainer.Children.RemoveAll(c => c.RShape.Equals(s)); //Remove the component from the tree
                            }
                            break;
                        /*case "alternativeTitle":
                        case "alternativeIdentifier":
                        case "alternativeDescription":
                        */
                        case "alternative":
                            RComponent component = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };
                            int index = component.AlternativeIndex;
                            Model.Alternatives.RemoveAt(index);
                            View.DeleteAlternative(index, false);
                            break;
                        case "alternatives":
                        case "forces":
                        case "relatedDocuments":
                        case "informationBox":
                            View.Children.RemoveAll(obj => obj.RShape.Equals(s));
                            //todo extract
                            break;
                        case "forceContainer":
                        case "forceConcern":
                        case "forceDescription":
                            MarkerEventHandlerRegistry.Instance.HandleEvent(rationallyType + ".delete", Model, s, "");
                            break;
                        case "forceValue":
                            RComponent forceComponent = new RComponent(s.ContainingPage);
                            forceComponent.RShape = s;
                            if (Model.Alternatives.Any(a => a.Identifier == forceComponent.AlternativeIdentifier)) //if NOT, an alternative was deleted => so do not remove the whole force row
                            {
                                MarkerEventHandlerRegistry.Instance.HandleEvent(rationallyType + ".delete", Model, s, "");
                            }
                            break;
                    }
                }
                else
                {
                    RebuildTree(s.ContainingPage.Document);
                }
            }
        }
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            //Shutdown += ThisAddIn_Shutdown;
        }

        //#region Event delegaters
        private void DelegateCreateDocumentEvent(IVDocument d)
        {
            if (d.Template.ToLower().Contains("rationally"))
            {
                new DocumentCreatedEventHandler(d, Model);

                DocumentCreation = true;
            }
        }
    }
}
