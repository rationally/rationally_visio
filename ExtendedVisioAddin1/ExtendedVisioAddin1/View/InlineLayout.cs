﻿using System;
using System.Collections.Generic;

namespace ExtendedVisioAddin1.View
{
    internal class InlineLayout : ILayoutManager
    {
        private readonly RContainer toManage;

        public InlineLayout(RContainer toManage)
        {
            this.toManage = toManage;
        }

        public void Repaint()
        {
            //start the drawing at the left top of the container
            Draw(toManage.CenterX-(toManage.Width/2.0), toManage.CenterY+(toManage.Height/2.0), 0,0,0, new Queue<RComponent>(toManage.Children));
        }
        private void Draw(double x, double y, double currentLineHeight, double contentXEnd, double contentYEnd, Queue<RComponent> components)
        {
            //Base Case
            if (components.Count == 0)
            {
                ShrinkContainer(contentXEnd,contentYEnd);
                return;
            }

            RComponent toDraw = components.Dequeue();
            double toDrawWidth = toDraw.MarginLeft + toDraw.Width + toDraw.MarginRight; //expected increase in x
            double toDrawHeight = toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom;//expected height in y

            if (toDraw is AlternativeContainer)
            {
                var a = 5;
            }
            PrepareContainerExpansion(x,y,toDrawWidth,0); //if the container streches to support the drawing, the container height does not need to change
            double left = toManage.CenterX + (toManage.Width/2.0);
            double right = x + toDrawWidth;
            double d = left/right;//HACK: these three variables somehow fix floating errors in the values in the conditions below
            if (toManage.CenterX + (toManage.Width/2.0) < x + toDrawWidth) //the new component does not fit next to the last component on the same line in the container
            {
                x = toManage.CenterX - (toManage.Width/2.0);//go to a new line
                y -= currentLineHeight; //the new line of components should not overlap with the one above
                PrepareContainerExpansion(x,y,0,toDrawHeight);   
            }

            double dropX = x + toDraw.MarginLeft + (toDraw.Width/2.0);
            double dropY = y - toDraw.MarginTop - (toDraw.Height/2.0);
            double deltaX = dropX - toDraw.CenterX;
            double deltaY = dropY - toDraw.CenterY;
            toDraw.CenterX = dropX;
            toDraw.CenterY = dropY;
            
            /*if (toManage is RContainer)
            {
                bool containerLocked = toManage.MsvSdContainerLocked;
                toManage.MsvSdContainerLocked = false;
                toManage.RShape.ContainerProperties.AddMember(toDraw.RShape, VisMemberAddOptions.visMemberAddDoNotExpand);
                toManage.MsvSdContainerLocked = containerLocked;
            }*/
            //toDraw can have children, that should maintain on the same relative position
            if (toDraw is RContainer)
            {
                foreach (RComponent c in ((RContainer) toDraw).Children)
                {
                    c.CenterX += deltaX;
                    c.CenterY += deltaY;
                }
            }

            x = x + toDrawWidth;
            currentLineHeight = Math.Max(currentLineHeight, toDrawHeight);
            contentXEnd = Math.Max(contentXEnd, dropX + toDrawWidth);
            contentYEnd = Math.Min(contentYEnd, y - toDrawHeight);

            //Recursive Case
            Draw(x,y,currentLineHeight,contentXEnd,contentYEnd,components);
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
            

            bool expandXIfNeeded = ((int) toManage.UsedSizingPolicy & (int) SizingPolicy.ExpandXIfNeeded) > 0;
            bool expandYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ExpandYIfNeeded) > 0;

            //NOTE: expansion is two directional: divided between to the left and to the right
            //update the center according to the new height and original top left (because that should stay the same)

            if (overflowInX && expandXIfNeeded)
            { 
                toManage.Width = (x + xIncrease) - topLeftX + 0.01; 
                toManage.CenterX = topLeftX + (toManage.Width/2.0);

            }

            if (overflowInY && expandYIfNeeded)
            {
                toManage.Height = topLeftY - (y - yIncrease) + 0.01;
                toManage.CenterY = topLeftY - (toManage.Height/2.0);
            }
        }

        private void ShrinkContainer(double contentXEnd, double contentYEnd)
        {
            double topLeftX = toManage.CenterX - (toManage.Width / 2.0);
            double topLeftY = toManage.CenterY + (toManage.Height / 2.0);

            bool underflowInX = (topLeftX + toManage.Width) > contentXEnd;
            bool underflowInY = (topLeftY - toManage.Height) > contentYEnd;

            bool shrinkXIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkXIfNeeded) > 0;
            bool shrinkYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkYIfNeeded) > 0;
        }
    }
}
