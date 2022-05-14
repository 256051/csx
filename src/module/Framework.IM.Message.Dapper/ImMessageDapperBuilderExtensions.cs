using Framework.IM.Message.Domain;
using Framework.IM.Message.Store;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Framework.IM.Message.Dapper
{
    public static class ImMessageDapperBuilderExtensions
    {
        public static ImMessageBuilder AddDapperStores(this ImMessageBuilder builder)
        {
            AddStores(builder.Services,builder.UserMessageType);
            return builder;
        }


        private static void AddStores(IServiceCollection service,Type userMessage)
        {
            var userMessageType = FindGenericBaseType(userMessage, typeof(UserMessage<>));
            if (userMessageType == null)
                throw new InvalidOperationException("userMessageType not found");
            var keyType = userMessageType.GenericTypeArguments[0];
            var workflowSotre = typeof(UserMessageStore<,>).MakeGenericType(userMessage, keyType);
            service.AddScoped(typeof(IUserMessageStore<>).MakeGenericType(userMessage), workflowSotre);
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
