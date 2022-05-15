using Microsoft.Extensions.Primitives;
using System;
using System.Threading;

namespace Ms.Configuration
{
    public class ConfigurationReloadToken : IChangeToken
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();

        /// <summary>
        /// Indicates if this token will proactively raise callbacks. Callbacks are still guaranteed to be invoked, eventually.
        /// </summary>
        /// <returns>True if the token will proactively raise callbacks.</returns>
        public bool ActiveChangeCallbacks => true;

        /// <summary>
        /// Gets a value that indicates if a change has occurred.
        /// </summary>
        /// <returns>True if a change has occurred.</returns>
        public bool HasChanged => _cts.IsCancellationRequested;

        /// <summary>
        /// Registers for a callback that will be invoked when the entry has changed. <see cref="Microsoft.Extensions.Primitives.IChangeToken.HasChanged"/>
        /// MUST be set before the callback is invoked.
        /// </summary>
        /// <param name="callback">The callback to invoke.</param>
        /// <param name="state">State to be passed into the callback.</param>
        /// <returns>The <see cref="CancellationToken"/> registration.</returns>
        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => _cts.Token.Register(callback, state);

        /// <summary>
        ///���������¼���ʱ������������
        /// </summary>
        public void OnReload() => _cts.Cancel();
    }
}