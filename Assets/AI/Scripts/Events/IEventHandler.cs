public interface IEventHandler
{
    void Raise();
}
public interface IEventHandler<T>
{
    void Raise(T value);
}