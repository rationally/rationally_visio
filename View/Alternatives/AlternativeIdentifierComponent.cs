using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeIdentifierComponent : TextLabel, IAlternativeComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex IdentRegex = new Regex(@"AlternativeIdent(\.\d+)?$");
        public AlternativeIdentifierComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            
                InitStyle();
        }

        public AlternativeIdentifierComponent(Page page, int index, string text) : base(page, text)
        {
            RationallyType = "alternativeIdentifier";
            AddUserRow("index");
            Index = index;

            Name = "AlternativeIdent";

            AddAction("addAlternative", "QUEUEMARKEREVENT(\"add\")", "\"Add alternative\"", false);
            AddAction("deleteAlternative", "QUEUEMARKEREVENT(\"delete\")", "\"Delete this alternative\"", false);
            Height = 0.2;
            Width = 0.3;
            InitStyle();
        }

        private void InitStyle()
        {
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
            UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded;
        }

        public void SetAlternativeIdentifier(int alternativeIndex)
        {
            Index = alternativeIndex;
            if (Text != (char) (65 + alternativeIndex) + ":")
            {
                Text = (char) (65 + alternativeIndex) + ":";
            }
        }
        public static bool IsAlternativeIdentifier(string name) => IdentRegex.IsMatch(name);
        

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //No need to do this during an update, Visio handles this
            {
                UpdateReorderFunctions(Globals.RationallyAddIn.Model.Alternatives.Count - 1);
            }
            base.Repaint();
        }
    }
}
