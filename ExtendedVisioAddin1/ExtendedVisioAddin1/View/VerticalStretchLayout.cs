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

        public void Draw(int x, int y)
        {
            
        }

        public void Repaint()
        {
            throw new NotImplementedException();
        }
    }
}
