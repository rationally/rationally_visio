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

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            Model = new RModel();
            View = new RView(Application.ActivePage);
            Model.AddObserver(View);
            Application.MarkerEvent += Application_MarkerEvent;
            Application.TemplatePaths = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += DelegateCreateDocumentEvent;
            Application.ShapeAdded += Application_ShapeAddedEvent;
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
                forcesContainer.Children.Last().Repaint();

            }
        }

        private void Application_WindowActivatedEvent(Window w)
        {
            if (w.Type == 1 && w.Document.Template.ToLower().Contains("rationally"))
            {
                View.Page = Application.ActivePage;
                RebuildTree(w.Document);
            }
        }
        
        private void RegisterEventHandlers()
        {
            MarkerEventHandlerRegistry registry = MarkerEventHandlerRegistry.Instance;
            registry.Register("alternatives.add", new AddAlternativeEventHandler());
            registry.Register("relatedDocuments.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedDocuments.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("alternative.delete", new RemoveAlternativeEventHandler());
            registry.Register("alternativeState.change", new EditAlternativeStateEventHandler());
            registry.Register("relatedFile.edit", new EditRelatedFileHandler());
            registry.Register("alternative.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternative.moveDown", new MoveDownAlternativeHandler());
            registry.Register("forces.add", new AddForceHandler());
            registry.Register("forceContainer.delete", new RemoveForceHandler());
        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {

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
            if (changedShape.Document.Template.ToLower().Contains("rationally") && cell.LocalName.Equals("Hyperlink.Row_1.Address") && changedShape.Name.Equals("RelatedUrl") && changedShape.CellExistsU["User.rationallyType", 0] != 0) //todo: testen of te rationallytype check het niet breekt
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

        }

        private void RebuildTree(IVDocument d)
        {
            View.Children.Clear();
            Model.Alternatives.Clear();
            foreach (Page page in d.Pages)
            {
                foreach (Shape shape in page.Shapes)
                {
                    if (AlternativesContainer.IsAlternativesContainer(shape.Name)) //Check if the shape is an Alternatives box
                    {
                        View.Children.Add(new AlternativesContainer(Application.ActivePage, shape));
                    }
                    else if (RelatedDocumentsContainer.IsRelatedDocumentsContainer(shape.Name))
                    {
                        RelatedDocumentsContainer relatedDocumentsContainer = new RelatedDocumentsContainer(Application.ActivePage, shape);
                        View.Children.Add(relatedDocumentsContainer);
                        new RepaintHandler(relatedDocumentsContainer);
                    }
                    else if (ForcesContainer.IsForcesContainer(shape.Name))
                    {
                        ForcesContainer forcesContainer = new ForcesContainer(Application.ActivePage, shape);
                        View.Children.Add(forcesContainer);
                        new RepaintHandler(forcesContainer);
                    }
                }
            }
        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            if (s.CellExistsU["User.rationallyType", 0] != 0 && !View.ExistsInTree(s))
            {
                View.AddToTree(s);
            }
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
            var x = s.Name;
            if (s.Document.Template.ToLower().Contains("rationally"))
            {
                if (s.CellExistsU["User.rationallyType", 0] != 0)
                {
                    string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];


                    //select all 'related documents' containers
                    List<RelatedDocumentsContainer> relatedDocumentsContainers = View.Children.Where(c => c is RelatedDocumentsContainer).Cast<RelatedDocumentsContainer>().ToList();

                    switch (rationallyType)
                    {
                        case "relatedDocumentContainer":
                            //for each container, remove the children of which the shape equals the to be deleted shape
                            relatedDocumentsContainers.ForEach(r => r.Children = r.Children.Where(c => !c.RShape.Equals(s)).ToList());
                            relatedDocumentsContainers.ForEach(r => new RepaintHandler(r));
                            break;
                        case "relatedUrl":
                        case "relatedFile":
                        case "relatedDocumentTitle":
                            foreach (RelatedDocumentsContainer relatedDocumentsContainer in relatedDocumentsContainers)
                            {
                                foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                                {
                                    if (relatedDocumentContainer.Children.Where(c => c.RShape.Equals(s)).ToList().Count > 0) //check if this related document contains the to be deleted component
                                    {
                                        relatedDocumentContainer.RShape.DeleteEx(0); //delete the parent wrapper of s, and it's subshapes (parallel to s)
                                        relatedDocumentsContainer.Children.Remove(relatedDocumentContainer); //remove the related document from the view tree
                                    }
                                }
                            }
                            break;
                        case "relatedUrlUrl":
                            foreach (RelatedDocumentsContainer relatedDocumentsContainer in relatedDocumentsContainers)
                            {
                                foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                                {
                                    relatedDocumentContainer.Children.RemoveAll(c => c.RShape.Equals(s)); //Remove the component from the tree
                                }
                            }
                            break;
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
                            //todo extract and/or call repaint
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
            Shutdown += ThisAddIn_Shutdown;
        }

        //#region Event delegaters
        private void DelegateCreateDocumentEvent(IVDocument d)
        {
            if (d.Template.ToLower().Contains("rationally"))
            {
                new DocumentCreatedEventHandler(d, Model);
            }
        }
    }
}
