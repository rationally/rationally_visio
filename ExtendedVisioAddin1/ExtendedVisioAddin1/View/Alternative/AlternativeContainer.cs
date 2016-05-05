using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativeContainer : HeaderlessContainer
    {
        
        public AlternativeContainer(Page page, Shape alternative) : base(page)
        {
            RShape = alternative;
            string title = null, state = null, desc = null;
            foreach (int shapeIdentifier in alternative.ContainerProperties.GetMemberShapes(16))
            {
                Shape alternativeComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (alternativeComponent.Name == "AlternativeTitle")
                {
                    AlternativeTitleComponent comp = new AlternativeTitleComponent(page, alternativeComponent);
                    Children.Add(comp);
                    title = comp.Text;
                }
                else if (alternativeComponent.Name == "AlternativeState")
                {
                    AlternativeStateComponent comp = new AlternativeStateComponent(page, alternativeComponent);
                    Children.Add(comp);
                    state = comp.Text;
                }
                else if (alternativeComponent.Name == "AlternativeIdent")
                {
                    Children.Add(new AlternativeIdentifierComponent(page, alternativeComponent));
                }
                else if (alternativeComponent.Name == "AlternativeDescription")
                {
                    AlternativeDescriptionComponent comp = new AlternativeDescriptionComponent(page, alternativeComponent);
                    Children.Add(comp);
                    desc = comp.Text;
                }
            }
            if (title != null && state != null && desc != null)
            {
                Globals.ThisAddIn.model.Alternatives.Add(new Alternative(title, state, desc));
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

            Name = "Alternative";
            AddUserRow("rationallyType");
            RationallyType = "alternative";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;

            //locks
            /*this.LockDelete = true;
            this.LockRotate = true;
            this.LockMoveX = true;
            this.LockMoveY = true;
            this.LockTextEdit = true;*/

            //Events
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"deleteAlternative\")", "\"Delete alternative\"", false);
            InitStyle();
        }

        private void InitStyle()
        {
            SetMargin(0.1);
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;
        }
    }
}
