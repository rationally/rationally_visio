﻿using Microsoft.Office.Interop.Visio;

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
            TextLabel dateLabel = new TextLabel(page, "Date: " + date);
            dateLabel.SetMargin(0.1);
            TextLabel versionLabel = new TextLabel(page, "Version: " + version);
            versionLabel.SetMargin(0.1);

            AddUserRow("rationallyType");
            RationallyType = "informationBox";
            RShape.Name = "InformationBox";

            Children.Add(authorLabel);
            Children.Add(dateLabel);
            Children.Add(versionLabel);
        }
    }
}
