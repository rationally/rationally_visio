using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
    class CheckBoxStateComponent : RationallyComponent
    {
        private double margin = 0.05; //border for the wrapper component
        private string checkedColor = "THEMEVAL()";
        private string unCheckedColor = "RGB(255,255,255)";
        public CheckBoxStateComponent(Page page) : base(page)
        {
            Document basicDocument = Globals.RationallyAddIn.Application.Documents.OpenEx("Basic Shapes.vss", (short)VisOpenSaveArgs.visOpenHidden);
            Master rectMaster = basicDocument.Masters["Rectangle"];

            RShape = page.Drop(rectMaster, 0, 0);
            basicDocument.Close();

            Width = CheckBoxComponent.CHECKBOX_SIZE - 2*margin;
            Height = CheckBoxComponent.CHECKBOX_SIZE - 2 * margin;

            AddUserRow("rationallyType");
            RationallyType = "checkBoxStateComponent";

            Check(false);
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
    }
}
