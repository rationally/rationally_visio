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

        public AlternativeContainer(Page page, int alternativeIndex, Alternative alternative) : base(page)
        {
            
            
            //1) state box
            AlternativeStateComponent stateComponent = new AlternativeStateComponent(page, alternativeIndex, alternative.Status);

            //2) identifier ("A:")
            string identifier = (char)(65 + alternativeIndex) + "";
            AlternativeIdentifierComponent identifierComponent = new AlternativeIdentifierComponent(page, alternativeIndex, identifier);
            identifierComponent.ToggleBoldFont(true);

            //3) title
            AlternativeTitleComponent titleComponent = new AlternativeTitleComponent(page, alternativeIndex, alternative.Title);

            //4) description area
            AlternativeDescriptionComponent descComponent = new AlternativeDescriptionComponent(page, alternativeIndex, alternative.Description);


            


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
            

            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternative";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

            //Events
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionAction, "Action_1", (short)VisRowTags.visTagDefault);
            this.RShape.CellsU["Actions.Action_1.Action"].Formula = "QUEUEMARKEREVENT(\"deleteAlternative\")";
            this.RShape.CellsU["Actions.Action_1.Menu"].Formula = "\"Delete alternative\"";
        }
    }
}
