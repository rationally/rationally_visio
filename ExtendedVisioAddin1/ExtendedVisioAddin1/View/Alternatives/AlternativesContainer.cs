﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Alternatives
{
    public class AlternativesContainer : RContainer
    {
        private static readonly Regex AlternativesRegex = new Regex(@"Alternatives(\.\d+)?$");
        
        public AlternativesContainer(Page page, Shape alternativesContainer) : base(page)
        {
            RShape = alternativesContainer;
            Array ident = alternativesContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = new List<int>((int[])ident).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => AlternativeContainer.IsAlternativeContainer(shape.Name)))
            {
                Children.Add(new AlternativeContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.AlternativeIndex).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        private void InitStyle()
        {
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            UsedSizingPolicy = SizingPolicy.ExpandYIfNeeded;
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RComponent shapeComponent = new RComponent(Page);
            shapeComponent.RShape = s;

            if (AlternativeContainer.IsAlternativeContainer(s.Name))
            {
                if (Children.All(c => c.AlternativeIndex != shapeComponent.AlternativeIndex)) //there is no forcecontainer stub with this index
                {
                    Children.Add(new AlternativeContainer(Page, s));
                }
                else
                {
                    //remove stub, insert s as new containers
                    AlternativeContainer stub = (AlternativeContainer) Children.First(c => c.AlternativeIndex == shapeComponent.AlternativeIndex);
                    Children.Remove(stub);
                        stub.RShape.Delete(); //NOT deleteEx
                    AlternativeContainer con = new AlternativeContainer(Page, s);
                    Children.Insert(con.AlternativeIndex, con);
                    Globals.ThisAddIn.Application.EndUndoScope(Globals.ThisAddIn.StartedUndoState, true);
                }
            }
            else
            {
                bool isAlternativeChild = AlternativeStateComponent.IsAlternativeState(s.Name) || AlternativeIdentifierComponent.IsAlternativeIdentifier(s.Name) || AlternativeTitleComponent.IsAlternativeTitle(s.Name) || AlternativeDescriptionComponent.IsAlternativeDescription(s.Name);

                if (isAlternativeChild && Children.All(c => c.AlternativeIndex != shapeComponent.AlternativeIndex)) //if parent not exists
                {
                    Globals.ThisAddIn.StartedUndoState = Globals.ThisAddIn.Application.BeginUndoScope("test");
                    AlternativeContainer stub = AlternativeContainer.GetStub(Page, shapeComponent.AlternativeIndex);
                    Children.Insert(stub.AlternativeIndex, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }

        public static bool IsAlternativesContainer(string name)
        {
            return AlternativesRegex.IsMatch(name);
        }

        public void RemoveAlternativesFromModel()
        {
            //Get a list of all alternativeIndices
            List<int> indexList = Children.Select(alternative => alternative.AlternativeIndex).ToList();
            indexList.Sort();
            indexList.Reverse(); //Reverse so indices don't change
            foreach (int index in indexList)
            {
                Globals.ThisAddIn.Model.Alternatives.RemoveAt(index);
            }
        }

    }
}
