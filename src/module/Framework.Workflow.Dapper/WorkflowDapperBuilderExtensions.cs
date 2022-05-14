using Framework.Workflow.Domain;
using Framework.Workflow.Store;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Framework.Workflow.Dapper
{
    public static class WorkflowDapperBuilderExtensions
    {
        public static WorkflowBuilder AddDapperStores(this WorkflowBuilder builder)
        {
            AddStores(builder.Services,builder.WorkflowType, builder.WorkflowInstanceType);
            return builder;
        }


        private static void AddStores(IServiceCollection service,Type workflow, Type workflowInstance)
        {
            var workflowType = FindGenericBaseType(workflow, typeof(Workflow<>));
            if (workflowType == null)
                throw new InvalidOperationException("WorkflowType not found");
            var keyType = workflowType.GenericTypeArguments[0];
            if (workflowInstance != null)
            {
                var workflowInstanceType = FindGenericBaseType(workflowInstance, typeof(WorkflowInstance<>));
                if (workflowInstanceType == null)
                {
                    throw new InvalidOperationException("WorkflowInstanceType not found");
                }
                var workflowSotre = typeof(WorkflowStore<,,>).MakeGenericType(workflow, workflowInstanceType, keyType);
                service.AddScoped(typeof(IWorkflowStore<>).MakeGenericType(workflow), workflowSotre);

                var workflowInstanceSotre = typeof(WorkflowInstanceStore<,>).MakeGenericType(workflowInstance, keyType);
                service.AddScoped(typeof(IWorkflowInstanceStore<>).MakeGenericType(workflowInstance), workflowInstanceSotre);
            }
        }

        /// <summary>
        /// 查找目标类型的底层类型 到genericBaseType结束  获取主键信息,并构建相关仓储
        /// </summary>
        /// <param name="currentType"></param>
        /// <param name="genericBaseType"></param>
        /// <returns></returns>
        private static TypeInfo FindGenericBaseType(Type currentType,Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
