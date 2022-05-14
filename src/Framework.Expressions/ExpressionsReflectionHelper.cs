using Framework.Core.Dependency;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Framework.Expressions
{
    public class ExpressionsReflectionHelper : IExpressionsReflectionHelper,ISingleton
    {
        private IExpressionsCacheManager _expressionsCacheManager;
        public ExpressionsReflectionHelper(IExpressionsCacheManager expressionsCacheManager)
        {
            _expressionsCacheManager = expressionsCacheManager;
        }

        public object PropertyGetValue(object modelInstance, string propertyName)
        {
            var cache = _expressionsCacheManager.GetCache<string, Func<object, object>>();
            var key = "PropertyGet_" + modelInstance.GetType().FullName + "_" + propertyName;
            var action = cache.GetOrAdd(key, func =>
            {
                //生成参数表达式树
                var instancrParam = Expression.Parameter(typeof(object), "instance");

                //生成(Type)instance表达式
                var instanceConvertedParam = Expression.Convert(
                    instancrParam, 
                    modelInstance.GetType());

                //生成(object)((Type)instance).propertyName 表达式
                instanceConvertedParam = Expression.Convert(Expression.Property(instanceConvertedParam, propertyName), typeof(object));

                //生成lamdba表达式
                var exp = Expression.Lambda<Func<object, object>>(instanceConvertedParam, instancrParam);
                return exp.Compile();
            });
            return action?.Invoke(modelInstance);
        }

        public void PropertySetValue(object modelInstance, string property, object propertyValue)
        {
            var cache = _expressionsCacheManager.GetCache<string, Action<object, object>>();
            var key = "PropertySet_" + modelInstance.GetType().FullName + "_" + property;
            var action = cache.GetOrAdd(key, func =>
            {
                //生成实例表达式树,并强转成指定类型
                var instanceParam = Expression.Parameter(typeof(object), "instance");
                var instanceConvertedParam = Expression.Convert(instanceParam, modelInstance.GetType());

                //生成实例属性值表达式树,并强转成指定类型
                var propertyValueParam=Expression.Parameter(typeof(object), "propertyValue");
                var propertyValueConvertedParam = Expression.Convert(propertyValueParam, propertyValue.GetType());

                //生成(instanceType)instance.Property表达式树
                var InstancePropertyParam =Expression.Property(instanceConvertedParam, property);

                //生成 (instanceType)instance.Property = (propertyType)instanceProperty表达式树
                var assign = Expression.Assign(InstancePropertyParam, propertyValueConvertedParam);

                //生成lamdba表达式
                var exp = Expression.Lambda<Action<object, object>>(assign, instanceParam, propertyValueParam);
                return exp.Compile();
            });
            action?.Invoke(modelInstance, propertyValue);
        }

        public PropertyInfo ReadMemberInfo<T>(Expression<Func<T, object>> expression)
        {
            Expression expressionBody = expression.Body;
            if (expressionBody.NodeType == ExpressionType.Convert)
            {
                expressionBody = ((UnaryExpression)expressionBody).Operand;
            }
            return (PropertyInfo)((MemberExpression)expressionBody).Member;
        }
    }
}
