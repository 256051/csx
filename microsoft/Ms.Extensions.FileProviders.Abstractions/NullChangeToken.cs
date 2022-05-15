using System;
using Microsoft.Extensions.Primitives;

namespace Ms.Extensions.FileProviders.Abstractions
{
    public class NullChangeToken : IChangeToken
    {
        public static NullChangeToken Singleton { get; } = new NullChangeToken();

        private NullChangeToken()
        {
        }

        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            return EmptyDisposable.Instance;
        }
    }
}
