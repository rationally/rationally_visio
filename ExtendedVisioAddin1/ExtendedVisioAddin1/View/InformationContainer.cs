using System.Linq;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.View
{
    internal class InformationContainer : HeaderlessContainer
    {
        public InformationContainer(Page page, string author, string date, string version) : base(page)
        {
            Width = 5.3;
            Height = 0.4;
            CenterX = 13.65;
            CenterY = 22.45;
            UsedSizingPolicy = SizingPolicy.FixedSize;

            TextLabel authorLabel = new TextLabel(page, "Author: ")
            {
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Height = 0.38,
                MarginTop = 0.02,
                MarginLeft = 0.02,
                Order = 0
            };
            authorLabel.ToggleBoldFont(true);
            authorLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            AuthorLabel authorLabelContent = new AuthorLabel(page, author)
            {
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Height = 0.38,
                MarginTop = 0.02,
                HAlign = 0, //left
                Order = 1
            };
            authorLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            TextLabel dateLabel = new TextLabel(page, "Date: ")
            {
                Height = 0.38,
                MarginTop = 0.02,
                MarginLeft = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Order = 2
            };
            dateLabel.ToggleBoldFont(true);
            dateLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            DateLabel dateLabelContent = new DateLabel(page, date)
            {
                Height = 0.38,
                MarginTop = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = 0, //left
                Order = 3
            };
            dateLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            TextLabel versionLabel = new TextLabel(page, "Version: ")
            {
                Height = 0.38,
                MarginTop = 0.02,
                MarginLeft = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Order = 4
            };
            versionLabel.ToggleBoldFont(true);
            versionLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            VersionLabel versionLabelContent = new VersionLabel(page, version)
            {
                Height = 0.38,
                MarginTop = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = 0, //left
                Order = 5
            };
            versionLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            AddUserRow("rationallyType");
            RationallyType = "informationBox";
            RShape.Name = "InformationBox";

            Children.Add(authorLabel);
            Children.Add(authorLabelContent);
            Children.Add(dateLabel);
            Children.Add(dateLabelContent);
            Children.Add(versionLabel);
            Children.Add(versionLabelContent);
        }

        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            if (AuthorLabel.IsAuthorLabel(s.Name))
            {
                Children.Add(new AuthorLabel(Page, s));
            }
            else if (DateLabel.IsDateLabel(s.Name))
            {
                Children.Add(new DateLabel(Page, s));
            }
            else if (VersionLabel.IsVersionLabel(s.Name))
            {
                Children.Add(new VersionLabel(Page, s));
            }
            else if (TextLabel.IsTextLabel(s.Name))
            {
                Children.Add(new TextLabel(Page,s));
            }
        }

        public override void Repaint()
        {
            Children = Children.OrderBy(c => c.Order).ToList();
            /*if (Children.Count == 6)
            {
                if (!(Children[1] is AuthorLabel))
                {
                    RComponent c = Children.Find(x => x is AuthorLabel);
                    Children.Remove(c);
                    Children.Insert(0, c);
                }
                if (!(Children[2] is DateLabel))
                {
                    RComponent c = Children.Find(x => x is DateLabel);
                    Children.Remove(c);
                    Children.Insert(1, c);
                }
                if (!(Children[4] is VersionLabel))
                {
                    RComponent c = Children.Find(x => x is VersionLabel);
                    Children.Remove(c);
                    Children.Insert(2, c);
                }
            }*/
            base.Repaint();
        }
    }
}
