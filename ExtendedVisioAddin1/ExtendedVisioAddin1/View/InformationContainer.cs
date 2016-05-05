using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Components;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class InformationContainer : HeaderlessContainer
    {
        public InformationContainer(Page page, string author, string date, string version) : base(page)
        {
            this.Width = 7;
            this.Height = 1.1;
            this.UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded; //TODO fix with expandY

            TextLabel authorLabel = new TextLabel(page, "Author: " + author);
            authorLabel.SetMargin(0.1);
            TextLabel dateLabel = new TextLabel(page, "Date: " + date);
            dateLabel.SetMargin(0.1);
            TextLabel versionLabel = new TextLabel(page, "Version: " + version);
            versionLabel.SetMargin(0.1);

            this.Children.Add(authorLabel);
            this.Children.Add(dateLabel);
            this.Children.Add(versionLabel);
        }
    }
}
