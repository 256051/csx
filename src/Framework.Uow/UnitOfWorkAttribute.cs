using System;
using System.Data;

namespace Framework.Uow
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnitOfWorkAttribute : Attribute
    {
        /// <summary>
        /// 工作单元是否事务
        /// </summary>
        public bool? IsTransactional { get; set; }

        /// <summary>
        /// 事务的隔离级别
        /// </summary>
        public IsolationLevel? IsolationLevel { get; set; }
        
        /// <summary>
        /// 是否禁用当前工作单元
        /// </summary>
        public bool IsDisabled { get; set; }

        /// <summary>
        /// 工作单元超时时间
        /// </summary>
        public int? Timeout { get; set; }

        public UnitOfWorkAttribute() { }

        public UnitOfWorkAttribute(bool isTransactional)
        {
            IsTransactional = isTransactional;
        }

        public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel)
        {
            IsTransactional = isTransactional;
            IsolationLevel = isolationLevel;
        }

        public virtual void SetOptions(UnitOfWorkOptions options)
        {
            if (IsTransactional.HasValue)
            {
                options.IsTransactional = IsTransactional.Value;
            }

            if (Timeout.HasValue)
            {
                options.Timeout = Timeout;
            }

            if (IsolationLevel.HasValue)
            {
                options.IsolationLevel = IsolationLevel;
            }
        }
    }
}
