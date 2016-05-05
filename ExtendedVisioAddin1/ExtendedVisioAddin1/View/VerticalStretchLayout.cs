using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.View
{
    class VerticalStretchLayout : ILayoutManager
    {
        private RContainer toManage;

        public VerticalStretchLayout(RContainer toManage)
        {
            this.toManage = toManage;
        }

        /// <summary>
        /// Places the component first in line in the container, at (x,y)
        /// </summary>
        /// <param name="x">top left x-coordinate to start drawing the component.</param>
        /// <param name="y">top left y-coordinate to start drawing the component.</param>
        /// <param name="components">Queue of components to draw.</param>
        public void Draw(double x, double y, Queue<RComponent> components )
        {
            //base case
            if (components.Count == 0)
            {
                return;
            }

            RComponent toDraw = components.Dequeue();
            double widthToDraw = toDraw.MarginLeft + toDraw.Width + toDraw.MarginRight;
            double heightToDraw = toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom;

            //allow container to stretch horizontally and/or vertically if the content component overflows in those directions
            this.PrepareContainerExpansion(x,y,widthToDraw,heightToDraw);

            //this layout stacks components vertically and stretches them horizontally to the width of the container
            this.StretchComponentIfNeeded(toDraw,toManage.Width);

            //calculate position to draw this component
            double drawX = x + (toDraw.Width/2.0) + toDraw.MarginLeft;
            double drawY = y - (toDraw.Height/2.0) - toDraw.MarginTop;
            double deltaX = drawX - (toDraw.CenterX);
            double deltaY = drawY - (toDraw.CenterY);

            //move the children of this container, and then the container itself
            if (toDraw is RContainer)
            {
                foreach (RComponent c in ((RContainer)toDraw).Children)
                {
                    c.CenterX += deltaX;
                    c.CenterY += deltaY;
                }
            }
            toDraw.CenterX = drawX;
            toDraw.CenterY = drawY;

            //update x and y for the next component
            //x remains the same
            y = y - (toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom);

            //recursive case
            Draw(x,y,components);

        }

        public void Repaint()
        {
            //draw (left top of content area) (children)
            this.Draw(toManage.CenterX - (toManage.Width/2.0),toManage.CenterY + (toManage.Height/2.0),new Queue<RComponent>(toManage.Children));
            toManage.Children.ForEach(c => c.Repaint());
        }

        /// <summary>
        /// Streches the container size, depending on its sizing policy and the increment in content that is scheduled
        /// </summary>
        /// <param name="x">x pointer of the drawing procedure.</param>
        /// <param name="y">y pointer of the drawing procedure.</param>
        /// <param name="xIncrease">Scheduled increase of content in x-dimension starting from x.</param>
        /// <param name="yIncrease">Scheduled increase of content in y-dimension starting from y.</param>
        private void PrepareContainerExpansion(double x, double y, double xIncrease, double yIncrease)
        {
            double topLeftX = toManage.CenterX - (toManage.Width / 2.0);
            double topLeftY = toManage.CenterY + (toManage.Height / 2.0);

            bool overflowInX = (topLeftX + toManage.Width) < (x + xIncrease);
            bool overflowInY = (topLeftY - toManage.Height) > (y - yIncrease); //coordinate system starts at left bottom. Y increases when going up on the page


            bool expandXIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ExpandXIfNeeded) > 0;
            bool expandYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ExpandYIfNeeded) > 0;

            //NOTE: expansion is two directional: divided between to the left and to the right
            //update the center according to the new height and original top left (because that should stay the same)

            if (overflowInX && expandXIfNeeded)
            {
                toManage.Width = (x + xIncrease) - topLeftX;
                toManage.CenterX = (topLeftX + toManage.Width) / 2.0;

            }

            if (overflowInY && expandYIfNeeded)
            {
                toManage.Height = topLeftY - (y - yIncrease);
                toManage.CenterY = (topLeftY - toManage.Height) / 2.0;
            }
        }

        /// <summary>
        /// Stretches the component horizontally to the containerWidth, if the component's width is smaller.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="containerWidth"></param>
        private void StretchComponentIfNeeded(RComponent component, double containerWidth)
        {
            double marginIncludedWidth = component.MarginLeft + component.Width + component.MarginRight;
            if (marginIncludedWidth < containerWidth)
            {
                component.Width = containerWidth - (component.MarginLeft + component.MarginRight);
            }
        }
    }
}
