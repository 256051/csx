using Framework.Core.Dependency;
using System.Threading;

namespace Framework.BackgroundWorkers
{
    public interface IBackgroundWorker: IRunnable, ISingleton
    {

    }
}
