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

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            ShowMyDialogBox();
            MessageBox.Show(decision + " by " + author +" with header " + header);

            this.Application.Documents.Add("");

            Documents visioDocs = this.Application.Documents;

            Document analogDocument = visioDocs.OpenEx("Analog and Digital Logic.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);

            Document basicDocument = visioDocs.OpenEx("Basic Shapes.vss",
                (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenDocked);

            Page activePage = this.Application.ActivePage;

            Document containerDocument = Application.Documents.OpenEx(Application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers,
VisMeasurementSystem.visMSUS), 0x40);

            activePage.PageSheet.CellsU["PageWidth"].Result[VisUnitCodes.visMillimeters] = 297; 
            activePage.PageSheet.CellsU["PageHeight"].Result[VisUnitCodes.visMillimeters] = 210;

            Master visioRectMaster = analogDocument.Masters.get_ItemU(@"Inverter");
            Shape visioRectShape = activePage.Drop(visioRectMaster, 4.25, 5.5);

            Master visioCircleMaster = basicDocument.Masters.get_ItemU(@"Circle");
            Shape visioCircleShape = activePage.Drop(visioRectMaster, 0, 0);
            visioCircleShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = 22;
            //add a header to the page
            //Shape headerShape = activePage.DrawRectangle(10,10,400,10);
            //headerShape.TextStyle = "Basic";
            //headerShape.LineStyle = "TextOnly";
            //headerShape.FillStyle = "TextOnly";
            //headerShape.Characters.Text = "Deployment of Step 2 and Step 3";
            //headerShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = 22;

            //this.Application.ActiveWindow.Select(visioRectShape, (short)VisSelectArgs.visSelect);

            Shape descriptionContainer = activePage.DropContainer(containerDocument.Masters.get_ItemU("Alternating"), visioRectShape);
            //descriptionContainer.Name = "mand";
            descriptionContainer.Text = "Description";
            Master containerElement1master = basicDocument.Masters.get_ItemU(@"Rectangle");
            Shape containerElement1 = activePage.Drop(containerElement1master, 4.25, 5.5);
            descriptionContainer.ContainerProperties.AddMember(containerElement1, VisMemberAddOptions.visMemberAddExpandContainer);
            
            //descriptionContainer.SetBegin(100, 100);
            foreach (object shape in activePage.Shapes)
            {
                if (shape == visioRectShape)
                {
                    var x = 0;
                }
            }
            visioRectShape.Text = @"Rectangle text.";

            this.Application.ActiveWindow.Select(visioRectShape, (short)VisSelectArgs.visSelect);
            activePage.DropContainer(containerDocument.Masters.ItemU["Alternating"], visioRectShape);
            /*Visio.Master visioStarMaster = visioStencil.Masters.get_ItemU(@"Cube");
            Visio.Shape visioStarShape = visioPage.Drop(visioStarMaster, 2.0, 5.5);
            visioStarShape.Text = @"Star text.";

            Visio.Master visioHexagonMaster = visioStencil.Masters.get_ItemU(@"Hexagon");
            Visio.Shape visioHexagonShape = visioPage.Drop(visioHexagonMaster, 7.0, 5.5);
            visioHexagonShape.Text = @"Hexagon text.";*/
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
        
        #endregion
    }
}
