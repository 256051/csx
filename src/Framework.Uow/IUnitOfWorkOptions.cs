using System.Data;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元参数配置约束
    /// </summary>
    public interface IUnitOfWorkOptions
    {
        /// <summary>
        /// 是否事务
        /// </summary>
        bool? IsTransactional { get; }

        /// <summary>
        /// 事务的隔离级别
        /// </summary>
        IsolationLevel? IsolationLevel { get; }
    }
}
