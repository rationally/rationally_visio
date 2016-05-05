namespace ExtendedVisioAddin1.Model
{
    interface IObservable<T>
    {
        void AddObserver(IObserver<T> observer);
        void RemoveObserver(IObserver<T> observer);
    }
}
