using System.Threading;

namespace Framework.BackgroundWorkers.Abstractions
{
    public interface IBackgroundWorkerManager:IRunnable
    {
        /// <summary>
        /// 添加一个后台工作者线程
        /// </summary>
        /// <param name="worker"></param>
        void Add(IBackgroundWorker worker);
    }
}
