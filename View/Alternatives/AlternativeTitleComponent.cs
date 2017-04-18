using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.Model;
using Rationally.Visio.View.ContextMenu;

namespace Rationally.Visio.View.Alternatives
{
    internal sealed class AlternativeTitleComponent : TextLabel
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly Regex TitleRegex = new Regex(@"AlternativeTitle(\.\d+)?$");

        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        public AlternativeTitleComponent(Page page, Shape alternativeComponent) : base(page, alternativeComponent)
        {
            Shape = alternativeComponent;
            InitStyle();
        }


        public AlternativeTitleComponent(Page page, int index, string text) : base(page, text)
        {
            RationallyType = "alternativeTitle";
            Index = index;

            Name = "AlternativeTitle";

            ContextMenuItem addAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_AddAlternative, Messages.Menu_AddAlternative);
            //addAlternativeMenuItem.Action = ?? //TODO implement
            ContextMenuItem removeAlternativeMenuItem = ContextMenuItem.CreateAndRegister(this, VisioFormulas.EventId_DeleteAlternative, Messages.Menu_DeleteAlternative);
            //removeAlternativeMenuItem.Action = ?? //TODO implement
            Width = 3.7;
            Height = 0.2;
            InitStyle();
        }

        private void InitStyle()
        {
            
            HAlign = 0;//Enum is wrong, align left
            MarginLeft = 0.05;
            MarginRight = 0;
            MarginBottom = 0;
            MarginTop = 0.1;
            UsedSizingPolicy = SizingPolicy.FixedSize;
            
        }

        public static bool IsAlternativeTitle(string name) => TitleRegex.IsMatch(name);

        private void UpdateReorderFunctions()
        {
            AddAction("moveUp", "QUEUEMARKEREVENT(\"moveUp\")", "\"Move up\"", false);
            AddAction("moveDown", "QUEUEMARKEREVENT(\"moveDown\")", "\"Move down\"", false);

            if (Index == 0) //Top shape can't move up
            {
                DeleteAction("moveUp");
            }

            if (Index == Globals.RationallyAddIn.Model.Alternatives.Count - 1)
            {
                DeleteAction("moveDown");
            }
        }

        public override void Repaint()
        {

            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing)
            {
                UpdateReorderFunctions();
                if (Globals.RationallyAddIn.Model.Alternatives.Count > Index)
                {
                    Alternative alternative = Globals.RationallyAddIn.Model.Alternatives[Index];
                    Text = alternative.Title;
                }
            }
            base.Repaint();
        }
    }
}
