using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Components
{
    class InformationContainer : RationallyComponent
    {
        private string author;
        private string date;
        private string version;

        public InformationContainer(string author, string date, string version)
        {
            this.author = author;
            this.date = date;
            this.version = version;
        }
        /// <summary>
        /// Draws a container with an author date and version on the given position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public new IVShape Draw(double x, double y)
        {
            //1) draws the three text elements
            TextLabel authorLabel = new TextLabel("Author: " + author + "\t\t Date: " + date + "\t\t Version: " + version);
            //TextLabel dateLabel = new TextLabel("Date: " + this.date);
            //TextLabel versionLabel = new TextLabel("Version: " + this.version);

            //Globals.ThisAddIn.Application.ActiveWindow.DeselectAll();

            IVShape authorShape = authorLabel.Draw(x+0.1,y-0.1);
            //Shape dateShape = dateLabel.Draw(x+3,y-0.1);
            //Shape versionShape = versionLabel.Draw(x+6,y-0.1);
            //Globals.ThisAddIn.Application.ActiveWindow.Select(authorShape, (short)VisSelectArgs.visSelect);
            //Globals.ThisAddIn.Application.ActiveWindow.Select(dateShape, (short)VisSelectArgs.visSelect);
            //Globals.ThisAddIn.Application.ActiveWindow.Select(versionShape, (short)VisSelectArgs.visSelect);

            //2) create a container
            Document containerDocument = Globals.ThisAddIn.Application.Documents.OpenEx(Globals.ThisAddIn.Application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers,
            VisMeasurementSystem.visMSUS), (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visAddHidden);
            Master plainContainer = containerDocument.Masters.ItemU[@"Alternating"];

            //3) wrap the text components in the container
            Shape informationShape = Globals.ThisAddIn.Application.ActivePage.DropContainer(plainContainer, authorShape);
            //informationShape.CellsU["Height"].ResultIU = authorShape.CellsU["Height"].ResultIU + authorShape.CellsU["TopMargin"].Result[VisUnitCodes.visInches] + authorShape.CellsU["BottomMargin"].Result[VisUnitCodes.visInches];
            informationShape.Text = "Decision Information";//"Author:" + author + "\t\t Date:" + date + "\t\t Version:" + version;
            
            return informationShape;
        }
    }
}
