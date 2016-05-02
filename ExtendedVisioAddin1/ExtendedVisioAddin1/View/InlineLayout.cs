using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.View
{
    internal class InlineLayout : ILayoutManager
    {
        private RContainer toManage;

        public InlineLayout(RContainer toManage)
        {
            this.toManage = toManage;
        }

        public void Repaint()
        {
            //start the drawing at the left top of the container
            this.Draw(toManage.X-(toManage.Width/2.0), toManage.Y+(toManage.Height/2.0), 0, new Queue<RComponent>(toManage.Children));
        }

        private void Draw(double x, double y, double currentLineHeight, Queue<RComponent> components)
        {
            //Base Case
            if (components.Count == 0)
            {
                return;//TODO shrinking
            }

            RComponent toDraw = components.Dequeue();
            double toDrawWidth = toDraw.MarginLeft + toDraw.Width + toDraw.MarginRight; //expected increase in x
            double toDrawHeight = toDraw.MarginTop + toDraw.Height + toDraw.MarginBottom;//expected height in y

            PrepareContainerExpansion(x,y,toDrawWidth,0); //if the container streches to support the drawing, the container height does not need to change

            if (toManage.X + (toDrawWidth/2) < x + toDrawWidth) //the new component does not fit next to the last component on the same line in the container
            {
                x = 0;//go to a new line
                y -= currentLineHeight; //the new line of components should not overlap with the one above
                PrepareContainerExpansion(x,y,0,toDrawHeight);   
            }

            toManage.Page.Drop(toDraw,x,y);//TODO +half size
        }

        /// <summary>
        /// Streches the container size, depending on its sizing policy and the increment in content that is scheduled
        /// </summary>
        /// <param name="x">x pointer of the drawing procedure.</param>
        /// <param name="y">y pointer of the drawing procedure.</param>
        /// <param name="xIncrease">Scheduled increase of content in x.</param>
        /// <param name="yIncrease">Scheduled increase of content in y.</param>
        private void PrepareContainerExpansion(double x, double y, double xIncrease, double yIncrease)
        {
            double topLeftX = toManage.X - (toManage.Width/2.0);
            double topLeftY = toManage.X - (toManage.Width / 2.0);

            bool overflowInX = (topLeftX + toManage.Width) < (x + xIncrease);
            bool overflowInY = (topLeftY - toManage.Height) < (y - yIncrease); //coordinate system starts at left bottom. Y increases when going up on the page
            

            bool expandXIfNeeded = ((int) toManage.UsedSizingPolicy & (int) SizingPolicy.ExpandXIfNeeded) > 0;
            bool expandYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ExpandYIfNeeded) > 0;



            //NOTE: expansion is two directional: divided between to the left and to the right
            //update the center according to the new height and original top left (because that should stay the same)

            if (overflowInX && expandXIfNeeded)
            { 
                toManage.Width = (x + xIncrease) - topLeftX; 
                toManage.X = (topLeftX + toManage.Width)/2.0;

            }

            if (overflowInY && expandYIfNeeded)
            {
                toManage.Height = topLeftY - (y - yIncrease);
                toManage.Y = (topLeftY - toManage.Height)/2.0;
            }
        }

        private void ShrinkContainer(double contentXEnd, double contentYEnd)
        {
            double topLeftX = toManage.X - (toManage.Width / 2.0);
            double topLeftY = toManage.X - (toManage.Width / 2.0);

            bool underflowInX = (topLeftX + toManage.Width) > contentXEnd;
            bool underflowInY = (topLeftY - toManage.Height) > contentYEnd;

            bool shrinkXIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkXIfNeeded) > 0;
            bool shrinkYIfNeeded = ((int)toManage.UsedSizingPolicy & (int)SizingPolicy.ShrinkYIfNeeded) > 0;
        }
    }
}
