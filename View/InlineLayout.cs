using System;
using System.Collections.Generic;
using Rationally.Visio.RationallyConstants;

// ReSharper disable ArrangeRedundantParentheses

namespace Rationally.Visio.View
{
    internal class InlineLayout : ILayoutManager
    {
        private readonly RationallyContainer toManage;

        public InlineLayout(RationallyContainer toManage)
        {
            this.toManage = toManage;
        }

        public void Repaint()
        {
            if (toManage.Children.Count == 0) { return; }

            //start the drawing at the left top of the container
            Draw(toManage.CenterX - (toManage.Width / 2.0), toManage.CenterY + (toManage.Height / 2.0), 0, 0, double.MaxValue, new Queue<RationallyComponent>(toManage.Children));
        }

        private void Draw(double x, double y, double currentLineHeight, double contentXEnd, double contentYEnd, Queue<RationallyComponent> components)
        {
            while (components.Count > 0)
            {
                RationallyComponent toDraw = components.Dequeue();
                double toDrawWidth = toDraw.MarginLeft + toDraw.Width + toDraw.MarginRight; //expected increase in x
                double toDrawHeight = toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom; //expected height in y

                PrepareContainerExpansion(x, y, toDrawWidth, 0); //if the container streches to support the drawing, the container height does not need to change
                if (toManage.CenterX + (toManage.Width/2.0) < x + toDrawWidth) //the new component does not fit next to the last component on the same line in the container
                {
                    x = toManage.CenterX - (toManage.Width/2.0); //go to a new line
                    y -= currentLineHeight; //the new line of components should not overlap with the one above
                    PrepareContainerExpansion(x, y, 0, toDrawHeight);
                }

                double dropX = x + toDraw.MarginLeft + (toDraw.Width/2.0);
                double dropY = y - toDraw.MarginTop - (toDraw.Height/2.0);
                double deltaX = dropX - toDraw.CenterX;
                double deltaY = dropY - toDraw.CenterY;
                toDraw.CenterX = dropX;
                toDraw.CenterY = dropY;

                if (toDraw is RationallyContainer)
                {
                    foreach (RationallyComponent c in ((RationallyContainer) toDraw).Children)
                    {
                        c.CenterX += deltaX;
                        c.CenterY += deltaY;
                        c.MoveChildren(deltaX, deltaY);
                    }
                }
                else
                {
                    toDraw.MoveChildren(deltaX, deltaY);
                }

                x = x + toDrawWidth;
                currentLineHeight = Math.Max(currentLineHeight, toDrawHeight);
                contentXEnd = Math.Max(contentXEnd, dropX + (toDrawWidth/2.0));
                contentYEnd = Math.Min(contentYEnd, y - toDrawHeight);
            }

            //the container might still be not high enough, if the initial height is very small and expandX is true
            if ((toManage.UsedSizingPolicy & SizingPolicy.ExpandYIfNeeded) > 0 && currentLineHeight > toManage.Height)
            {
                double topLeftY = toManage.CenterY + (toManage.Height / 2.0);
                toManage.Height = currentLineHeight;
                toManage.CenterY = topLeftY - (toManage.Height / 2.0);
            }
            ShrinkContainer(contentXEnd, contentYEnd);
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
                toManage.Width = (x + xIncrease) - topLeftX + Constants.Epsilon;
                toManage.CenterX = topLeftX + (toManage.Width / 2.0);

            }

            if (overflowInY && expandYIfNeeded)
            {
                toManage.Height = topLeftY - (y - yIncrease) + Constants.Epsilon;
                toManage.CenterY = topLeftY - (toManage.Height / 2.0);
            }
        }

        private void ShrinkContainer(double contentXEnd, double contentYEnd)
        {
            double topLeftX = toManage.CenterX - (toManage.Width / 2.0);
            double topLeftY = toManage.CenterY + (toManage.Height / 2.0);

            bool underflowInX = (topLeftX + toManage.Width) > contentXEnd;
            bool underflowInY = (topLeftY - toManage.Height) < contentYEnd;

            bool shrinkXIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkXIfNeeded) > 0;
            bool shrinkYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkYIfNeeded) > 0;
            
            if (underflowInX && shrinkXIfNeeded)
            {
                toManage.Width = contentXEnd - topLeftX + Constants.Epsilon;
                toManage.CenterX = topLeftX + (toManage.Width / 2.0);
            }

            if (underflowInY && shrinkYIfNeeded)
            {
                toManage.Height = topLeftY - contentYEnd + Constants.Epsilon;
                toManage.CenterY = topLeftY - (toManage.Height / 2.0);
            }
        }
    }
}
