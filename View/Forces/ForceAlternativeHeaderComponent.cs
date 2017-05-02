using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;

namespace Rationally.Visio.View.Forces
{
    internal class ForceAlternativeHeaderComponent : VisioShape
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex ForceAlternativeHeaderComponentRegex = new Regex(@"ForceAlternativeHeaderComponent(\.\d+)?$");
        
        private ForceAlternativeHeaderComponent(Page page) : base(page) 
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx(VisioFormulas.BasicStencil, (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];
            Shape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            AddUserRow("alternativeUniqueId");
            ForceAlternativeId = -2;//for debugging, to distinguish from default highest of -1

            AddUserRow("alternativeIdentifier");
            AlternativeIdentifierString = "";
            
            RationallyType = "forceAlternativeHeaderComponent";
            Name = "ForceAlternativeHeaderComponent";
            InitStyle();
        }

        private void InitStyle()
        {
            Width = 1.0 / 2.54;
            Height = 0.33;
            Text = "0";
            ToggleBoldFont(true);
            LineColor = "RGB(89,131,168)";
            BackgroundColor = "RGB(255,255,255)";
            FontColor = "RGB(89,131,168)";
        }

        public ForceAlternativeHeaderComponent(Page page, string altId, int id) : this(page)
        {
            ForceAlternativeId = id;
            AlternativeIdentifierString = altId;
            Text = altId;
        }

        public ForceAlternativeHeaderComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;
        }

        public static bool IsForceAlternativeHeaderComponent(string name) => ForceAlternativeHeaderComponentRegex.IsMatch(name);

        private void UpdateAlternativeLabels()
        {
            //locate alternative from model
            Alternative alternative = Globals.RationallyAddIn.Model.Alternatives.First(a => a.Id == ForceAlternativeId);

            AlternativeIdentifierString = alternative.IdentifierString;
        }

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio does this for us
            {
                UpdateAlternativeLabels();
            }
            if (Text != AlternativeIdentifierString) //Don't perform useless operations
            {
                Text = AlternativeIdentifierString;
            }
            base.Repaint();
        }
    }
}
