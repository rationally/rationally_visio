﻿using System;
using System.Windows.Forms;
using rationally_visio;
using Microsoft.Office.Interop.Visio;
using ExtendedVisioAddin1.EventHandlers;
using ExtendedVisioAddin1.Model;

namespace ExtendedVisioAddin1
{
    public partial class ThisAddIn
    {
        private string author;
        private string decision;
        private string header;
        private Document rationallyDocument;
        private RModel model;


        private void ThisAddIn_Startup(object sender, EventArgs e)
        {
            model = new RModel();

            //ShowMyDialogBox();
            //MessageBox.Show(decision + " by " + author +" with header " + header);
            Application.MarkerEvent += new EApplication_MarkerEventEventHandler(Application_MarkerEvent);
            Application.TemplatePaths = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\";
            Application.DocumentCreated += new EApplication_DocumentCreatedEventHandler(DelegateCreateDocumentEvent);
            Application.DocumentOpened += new EApplication_DocumentOpenedEventHandler(Application_DocumentOpenedEvent);

            Documents visioDocs = this.Application.Documents;

            




            Page activePage = this.Application.ActivePage;

           

            //add a header to the page
            /*Shape headerShape = activePage.DrawRectangle(0.1, 8, 5, 8);
            //headerShape.TextStyle = "Basic";
            headerShape.LineStyle = "Text Only";
            headerShape.FillStyle = "Text Only";
            headerShape.Text = "Deployment of Step 2 and Step 34";
            headerShape.Characters.Text = "Deployment of Step 2 and Step 3";
            headerShape.Characters.CharProps[(short)VisCellIndices.visCharacterSize] = 22;
            headerShape.CellsSRC[(short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowLine, (short)VisCellIndices.visLinePattern].ResultIU = 0;*/

            //descriptionContainer.SetBegin(100, 100);
            /*foreach (Shape shape in activePage.Shapes)
            {
                var x = shape.CellExistsU["type", 0];
                var y = shape.CellExistsU["type", 1];
            }*/

            /*Master forcesMaster = rationallyDocument.Masters.ItemU[@"Forces"];
            Shape forceShape = activePage.Drop(forcesMaster, 4, 3);
            var a = forceShape.CellsU["User.rationallyType"];
            string forcesType = forceShape.CellsU["User.rationallyType"].ResultStr["value"];*/

            //activePage.DropContainer(containerDocument.Masters.ItemU["Alternating"], forceShape);
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
                    Master forcesMaster = rationallyDocument.Masters.ItemU[@"Force"];

                    s.Drop(forcesMaster, 1, 1);
                } else if (s.CellsU["User.rationallyType"].ResultStr["Value"] == "alternatives")
                {
                    AddAlternativeEventHandler a = new AddAlternativeEventHandler(model);
                }
            }
        }



        private void Application_DocumentOpenedEvent(IVDocument d)
        {
            //if (d.Template.ToLower().Contains("rationally"))
            //{
                string docPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + @"\My Shapes\DecisionsStencil.vssx";
                rationallyDocument = this.Application.Documents.OpenEx(docPath,
    ((short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visAddDocked));

                //Document containerDocument = Application.Documents.OpenEx(Application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers,
                 //       VisMeasurementSystem.visMSUS), (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);

                //ShowMyDialogBox();
            //}
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
        }
    }
}
