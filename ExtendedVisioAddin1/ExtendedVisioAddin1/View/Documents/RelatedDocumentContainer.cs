using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View.Documents
{
    internal class RelatedDocumentContainer : HeaderlessContainer
    {
        private static readonly Regex RelatedRegex = new Regex(@"Related Document(\.\d+)?$");

        public RelatedDocumentContainer(Page page, Shape containerShape) : base(page, false)
        {
            RShape = containerShape;
            Array ident = containerShape.ContainerProperties.GetMemberShapes(16);
            List<Shape> shapes = (new List<int>((int[]) ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
            string name = null, path = null;
            bool file = false;
            Shape titleShape = shapes.FirstOrDefault(shape => RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(shape.Name));
            if (titleShape != null)
            {
                Children.Add(new RelatedDocumentTitleComponent(page, titleShape));
                name = titleShape.Text;
            }

            Shape fileShape = shapes.FirstOrDefault(shape => RelatedFileComponent.IsRelatedFileComponent(shape.Name));
            if (fileShape != null)
            {
                Children.Add(new RelatedFileComponent(page, fileShape));
                path = fileShape.Text;
                file = true;
            }
            else
            {
                Shape urlShape = shapes.FirstOrDefault(shape => RelatedUrlComponent.IsRelatedUrlComponent(shape.Name));
                if (urlShape != null)
                {
                    Children.Add(new RelatedUrlComponent(page, urlShape));
                    path = urlShape.Text;
                    file = false;
                }

                Shape urlUrlShape = shapes.FirstOrDefault(shape => RelatedURLURLComponent.IsRelatedUrlUrlComponent(shape.Name));
                if (urlUrlShape != null)
                {
                    Children.Add(new RelatedURLURLComponent(page, urlUrlShape));
                }

            }
            
            if (name != null && path != null)
            {
                if (DocumentIndex <= Globals.ThisAddIn.Model.Documents.Count)
                {
                    Globals.ThisAddIn.Model.Documents.Insert(DocumentIndex, new RelatedDocument(path, name, file));;
                }
                else
                {
                    Globals.ThisAddIn.Model.Documents.Add(new RelatedDocument(path, name, file));
                }
            }
            //LayoutManager = new VerticalStretchLayout(this);
            InitStyle();
        }

        public RelatedDocumentContainer(Page page, int index, RelatedDocument document) : base(page)
        {
            //1) make a title component for the source and add it to the container
            RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(page, index, document.Name + ":");
            Children.Add(relatedDocumentTitleComponent);
            if (document.IsFile)
            {
                //2) make a shortcut to the file
                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(page, index, document.Path);
                Children.Add(relatedFileComponent);
            }
            else
            {
                //2) make a shortcut to the url
                RelatedUrlComponent relatedUrlComponent = new RelatedUrlComponent(page, index, document.Path);
                Children.Add(relatedUrlComponent);
                //3) add a text element that displays the full URL
                RelatedURLURLComponent urlLabel = new RelatedURLURLComponent(page, index, document.Path);
                Children.Add(urlLabel);
            }
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentContainer";
            Name = "Related Document";
            AddUserRow("documentIndex");
            DocumentIndex = index;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "\"Add file\"", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "\"Add url\"", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "\"Delete document\"", false);

            Width = 5;
            Height = 1;
            InitStyle();
        }

        public RelatedDocumentContainer(Page page, int index) : base(page)
        {
            AddUserRow("rationallyType");
            RationallyType = "relatedDocumentContainer";
            Name = "Related Document";
            AddUserRow("documentIndex");
            DocumentIndex = index;
            Width = 5;
            Height = 1;
            InitStyle();
        }

        public void InitStyle()
        {
            
            LinePattern = 16; //borderless
            MarginTop = (DocumentIndex == 0) ? 0.3 : 0.0;
            MarginBottom = 0;
            
            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(s.Name))
            {
                RelatedDocumentTitleComponent com = new RelatedDocumentTitleComponent(Page, s);
                if (com.DocumentIndex == DocumentIndex)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedFileComponent.IsRelatedFileComponent(s.Name))
            {
                RelatedFileComponent com = new RelatedFileComponent(Page, s);
                if (com.DocumentIndex == DocumentIndex)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedUrlComponent.IsRelatedUrlComponent(s.Name))
            {
                RelatedUrlComponent com = new RelatedUrlComponent(Page, s);
                if (com.DocumentIndex == DocumentIndex)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedURLURLComponent.IsRelatedUrlUrlComponent(s.Name))
            {
                RelatedURLURLComponent com = new RelatedURLURLComponent(Page, s);
                if (com.DocumentIndex == DocumentIndex)
                {
                    Children.Add(com);
                }
            }
        }

        public static bool IsRelatedDocumentContainer(string name)
        {
            return RelatedRegex.IsMatch(name);
        }

        public void SetDocumentIdentifier(int documentIndex)
        {
            Children.ForEach(c => c.DocumentIndex = documentIndex);
            DocumentIndex = documentIndex;
            InitStyle();
        }

        public void EditFile(RelatedDocument doc, int index)
        {
            List<RelatedFileComponent> comp = Children.Where(c => c is RelatedFileComponent).Cast<RelatedFileComponent>().ToList();
            comp.ForEach(c =>
            {
                Children.Remove(c);
                c.RShape.Delete();
            });
            //Make a shortcut to the file
            RelatedFileComponent relatedFileComponent = new RelatedFileComponent(Page, index, doc.Path);
            Children.Add(relatedFileComponent);
            Children.Where(c => c is RelatedDocumentTitleComponent).ToList().ForEach(x => x.Text = doc.Path);
        }

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (DocumentIndex == 0)
            {
                DeleteAction("moveUp");
            }

            if (DocumentIndex == Globals.ThisAddIn.Model.Documents.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {
            UpdateReorderFunctions();
            base.Repaint();
        }

    }
}
