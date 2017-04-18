using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View.Documents
{
    internal sealed class RelatedDocumentContainer : HeaderlessContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex RelatedRegex = new Regex(@"Related Document(\.\d+)?$");

        public RelatedDocumentContainer(Page page, Shape containerShape) : base(page, false)
        {
            Shape = containerShape;
            Array ident = containerShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
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
                RelatedFileComponent relatedFileComponent = new RelatedFileComponent(page, fileShape);
                Children.Add(relatedFileComponent);
                path = relatedFileComponent.FilePath;
                file = true;
            }
            else
            {
                Shape urlShape = shapes.FirstOrDefault(shape => RelatedUrlComponent.IsRelatedUrlComponent(shape.Name));
                if (urlShape != null)
                {
                    Children.Add(new RelatedUrlComponent(page, urlShape));
                    
                }

                Shape urlUrlShape = shapes.FirstOrDefault(shape => RelatedURLURLComponent.IsRelatedUrlUrlComponent(shape.Name));
                if (urlUrlShape != null)
                {
                    Children.Add(new RelatedURLURLComponent(page, urlUrlShape));
                    path = urlUrlShape.Text;
                }

            }
            
            if ((name != null) && (path != null))
            {
                RelatedDocument doc = new RelatedDocument(path, name, file, Id);
                if (Index <= Globals.RationallyAddIn.Model.Documents.Count)
                {
                    Globals.RationallyAddIn.Model.Documents.Insert(Index, doc);
                }
                else
                {
                    Globals.RationallyAddIn.Model.Documents.Add(doc);
                }
            }
            MarginTop = Index == 0 ? 0.3 : 0.0;
            MarginBottom = 0;

            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public RelatedDocumentContainer(Page page, int index, RelatedDocument document, int docId) : base(page)
        {
            //1) make a title component for the source and add it to the container
            RelatedDocumentTitleComponent relatedDocumentTitleComponent = new RelatedDocumentTitleComponent(page, index, document.Name);
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
            AddUserRow("index");
            Index = index;
            AddUserRow("uniqueId");
            Id = docId;

            AddAction("addRelatedFile", "QUEUEMARKEREVENT(\"addRelatedFile\")", "Add file", false);
            AddAction("addRelatedUrl", "QUEUEMARKEREVENT(\"addRelatedUrl\")", "Add url", false);
            AddAction("deleteRelatedDocument", "QUEUEMARKEREVENT(\"delete\")", "Delete document", false);

            MsvSdContainerLocked = true;
            

            Width = 5;
            Height = 1;
            InitStyle();
        }

        private void InitStyle()
        {
            
            LinePattern = 16; //borderless
            MarginTop = Index == 0 ? 0.3 : 0.0;
            MarginBottom = 0;

            UsedSizingPolicy |= SizingPolicy.ExpandYIfNeeded;
        }

        public override void AddToTree(Shape s, bool allowAddOfSubpart)
        {
            if (RelatedDocumentTitleComponent.IsRelatedDocumentTitleContainer(s.Name))
            {
                RelatedDocumentTitleComponent com = new RelatedDocumentTitleComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedFileComponent.IsRelatedFileComponent(s.Name))
            {
                RelatedFileComponent com = new RelatedFileComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedUrlComponent.IsRelatedUrlComponent(s.Name))
            {
                RelatedUrlComponent com = new RelatedUrlComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
            else if (RelatedURLURLComponent.IsRelatedUrlUrlComponent(s.Name))
            {
                RelatedURLURLComponent com = new RelatedURLURLComponent(Page, s);
                if (com.Index == Index)
                {
                    Children.Add(com);
                }
            }
        }

        public static bool IsRelatedDocumentContainer(string name) => RelatedRegex.IsMatch(name);

        public void SetDocumentIdentifier(int documentIndex)
        {
            Children.ForEach(c => c.Index = documentIndex);
            Index = documentIndex;
            InitStyle();
        }

        public void EditFile(RelatedDocument doc, int index)
        {
            List<RelatedFileComponent> comp = Children.Where(c => c is RelatedFileComponent).Cast<RelatedFileComponent>().ToList();
            comp.ForEach(c =>
            {
                Children.Remove(c);
                c.Shape.Delete();
            });
            //Make a shortcut to the file
            RelatedFileComponent relatedFileComponent = new RelatedFileComponent(Page, index, doc.Path);
            Children.Add(relatedFileComponent);
            Children.Where(c => c is RelatedDocumentTitleComponent).ToList().ForEach(x => x.Text = doc.Path);
        }
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Documents.Count - 1);
            }
            base.Repaint();
        }

    }
}
