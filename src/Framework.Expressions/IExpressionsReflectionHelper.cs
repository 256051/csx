using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Framework.Expressions
{
    /// <summary>
    /// 基于表达式树的反射帮助类
    /// </summary>
    public interface IExpressionsReflectionHelper
    {
        /// <summary>
        /// 获取成员表达式的成员信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        PropertyInfo ReadMemberInfo<T>(Expression<Func<T, object>> expression);

        /// <summary>
        /// 根据传入对象实例和属性名称和属性值构建设置实例值得表达式树,并缓存
        /// </summary>
        /// <param name="modelInstance">模型实例</param>
        /// <param name="property">模型属性名称</param>
        /// <param name="value">模型属性值</param>
        void PropertySetValue(object modelInstance, string property, object propertyValue);

        /// <summary>
        /// 根据传入对象实例和属性名称和属性值构建获取对象属性值得表达式树,并缓存
        /// </summary>
        /// <param name="modelInstance"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        object PropertyGetValue(object modelInstance, string propertyName);
    }
}
