using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeShape : HeaderlessContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex AlternativeRegex = new Regex($@"{ShapeNames.Alternative}(\.\d+)?$");

        public override int Index
        {
            get { return base.Index; }
            set {
                base.Index = value;
                MarginTop = Index == 0 ? 0.3 : 0;
            }
        }
        public AlternativeShape(Page page, Shape alternative) : base(page, false)
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
                else if (AlternativeIdentifierShape.IsAlternativeIdentifier(alternativeComponent.Name))
                {
                    Children.Add(new AlternativeIdentifierShape(page, alternativeComponent));
                }
                else if (AlternativeDescriptionShape.IsAlternativeDescription(alternativeComponent.Name))
                {
                    AlternativeDescriptionShape comp = new AlternativeDescriptionShape(page, alternativeComponent);
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

        /*public static AlternativeShape CreateWithNewShape(Page page, int index, Alternative alternative)
        {
            AlternativeShape alternativeShape = new AlternativeShape(page, index, alternative);

            string title = null, state = null;
            foreach (int shapeIdentifier in alternativeShape.Shape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested))
            {
                Shape alternativeComponent = page.Shapes.ItemFromID[shapeIdentifier];
                if (AlternativeTitleComponent.IsAlternativeTitle(alternativeComponent.Name))
                {
                    AlternativeTitleComponent comp = new AlternativeTitleComponent(page, alternativeComponent);
                    alternativeShape.Children.Add(comp);
                    title = comp.Text;
                }
                else if (AlternativeStateShape.IsAlternativeState(alternativeComponent.Name))
                {
                    AlternativeStateShape comp = AlternativeStateShape.CreateFromShape(page, alternativeComponent);
                    alternativeShape.Children.Add(comp);
                    state = comp.Text;
                }
                else if (AlternativeIdentifierShape.IsAlternativeIdentifier(alternativeComponent.Name))
                {
                    alternativeShape.Children.Add(new AlternativeIdentifierShape(page, alternativeComponent));
                }
                else if (AlternativeDescriptionComponent.IsAlternativeDescription(alternativeComponent.Name))
                {
                    AlternativeDescriptionComponent comp = new AlternativeDescriptionComponent(page, alternativeComponent);
                    alternativeShape.Children.Add(comp);
                }
            }
            if ((title != null) && (state != null))
            {
                if (alternativeShape.Index <= Globals.RationallyAddIn.Model.Alternatives.Count)
                {
                    Alternative newAlternative = new Alternative(title, state, alternativeShape.Id);
                    newAlternative.GenerateIdentifier(alternativeShape.Index);
                    Globals.RationallyAddIn.Model.Alternatives.Insert(alternativeShape.Index, newAlternative);
                    int indexCounter = alternativeShape.Index;
                    foreach (Alternative alt in Globals.RationallyAddIn.Model.Alternatives.Skip(indexCounter + 1).ToList()) //Skip up till and including the new Alternative
                    {
                        alt.GenerateIdentifier(++indexCounter);
                    }
                }
                else
                {
                    Alternative newAlternative = new Alternative(title, state, alternativeShape.Id);
                    newAlternative.GenerateIdentifier(Globals.RationallyAddIn.Model.Alternatives.Count);
                    Globals.RationallyAddIn.Model.Alternatives.Add(newAlternative);

                }
            }
            alternativeShape.UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkYIfNeeded | SizingPolicy.ShrinkXIfNeeded;
            alternativeShape.MarginTop = alternativeShape.Index == 0 ? 0.3 : 0.0;

            return alternativeShape;
        }*/

        public AlternativeShape(Page page, int index, Alternative alternative) : base(page)
        {
            //1) state box
            AlternativeState alternativeState;
            Enum.TryParse(alternative.Status, out alternativeState);
            AlternativeStateShape stateShape = AlternativeStateShape.CreateWithNewShape(page, index, alternativeState);
            Log.Debug("Created state component");
        
            //2) identifier ("A:")
            string identifier = (char)(65 + index) + ":";
            AlternativeIdentifierShape identifierShape = new AlternativeIdentifierShape(page, index, identifier);
            identifierShape.ToggleBoldFont(true);
            Log.Debug("created identifier component");
            //3) title
            AlternativeTitleComponent titleComponent = new AlternativeTitleComponent(page, index, alternative.Title);
            Log.Debug("created title component");
            //4) description area
            AlternativeDescriptionShape descShape = new AlternativeDescriptionShape(page, index);
            Log.Debug("created description component");

            Children.Add(identifierShape);
            Children.Add(titleComponent);
            Children.Add(stateShape);
            Children.Add(descShape);

            Name = ShapeNames.Alternative;
            RationallyType = ShapeNames.TypeAlternative;
            Index = index;
            Id = alternative.Id;

            Log.Debug("Done with shapesheet identifying properties");
            //locks
            MsvSdContainerLocked = true;

            //Events
            AddAction("deleteAlternative", string.Format(VisioFormulas.Formula_QUEUMARKEREVENT,"delete"), Messages.Menu_DeleteAlternative, false);

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
            else if (AlternativeIdentifierShape.IsAlternativeIdentifier(s.Name))
            {
                AlternativeIdentifierShape com = new AlternativeIdentifierShape(Page, s);
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
            else if (AlternativeDescriptionShape.IsAlternativeDescription(s.Name))
            {
                AlternativeDescriptionShape com = new AlternativeDescriptionShape(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
        }

        public static bool IsAlternativeContainer(string name) => AlternativeRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio takes care of this
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
            }
            if (Children.Count == 4)
            {
                if (!(Children[0] is AlternativeIdentifierShape))
                {
                    VisioShape c = Children.Find(x => x is AlternativeIdentifierShape);
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
