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

            TextLabel authorLabel = new TextLabel(page, "Author: " + author);
            
            authorLabel.BackgroundColor = "RGB(255,255,255)";
            authorLabel.FontColor = "RGB(89,131,168)";
            authorLabel.Height = 0.38;
            authorLabel.MarginTop = 0.02;
            authorLabel.MarginLeft = 0.02;
            authorLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            TextLabel dateLabel = new TextLabel(page, "Date: " + date);
            dateLabel.Height = 0.38;
            dateLabel.MarginTop = 0.02;
            dateLabel.MarginLeft = 0.02;
            dateLabel.BackgroundColor = "RGB(255,255,255)";
            dateLabel.FontColor = "RGB(89,131,168)";
            dateLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            TextLabel versionLabel = new TextLabel(page, "Version: " + version);
            versionLabel.Height = 0.38;
            versionLabel.MarginTop = 0.02;
            versionLabel.MarginLeft = 0.02;
            versionLabel.BackgroundColor = "RGB(255,255,255)";
            versionLabel.FontColor = "RGB(89,131,168)";
            versionLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            AddUserRow("rationallyType");
            RationallyType = "informationBox";
            RShape.Name = "InformationBox";

            Children.Add(authorLabel);
            Children.Add(dateLabel);
            Children.Add(versionLabel);
        }
    }
}
