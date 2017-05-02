using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Forces
{
    internal sealed class ForceValueComponent : VisioShape
    {
        private static readonly Regex ForceValueRegex = new Regex(@"ForceValue(\.\d+)?$");
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ForceValueComponent(Page page, int forceAlternativeId, string altId, int index) : base(page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(VisioFormulas.BasicStencil, (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            Shape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeUniqueId");
            
            Index = index;
            
            RationallyType = "forceValue";
            Name = "ForceValue";

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifierString = altId;

            AddAction("addForce", "QUEUEMARKEREVENT(\"add\")", "Add force", false);
            AddAction("deleteForce", "QUEUEMARKEREVENT(\"delete\")", "Delete this force", false);
            ForceAlternativeId = forceAlternativeId;
            Globals.RationallyAddIn.Model.Forces.ForEach(force =>
            {
                if (!force.ForceValueDictionary.ContainsKey(forceAlternativeId))
                {
                    force.ForceValueDictionary.Add(forceAlternativeId, "0");
                }
            });

            InitStyle();
        }

        private void InitStyle()
        {
            Width = 1.0 / 2.54;
            Height = 0.33;
            ToggleBoldFont(true);
            LineColor = "RGB(89,131,168)";
        }

        public ForceValueComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;
        }

        public static bool IsForceValue(string name) => ForceValueRegex.IsMatch(name);
        

        private void UpdateAlternativeLabels()
        {
            //locate alternative from model
            Alternative alternative = Globals.RationallyAddIn.Model.Alternatives.First(a => a.Id == ForceAlternativeId);
            AlternativeIdentifierString = alternative.IdentifierString;
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)//Visio does this for us
            {
                UpdateAlternativeLabels();
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Forces.Count - 1);

                if (Text != Globals.RationallyAddIn.Model.Forces[Index].ForceValueDictionary[ForceAlternativeId])
                {
                    Text = Globals.RationallyAddIn.Model.Forces[Index].ForceValueDictionary[ForceAlternativeId];
                }

                string toParse = Text.StartsWith("+") ? Text.Substring(1) : Text;
                int value;
                int.TryParse(toParse, out value);

                if (value < 0)
                {
                    BackgroundColor = "RGB(153,12,0)";
                    FontColor = "RGB(255,255,255)";
                }
                else if (value > 0)
                {
                    BackgroundColor = "RGB(0,175,0)";
                    FontColor = "RGB(255,255,255)";
                }
                else
                {
                    BackgroundColor = "RGB(210,210,0)";
                    FontColor = "RGB(255,255,255)";
                }
            }
            base.Repaint();
        }



    }
}

