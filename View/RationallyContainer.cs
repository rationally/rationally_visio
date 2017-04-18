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
    public class RationallyContainer : VisioShape
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public List<VisioShape> Children { get; protected set; }
        protected ILayoutManager LayoutManager { private get; set; }
        public SizingPolicy UsedSizingPolicy { get; protected set; }


        protected RationallyContainer(Page page) : base(page)
        {
            Children = new List<VisioShape>();
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
            foreach (VisioShape c in Children.Where(x => !x.Deleted))
            {
                bool lockContainer = false;
                if (c is RationallyContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                Shape.ContainerProperties.AddMember(c.Shape, VisMemberAddOptions.visMemberAddDoNotExpand);
                
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
            foreach (VisioShape c in Children)
            {
                MsvSdContainerLocked = false;
                bool lockContainer = false;
                if (c is RationallyContainer)
                {
                    lockContainer = c.MsvSdContainerLocked;
                    c.MsvSdContainerLocked = false;
                }

                Shape.ContainerProperties.RemoveMember(c.Shape);
                
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
            set { Shape.CellsU["User.MsvSDContainerMargin"].ResultIU = value; }
        }

        public override bool ExistsInTree(Shape s) => Shape.Equals(s) || Children.Exists(x => x.ExistsInTree(s));


        public override VisioShape GetComponentByShape(Shape s) => Shape.Equals(s) ? this : Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);

        public bool DeleteFromTree(Shape s)
        {
            foreach (VisioShape c in Children)
            {
                if (c.Shape.Equals(s))
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
            Shape.Delete();
        }

         public override int Index
         {
             get { return base.Index; }
             set
             {
                 base.Index = value;
                 Children.ForEach(c => c.Index = value);
             }
         }
    }
}
