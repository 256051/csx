using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    ///    启动/停止 自线程服务的接口
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// 启动服务
        /// </summary>
        Task StartAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// 停止服务
        /// </summary>
        Task StopAsync(CancellationToken cancellationToken = default);
    }
}
