using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    internal class InformationContainer : HeaderlessContainer
    {
        public InformationContainer(Page page, string author, string date, string version) : base(page)
        {
            Width = 0.1;
            Height = 0.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded | SizingPolicy.ExpandYIfNeeded | SizingPolicy.ShrinkXIfNeeded | SizingPolicy.ShrinkYIfNeeded;

            TextLabel authorLabel = new TextLabel(page, "Author: " + author);
            authorLabel.SetMargin(0.1);
            authorLabel.SetUsedSizingPolicy(UsedSizingPolicy);
            TextLabel dateLabel = new TextLabel(page, "Date: " + date);
            dateLabel.SetMargin(0.1);
            dateLabel.SetUsedSizingPolicy(UsedSizingPolicy);
            TextLabel versionLabel = new TextLabel(page, "Version: " + version);
            versionLabel.SetMargin(0.1);
            versionLabel.SetUsedSizingPolicy(UsedSizingPolicy);

            AddUserRow("rationallyType");
            RationallyType = "informationBox";
            RShape.Name = "InformationBox";

            Children.Add(authorLabel);
            Children.Add(dateLabel);
            Children.Add(versionLabel);
        }
    }
}
