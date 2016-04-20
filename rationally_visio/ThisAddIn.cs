using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Interop.Visio;
using Office = Microsoft.Office.Core;

namespace rationally_visio
{
    public partial class ThisAddIn
    {
        private string author;
        private string decision;
        private string header;
        private Document rationallyDocument;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            //ShowMyDialogBox();
            //MessageBox.Show(decision + " by " + author +" with header " + header);
            Application.MarkerEvent += new EApplication_MarkerEventEventHandler(Application_MarkerEvent);
            Application.TemplatePaths = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += new EApplication_DocumentCreatedEventHandler(Application_DocumentCreatedEvent);
            Application.DocumentOpened += new EApplication_DocumentOpenedEventHandler(Application_DocumentOpenedEvent);
            this.Application.Documents.Add("");

            Documents visioDocs = this.Application.Documents; 

            Document analogDocument = visioDocs.OpenEx("Analog and Digital Logic.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked); 

            Document basicDocument = visioDocs.OpenEx("Basic Shapes.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);
            

            string docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\DecisionsStencil.vssx";
            rationallyDocument = this.Application.Documents.OpenEx(docPath,
                ((short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked +
                 (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenRO));

            Page activePage = this.Application.ActivePage;

            Document containerDocument = Application.Documents.OpenEx(Application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers,
                        VisMeasurementSystem.visMSUS), 0x40); 

            activePage.PageSheet.CellsU["PageWidth"].Result[VisUnitCodes.visMillimeters] = 297; 
            activePage.PageSheet.CellsU["PageHeight"].Result[VisUnitCodes.visMillimeters] = 210;

            //add a header to the page
            Shape headerShape = activePage.DrawRectangle(0.1, 8, 5, 8); 
            //headerShape.TextStyle = "Basic";
            headerShape.LineStyle = "Text Only";
            headerShape.FillStyle = "Text Only";
            headerShape.Text = "Deployment of Step 2 and Step 34";
            headerShape.Characters.Text = "Deployment of Step 2 and Step 3";
            headerShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = 22;
            headerShape.CellsSRC[(short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLine, (short)VisCellIndices.visLinePattern].ResultIU = 0;

            //descriptionContainer.SetBegin(100, 100);
            foreach (Shape shape in activePage.Shapes)
            {
                var x = shape.CellExistsU["type", 0];
                var y = shape.CellExistsU["type", 1];
            }

            Master forcesMaster = rationallyDocument.Masters.ItemU[@"Forces"];
            Shape forceShape = activePage.Drop(forcesMaster, 4, 3);
            var a = forceShape.CellsU["User.rationallyType"];
            string forcesType = forceShape.CellsU["User.rationallyType"].ResultStr["value"];

            activePage.DropContainer(containerDocument.Masters.ItemU["Alternating"], forceShape);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            //Comment to test pulling
        }

        public void ShowMyDialogBox()
        {
            SheetSetUp testDialog = new SheetSetUp();

            // Show testDialog as a modal dialog and determine if DialogResult = OK.
            if (testDialog.ShowDialog() == DialogResult.OK)
            {
                // Read the contents of testDialog's TextBox.
                this.author = testDialog.textBoxAuthor.Text;
                this.decision = testDialog.textBoxName.Text;
                this.header = testDialog.textBoxHeader.Text;
            }
            testDialog.Dispose();
        }

        protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new RationallyRibbon();
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
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
                    Master forcesMaster = rationallyDocument.Masters.ItemU[@"Force"];

                    s.Drop(forcesMaster, 1, 1);
                }
            }
        }

        private void Application_DocumentCreatedEvent(IVDocument d)
        {
            if (d.Template.ToLower().Contains("rationally"))
            {
                ShowMyDialogBox();
            }
        }

        private void Application_DocumentOpenedEvent(IVDocument d)
        {
            if (d.Template.ToLower().Contains("rationally"))
            {
                ShowMyDialogBox();
            }
        }
        #endregion
    }
}
