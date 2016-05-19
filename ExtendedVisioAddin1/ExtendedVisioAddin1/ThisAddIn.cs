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
        public RModel Model { get; set; }
        public RView View { get; set; }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
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
            foreach (IVShape s in selection)
            {
                switch (s.CellsU["User.rationallyType"].ResultStr["Value"])
                {
                    case "forces":
                        //create a master
                        Master forcesMaster = Model.RationallyDocument.Masters.ItemU[@"Force"];

                        s.Drop(forcesMaster, 1, 1);
                        break;
                    case "alternatives":
                        new AddAlternativeEventHandler(Model);
                        break;
                    case "alternativeState":
                        if (context.Split('.')[0] == "stateChange")
                        {
                            new EditAlternativeStateEventHandler(Model, context.Split('.')[1]);
                        }
                        break;
                    case "alternative":
                        new RemoveAlternativeEventHandler(Model);
                        break;
                    case "relatedDocuments":
                        switch (context)
                        {
                            case "relatedUrlAdd":
                                new AddRelatedUrlHandler();
                                break;
                            case "relatedDocumentAdd":
                                new AddRelatedDocumentHandler();
                                break;
                        }
                        break;
                    case "relatedFile":
                        if (context == "relatedFileComponentEdit")
                        {
                            new EditRelatedFileHandler();
                        }
                        break;
                }
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

        private void Application_DeleteShapeEvent(IVShape s)
        {
            if (s.CellExistsU["User.rationallyType", 0] != 0)
            {
                string rationallyType = s.CellsU["User.rationallyType"].ResultStr["Value"];
                RView view = Globals.ThisAddIn.View;
                //select all 'related documents' containers
                List<RelatedDocumentsContainer> relatedDocumentsContainers = view.Children.Where(c => c is RelatedDocumentsContainer).Cast<RelatedDocumentsContainer>().ToList();

                if (rationallyType == "relatedDocumentContainer")
                {
                    //for each container, remove the children of which the shape equals the to be deleted shape
                    relatedDocumentsContainers.ForEach(r => r.Children = r.Children.Where(c => !c.RShape.Equals(s)).ToList());
                    new RepaintHandler();
                }
                else if (rationallyType == "relatedUrl" || rationallyType == "relatedFile" || rationallyType == "relatedDocumentTitle") //a subpart 
                {
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
