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
            //AlternativeStateComponent stateComponent = new AlternativeStateComponent(page, alternativeIndex, alternative.Status);

            //2) identifier ("A:")
            string identifier = (char)(65 + alternativeIndex) + "";
            AlternativeIdentifierComponent identifierComponent = new AlternativeIdentifierComponent(page, alternativeIndex, identifier);
            identifierComponent.ToggleBoldFont(true);

            //3) title
            AlternativeTitleComponent titleComponent = new AlternativeTitleComponent(page, alternativeIndex, alternative.Title);

            //4) description area
            //AlternativeDescriptionComponent descComponent = new AlternativeDescriptionComponent(page, alternativeIndex, alternative.Description);

            //Children.Add(stateComponent);
            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            //Children.Add(descComponent);

            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "rationallyType", (short)VisRowTags.visTagDefault);
            this.RationallyType = "alternative";
            this.RShape.AddNamedRow((short)VisSectionIndices.visSectionUser, "alternativeIndex", (short)VisRowTags.visTagDefault);
            this.AlternativeIndex = alternativeIndex;

            //locks
            this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;
            this.LockTextEdit = true;

            //Events
            this.AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"deleteAlternative\")", "\"Delete alternative\"", false);
        }
    }
}
