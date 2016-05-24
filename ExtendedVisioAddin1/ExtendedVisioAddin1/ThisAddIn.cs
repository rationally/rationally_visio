using System;
using System.Collections.Generic;
using System.Linq;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
using ExtendedVisioAddin1.View.Documents;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Visio;
using rationally_visio;
using Shape = Microsoft.Office.Interop.Visio.Shape;

namespace ExtendedVisioAddin1
{
    public partial class ThisAddIn
    {
        //TODO: application static kan mss mooier
        public static bool PreventAddEvent;
        public static bool PreventDeleteEvent;

        public RModel Model { get; set; }
        public RView View { get; set; }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            PreventAddEvent = false;
            PreventDeleteEvent = false;
            Model = new RModel();
            //Model.Alternatives.Add(new Alternative("titelo","Accepted","dessehcription"));
            //Model.Alternatives.Add(new Alternative("titelo dos", "Accepted", "dessehcription"));
            View = new RView(Application.ActivePage);
            
            Model.AddObserver(View);
            Application.MarkerEvent += Application_MarkerEvent;
            Application.TemplatePaths = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += DelegateCreateDocumentEvent;
            Application.DocumentOpened += Application_DocumentOpenedEvent;
            Application.ShapeAdded += Application_ShapeAddedEvent;
            Application.ShapeChanged += Application_ShapeChangedEvent;
            Application.MasterAdded += Application_MasterAddedEvent;
            Application.MasterChanged += Application_MasterChangedEvent;
            Application.BeforeShapeDelete += Application_DeleteShapeEvent;
            Application.CellChanged += Application_CellChangedEvent;
            Application.ShapeParentChanged += Application_ShapeParentChangedEvent;

            RegisterEventHandlers();
        }

        private void Application_ShapeParentChangedEvent(Shape shape)
        {
            /*Application.UndoEnabled = true;
            Application.Undo();*/
        }

        private void RegisterEventHandlers()
        {
            MarkerEventHandlerRegistry registry = MarkerEventHandlerRegistry.Instance;
            registry.Register("alternatives.add",new AddAlternativeEventHandler());
            registry.Register("relatedDocuments.addRelatedFile", new AddRelatedDocumentHandler());
            registry.Register("relatedDocuments.addRelatedUrl", new AddRelatedUrlHandler());
            registry.Register("alternative.delete", new RemoveAlternativeEventHandler());
            registry.Register("alternativeState.change", new EditAlternativeStateEventHandler());
            registry.Register("relatedFile.edit", new EditRelatedFileHandler());
            registry.Register("alternative.moveUp", new MoveUpAlternativeHandler());
            registry.Register("alternative.moveDown", new MoveDownAlternativeHandler());
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
            Selection selection = Application.ActiveWindow.Selection;//event must originate from selected element
            //for (int i = 0; i < selection.Count; i++) 
            foreach (Shape s in selection)
            {
                if (s.CellExistsU["User.rationallyType",0] != 0)
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

        private void Application_CellChangedEvent(Cell cell)
        {
            Shape changedShape = cell.Shape;
            if ( cell.LocalName.Equals("Hyperlink.Row_1.Address") && changedShape.Name.Equals("RelatedUrl"))
            {
                //find the container that holds all Related Documents
                RelatedDocumentsContainer relatedDocumentsContainer = (RelatedDocumentsContainer)View.Children.First(c => c is RelatedDocumentsContainer);
                //find the related document holding the changed shape (one of his children has RShape equal to changedShape)
                RelatedDocumentContainer relatedDocumentContainer = relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().First(dc => dc.Children.Where(c => c.RShape.Equals(changedShape)).ToList().Count > 0);
                //update the text of the URL display component to the new url
                RelatedURLURLComponent relatedURLURLComponent = ((RelatedURLURLComponent)relatedDocumentContainer.Children.First(c => c is RelatedURLURLComponent));
                relatedURLURLComponent.Text = changedShape.Hyperlink.Address;
                //new RepaintHandler();
            }
            
        }

        private void Application_DocumentOpenedEvent(IVDocument d)
        {
            if (d.Template.ToLower().Contains("rationally"))
            {
                foreach (Shape shape in Application.ActivePage.Shapes)
                {
                    if (AlternativesContainer.IsAlternativesContainer(shape.Name)) //Check if the shape is an Alternatives box
                    {
                        View.Children.Add(new AlternativesContainer(Application.ActivePage, shape));
                    } else if (RelatedDocumentsContainer.IsRelatedDocumentsContainer(shape.Name))
                    {
                        View.Children.Add(new RelatedDocumentsContainer(Application.ActivePage, shape));
                    }
                }

                new RepaintHandler();
            }

        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            if (PreventAddEvent) return;

            if (AlternativesContainer.IsAlternativesContainer(s.Name))
            {
                if (View.Children.Exists(x => AlternativesContainer.IsAlternativesContainer(x.Name)))
                {
                    //TODO: turn this on, one day
                    /*DialogResult confirmResult = MessageBox.Show("Are you sure you want to add another alternatives box? \n This may cause problems with adding or deleting alternatives", "Confirm addition", MessageBoxButtons.YesNo);
                    if (confirmResult == DialogResult.No)
                    {
                        s.DeleteEx(0);
                        return;
                    }*/
                }
                else
                {
                    View.Children.Add(new AlternativesContainer(Application.ActivePage, s));
                }
                
            }
        }


        private void Application_ShapeChangedEvent(Shape s)
        {
            var x = 0;
        }

        private void Application_MasterAddedEvent(Master m)
        {
            if (m.Name == "Alternatives") //todo: wth
            {
                m.Delete();
            }
        }

        private void Application_MasterChangedEvent(Master m)
        {
        }

        private void Application_DeleteShapeEvent(Shape s)
        {
            if (PreventDeleteEvent) return;

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
                        new RepaintHandler();
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
                                    relatedDocumentContainer.RShape.DeleteEx(0);//delete the parent wrapper of s, and it's subshapes (parallel to s)
                                    relatedDocumentsContainer.Children.Remove(relatedDocumentContainer);//remove the related document from the view tree
                                }
                            }
                        }
                        break;
                    case "relatedUrlUrl":
                        foreach (RelatedDocumentsContainer relatedDocumentsContainer in relatedDocumentsContainers)
                        {
                            foreach (RelatedDocumentContainer relatedDocumentContainer in relatedDocumentsContainer.Children.Where(c => c is RelatedDocumentContainer).Cast<RelatedDocumentContainer>().ToList())
                            {
                                relatedDocumentContainer.Children.RemoveAll(c => c.RShape.Equals(s));
                                    //Remove the component from the tree
                            }
                        }
                        break;
                    case "alternative":
                        RComponent component = new RComponent(Globals.ThisAddIn.Application.ActivePage) { RShape = s };
                        int index = component.AlternativeIndex;
                        Model.Alternatives.RemoveAt(index);
                        View.DeleteAlternative(index, false);
                        break;
                    case "informationBox":
                        View.Children.RemoveAll(obj => obj.RShape.Equals(s));
                        break;
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
            new DocumentCreatedEventHandler(d, Model);
            Application_DocumentOpenedEvent(d);
        }
    }
}
