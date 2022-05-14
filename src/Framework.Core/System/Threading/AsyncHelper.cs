using Framework.Core;
using Nito.AsyncEx;
using System.Reflection;
using System.Threading.Tasks;

namespace System.Threading
{
    public static class AsyncHelper
    {
        /// <summary>
        /// 去掉Task包围
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type UnwrapTask(Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type == typeof(Task))
            {
                return typeof(void);
            }

            if (type.IsTaskOfT())
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        /// <summary>
        /// 是否是被Task包围的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsTaskOfT(this Type type)
        {
            return type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>);
        }

        /// <summary>
        /// 开启一个新的task
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> func)
        {
            return AsyncContext.Run(func);
        }

        /// <summary>
        /// 开启一个新的task
        /// </summary>
        /// <param name="action"></param>
        public static void RunSync(Func<Task> action)
        {
            AsyncContext.Run(action);
        }
    }
}
