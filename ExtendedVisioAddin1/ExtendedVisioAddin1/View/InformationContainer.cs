using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;
using Rationally.Visio.View.Information;

namespace Rationally.Visio.View
{
    internal class InformationContainer : HeaderlessContainer
    {
        private static readonly Regex InformationContainerRegex = new Regex(@"InformationBox(\.\d+)?$");

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
                HAlign = Constants.LeftAlignment,
                Order = 1,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
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
                HAlign = Constants.LeftAlignment, 
                Order = 3,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
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
                HAlign = Constants.LeftAlignment,
                Order = 5,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
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

        public InformationContainer(Page page, Shape s) : base(page)
        {
            RShape = s;
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

            base.Repaint();
        }

        public static bool IsInformationContainer(string name)
        {
            return InformationContainerRegex.IsMatch(name);
        }
    }
}
