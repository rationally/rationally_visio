using System.Collections.Generic;
using System.Linq;
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
            if (!Globals.ThisAddIn.Application.IsUndoingOrRedoing)
            {
                LayoutManager.Repaint();
            }
        }

        public override void PlaceChildren()
        {
            bool oldLock = MsvSdContainerLocked;
            MsvSdContainerLocked = false;
            foreach (RComponent c in Children)
            {
                bool lockContainer = false; //TODO: WAT
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
            MsvSdContainerLocked = oldLock;
            
            Children.ForEach(c => c.PlaceChildren());
        }

        public override void RemoveChildren()
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

                RShape.ContainerProperties.RemoveMember(c.RShape);
                
                if (c is RContainer)
                {
                    c.MsvSdContainerLocked = lockContainer;
                }
            }

            Children.ForEach(c => c.RemoveChildren());
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

        
        public override RComponent GetComponentByShape(Shape s)
        {
            //1) check if current comp is the wanted one, else check it for all the children, then return it if it exists
            return RShape.Equals(s) ? this : Children.FirstOrDefault(c => c.GetComponentByShape(s) != null)?.GetComponentByShape(s);
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

        public virtual bool DeleteFromTree(RComponent toDelete)
        {
            foreach (RComponent c in Children)
            {
                if (c.Equals(toDelete))
                {
                    Children.Remove(c);
                    return true;
                }
                else if (c is RContainer)
                {
                    RContainer container = c as RContainer;
                    if (container.DeleteFromTree(c)) return true;
                }
            }
            return false;
        }
    }
}
