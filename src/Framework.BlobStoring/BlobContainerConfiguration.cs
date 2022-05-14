using Framework.Core;
using Framework.Core.Collections;
using System;
using System.Collections.Generic;

namespace Framework.BlobStoring
{
    /// <summary>
    /// 容器配置
    /// </summary>
    public class BlobContainerConfiguration
    {
        /// <summary>
        /// 容器提供类型
        /// </summary>
        public Type ProviderType { get; set; }

        /// <summary>
        /// 容器名称和块名称的标准化器,交个各自云服务提供商的子模块实现
        /// </summary>
        public ITypeList<IBlobNamingNormalizer> NamingNormalizers { get; }

        /// <summary>
        /// 容器配置
        /// </summary>
        private readonly Dictionary<string, object> _properties;

        /// <summary>
        /// 已存在的容器配置
        /// </summary>
        private readonly BlobContainerConfiguration _fallbackConfiguration;

        public BlobContainerConfiguration(BlobContainerConfiguration fallbackConfiguration = null)
        {
            _fallbackConfiguration = fallbackConfiguration;
            NamingNormalizers = new TypeList<IBlobNamingNormalizer>();
            _properties = new Dictionary<string, object>();
        }

        public T GetConfigurationOrDefault<T>(string name, T defaultValue = default)
        {
            return (T)GetConfigurationOrNull(name, defaultValue);
        }

        public object GetConfigurationOrNull(string name, object defaultValue = null)
        {
            return _properties.GetOrDefault(name) ??
                   _fallbackConfiguration?.GetConfigurationOrNull(name, defaultValue) ??
                   defaultValue;
        }

        public BlobContainerConfiguration SetConfiguration(string name, object value)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(value, nameof(value));
            _properties[name] = value;
            return this;
        }

        public BlobContainerConfiguration ClearConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            _properties.Remove(name);
            return this;
        }
    }
}
