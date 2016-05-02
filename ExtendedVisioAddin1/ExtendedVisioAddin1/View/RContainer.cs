﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.EventHandlers;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    internal class RContainer : RComponent
    {
        public List<RComponent> Children { get; set; }
        public ILayoutManager LayoutManager { get; set; }
        public SizingPolicy UsedSizingPolicy { get; set; }
        public RContainer(Page page) : base(page)
        {
            this.Children = new List<RComponent>();
        }

        public new void Repaint()
        {
            Children.ForEach(c => c.Repaint());
            LayoutManager.Repaint();
        }

    }
}