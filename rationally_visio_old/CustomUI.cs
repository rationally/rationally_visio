using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>This constructor is intentionally left blank.</summary>
        public CustomUI()
        {

            // No initialization is required.
        }

        /// <summary>This method loads custom UI from an XML file and 
        /// associates it with the document object passed in.</summary>
        /// <param name="targetDocument">An open document in a running 
        /// Visio application</param>
        public void DemoCustomUIStart(
            Microsoft.Office.Interop.Visio.Document targetDocument)
        {

            Microsoft.Office.Interop.Visio.Application visioApplication =
                targetDocument.Application;

            customRibbon = new RationallyRibbon();

            // Passing in null rather than targetDocument would make the custom
            // UI available for all documents.
            visioApplication.RegisterRibbonX(
                customRibbon,
                targetDocument,
                Microsoft.Office.Interop.Visio.VisRibbonXModes.visRXModeDrawing,
                "RegisterRibbonX example");
        }

        /// <summary>This method removes custom UI from a document.</summary>
        /// <param name="targetDocument">An open document in a running 
        /// Visio application that has custom UI associated with it</param>
        public void DemoCustomUIStop(
            Microsoft.Office.Interop.Visio.Document targetDocument)
        {

            Microsoft.Office.Interop.Visio.Application visioApplication =
                targetDocument.Application;

            visioApplication.UnregisterRibbonX(customRibbon, targetDocument);
        }
    }


}
