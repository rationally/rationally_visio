using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtendedVisioAddin1.Model
{
    interface IObservable<T>
    {
        void AddObserver(IObserver<T> observer);
        void RemoveObserver(IObserver<T> observer);
    }
}
