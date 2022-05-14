using Framework.Core.Dependency;
using System.Threading;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元环境域实现
    /// 注:使用AsyncLocal是因为基于Task的自动调度机制在异步上下文的切换中,可能存在丢失数据的情况.AsyncLocal则是用来解决该问题的
    /// </summary>
    public class AmbientUnitOfWork: IAmbientUnitOfWork, ISingleton
    {
        public IUnitOfWork UnitOfWork => _currentUow.Value;

        private readonly AsyncLocal<IUnitOfWork> _currentUow;

        public AmbientUnitOfWork()
        {
            _currentUow = new AsyncLocal<IUnitOfWork>();
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            _currentUow.Value = unitOfWork;
        }
    }
}
