﻿using System;
using System.Collections.Generic;
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
            Children = new List<RComponent>();
            LayoutManager = new InlineLayout(this);
        }

        public override void Repaint()
        {
            Children.ForEach(c => c.Repaint());
            LayoutManager.Repaint();
        }

        public override void PlaceChildren()
        {
            foreach (RComponent c in Children)
            {
                MsvSdContainerLocked = false;//TODO reset
                bool lockContainer = false;
                if (c is RContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                RShape.ContainerProperties.AddMember(c.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);

                if (c is RContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }

            Children.ForEach(c => c.PlaceChildren());
        }

        [Obsolete]
        public override void CascadingDelete()
        { //TODO: remove delete locks
            foreach (RComponent c in Children)
            {
                c.CascadingDelete();
            }
            RShape.Delete();
        }
    }
}
