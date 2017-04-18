using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Planning;

namespace Rationally.Visio.View
{
    internal class CheckBoxStateComponent : VisioShape
    {
        private double margin = 0.05; //border for the wrapper component
        private string checkedColor = "THEMEVAL()";
        private string unCheckedColor = "RGB(255,255,255)";
        private static readonly Regex Regex = new Regex(@"CheckBoxStateComponent(\.\d+)?$");

        public CheckBoxStateComponent(Page page, int index, bool isFinished) : base(page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            Shape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            Width = CheckBoxComponent.CheckboxSize - 2*margin;
            Height = CheckBoxComponent.CheckboxSize - 2 * margin;
            
            RationallyType = "checkBoxStateComponent";
            Name = "CheckBoxStateComponent";
            Index = index;//TODO implement via model
            LockTextEdit = true;
            //LockDelete = true;

            //Check(isFinished);
            InitStyle();
        }

        public CheckBoxStateComponent(Page page, Shape shape) : base(page)
        {
            Shape = shape;
            InitStyle();
        }

        private void InitStyle() => SetMargin(margin);

        private void Check(bool isChecked)
        {
            
            //update model
            Globals.RationallyAddIn.Model.PlanningItems[Index].Finished = isChecked;
            PlanningContainer planningContainer = Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is PlanningContainer) as PlanningContainer;
            planningContainer.Children[Index].Repaint();
            /*PlanningContainer planningContainer = (Globals.RationallyAddIn.View.Children.FirstOrDefault(c => c is PlanningContainer) as PlanningContainer);
            //locate parent of stateComponent
            PlanningItemComponent toStrikeThrough = planningContainer?.Children.Cast<PlanningItemComponent>().First(item => (item.Children.First(c => c is CheckBoxComponent) as CheckBoxComponent).Children.Contains(this));
            toStrikeThrough.Children.First(c => c is PlanningItemTextComponent).StrikeThrough = isChecked;*/
        }

        public bool Checked
        {
            get { return BackgroundColor == checkedColor; }
            set { Check(value); }
        }

        public void Toggle() => Check(!Checked);

        /// <summary>
        /// Validates whether the passed coordinate is within the four sides of the square that is this component.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool WasClicked(double x, double y) => (x > (CenterX - (Width/2))) && (x < (CenterX + (Width/2))) &&
                                                      (y > (CenterY - (Height/2))) && (y < (CenterY + (Height/2)));

        public override void Repaint()
        {
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio takes care of this
            {
                BackgroundColor = Globals.RationallyAddIn.Model.PlanningItems[Index].Finished ? checkedColor : unCheckedColor;
            }
            base.Repaint();
        }

        public static bool IsCheckBoxStateComponent(string name) => Regex.IsMatch(name);
    }
}
