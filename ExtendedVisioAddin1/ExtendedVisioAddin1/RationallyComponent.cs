using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1
{


    class RationallyComponent
    {
        private Master componentMaster;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="componentMaster">Component that is represented by this class.</param>
        public RationallyComponent(Master componentMaster)
        {
            this.componentMaster = componentMaster;
        }

        public void Place()
        {

        }
    }
}
