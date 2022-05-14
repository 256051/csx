using Framework.Core;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace System.Threading
{
    /// <summary>
    /// 顺序调度线程,一个接一个的方式
    /// 对于字符串,所有的相同的字符串会操作同一个锁
    /// </summary>
    public static class RegularThreadHelper
    {
        public static readonly ConcurrentDictionary<string, object> RegularDictionary = new ConcurrentDictionary<string, object>();

        private static readonly ConcurrentDictionary<string, SemaphoreSlim> AsyncRegularConcurrentDictionary = new ConcurrentDictionary<string, SemaphoreSlim>();

        /// <summary>
        /// 并发控制,确保action能正常执行
        /// </summary>
        public static void Invoke(object obj, Action action)
        {
            lock (GetLockObject(obj))
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// 获取锁对象,字符串优化,相同的字符串可以拿到同一个锁对象(争对文件一类的操作)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object GetLockObject(object obj)
        {
            var key = obj as string;
            return key.IsNullOrEmpty() ? obj : RegularDictionary.GetOrAdd(key, i => new object());
        }

        /// <summary>
        /// 针对异步架构的线程同步
        /// </summary>
        /// <param name="lockObj"></param>
        /// <param name="asyncAction"></param>
        /// <returns></returns>
        public static async Task<TResult> InvokeAsync<TResult>(object lockObj, Func<Task<TResult>> asyncAction)
        {
            var asyncLock = GetAsyncLockObj(lockObj);
            await asyncLock.WaitAsync();
            try
            {
                return await asyncAction();
            }
            finally
            {
                asyncLock.Release();
            }
        }

        /// <summary>
        /// 针对异步架构的线程同步
        /// </summary>
        /// <param name="lockObj"></param>
        /// <param name="asyncAction"></param>
        /// <returns></returns>
        public static async Task InvokeAsync(object lockObj, Action action)
        {
            var asyncLock = GetAsyncLockObj(lockObj);
            await asyncLock.WaitAsync();
            try
            {
                action();
            }
            finally
            {
                asyncLock.Release();
            }
        }


        /// <summary>
        /// 获取一个异步线程锁,通过SemaphoreSlim信号量,控制异步架构中,每次只能进入一个线程
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static SemaphoreSlim GetAsyncLockObj(object target)
        {
            var key = target as string;
            key = key ?? target.GetHashCode().ToString();
            return AsyncRegularConcurrentDictionary.GetOrAdd(key, k => new SemaphoreSlim(1, 1));
        }
    }
}
