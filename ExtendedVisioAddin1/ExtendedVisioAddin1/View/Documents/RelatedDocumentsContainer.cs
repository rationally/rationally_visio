﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    class RelatedDocumentsContainer : RContainer
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Documents(\.\d+)?$");
        
        public RelatedDocumentsContainer(Page page, Shape relatedDocumentsContainer) : base(page)
        {
            RShape = relatedDocumentsContainer;
            Array ident = relatedDocumentsContainer.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => RelatedDocumentContainer.IsRelatedDocumentContainer(shape.Name)))
            {
                Children.Add(new RelatedDocumentContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.DocumentIndex).ToList();
            LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RComponent shapeComponent = new RComponent(Page);
            shapeComponent.RShape = s;

            if (RelatedDocumentContainer.IsRelatedDocumentContainer(s.Name))
            {
                if (Children.All(c => c.DocumentIndex != shapeComponent.DocumentIndex)) //there is no container stub with this index
                {
                    RelatedDocumentContainer con = new RelatedDocumentContainer(Page, s);
                    Children.Insert(con.DocumentIndex, con);
                }
                else
                {
                    //remove stub, insert s as the shape of the stub wrapper
                    RelatedDocumentContainer stub = (RelatedDocumentContainer)Children.First(c => c.DocumentIndex == shapeComponent.DocumentIndex);
                    Children.Remove(stub);
                    stub.RShape.Delete(); //NOT deleteEx
                    Children.Add(new RelatedDocumentContainer(Page, s));
                }

                
            }
            else
            {
                bool isDocumentChild = RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(s.Name) || RelatedFileComponent.IsRelatedFileComponent(s.Name) || RelatedUrlComponent.IsRelatedUrlComponent(s.Name) || RelatedURLURLComponent.IsRelatedUrlUrlComponent(s.Name);

                if (isDocumentChild && Children.All(c => c.DocumentIndex != shapeComponent.DocumentIndex)) //if parent not exists
                {
                    RelatedDocumentContainer stub = RelatedDocumentContainer.GetStub(Page, shapeComponent.DocumentIndex);
                    Children.Insert(stub.DocumentIndex, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }


        public void InitStyle()
        {
            //MakeListItem();
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public static bool IsRelatedDocumentsContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }
    }
}
