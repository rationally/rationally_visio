using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.EventHandlers;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RContainer : RComponent
    {
        public List<RComponent> Children { get; set; }
        public ILayoutManager LayoutManager { get; set; }
        public SizingPolicy UsedSizingPolicy { get; set; }
        public RContainer(Page page) : base(page)
        {
            this.Children = new List<RComponent>();
            this.LayoutManager = new InlineLayout(this);
        }

        public override void Repaint()
        {
            Children.ForEach(c => c.Repaint());
            LayoutManager.Repaint();
        }

        public override void PlaceChildren()
        {
            foreach (RComponent c in this.Children)
            {
                this.MsvSdContainerLocked = false;//TODO reset
                bool lockContainer = false;
                if (c is RContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                this.RShape.ContainerProperties.AddMember(c.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);

                if (c is RContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }

            this.Children.ForEach(c => c.PlaceChildren());
        }

        [Obsolete]
        public override void CascadingDelete()
        { //TODO: remove delete locks
            foreach (RComponent c in this.Children)
            {
                c.CascadingDelete();
            }
            this.RShape.Delete();
        }
    }
}
