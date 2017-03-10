using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    class CheckBoxStateComponent : RationallyComponent
    {
        private double margin = 0.05; //border for the wrapper component
        private string checkedColor = "THEMEVAL()";
        private string unCheckedColor = "RGB(255,255,255)";
        private static readonly Regex regex = new Regex(@"CheckBoxStateComponent(\.\d+)?$");

        public CheckBoxStateComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            Width = CheckBoxComponent.CHECKBOX_SIZE - 2*margin;
            Height = CheckBoxComponent.CHECKBOX_SIZE - 2 * margin;

            AddUserRow("rationallyType");
            AddUserRow("Index");
            RationallyType = "checkBoxStateComponent";
            Name = "CheckBoxStateComponent";
            Index = 0;//TODO implement via model
            LockTextEdit = true;

            Check(false);
            InitStyle();
        }

        public CheckBoxStateComponent(Page page, Shape shape) : base(page)
        {
            RShape = shape;
            InitStyle();
        }

        private void InitStyle()
        {
            SetMargin(margin);
        }

        private void Check(bool isChecked)
        {
            if (isChecked)
            {
                BackgroundColor = checkedColor;
            }
            else
            {
                BackgroundColor = unCheckedColor;
            }
        }

        public bool Checked
        {
            get { return BackgroundColor == checkedColor; }
            set { Check(value); }
        }

        public void Toggle()
        {
            Checked = !Checked;
        }

        /// <summary>
        /// Validates whether the passed coordinate is within the four sides of the square that is this component.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool WasClicked(double x, double y) => (x > (CenterX - (Width/2))) && (x < (CenterX + (Width/2))) &&
                                                      (y > (CenterY - (Height/2))) && (y < (CenterY + (Height/2)));

        public static bool IsCheckBoxStateComponent(string name) => regex.IsMatch(name);
    }
}
