using System.Reflection;

namespace System
{
    public static class TypeExtensions
    {
        public static string GetFullNameWithAssemblyName(this Type type)
        {
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }
        public static TypeInfo FindGenericBaseType(this Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = typeInfo.IsGenericType ? typeInfo.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                    return typeInfo;
                type = type.BaseType;
            }
            return null;
        }
    }
}
