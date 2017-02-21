using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceTotalComponent : RationallyComponent
    {
        private static readonly Regex ForceTotalComponentRegex = new Regex(@"ForceTotalComponent(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private ForceTotalComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeUniqueId");
            ForceAlternativeId = -2;

            AddUserRow("alternativeIndex");
            AlternativeIndex = -2;

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifierString = "";

            AddUserRow("rationallyType");
            RationallyType = "forceTotalComponent";
            Name = "ForceTotalComponent";
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            LineColor = "RGB(89,131,168)";
            ToggleBoldFont(true);
        }

        public ForceTotalComponent(Page page, int altIndex, string altId, int id) : this(page)
        {
            ForceAlternativeId = id;
            AlternativeIdentifierString = altId;
            AlternativeIndex = altIndex;
        }

        public ForceTotalComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
        }

        public static bool IsForceTotalComponent(string name) => ForceTotalComponentRegex.IsMatch(name);

        private void UpdateAlternativeLabels()
        {
            //locate alternative from model
            Alternative alternative = Globals.RationallyAddIn.Model.Alternatives.First(a => a.Id == ForceAlternativeId);
            
            AlternativeIdentifierString = alternative.IdentifierString;
            AlternativeIndex = Globals.RationallyAddIn.Model.Alternatives.IndexOf(alternative);
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateAlternativeLabels();

                int total = 0;
                List<ForceValueComponent> totalCandidates = new List<ForceValueComponent>();

                ForcesContainer forcesContainer = (ForcesContainer)Globals.RationallyAddIn.View.Children.First(c => c is ForcesContainer);
                //for each forcecontainer, look up the forcevalue related to this' total and store it in totalCandidates
                forcesContainer.Children.Where(c => c is ForceContainer).Cast<ForceContainer>().ToList().ForEach(c => c.Children.Where(d => d is ForceValueComponent).ToList().Cast<ForceValueComponent>().Where(fv => fv.ForceAlternativeId == ForceAlternativeId).ToList().ForEach(childForTotal => totalCandidates.Add(childForTotal)));

                foreach (ForceValueComponent fv in totalCandidates)
                {
                    int v;

                    string toParse = fv.Text.StartsWith("+") ? fv.Text.Substring(1) : fv.Text;

                    if (int.TryParse(toParse, out v))
                    {
                        total += v;
                    }
                }
                if (total < 0)
                {
                    BackgroundColor = "RGB(153,12,0)";
                    FontColor = "RGB(255,255,255)";
                }
                else if (total > 0)
                {
                    BackgroundColor = "RGB(0,175,0)";
                    FontColor = "RGB(255,255,255)";
                }
                else
                {
                    BackgroundColor = "RGB(210,210,0)";
                    FontColor = "RGB(255,255,255)";
                }
                if (int.Parse(Text) != total)
                {
                    Text = total + "";
                }
            }
        }
    }
}
