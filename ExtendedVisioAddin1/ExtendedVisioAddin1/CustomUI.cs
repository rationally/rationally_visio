using Microsoft.Office.Interop.Visio;

namespace rationally_visio
{
    /// <summary>This class demonstrates how to import custom UI and display it
    /// only for a specific Visio document. The custom UI is shown when the 
    /// document is active, and is not shown in other contexts, such as when 
    /// other documents are active or in other views such as Print Preview.
    /// </summary>
    public class CustomUI
    {

        /// <summary>A reference to the sample class that creates and manages 
        /// the custom UI.</summary>
        private RationallyRibbon customRibbon;

        /// <summary>This method loads custom UI from an XML file and 
        /// associates it with the document object passed in.</summary>
        /// <param name="targetDocument">An open document in a running 
        /// Visio application</param>
        public void DemoCustomUIStart(
            Document targetDocument)
        {

            Application visioApplication =
                targetDocument.Application;

            customRibbon = new RationallyRibbon();

            // Passing in null rather than targetDocument would make the custom
            // UI available for all documents.
            visioApplication.RegisterRibbonX(
                customRibbon,
                targetDocument,
                VisRibbonXModes.visRXModeDrawing,
                "RegisterRibbonX example");
        }

        /// <summary>This method removes custom UI from a document.</summary>
        /// <param name="targetDocument">An open document in a running 
        /// Visio application that has custom UI associated with it</param>
        public void DemoCustomUIStop(
            Document targetDocument)
        {

            Application visioApplication =
                targetDocument.Application;

            visioApplication.UnregisterRibbonX(customRibbon, targetDocument);
        }
    }


}
