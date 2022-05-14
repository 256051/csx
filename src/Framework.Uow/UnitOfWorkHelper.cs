using System.Linq;
using System.Reflection;

namespace Framework.Uow
{
    public static class UnitOfWorkHelper
    {
        /// <summary>
        /// 获取传入方法上的工作单元特性,如果方法上没有,则找类上的
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <returns></returns>
        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MethodInfo methodInfo)
        {
            var attrs = methodInfo.GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            attrs = methodInfo.DeclaringType.GetTypeInfo().GetCustomAttributes(true).OfType<UnitOfWorkAttribute>().ToArray();
            if (attrs.Length > 0)
            {
                return attrs[0];
            }

            return null;
        }
    }
}
