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
            foreach (int shapeIdentifier in alternative.ContainerProperties.GetMemberShapes(16))
            {
                Shape alternativeComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (alternativeComponent.Name == "AlternativeTitle")
                {
                    this.Children.Add(new AlternativeTitleComponent(page, alternativeComponent));
                }
                else if (alternativeComponent.Name == "AlternativeState")
                {
                    this.Children.Add(new AlternativeStateComponent(page, alternativeComponent));
                }
                else if (alternativeComponent.Name == "AlternativeIdent")
                {
                    this.Children.Add(new AlternativeIdentifierComponent(page, alternativeComponent));
                }
                else if (alternativeComponent.Name == "AlternativeDescription")
                {
                    this.Children.Add(new AlternativeDescriptionComponent(page, alternativeComponent));
                }
            }
            InitStyle();
        }

        public AlternativeContainer(Page page, int alternativeIndex, Alternative alternative) : base(page)
        {
            //1) state box
            AlternativeStateComponent stateComponent = new AlternativeStateComponent(page, alternativeIndex, alternative.Status);

            //2) identifier ("A:")
            string identifier = (char)(65 + alternativeIndex) + ":";
            AlternativeIdentifierComponent identifierComponent = new AlternativeIdentifierComponent(page, alternativeIndex, identifier);
            identifierComponent.ToggleBoldFont(true);

            //3) title
            AlternativeTitleComponent titleComponent = new AlternativeTitleComponent(page, alternativeIndex, alternative.Title);

            //4) description area
            AlternativeDescriptionComponent descComponent = new AlternativeDescriptionComponent(page, alternativeIndex, alternative.Description);

            Children.Add(stateComponent);
            //this.RShape.ContainerProperties.AddMember(stateComponent.RShape,VisMemberAddOptions.visMemberAddDoNotExpand);
            Children.Add(identifierComponent);
            //this.RShape.ContainerProperties.AddMember(identifierComponent.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);
            Children.Add(titleComponent);
            //this.RShape.ContainerProperties.AddMember(titleComponent.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);
            Children.Add(descComponent);
            //this.RShape.ContainerProperties.AddMember(descComponent.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);

            this.Name = "Alternative";
            this.AddUserRow("rationallyType");
            this.RationallyType = "alternative";
            this.AddUserRow("alternativeIndex");
            this.AlternativeIndex = alternativeIndex;

            //locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;
            this.LockTextEdit = true;*/

            //Events
            this.AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"deleteAlternative\")", "\"Delete alternative\"", false);
            InitStyle();
        }

        private void InitStyle()
        {
            this.SetMargin(0.1);
            this.UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;
        }
    }
}
