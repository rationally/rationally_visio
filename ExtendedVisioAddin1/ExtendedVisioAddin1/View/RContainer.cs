using System;
using System.Collections.Generic;
using ExtendedVisioAddin1.View.Alternatives;
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

                var n = RShape.Name;
                if (c is RContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }

            Children.ForEach(c => c.PlaceChildren());
        }

        public override bool ExistsInTree(Shape s)
        {
            return RShape.Equals(s) || Children.Exists(x => x.ExistsInTree(s));
        }


        public virtual bool DeleteFromTree(Shape s)
        {
            foreach (RComponent c in Children)
            {
                if (c.RShape.Equals(s))
                {
                    Children.Remove(c);
                    if (c is AlternativesContainer)
                    {
                        AlternativesContainer container = c as AlternativesContainer;
                        container.RemoveAlternativesFromModel();
                    }
                    return true;
                }
                else if (c is RContainer)
                {
                    RContainer container = c as RContainer;
                    if (container.DeleteFromTree(s)) return true;
                }
            }
            return false;
        }
    }
}
