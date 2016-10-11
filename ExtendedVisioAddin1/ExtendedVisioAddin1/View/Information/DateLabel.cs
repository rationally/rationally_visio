using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View.Information
{
    class DateLabel : TextLabel
    {
        private static readonly Regex DateRegex = new Regex(@"DateLabel(\.\d+)?$");

        public DateLabel(Page page, Shape shape) : base(page, shape)
        {
            RShape = shape;
        }

        public DateLabel(Page page, string labelText) : base(page, labelText)
        {
            AddUserRow("rationallyType");
            RationallyType = "informationVersion";

            Name = "InformationVersion";
        }

        public override void Repaint()
        {
            this.Text = Globals.RationallyAddIn.Model.Date;
            base.Repaint();
        }

        public static bool IsDateLabel(string name)
        {
            return DateRegex.IsMatch(name);
        }
    }
}
