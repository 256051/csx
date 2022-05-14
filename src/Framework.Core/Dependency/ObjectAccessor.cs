namespace Framework.Core.Dependency
{
    public class ObjectAccessor<T> : IObjectAccessor<T>
    {
        public T Value { get; set; }

        public ObjectAccessor()
        {

        }

        public ObjectAccessor(T obj)
        {
            Value = obj;
        }
    }
}
