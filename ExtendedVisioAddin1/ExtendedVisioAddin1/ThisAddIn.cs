using System;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View;
using ExtendedVisioAddin1.View.Alternatives;
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
                if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "forces") //TODO check context
                {
                    //create a master
                    Master forcesMaster = Model.RationallyDocument.Masters.ItemU[@"Force"];

                    s.Drop(forcesMaster, 1, 1);
                } else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "alternatives")
                {
                    
                    new AddAlternativeEventHandler(Model);
                }
                else if (s.CellsU["User.rationallyType"].ResultStr["Value"].Contains("alternativeState"))
                {
                    if (context.Split('.')[0] == "stateChange")
                    {
                        new EditAlternativeStateEventHandler(Model, context.Split('.')[1]);
                    }
                }
                else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "alternative")
                {
                    RemoveAlternativeEventHandler c = new RemoveAlternativeEventHandler(model);
                } else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "relatedDocuments")
                {
                    new AddRelatedDocumentHandler();
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
