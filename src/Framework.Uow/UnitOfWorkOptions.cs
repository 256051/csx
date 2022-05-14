using System.Data;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元参数
    /// </summary>
    public class UnitOfWorkOptions: IUnitOfWorkOptions
    {
        /// <summary>
        /// 工作单元是否事务
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }

        /// <summary>
        /// 工作单元超时时间
        /// </summary>
        public int? Timeout { get; set; }

        public UnitOfWorkOptions()
        {

        }

        public UnitOfWorkOptions(bool isTransactional = false, IsolationLevel? isolationLevel = null)
        {
            IsTransactional = isTransactional;
            IsolationLevel = isolationLevel;
        }

        public UnitOfWorkOptions Clone()
        {
            return new UnitOfWorkOptions
            {
                IsTransactional = IsTransactional,
                IsolationLevel = IsolationLevel
            };
        }
    }
}
