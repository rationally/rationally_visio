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
        public AlternativeContainer(Page page, Shape alternative) : base(page)
        {
            this.RShape = alternative;
            foreach (int shapeIdentifier in alternative.ContainerProperties.GetMemberShapes(0))
            {
                Shape alternativeComponent = page.Shapes.ItemFromID[shapeIdentifier];
                //this.Children.Add(new AlternativeContainer(page, alternative));
            }
        }

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

            Children.Add(stateComponent);
            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            Children.Add(descComponent);

            this.AddUserRow("rationallyType");
            this.RationallyType = "alternative";
            this.AddUserRow("alternativeIndex");
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
