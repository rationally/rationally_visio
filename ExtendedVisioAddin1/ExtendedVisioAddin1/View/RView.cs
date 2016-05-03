using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtendedVisioAddin1.Model;
using Microsoft.Office.Interop.Visio;

namespace ExtendedVisioAddin1.View
{
    public class RView : RContainer, Model.IObserver<RModel>
    {
        public RView(Page page) : base(page)
        {

        }

        public void Notify(RModel observable)
        {
            throw new NotImplementedException();
        }
    }
}
