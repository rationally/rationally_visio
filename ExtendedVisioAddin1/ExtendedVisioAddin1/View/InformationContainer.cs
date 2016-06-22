using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
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
                MarginLeft = 0.02
            };
            authorLabel.ToggleBoldFont(true);
            authorLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            TextLabel authorLabelContent = new TextLabel(page, author)
            {
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Height = 0.38,
                MarginTop = 0.02,
                HAlign = 0 //left
            };
            authorLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            TextLabel dateLabel = new TextLabel(page, "Date: ")
            {
                Height = 0.38,
                MarginTop = 0.02,
                MarginLeft = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)"
            };
            dateLabel.ToggleBoldFont(true);
            dateLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            TextLabel dateLabelContent = new TextLabel(page, date)
            {
                Height = 0.38,
                MarginTop = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = 0 //left
            };
            dateLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            TextLabel versionLabel = new TextLabel(page, "Version: ")
            {
                Height = 0.38,
                MarginTop = 0.02,
                MarginLeft = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)"
            };
            versionLabel.ToggleBoldFont(true);
            versionLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            TextLabel versionLabelContent = new TextLabel(page, version)
            {
                Height = 0.38,
                MarginTop = 0.02,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = 0 //left
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
    }
}
