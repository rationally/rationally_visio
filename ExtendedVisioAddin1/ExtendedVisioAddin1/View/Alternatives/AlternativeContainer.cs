using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using ExtendedVisioAddin1.View.Forces;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    internal class AlternativeContainer : HeaderlessContainer, IAlternativeComponent
    {
        private static readonly Regex AlternativeRegex = new Regex(@"Alternative(\.\d+)?$");
        public AlternativeContainer(Page page, Shape alternative) : base(page, false)
        {
            RShape = alternative;
            string title = null, state = null, desc = null;
            foreach (int shapeIdentifier in alternative.ContainerProperties.GetMemberShapes(16))
            {
                Shape alternativeComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (AlternativeTitleComponent.IsAlternativeTitle(alternativeComponent.Name))
                {
                    AlternativeTitleComponent comp = new AlternativeTitleComponent(page, alternativeComponent);
                    Children.Add(comp);
                    title = comp.Text;
                }
                else if (AlternativeStateComponent.IsAlternativeState(alternativeComponent.Name))
                {
                    AlternativeStateComponent comp = new AlternativeStateComponent(page, alternativeComponent);
                    Children.Add(comp);
                    state = comp.Text;
                }
                else if (AlternativeIdentifierComponent.IsAlternativeIdentifier(alternativeComponent.Name))
                {
                    Children.Add(new AlternativeIdentifierComponent(page, alternativeComponent));
                }
                else if (AlternativeDescriptionComponent.IsAlternativeDescription(alternativeComponent.Name))
                {
                    AlternativeDescriptionComponent comp = new AlternativeDescriptionComponent(page, alternativeComponent);
                    Children.Add(comp);
                    desc = comp.Text;
                }
            }
            if (title != null && state != null && desc != null)
            {
                string identifier = (char)(65 + Globals.ThisAddIn.Model.Alternatives.Count) + ":";
                if (AlternativeIndex <= Globals.ThisAddIn.Model.Alternatives.Count)
                {
                    Globals.ThisAddIn.Model.Alternatives.Insert(AlternativeIndex, new Alternative(title, state, desc, identifier));
                }
                else
                {
                    Globals.ThisAddIn.Model.Alternatives.Add(new Alternative(title, state, desc, identifier));
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
            
            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            Children.Add(stateComponent);
            Children.Add(descComponent);

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
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete alternative\"", false);
            InitStyle();
        }

        public AlternativeContainer(Page page, int alternativeIndex) : base(page)
        {

           

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
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete alternative\"", false);
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded;
            MarginTop = (AlternativeIndex == 0) ? 0.3 : 0.0;
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            LinePattern = 16;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            AlternativeIndex = alternativeIndex;
            Children.ForEach(child => ((IAlternativeComponent)child).SetAlternativeIdentifier(alternativeIndex));
            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            if (AlternativeStateComponent.IsAlternativeState(s.Name))
            {
                AlternativeStateComponent com = new AlternativeStateComponent(Page, s);
                if (com.AlternativeIndex == AlternativeIndex)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeIdentifierComponent.IsAlternativeIdentifier(s.Name))
            {
                AlternativeIdentifierComponent com = new AlternativeIdentifierComponent(Page, s);
                if (com.AlternativeIndex == AlternativeIndex)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeTitleComponent.IsAlternativeTitle(s.Name))
            {
                AlternativeTitleComponent com = new AlternativeTitleComponent(Page, s);
                if (com.AlternativeIndex == AlternativeIndex)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeDescriptionComponent.IsAlternativeDescription(s.Name))
            {
                AlternativeDescriptionComponent com = new AlternativeDescriptionComponent(Page, s);
                if (com.AlternativeIndex == AlternativeIndex)
                {
                    Children.Add(com);
                }
            }
        }

        public static bool IsAlternativeContainer(string name)
        {
            return AlternativeRegex.IsMatch(name);
        }

        public override void Repaint()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (AlternativeIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (AlternativeIndex == Globals.ThisAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }

            base.Repaint();
        }

        /// <summary>
        /// Returns a stub ForceContainer. This shape can be deleted without any bavaviour being triggered.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="alternativeIndex"></param>
        /// <returns></returns>
        public static AlternativeContainer GetStub(Page page, int alternativeIndex)
        {
            AlternativeContainer stub = new AlternativeContainer(page, alternativeIndex);
            stub.AddUserRow("isStub");
            stub.IsStub = "true";
            return stub;
        }
    }
}
