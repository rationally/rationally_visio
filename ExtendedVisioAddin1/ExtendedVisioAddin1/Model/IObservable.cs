namespace ExtendedVisioAddin1.Model
{
    internal interface IObservable<out T>
    {
        void AddObserver(IObserver<T> observer);
        void RemoveObserver(IObserver<T> observer);
    }
}
