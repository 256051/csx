using Framework.Core.Dependency;
using System.Threading;

namespace Framework.BackgroundWorkers.Abstractions
{
    public interface IBackgroundWorker: IRunnable, ISingleton
    {

    }
}
