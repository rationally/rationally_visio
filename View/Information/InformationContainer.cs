using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.RationallyConstants;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View.Information
{
    internal class InformationContainer : HeaderlessContainer
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex InformationContainerRegex = new Regex(@"Information(\.\d+)?$");

        public InformationContainer(Page page, string author, string date, string version) : base(page)
        {




            AddUserRow("rationallyType");
            RationallyType = "informationBox";
            RShape.Name = "Information";

            InitContent(page, author, date, version);
            InitStyle();
        }

        public InformationContainer(Page page, Shape s) : base(page, false)
        {
            RShape = s;
            RationallyModel model = Globals.RationallyAddIn.Model;
            if ((s.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested).Length == 0) && !Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                InitContent(page, model.Author, model.DateString, model.Version);
            }
            else
            {
                Array ident = RShape.ContainerProperties.GetMemberShapes((int)VisContainerFlags.visContainerFlagsExcludeNested);
                List<Shape> shapes = (new List<int>((int[])ident)).Select(i => page.Shapes.ItemFromID[i]).ToList();
                foreach (Shape shape in shapes)
                {
                    if (TextLabel.IsTextLabel(shape.Name))
                    {

                        Children.Add(new PaddedTextLabel(page, shape));

                    }
                    else if (AuthorLabel.IsAuthorLabel(shape.Name))
                    {
                        Children.Add(new AuthorLabel(page, shape));
                        model.Author = shape.Text;
                    }
                    else if (DateLabel.IsDateLabel(shape.Name))
                    {
                        Children.Add(new DateLabel(page, shape));
                        model.DateString = shape.Text;
                    }
                    else if (VersionLabel.IsVersionLabel(shape.Name))
                    {
                        Children.Add(new VersionLabel(page, shape));
                        model.Version = shape.Text;
                    }

                }
                Children = Children.OrderBy(c => c.Order).ToList();
            }

        }

        private void InitStyle()
        {
            Width = 8;
            Height = 0.4;
            CenterX = 12.30;
            CenterY = 22.45;
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                RShape.ContainerProperties.ResizeAsNeeded = 0;
                ContainerPadding = 0;
            }
            MsvSdContainerLocked = true;
            UsedSizingPolicy = SizingPolicy.FixedSize;
        }

        private void InitContent(Page page, string author, string date, string version)
        {
            PaddedTextLabel authorLabel = new PaddedTextLabel(page, "Author: ")
            {
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Height = 0.38,
                Order = 0
            };
            authorLabel.ToggleBoldFont(true);
            authorLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            authorLabel.RationallyType = "informationAuthorLabel";
            //authorLabel.MarginTop = 0.1;

            AuthorLabel authorLabelContent = new AuthorLabel(page, author)
            {
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Height = 0.38,
                HAlign = Constants.LeftAlignment,
                Order = 1,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
            };
            //authorLabelContent.


            PaddedTextLabel dateLabel = new PaddedTextLabel(page, "Date: ")
            {
                Height = 0.38,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Order = 2
            };
            dateLabel.ToggleBoldFont(true);
            dateLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            dateLabel.RationallyType = "informationDateLabel";
            //dateLabel.SetMargin(0.02);

            DateLabel dateLabelContent = new DateLabel(page, date)
            {
                Height = 0.38,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = Constants.LeftAlignment,
                Order = 3,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
            };
            dateLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            PaddedTextLabel versionLabel = new PaddedTextLabel(page, "Version: ")
            {
                Height = 0.38,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                Order = 4
            };
            versionLabel.ToggleBoldFont(true);
            versionLabel.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);
            versionLabel.RationallyType = "informationVersionLabel";
            //versionLabel.SetMargin(0.1);

            VersionLabel versionLabelContent = new VersionLabel(page, version)
            {
                Height = 0.38,
                BackgroundColor = "RGB(255,255,255)",
                FontColor = "RGB(89,131,168)",
                HAlign = Constants.LeftAlignment,
                Order = 5,
                LockTextEdit = true,
                EventDblClick = "QUEUEMARKEREVENT(\"openWizard\")"
            };
            versionLabelContent.SetUsedSizingPolicy(SizingPolicy.ExpandXIfNeeded | SizingPolicy.ShrinkXIfNeeded);

            Children.Add(authorLabel);
            Children.Add(authorLabelContent);
            Children.Add(dateLabel);
            Children.Add(dateLabelContent);
            Children.Add(versionLabel);
            Children.Add(versionLabelContent);
        }

        public override void AddToTree(Shape s, bool allowAddInChildren)
        {
            string rationallyType = s.CellsU[CellConstants.RationallyType].ResultStr["Value"];
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
            else if (TextLabel.IsTextLabel(s.Name) && ((rationallyType == "informationVersionLabel") || (rationallyType == "informationDateLabel") || (rationallyType == "informationAuthorLabel")))
            {
                Children.Add(new PaddedTextLabel(Page, s));
            }
        }

        public override void Repaint()
        {
            Children = Children.OrderBy(c => c.Order).ToList();
            base.Repaint();
        }

        public static bool IsInformationContainer(string name) => InformationContainerRegex.IsMatch(name);
    }
}
