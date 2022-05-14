using Framework.Core.Dependency;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Framework.RabbitMQ
{
    public class ChannelPool : IChannelPool, ISingleton
    {
        protected ConcurrentDictionary<string, ChannelPoolItem> Channels { get; }

        protected bool _isDisposed;
        protected void CheckDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException(nameof(ChannelPool));
            }
        }

        public void Dispose()
        {
            
        }
    }

    public class ChannelPoolItem : IDisposable
    {
        public IModel Channel { get; }

        public bool IsInUse
        {
            get => _isInUse;
            private set => _isInUse = value;
        }
        private volatile bool _isInUse;

        public ChannelPoolItem(IModel channel)
        {
            Channel = channel;
        }

        public void Acquire()
        {
            lock (this)
            {
                while (IsInUse)
                {
                    Monitor.Wait(this);
                }

                IsInUse = true;
            }
        }

        public void WaitIfInUse(TimeSpan timeout)
        {
            lock (this)
            {
                if (!IsInUse)
                {
                    return;
                }

                Monitor.Wait(this, timeout);
            }
        }

        public void Release()
        {
            lock (this)
            {
                IsInUse = false;
                Monitor.PulseAll(this);
            }
        }

        public void Dispose()
        {
            Channel.Dispose();
        }
    }
}
