using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using ExtendedVisioAddin1.View;
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

        public IVShape Paint(IVShape alternatives, int alternativeIndex, RModel model)
        {
            


            
            Page activePage = application.ActivePage;
            Window activeWindow = application.ActiveWindow;

            RComponent altComponent = new RComponent(activePage);
            altComponent.RShape = alternatives;

            double ALTERNATIVE_WIDTH = altComponent.Width; //inches

            

            //wrapper for one whole alternative

            //--- define sub parts of the alternative. Only a Selection can be used to fill a container, so empty the current selection of the window and fill it with sub parts
            activeWindow.DeselectAll();

            altComponent.MsvSdContainerLocked = false;

            HeaderlessContainer alternativeContainer = new HeaderlessContainer(activePage);
            //activeWindow.Select(droppedAlternative, (short)VisSelectArgs.visSelect);
            //droppedAlternative.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;

            IVShape droppedAlternative = alternativeContainer.RShape;


            RComponent alternative = new RComponent(activePage);
            alternative.RShape = droppedAlternative;

            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            alternative.RationallyType = "alternative";
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            alternative.AlternativeIndex = alternativeIndex;

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
            altComponent.RShape.ContainerProperties.AddMember(droppedAlternative,VisMemberAddOptions.visMemberAddUseResizeSetting);
            //alternative.CenterY = altComponent.CenterY;

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

            //Events
            droppedAlternative.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            droppedAlternative.CellsU["Actions.Action_1.Action"].Formula = "QUEUEMARKEREVENT(\"deleteAlternative\")";
            droppedAlternative.CellsU["Actions.Action_1.Menu"].Formula = "\"Delete alternative\"";




            //alternatives.ContainerProperties.ResizeAsNeeded = VisContainerAutoResize.visContainerAutoResizeExpandContract;
            //droppedAlternative.AddToContainers();//TODO position alternative above alternatives
            altComponent.MsvSdContainerLocked = true;
            basicDocument.Close();

            return droppedAlternative;
        }
    }
}
