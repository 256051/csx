using Framework.Core;
using System.Data;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元默认配置
    /// </summary>
    public class UnitOfWorkDefaultOptions
    {
        public UnitOfWorkTransactionBehavior TransactionBehavior { get; set; } = UnitOfWorkTransactionBehavior.Calculate;

        /// <summary>
        /// 事务隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; } 

        /// <summary>
        /// 是否事务
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 如果UnitOfWorkOptions没有设置,那么获取默认配置
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        internal UnitOfWorkOptions Normalize(UnitOfWorkOptions options)
        {
            if (options.IsolationLevel == null)
            {
                options.IsolationLevel = IsolationLevel;
            }

            if (options.IsTransactional==null)
            {
                options.IsTransactional = IsTransactional;
            }
            return options;
        }

        /// <summary>
        /// 根据条件计算得出是否需要开启事务
        /// </summary>
        /// <param name="calculateValue"></param>
        /// <returns></returns>
        public bool CalculateIsTransactional(bool calculateValue)
        {
            switch (TransactionBehavior)
            {
                case UnitOfWorkTransactionBehavior.Enabled:
                    return true;
                case UnitOfWorkTransactionBehavior.Disabled:
                    return false;
                case UnitOfWorkTransactionBehavior.Calculate:
                    return calculateValue;
                default:
                    throw new FrameworkException("Not implemented TransactionBehavior value: " + TransactionBehavior);
            }
        }
    }
}
