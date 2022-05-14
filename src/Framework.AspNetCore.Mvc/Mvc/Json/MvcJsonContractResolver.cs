using Framework.Core.Dependency;
using Framework.Core.Reflection;
using Framework.Json.Newtonsoft;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Reflection;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    /// <summary>
    /// Mvc Json序列化解析器  重要用于重写时间序列化规则
    /// </summary>
    public class MvcJsonContractResolver : DefaultContractResolver, ITransient
    {
        private readonly Lazy<JsonIsoDateTimeConverter> _dateTimeConverter;
        public MvcJsonContractResolver(IServiceProvider serviceProvider)
        {
            _dateTimeConverter = new Lazy<JsonIsoDateTimeConverter>(
                serviceProvider.GetRequiredService<JsonIsoDateTimeConverter>,
                true
            );

            NamingStrategy = new CamelCaseNamingStrategy();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            ModifyProperty(member, property);

            return property;
        }

        /// <summary>
        /// 返回类型中的时间成员如果没有打DisableDateTimeNormalizationAttribute特性
        /// 那么 时间转换器统一用JsonIsoDateTimeConverter
        /// </summary>
        /// <param name="member"></param>
        /// <param name="property"></param>
        protected virtual void ModifyProperty(MemberInfo member, JsonProperty property)
        {
            if (property.PropertyType != typeof(DateTime) && property.PropertyType != typeof(DateTime?))
            {
                return;
            }

            if (ReflectionHelper.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<DisableDateTimeNormalizationAttribute>(member) == null)
            {
                property.Converter = _dateTimeConverter.Value;
            }
        }
    }
}
