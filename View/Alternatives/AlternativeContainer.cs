using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeContainer : HeaderlessContainer, IAlternativeComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex AlternativeRegex = new Regex(@"Alternative(\.\d+)?$");
        public AlternativeContainer(Page page, Shape alternative) : base(page, false)
        {
            Shape = alternative;
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
                else if (AlternativeStateShape.IsAlternativeState(alternativeComponent.Name))
                {
                    AlternativeStateShape comp = AlternativeStateShape.CreateFromShape(page, alternativeComponent);
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
            if ((title != null) && (state != null))
            {
                if (Index <= Globals.RationallyAddIn.Model.Alternatives.Count)
                {
                    Alternative newAlternative = new Alternative(title, state, Id);
                    newAlternative.GenerateIdentifier(Index);
                    Globals.RationallyAddIn.Model.Alternatives.Insert(Index, newAlternative);
                    int index = Index;
                    foreach (Alternative alt in Globals.RationallyAddIn.Model.Alternatives.Skip(index + 1).ToList()) //Skip up till and including the new Alternative
                    {
                        alt.GenerateIdentifier(++index);
                    }
                }
                else
                {
                    Alternative newAlternative = new Alternative(title, state, Id);
                    newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
                    Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);

                }
            }
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ShrinkXIfNeeded;
            MarginTop = Index == 0 ? 0.3 : 0.0;

        }

        public AlternativeContainer(Page page, int index, Alternative alternative) : base(page)
        {
            //1) state box
            AlternativeStates alternativeState;
            Enum.TryParse(alternative.Status, out alternativeState);
            AlternativeStateShape stateShape = AlternativeStateShape.CreateWithNewShape(page, index, alternativeState);
            Log.Debug("Created state component");
        
            //2) identifier ("A:")
            string identifier = (char)(65 + index) + ":";
            AlternativeIdentifierComponent identifierComponent = new AlternativeIdentifierComponent(page, index, identifier);
            identifierComponent.ToggleBoldFont(true);
            Log.Debug("created identifier component");
            //3) title
            AlternativeTitleComponent titleComponent = new AlternativeTitleComponent(page, index, alternative.Title);
            Log.Debug("created title component");
            //4) description area
            AlternativeDescriptionComponent descComponent = new AlternativeDescriptionComponent(page, index);
            Log.Debug("created description component");

            Children.Add(identifierComponent);
            Children.Add(titleComponent);
            Children.Add(stateShape);
            Children.Add(descComponent);

            Name = "Alternative";
            AddUserRow("rationallyType");
            RationallyType = "alternative";
            AddUserRow("index");
            Index = index;
            AddUserRow("uniqueId");
            Id = alternative.Id;

            Log.Debug("Done with shapesheet identifying properties");
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
            MarginTop = Index == 0 ? 0.3 : 0.0;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                Shape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            Index = alternativeIndex;
            Children.ForEach(child => ((IAlternativeComponent)child).SetAlternativeIdentifier(alternativeIndex));
            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            if (AlternativeStateShape.IsAlternativeState(s.Name))
            {
                AlternativeStateShape com = AlternativeStateShape.CreateFromShape(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeIdentifierComponent.IsAlternativeIdentifier(s.Name))
            {
                AlternativeIdentifierComponent com = new AlternativeIdentifierComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeTitleComponent.IsAlternativeTitle(s.Name))
            {
                AlternativeTitleComponent com = new AlternativeTitleComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (AlternativeDescriptionComponent.IsAlternativeDescription(s.Name))
            {
                AlternativeDescriptionComponent com = new AlternativeDescriptionComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
        }

        public static bool IsAlternativeContainer(string name) => AlternativeRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (Index == 0)
            {
                DeleteAction("moveUp");
            }

            if (Index == Globals.RationallyAddIn.Model.Alternatives.Count - 1)
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
                    VisioShape c = Children.Find(x => x is AlternativeIdentifierComponent);
                    Children.Remove(c);
                    Children.Insert(0, c);
                }
                if (!(Children[1] is AlternativeTitleComponent))
                {
                    VisioShape c = Children.Find(x => x is AlternativeTitleComponent);
                    Children.Remove(c);
                    Children.Insert(1, c);
                }
                if (!(Children[2] is AlternativeStateShape))
                {
                    VisioShape c = Children.Find(x => x is AlternativeStateShape);
                    Children.Remove(c);
                    Children.Insert(2, c);
                }
            }
            base.Repaint();
        }
    }
}
