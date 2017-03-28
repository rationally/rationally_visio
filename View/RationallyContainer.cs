using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.Office.Interop.Visio;
using Rationally.Visio.View.Alternatives;

namespace Rationally.Visio.View
{
     /// <summary>
     /// Represents a container object in Rationally. Name is a shorthand for Rationally Container.
     /// </summary>
    public class RationallyContainer : RationallyComponent
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<RationallyComponent> Children { get; protected set; }
        protected ILayoutManager LayoutManager { private get; set; }
        public SizingPolicy UsedSizingPolicy { get; protected set; }


        protected RationallyContainer(Page page) : base(page)
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
            foreach (RationallyComponent c in Children.Where(x => !x.Deleted))
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
            
            Children.Where(x => !x.Deleted).ToList().ForEach(c => c.PlaceChildren());
        }

        public override void UpdateIndex(int index)
        {
            //set our own index to the new value
            if (Index == index)
            {
                return;
            }
            Index = index;
            //recursively set the one of our children
            Children.ForEach(c => UpdateIndex(index));
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

        protected double ContainerPadding
        {
            set { RShape.CellsU["User.MsvSDContainerMargin"].ResultIU = value; }
        }

        public override bool ExistsInTree(Shape s) => RShape.Equals(s) || Children.Exists(x => x.ExistsInTree(s));


        public override RationallyComponent GetComponentByShape(Shape s) => RShape.Equals(s) ? this : Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);

        public bool DeleteFromTree(Shape s)
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

                if (c is RationallyContainer)
                {
                    RationallyContainer container = c as RationallyContainer;
                    if (container.DeleteFromTree(s))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Marks all child components as deleted, deletes them and then does the same for this component.
        /// </summary>
        public override void DeleteRecursive()
        {
            Children.ForEach(c => c.DeleteRecursive());
            Deleted = true;
            RShape.Delete();
        }
    }
}
