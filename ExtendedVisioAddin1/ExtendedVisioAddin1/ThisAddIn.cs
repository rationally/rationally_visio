using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using ExtendedVisioAddin1.Properties;
using rationally_visio;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1
{
    public partial class ThisAddIn
    {
        private readonly AddinUI AddinUI = new AddinUI();
        private string author;
        private string decision;
        private string header;
        private Document rationallyDocument;

        /// <summary>
        /// A simple command
        /// </summary>
        public void Command1()
        {
            MessageBox.Show(
                "Hello from command 1!",
                "Rationally");
        }

        /// <summary>
        /// A command to demonstrate conditionally enabling/disabling.
        /// The command gets enabled only when a shape is selected
        /// </summary>
        public void Command2()
        {
            if (Application == null || Application.ActiveWindow == null || Application.ActiveWindow.Selection == null)
                return;

            MessageBox.Show(
                string.Format("Hello from (conditional) command 2! You have {0} shapes selected.", Application.ActiveWindow.Selection.Count),
                "Rationally");
        }

        /// <summary>
        /// Callback called by the UI manager when user clicks a button
        /// Should do something meaningful when corresponding action is called.
        /// </summary>
        public void OnCommand(string commandId)
        {
            switch (commandId)
            {
                case "Command1":
                    Command1();
                    return;

                case "Command2":
                    Command2();
                    return;
            }
        }

        /// <summary>
        /// Callback called by UI manager.
        /// Should return if corresponding command should be enabled in the user interface.
        /// By default, all commands are enabled.
        /// </summary>
        public bool IsCommandEnabled(string commandId)
        {
            switch (commandId)
            {
                case "Command1":    // make command1 always enabled
                    return true;

                case "Command2":    // make command2 enabled only if a drawing is opened
                    return Application != null
                        && Application.ActiveWindow != null
                        && Application.ActiveWindow.Selection.Count > 0;
                default:
                    return true;
            }
        }

        /// <summary>
        /// Callback called by UI manager.
        /// Should return if corresponding command (button) is pressed or not (makes sense for toggle buttons)
        /// </summary>
        public bool IsCommandChecked(string command)
        {
            return false;
        }
        /// <summary>
        /// Callback called by UI manager.
        /// Returns a label associated with given command.
        /// We assume for simplicity taht command labels are named simply named as [commandId]_Label (see resources)
        /// </summary>
        public string GetCommandLabel(string command)
        {
            return Resources.ResourceManager.GetString(command + "_Label");
        }

        /// <summary>
        /// Returns a bitmap associated with given command.
        /// We assume for simplicity that bitmap ids are named after command id.
        /// </summary>
        public Bitmap GetCommandBitmap(string id)
        {
            return (Bitmap)Resources.ResourceManager.GetObject(id);
        }

        internal void UpdateUI()
        {
            AddinUI.UpdateCommandBars();
        }

        private void Application_SelectionChanged(Window window)
        {
            UpdateUI();
        }

        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            var version = int.Parse(Application.Version, NumberStyles.AllowDecimalPoint);
            if (version < 14)
                AddinUI.StartupCommandBars("Rationally", new[] { "Command1", "Command2" });
            Application.SelectionChanged += Application_SelectionChanged;

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

        private void ThisAddIn_Shutdown(object sender, EventArgs e)
        {
            AddinUI.ShutdownCommandBars();
            Application.SelectionChanged -= Application_SelectionChanged;

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


        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            Startup += ThisAddIn_Startup;
            Shutdown += ThisAddIn_Shutdown;
        }

    }
}
