using System;
using System.Windows.Forms;
using rationally_visio;
using Microsoft.Office.Interop.Visio;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;

namespace ExtendedVisioAddin1
{
    public partial class ThisAddIn
    {
        //TODO: application static kan mss mooier
        public RModel model { get; set; }


        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            model = new RModel();
            model.Alternatives.Add(new Alternative("titelo","Accepted","dessehcription"));
            
            Application.MarkerEvent += new EApplication_MarkerEventEventHandler(Application_MarkerEvent);
            Application.TemplatePaths = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += new EApplication_DocumentCreatedEventHandler(DelegateCreateDocumentEvent);
            Application.DocumentOpened += new EApplication_DocumentOpenedEventHandler(Application_DocumentOpenedEvent);
            Application.ShapeAdded += new EApplication_ShapeAddedEventHandler(Application_ShapeAddedEvent);
            Application.ShapeChanged += new EApplication_ShapeChangedEventHandler(Application_ShapeChangedEvent);
            Application.MasterAdded += new EApplication_MasterAddedEventHandler(Application_MasterAddedEvent);
            Application.MasterChanged += new EApplication_MasterChangedEventHandler(Application_MasterChangedEvent);

        }

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {

        }



        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new RationallyRibbon();
        }

        private void Application_MarkerEvent(Microsoft.Office.Interop.Visio.Application application, int sequence, string context)
        {
            Selection selection = this.Application.ActiveWindow.Selection;//event must originate from selected element
            //for (int i = 0; i < selection.Count; i++) 
            foreach (IVShape s in selection)
            {
                if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "forces") //TODO check context
                {
                    //create a master
                    Master forcesMaster = model.RationallyDocument.Masters.ItemU[@"Force"];

                    s.Drop(forcesMaster, 1, 1);
                } else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "alternatives")
                {
                    
                    AddAlternativeEventHandler a = new AddAlternativeEventHandler(model);
                }
                else if (s.CellsU["User.rationallyType"].ResultStr["Value"].Contains("alternativeState"))
                {
                    if (context.Split('.')[0] == "stateChange")
                    {
                        EditAlternativeStateEventHandler b = new EditAlternativeStateEventHandler(model, context.Split('.')[1]);
                    }
                }
                else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "alternative")
                {
                    RemoveAlternativeEventHandler c = new RemoveAlternativeEventHandler(model);
                }
            }
        }



        private void Application_DocumentOpenedEvent(IVDocument d)
        {
        }

        private void Application_ShapeAddedEvent(Shape s)
        {
            string yx = ((IVShape)s.ContainingShape).Name;
            var x = 0;
        }

        private void Application_ShapeChangedEvent(Shape s)
        {
            var x = 0;
        }

        private void Application_MasterAddedEvent(Master m)
        {
            var x = 0;
        }

        private void Application_MasterChangedEvent(Master m)
        {
            var x = 0;
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
            new DocumentCreatedEventHandler(d, model);
            new RepaintHandler(model);
        }
    }
}
