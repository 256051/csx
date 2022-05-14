using Framework.Core;
using System;
using System.Reflection;

namespace Framework.BlobStoring
{
    public class BlobContainerAttribute:Attribute
    {
        public string Name { get; }

        public BlobContainerAttribute(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Name = name;
        }

        public virtual string GetName(Type type)
        {
            return Name;
        }

        /// <summary>
        /// 获取容器名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetContainerName<T>()
        {
            return GetContainerName(typeof(T));
        }

        public static string GetContainerName(Type type)
        {
            var nameAttribute = type.GetCustomAttribute<BlobContainerAttribute>();
            if (nameAttribute == null)
            {
                return type.FullName;
            }
            return nameAttribute.GetName(type);
        }
    }
}
