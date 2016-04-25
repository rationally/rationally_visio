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
        public new Shape Draw(double x, double y)
        {
            //1) draws the three text elements
            TextLabel authorLabel = new TextLabel("Author: " + this.author);
            TextLabel dateLabel = new TextLabel("Date: " + this.date);
            TextLabel versionLabel = new TextLabel("Version: " + this.version);

            Globals.ThisAddIn.Application.ActiveWindow.DeselectAll();

            Shape authorShape = authorLabel.Draw(x+0.1,y-0.1);
            Shape dateShape = dateLabel.Draw(x+3,y-0.1);
            Shape versionShape = versionLabel.Draw(x+6,y-0.1);
            Globals.ThisAddIn.Application.ActiveWindow.Select(authorShape, (short)VisSelectArgs.visSelect);
            Globals.ThisAddIn.Application.ActiveWindow.Select(dateShape, (short)VisSelectArgs.visSelect);
            Globals.ThisAddIn.Application.ActiveWindow.Select(versionShape, (short)VisSelectArgs.visSelect);

            //2) create a container
            Document containerDocument = Globals.ThisAddIn.Application.Documents.OpenEx(Globals.ThisAddIn.Application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers,
            VisMeasurementSystem.visMSUS), (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visAddHidden);
            Master plainContainer = containerDocument.Masters.ItemU[@"Plain"];

            //3) wrap the text components in the container
            Shape informationShape = Globals.ThisAddIn.Application.ActivePage.DropContainer(plainContainer, Globals.ThisAddIn.Application.ActiveWindow.Selection);
            informationShape.Text = "";
            return informationShape;
        }
    }
}
