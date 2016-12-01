using System.Linq;
using System.Text.RegularExpressions;
using Rationally.Visio.Model;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeContainer : HeaderlessContainer, IAlternativeComponent
    {
        private static readonly Regex AlternativeRegex = new Regex(@"Alternative(\.\d+)?$");
        public AlternativeContainer(Page page, Shape alternative) : base(page, false)
        {
            RShape = alternative;
            string title = null, state = null;
            foreach (int shapeIdentifier in alternative.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested))
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
                }
            }
            if (title != null && state != null)
            {
                if (AlternativeIndex <= Globals.RationallyAddIn.Model.Alternatives.Count)
                {
                    Alternative newAlternative = new Alternative(title, state, UniqueIdentifier);
                    newAlternative.GenerateIdentifier(AlternativeIndex);
                    Globals.RationallyAddIn.Model.Alternatives.Insert(AlternativeIndex, newAlternative);
                    int index = AlternativeIndex;
                    foreach (Alternative alt in Globals.RationallyAddIn.Model.Alternatives.Skip(index + 1).ToList()) //Skip up till and including the new Alternative
                    {
                        alt.GenerateIdentifier(++index);
                    }
                }
                else
                {
                    Alternative newAlternative = new Alternative(title, state, UniqueIdentifier);
                    newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
                    Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);

                }
            }
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ShrinkXIfNeeded;
            MarginTop = AlternativeIndex == 0 ? 0.3 : 0.0;

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
            AlternativeDescriptionComponent descComponent = new AlternativeDescriptionComponent(page, alternativeIndex);
            
            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            Children.Add(stateComponent);
            Children.Add(descComponent);

            Name = "Alternative";
            AddUserRow("rationallyType");
            RationallyType = "alternative";
            AddUserRow("alternativeIndex");
            AlternativeIndex = alternativeIndex;
            AddUserRow("uniqueId");
            UniqueIdentifier = alternative.UniqueIdentifier;

            //locks
            MsvSdContainerLocked = true;

            //Events
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete alternative\"", false);

            LinePattern = 16;
            InitStyle();
        }

        private void InitStyle()
        {
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ShrinkXIfNeeded;
            MarginTop = AlternativeIndex == 0 ? 0.3 : 0.0;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
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

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (AlternativeIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (AlternativeIndex == Globals.RationallyAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio takes care of this
            {
                UpdateReorderFunctions();
            }
            if (Children.Count == 4)
            {
                if (!(Children[0] is AlternativeIdentifierComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeIdentifierComponent);
                    Children.Remove(c);
                    Children.Insert(0, c);
                }
                if (!(Children[1] is AlternativeTitleComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeTitleComponent);
                    Children.Remove(c);
                    Children.Insert(1, c);
                }
                if (!(Children[2] is AlternativeStateComponent))
                {
                    RationallyComponent c = Children.Find(x => x is AlternativeStateComponent);
                    Children.Remove(c);
                    Children.Insert(2, c);
                }
            }
            base.Repaint();
        }
    }
}
