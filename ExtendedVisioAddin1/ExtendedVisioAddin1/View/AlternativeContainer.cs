using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeContainer : HeaderlessContainer
    {
        public AlternativeContainer(Page page, int alternativeIndex, Alternative alternative, RModel model, Window window) : base(page)
        {
            Application application = Globals.ThisAddIn.Application;
            Document basicDocument = application.Documents.OpenEx("Basic Shapes.vss", (short)Microsoft.Office.Interop.Visio.VisOpenSaveArgs.visOpenHidden);
            //1) state box
            Master stateRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape stateRectangle = page.Drop(stateRectangleMaster, (STATUS_WIDTH / 2), 0);
            RComponent stateComponent = new AlternativeStateComponent(page, alternative.Status);
            stateComponent.RShape = stateRectangle;

            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            stateComponent.RationallyType = "alternativeState";
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            stateComponent.AlternativeIndex = alternativeIndex;
            window.Select(stateRectangle, (short)VisSelectArgs.visSelect);

            //Events
            stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            stateRectangle.CellsU["Actions.Action_1.Action"].Formula = "";
            stateRectangle.CellsU["Actions.Action_1.Menu"].Formula = "\"Change state\"";
            stateRectangle.CellsU["Actions.Action_1.FlyoutChild"].Formula = "FALSE";

            for (int i = 0; i < model.AlternativeStates.Count; i++)
            {
                string stateName = "State_" + i;
                stateRectangle.AddNamedRow((short)VisSectionIndices.visSectionAction, stateName, (short)VisRowTags.visTagDefault);
                stateRectangle.CellsU["Actions." + stateName + ".Action"].Formula = "QUEUEMARKEREVENT(\"stateChange." + model.AlternativeStates[i] + "\")";
                stateRectangle.CellsU["Actions." + stateName + ".Menu"].Formula = "\"" + model.AlternativeStates[i] + "\"";
                stateRectangle.CellsU["Actions." + stateName + ".FlyoutChild"].Formula = "TRUE";
            }


            //2) identifier ("A:")
            string identifier = (char)(65 + alternativeIndex) + "";
            Master identifierRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape identifierRectangle = page.Drop(identifierRectangleMaster, STATUS_WIDTH + MARGIN + (IDENTIFIER_WIDTH / 2), 0);
            RComponent identifierComponent = new TextLabel(page, identifier);
            identifierComponent.RShape = identifierRectangle;
            identifierComponent.ToggleBoldFont(true);

            //events
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            identifierComponent.RationallyType = "alternativeIdentifier";
            identifierRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            identifierComponent.AlternativeIndex = alternativeIndex;

            //3) title
            double TITLE_WIDTH = ALTERNATIVE_WIDTH - (STATUS_WIDTH + MARGIN + IDENTIFIER_WIDTH + MARGIN);
            Master titleRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape titleRectangle = page.Drop(titleRectangleMaster, STATUS_WIDTH + MARGIN + IDENTIFIER_WIDTH + MARGIN + (TITLE_WIDTH / 2), 0);

            RComponent titleComponent = new TextLabel(page, alternative.Title);
            titleComponent.RShape = titleRectangle;

            //events
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            titleComponent.RationallyType = "alternativeTitle";
            titleRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            titleComponent.AlternativeIndex = alternativeIndex;

            //4) description area
            Master descRectangleMaster = basicDocument.Masters["Rectangle"];
            Shape descRectangle = page.Drop(descRectangleMaster, ALTERNATIVE_WIDTH / 2, -((TOP_ROW_HEIGHT / 2) + MARGIN + (DESCRIPTION_HEIGHT / 2)));

            RComponent descComponent = new RComponent(page);
            descComponent.RShape = descRectangle;

            descComponent.Width = ALTERNATIVE_WIDTH;
            descComponent.Height = DESCRIPTION_HEIGHT;
            descComponent.Text = alternative.Description;

            //events
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            descComponent.RationallyType = "alternativeDescription";
            descRectangle.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            descComponent.AlternativeIndex = alternativeIndex;


            //locks
            /*stateComponent.LockDelete = true;
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
            descComponent.LockMoveY = true;*/


            Children.Add(stateComponent);
            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            Children.Add(descComponent);
            basicDocument.Close();
        }
    }
}
