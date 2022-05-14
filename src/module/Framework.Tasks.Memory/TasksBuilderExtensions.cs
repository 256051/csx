using Framework.Tasks.Domain;
using Framework.Tasks.Store;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Framework.Tasks.Memory
{
    public static class TasksBuilderExtensions
    {
        /// <summary>
        /// 任务管理内存Store
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static TasksBuilder AddMemoryStore(this TasksBuilder builder)
        {
            AddStores(builder.Services,builder.TasksType, builder.TaskSessionType);
            return builder;
        }


        private static void AddStores(IServiceCollection service,Type tasks,Type taskSessions)
        {
            var tasksType = FindGenericBaseType(tasks, typeof(TaskModel<>));
            if (tasksType == null)
                throw new InvalidOperationException("tasksType not found");
            var keyType = tasksType.GenericTypeArguments[0];
            if (taskSessions != null)
            {
                var tasksSotre = typeof(TaskStore<,>).MakeGenericType(tasks, keyType);
                service.AddSingleton(typeof(ITaskStore<>).MakeGenericType(tasks), tasksSotre);

                var taskSessionsType = FindGenericBaseType(taskSessions, typeof(TaskSession<>));
                if (taskSessionsType == null)
                    throw new InvalidOperationException("taskSessionsType not found");
                var taskSessionKeyType = taskSessionsType.GenericTypeArguments[0];
                var taskSessionsSotre = typeof(TaskSessionStore<,,>).MakeGenericType(taskSessions, tasks, taskSessionKeyType);
                service.AddSingleton(typeof(ITaskSessionStore<>).MakeGenericType(taskSessions), taskSessionsSotre);
            }
            else {
                var tasksSotre = typeof(TaskStore<,>).MakeGenericType(tasks, keyType);
                service.AddSingleton(typeof(ITaskStore<>).MakeGenericType(tasks), tasksSotre);
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
