﻿using System.Collections.Generic;
using System.Linq;
using Rationally.Visio.View.Alternatives;
using Microsoft.Office.Interop.Visio;

namespace Rationally.Visio.View
{
     /// <summary>
     /// Represents a container object in Rationally. Name is a shorthand for Rationally Container.
     /// </summary>
    public class RationallyContainer : RationallyComponent
    {
        public List<RationallyComponent> Children { get; set; }
        public ILayoutManager LayoutManager { get; set; }
        public SizingPolicy UsedSizingPolicy { get; set; }
        public RationallyContainer(Page page) : base(page)
        {
            Children = new List<RationallyComponent>();
            LayoutManager = new InlineLayout(this);
        }

        public override void Repaint()
        {
            Children.ForEach(c => c.Repaint());
            if (!Globals.RationallyAddIn.Application.IsUndoingOrRedoing) //Visio handles this for us
            {
                LayoutManager.Repaint();
            }
        }

        public override void PlaceChildren()
        {
            bool oldLock = MsvSdContainerLocked;
            MsvSdContainerLocked = false;
            foreach (RationallyComponent c in Children)
            {
                bool lockContainer = false;
                if (c is RationallyContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                RShape.ContainerProperties.AddMember(c.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);
                
                if (c is RationallyContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }
            MsvSdContainerLocked = oldLock;
            
            Children.ForEach(c => c.PlaceChildren());
        }

        public override void RemoveChildren()
        {
            foreach (RationallyComponent c in Children)
            {
                MsvSdContainerLocked = false;
                bool lockContainer = false;
                if (c is RationallyContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                RShape.ContainerProperties.RemoveMember(c.RShape);
                
                if (c is RationallyContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }

            Children.ForEach(c => c.RemoveChildren());
        }

        public override void RemoveDeleteLock(bool recursive)
        {
            LockDelete = false;
            if (recursive)
            {
                Children.ForEach(c => c.RemoveDeleteLock(true));
            }
        }

        public double ContainerPadding
        {
            get { return RShape.CellsU["User.MsvSDContainerMargin"].ResultIU; }
            set { RShape.CellsU["User.MsvSDContainerMargin"].ResultIU = value; }
        }

        public override bool ExistsInTree(Shape s)
        {
            return RShape.Equals(s) || Children.Exists(x => x.ExistsInTree(s));
        }

        
        public override RationallyComponent GetComponentByShape(Shape s)
        {
            //1) check if current comp is the wanted one, else check it for all the children, then return it if it exists
            return RShape.Equals(s) ? this : Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
        }

        public virtual bool DeleteFromTree(Shape s)
        {
            foreach (RationallyComponent c in Children)
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
                else if (c is RationallyContainer)
                {
                    RationallyContainer container = c as RationallyContainer;
                    if (container.DeleteFromTree(s)) return true;
                }
            }
            return false;
        }

        public virtual bool DeleteFromTree(RationallyComponent toDelete)
        {
            foreach (RationallyComponent c in Children)
            {
                if (c.Equals(toDelete))
                {
                    Children.Remove(c);
                    return true;
                }

                if (c is RationallyContainer)
                {
                    RationallyContainer container = c as RationallyContainer;
                    if (container.DeleteFromTree(c)) return true;
                }
            }
            return false;
        }
    }
}
