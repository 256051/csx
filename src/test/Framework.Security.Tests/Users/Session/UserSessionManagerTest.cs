using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using Xunit;

namespace Framework.Security.Tests.Users.Session
{
    public class UserSessionManagerTest: SecurityTest
    {
        /// <summary>
        /// 主方法
        /// </summary>
        [Fact]
        public void Execute()
        {
            var manager = ServiceProvider.GetRequiredService<ContextManager>();
            var context=manager.CreateContext();

            //业务方法拿到上下文的方式
            var currentContext=manager.CurrentContext;

            //释放上下文
            context.Dispose();
        }

        /// <summary>
        /// 上下文
        /// </summary>
        public class Contenxt:ITransient, IDisposable
        { 
            public string Data { get; set; }

            /// <summary>
            /// 上下文释放事件
            /// </summary>
            public event EventHandler<ContenxtEventArgs> Disposed;

            public void Dispose()
            {
                Disposed?.Invoke(this, new ContenxtEventArgs(this));
            }
        }

        /// <summary>
        /// 上下文释放时间参数
        /// </summary>
        public class ContenxtEventArgs : EventArgs
        {
            public Contenxt Contenxt { get; }

            public ContenxtEventArgs(Contenxt contenxt)
            {
                Contenxt = contenxt;
            }
        }

        /// <summary>
        /// 上下文管理
        /// </summary>
        public class ContextManager:ISingleton
        {
            private AmbientContext _ambientContext;

            private readonly IServiceScopeFactory _serviceScopeFactory;

            public ContextManager(AmbientContext ambientContext, IServiceScopeFactory serviceScopeFactory)
            {
                _ambientContext = ambientContext;
                _serviceScopeFactory = serviceScopeFactory;
            }

            public Contenxt CurrentContext => GetCurrentContext();

            private Contenxt GetCurrentContext() => _ambientContext.Contenxt;

            /// <summary>
            /// 创建上下文
            /// </summary>
            /// <returns></returns>
            public Contenxt CreateContext()
            {
                var scope = _serviceScopeFactory.CreateScope();
                try
                {
                    var outContext = _ambientContext.Contenxt;
                    var context = scope.ServiceProvider.GetRequiredService<Contenxt>();
                    _ambientContext.SetContext(context);
                    context.Disposed += (sender, args) =>
                    {
                        _ambientContext.SetContext(outContext);
                        scope.Dispose();
                    };
                    return context;
                }
                catch
                {
                    scope.Dispose();
                    throw;
                }
            }
        }

        /// <summary>
        /// 上下文传递数据结构
        /// </summary>
        public class AmbientContext : ISingleton
        {
            private AsyncLocal<Contenxt> _currentContext;

            public Contenxt Contenxt => _currentContext.Value;

            public AmbientContext()
            {
                _currentContext = new AsyncLocal<Contenxt>();
            }

            public void SetContext(Contenxt contenxt)
            {
                _currentContext.Value = contenxt;
            }
        }
    }
}
