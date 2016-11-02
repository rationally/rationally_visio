using System.Collections.Generic;
using Rationally.Visio.RationallyConstants;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View
{
    internal class VerticalStretchLayout : ILayoutManager
    {
        private readonly RationallyContainer toManage;

        public VerticalStretchLayout(RationallyContainer toManage)
        {
            this.toManage = toManage;
        }

        /// <summary>
        /// Places the component first in line in the container, at (x,y)
        /// </summary>
        /// <param name="x">top left x-coordinate to start drawing the component.</param>
        /// <param name="y">top left y-coordinate to start drawing the component.</param>
        /// <param name="components">Queue of components to draw.</param>
        public void Draw(double x, double y, Queue<RationallyComponent> components)
        {
            while (components.Count > 0)
            {
                RationallyComponent toDraw = components.Dequeue();
                double totalWidthToDraw = toDraw.MarginLeft + toDraw.Width + toDraw.MarginRight;
                double totalHeightToDraw = toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom;

                //allow container to stretch horizontally and/or vertically if the content component overflows in those directions
                PrepareContainerExpansion(x, y, totalWidthToDraw, totalHeightToDraw);

                //this layout stacks components vertically and stretches them horizontally to the width of the container
                StretchComponentIfNeeded(toDraw, toManage.Width);

                //calculate position to draw this component
                double drawX = x + (toDraw.Width/2.0) + toDraw.MarginLeft;
                double drawY = y - (toDraw.Height/2.0) - toDraw.MarginTop;
                double deltaX = drawX - toDraw.CenterX;
                double deltaY = drawY - toDraw.CenterY;

                if (toDraw is RationallyContainer)
                {
                    foreach (RationallyComponent c in ((RationallyContainer) toDraw).Children)
                    {
                        if (c.RShape.ContainerProperties != null)
                        {
                            //moving children will disband the composite pattern between the shapes => remember children and later rebuild the pattern
                            c.StoreChildren();
                            c.MoveChildren(deltaX, deltaY);
                        }
                        c.CenterX += deltaX;
                        c.CenterY += deltaY;

                        if (c.RShape.ContainerProperties != null)
                        {
                            c.RestoreChildren();
                        }
                    }
                }
                else
                {
                    toDraw.MoveChildren(deltaX, deltaY);
                }
                toDraw.CenterX = drawX;
                toDraw.CenterY = drawY;

                //update x and y for the next component
                //x remains the same
                y = y - (toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom);
            }
            ShrinkContainer(y); //y points below the last added component
        }

        public void Repaint()
        {
            if (toManage.Children.Count == 0) { return; }
            
            //draw (left top of content area) (children)
            Draw(toManage.CenterX - (toManage.Width/2.0),toManage.CenterY + (toManage.Height/2.0),new Queue<RationallyComponent>(toManage.Children));
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
                toManage.Width = x + xIncrease - topLeftX + Constants.Epsilon;
                toManage.CenterX = topLeftX + (toManage.Width / 2.0);

            }

            if (overflowInY && expandYIfNeeded)
            {
                toManage.Height = topLeftY - (y - yIncrease) + Constants.Epsilon;
                toManage.CenterY = topLeftY - (toManage.Height / 2.0);
            }
        }

        /// <summary>
        /// Stretches the component horizontally to the containerWidth, if the component's width is smaller.
        /// </summary>
        /// <param name="component"></param>
        /// <param name="containerWidth"></param>
        private static void StretchComponentIfNeeded(RationallyComponent component, double containerWidth)
        {
            double marginIncludedWidth = component.MarginLeft + component.Width + component.MarginRight;
            if (marginIncludedWidth < containerWidth)
            {
                component.Width = containerWidth - (component.MarginLeft + component.MarginRight) - Constants.Epsilon;
            }
        }

        private void ShrinkContainer(double contentYEnd)
        {
            double topLeftY = toManage.CenterY + (toManage.Height / 2.0);

            bool underflowInY = (topLeftY - toManage.Height) < contentYEnd;

            bool shrinkYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkYIfNeeded) > 0;

            if (underflowInY && shrinkYIfNeeded)
            {
                toManage.Height = topLeftY - contentYEnd + Constants.Epsilon;
                toManage.CenterY = topLeftY - (toManage.Height / 2.0);
            }
        }
    }
}
