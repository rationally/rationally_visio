using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ExtendedVisioAddin1.Model
{
    public interface IObserver<T>
    {
        void Notify(T observable);
    }
}
