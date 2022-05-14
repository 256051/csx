namespace Framework.Core.Dependency
{
    public interface IObjectAccessor<out T>
    {
        T Value { get; }
    }
}
