using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Office = Microsoft.Office.Core;

// TODO:  Follow these steps to enable the Ribbon (XML) item:

// 1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
//    actions, such as clicking a button. Note: if you have exported this Ribbon from the Ribbon designer,
//    move your code from the event handlers to the callback methods and modify the code to work with the
//    Ribbon extensibility (RibbonX) programming model.

// 3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.  

// For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.


namespace rationally_visio
{
    [ComVisible(true)]
    public class RationallyRibbon : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        #region IRibbonExtensibility Members

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("rationally_visio.RationallyRibbon.xml");
        }

        #endregion

        #region Ribbon Callbacks
        //Create callback methods here. For more information about adding callback methods, visit http://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {
            ribbon = ribbonUI;
        }

        /// <summary>This method is a callback specified in the onLoad attribute 
        /// of the customUI element in the custom UI XML file. It is called by 
        /// Visio when the custom UI is first loaded.</summary>
        /// <param name="ribbonUI">A reference to the object representing the 
        /// custom UI loaded by Visio</param>
        public void OnRibbonLoad(Office.IRibbonUI ribbonUI)
        {
            // Do something with the newly constructed ribbon, such as capture
            // a local reference to it for later use.
            ribbon = ribbonUI;
        }

        /// <summary>This method is a callback specified in the custom UI XML 
        /// file. It is called by Visio when the associated button defined 
        /// in the XML is clicked.</summary>
        /// <param name="control">The Ribbon UI control that was activated</param>
        public void OnAction(Office.IRibbonControl control)
        {
            //System.Windows.Forms.MessageBox.Show("OnAction");
        }

        /// <summary>This method is a callback specified in the custom UI XML 
        /// file. It is called by Visio when the associated repurposed ribbon 
        /// control is clicked.</summary>
        /// <param name="control">The Ribbon UI control that was clicked</param>
        /// <param name="cancelDefault">If true, call the built-in command after 
        /// the custom code is complete</param>
        public void CommandOnAction(Office.IRibbonControl control,
            bool cancelDefault)
        {
            // Take a custom action when the user clicks Copy.
           // System.Windows.Forms.MessageBox.Show("CommandOnAction called: User clicked Copy.");

            cancelDefault = false;
        }

        #endregion

        #region Helpers

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            foreach (string name in resourceNames.Where(t => string.Compare(resourceName, t, StringComparison.OrdinalIgnoreCase) == 0))
            {
                using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(name)))
                {
                    if (resourceReader != null)
                    {
                        return resourceReader.ReadToEnd();
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
