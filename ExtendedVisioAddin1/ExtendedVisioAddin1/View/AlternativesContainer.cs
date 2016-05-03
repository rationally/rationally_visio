using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    class AlternativesContainer : RContainer
    {
        public AlternativesContainer(Page page, List<Alternative> alternatives) : base(page)
        {
            Master containerMaster = Globals.ThisAddIn.model.RationallyDocument.Masters["Alternatives"];
            RShape = Page.DropContainer(containerMaster, null);
            this.CenterX = 10;
            this.CenterY = 10;

            this.MsvSdContainerLocked = false;
            for (int i = 0; i < alternatives.Count; i++)
            {
                this.Children.Add(new AlternativeContainer(page, i, alternatives[i]));
            }
            this.UsedSizingPolicy = SizingPolicy.ExpandXIfNeeded & SizingPolicy.ExpandYIfNeeded;
            this.LayoutManager = new InlineLayout(this);
            this.MsvSdContainerLocked = true;
        }
    }
}
