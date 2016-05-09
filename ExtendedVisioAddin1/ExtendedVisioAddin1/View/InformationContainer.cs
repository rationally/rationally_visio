using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class InformationContainer : HeaderlessContainer
    {
        public InformationContainer(Page page, string author, string date, string version) : base(page)
        {
            Width = 7;
            Height = 1.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded; //TODO fix with expandY

            TextLabel authorLabel = new TextLabel(page, "Author: " + author);
            authorLabel.SetMargin(0.1);
            TextLabel dateLabel = new TextLabel(page, "Date: " + date);
            dateLabel.SetMargin(0.1);
            TextLabel versionLabel = new TextLabel(page, "Version: " + version);
            versionLabel.SetMargin(0.1);

            Children.Add(authorLabel);
            Children.Add(dateLabel);
            Children.Add(versionLabel);
        }
    }
}
