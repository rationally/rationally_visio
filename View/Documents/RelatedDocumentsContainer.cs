using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.EventHandlers;
using Rationally.Visio.Model;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View.Documents
{
    internal class RelatedDocumentsContainer : RationallyContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex RelatedRegex = new Regex(@"Related Documents(\.\d+)?$");
        
        public RelatedDocumentsContainer(Page page, Shape relatedDocumentsContainer) : base(page)
        {
            RShape = relatedDocumentsContainer;
            Array ident = relatedDocumentsContainer.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
            List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            foreach (Shape shape in shapes.Where(shape => RelatedDocumentContainer.IsRelatedDocumentContainer(shape.Name)))
            {
                Children.Add(new RelatedDocumentContainer(page, shape));
            }
            Children = Children.OrderBy(c => c.Index).ToList();
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
            LayoutManager = new VerticalStretchLayout(this);

        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            //make s into an rcomponent for access to wrapper
            RationallyComponent shapeComponent = new RationallyComponent(Page) {RShape = s};

            if (RelatedDocumentContainer.IsRelatedDocumentContainer(s.Name))
            {
                if (Children.All(c => c.Index != shapeComponent.Index)) //there is no container stub with this index
                {
                    RelatedDocumentContainer con = new RelatedDocumentContainer(Page, s);
                    Children.Insert(con.Index, con);
                }
                else
                {
                    //remove stub, insert s as the shape of the stub wrapper
                    RelatedDocumentStubContainer stub = (RelatedDocumentStubContainer)Children.First(c => c.Index == shapeComponent.Index);
                    Children.Remove(stub);
                    RelatedDocumentContainer con = new RelatedDocumentContainer(Page, s);
                    Children.Insert(con.Index, con);
                }

                
            }
            else
            {
                bool isDocumentChild = RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(s.Name) || RelatedFileComponent.IsRelatedFileComponent(s.Name) || RelatedUrlComponent.IsRelatedUrlComponent(s.Name) || RelatedURLURLComponent.IsRelatedUrlUrlComponent(s.Name);

                if (isDocumentChild && Children.All(c => c.Index != shapeComponent.Index)) //if parent not exists
                {
                    RelatedDocumentStubContainer stub = new RelatedDocumentStubContainer(Page, shapeComponent.Index);
                    Children.Insert(stub.Index, stub);
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
                else
                {
                    Children.ForEach(r => r.AddToTree(s, allowAddOfSubpart));
                }
            }
        }

        /// <summary>
        /// Adds a related document to the sheet.
        /// </summary>
        /// <param name="document"></param>
        public void AddRelatedDocument(RelatedDocument document)
        {
            //create a container that wraps the new document
            Children.Add(new RelatedDocumentContainer(Globals.RationallyAddIn.Application.ActivePage, Globals.RationallyAddIn.Model.Documents.Count - 1, document));

            RepaintHandler.Repaint(this);
        }

        /// <summary>
        /// Adds a related document to the sheet, with a specified document index
        /// </summary>
        /// <param name="document"></param>
        /// <param name="documentIndex"></param>
        public void InsertRelatedDocument(RelatedDocument document, int documentIndex)
        {
            //create a container that wraps the new document
            Children.Insert(Math.Min(documentIndex, Globals.RationallyAddIn.Model.Documents.Count - 1),new RelatedDocumentContainer(Globals.RationallyAddIn.Application.ActivePage, documentIndex, document));

            RepaintHandler.Repaint(this);
        }

        public static bool IsRelatedDocumentsContainer(string name) => RelatedRegex.IsMatch(name);

        public override void Repaint()
        {
            if (Globals.RationallyAddIn.Model.Documents.Count > Children.Count)
            {
                Globals.RationallyAddIn.Model.Documents
                    .Where(doc => Children.Count == 0 || Globals.RationallyAddIn.Model.Documents.IndexOf(doc) > Children.Last().Index).ToList()
                    .ForEach(doc => 
                        Children.Add(new RelatedDocumentContainer(Globals.RationallyAddIn.Application.ActivePage, Children.Count, doc))
                    );
            }
            base.Repaint();
        }
    }
}
