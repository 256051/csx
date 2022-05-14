using Framework.Core.Dependency;

namespace System.Threading
{
    public class NullCancellationTokenProvider : ICancellationTokenProvider,ITransient
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public CancellationToken Token { get; } = CancellationToken.None;

        public NullCancellationTokenProvider()
        {

        }
    }
}
