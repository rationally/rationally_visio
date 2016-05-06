namespace ExtendedVisioAddin1.Model
{
    public interface IObserver<in T>
    {
        void Notify(T observable);
    }
}
