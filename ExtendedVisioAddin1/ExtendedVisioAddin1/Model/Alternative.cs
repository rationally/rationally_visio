﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.Model
{
    internal class Alternative
    {
        private const double MARGIN = 0.1;
        private const double STATUS_WIDTH = 2;
        private const double IDENTIFIER_WIDTH = 0.4;
        private const double DESCRIPTION_HEIGHT = 2;
        private const double TOP_ROW_HEIGHT = 0.5;
        public static double ALTERNATIVE_HEIGHT => TOP_ROW_HEIGHT + MARGIN + DESCRIPTION_HEIGHT + 1;

        public string Description { get; set; }

        public string Status { get; set; }

        public string Title { get; set; }

        public Alternative(string title, string status, string description)
        {
            this.Title = title;
            this.Status = status;
            this.Description = description;
        }

        public Shape Paint(IVShape alternatives, int alternativeIndex)
        {
            RationallyComponent altComponent = new RationallyComponent(alternatives);

            double ALTERNATIVE_WIDTH = altComponent.Width; //inches

            Application application = Globals.ThisAddIn.Application;
            Page activePage = application.ActivePage;
            Window activeWindow = application.ActiveWindow;
            
            Document containerDocument = application.Documents.OpenEx(application.GetBuiltInStencilFile(VisBuiltInStencilTypes.visBuiltInStencilContainers, VisMeasurementSystem.visMSUS), (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short) Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);

            Master alternativeMaster = containerDocument.Masters["Plain"];//wrapper for one whole alternative

            //--- define sub parts of the alternative. Only a Selection can be used to fill a container, so empty the current selection of the window and fill it with sub parts
            activeWindow.DeselectAll();

            //1) state box
            Master stateRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape stateRectangle = activePage.Drop(stateRectangleMaster, (STATUS_WIDTH / 2), 0);
            RationallyComponent stateComponent = new RationallyComponent(stateRectangle);
            stateComponent.Width = STATUS_WIDTH;
            stateComponent.Height = TOP_ROW_HEIGHT;
            stateComponent.Text = this.Status;
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            stateComponent.RationallyType = "alternativeState";
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            stateComponent.AlternativeIndex = alternativeIndex;
            activeWindow.Select(stateRectangle, (short)VisSelectArgs.visSelect);

            //Events
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            stateRectangle.CellsU["Actions.Action_1.Action"].Formula = "QUEUEMARKEREVENT(\"editState\")";
            stateRectangle.CellsU["Actions.Action_1.Menu"].Formula = "\"Edit state\"";

            


            //2) identifier ("A:")
            string identifier = (char) (65 + alternativeIndex) + "";
            Master identifierRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape identifierRectangle = activePage.Drop(identifierRectangleMaster, STATUS_WIDTH + MARGIN + (IDENTIFIER_WIDTH/2), 0);
            RationallyComponent identifierComponent = new RationallyComponent(identifierRectangle);
            identifierComponent.ToggleBoldFont(true);
            identifierComponent.Text = identifier + ":";
            identifierComponent.Width = IDENTIFIER_WIDTH;
            identifierComponent.Height = TOP_ROW_HEIGHT;
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            identifierComponent.RationallyType = "alternativeIdentifier";
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            identifierComponent.AlternativeIndex = alternativeIndex;

            

            



            //3) title
            double TITLE_WIDTH = ALTERNATIVE_WIDTH - (STATUS_WIDTH + MARGIN + IDENTIFIER_WIDTH + MARGIN);
            Master titleRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape titleRectangle = activePage.Drop(titleRectangleMaster, STATUS_WIDTH + MARGIN + IDENTIFIER_WIDTH + MARGIN + (TITLE_WIDTH/2), 0);
            RationallyComponent titleComponent = new RationallyComponent(titleRectangle);
            titleComponent.Width = TITLE_WIDTH;
            titleComponent.Height = TOP_ROW_HEIGHT;
            titleComponent.Text = this.Title;
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            titleComponent.RationallyType = "alternativeTitle";
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            titleComponent.AlternativeIndex = alternativeIndex;
            

            

            //4) description area
            Master descRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape descRectangle = activePage.Drop(descRectangleMaster, ALTERNATIVE_WIDTH/2, -((TOP_ROW_HEIGHT/2) + MARGIN + (DESCRIPTION_HEIGHT/2)));
            RationallyComponent descComponent = new RationallyComponent(descRectangle);
            descComponent.Width = ALTERNATIVE_WIDTH;
            descComponent.Height = DESCRIPTION_HEIGHT;
            descComponent.Text = this.Description;
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            descComponent.RationallyType = "alternativeDescription";
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            descComponent.AlternativeIndex = alternativeIndex;

            

            altComponent.MsvSdContainerLocked = false;
            Shape droppedAlternative = activePage.DropContainer(alternativeMaster, null);//altComponent.CenterX, altComponent.CenterY
            //activeWindow.Select(droppedAlternative, (short)VisSelectArgs.visSelect);
            //droppedAlternative.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            RationallyComponent alternative = new RationallyComponent(droppedAlternative);

            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            alternative.RationallyType = "alternative";
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            alternative.AlternativeIndex = alternativeIndex;

            
            //droppedAlternative.CellsU["User.msvSDHeadingStyle"].ResultIU = 0;//TODO encap in rat com

            droppedAlternative.CellsU["User.msvSDHeadingStyle"].ResultIU = 0;//TODO encap in rat com
            droppedAlternative.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            activeWindow.Select(stateRectangle, (short)VisSelectArgs.visSelect);
            activeWindow.Select(identifierRectangle, (short)VisSelectArgs.visSelect);
            activeWindow.Select(titleRectangle, (short)VisSelectArgs.visSelect);
            activeWindow.Select(descRectangle, (short)VisSelectArgs.visSelect);

            droppedAlternative.ContainerProperties.AddMember(stateRectangle, VisMemberAddOptions.visMemberAddExpandContainer);
            droppedAlternative.ContainerProperties.AddMember(identifierRectangle, VisMemberAddOptions.visMemberAddExpandContainer);
            droppedAlternative.ContainerProperties.AddMember(titleRectangle, VisMemberAddOptions.visMemberAddExpandContainer);
            droppedAlternative.ContainerProperties.AddMember(descRectangle, VisMemberAddOptions.visMemberAddExpandContainer);

            activeWindow.Selection.Move(altComponent.CenterX - altComponent.Width/2, altComponent.CenterY + altComponent.Height/2 - alternativeIndex * Alternative.ALTERNATIVE_HEIGHT);
            altComponent.Shape1.ContainerProperties.AddMember(droppedAlternative,VisMemberAddOptions.visMemberAddUseResizeSetting);
            //alternative.CenterY = altComponent.CenterY;

            //locks
            stateComponent.LockDelete = true;
            stateComponent.LockRotate = true;
            stateComponent.LockMoveX = true;
            stateComponent.LockMoveY = true;
            stateComponent.LockHeight = true;
            stateComponent.LockTextEdit = true;
            stateComponent.LockWidth = true;

            identifierComponent.LockDelete = true;
            identifierComponent.LockRotate = true;
            identifierComponent.LockMoveX = true;
            identifierComponent.LockMoveY = true;
            identifierComponent.LockHeight = true;
            identifierComponent.LockTextEdit = true;
            identifierComponent.LockWidth = true;

            titleComponent.LockDelete = true;
            titleComponent.LockRotate = true;
            titleComponent.LockMoveX = true;
            titleComponent.LockMoveY = true;

            alternative.LockDelete = true;
            alternative.LockRotate = true;
            alternative.LockMoveX = true;
            alternative.LockMoveY = true;
            alternative.LockTextEdit = true;

            descComponent.LockDelete = true;
            descComponent.LockRotate = true;
            descComponent.LockMoveX = true;
            descComponent.LockMoveY = true;

            //Events
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            droppedAlternative.CellsU["Actions.Action_1.Action"].Formula = "QUEUEMARKEREVENT(\"deleteAlternative\")";
            droppedAlternative.CellsU["Actions.Action_1.Menu"].Formula = "\"Delete alternative\"";


            //alternatives.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            //droppedAlternative.AddToContainers();//TODO position alternative above alternatives
            altComponent.MsvSdContainerLocked = true;
            basicDocument.Close();
            containerDocument.Close();

            return droppedAlternative;
        }
    }
}
