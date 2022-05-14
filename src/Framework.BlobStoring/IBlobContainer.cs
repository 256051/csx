using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BlobStoring
{
    public interface IBlobContainer<TContainer> : IBlobContainer where TContainer : class
    {

    }

    public interface IBlobContainer
    {
        /// <summary>
        /// 保存对象
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="stream">流对象</param>
        /// <param name="overrideExisting">是否覆盖</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SaveAsync(
          string name,
          Stream stream,
          bool overrideExisting = false,
          CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 获取对象 存在null的情况
        /// </summary>
        /// <param name="name"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default
        );
    }
}
