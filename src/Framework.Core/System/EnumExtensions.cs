using System;
using System.ComponentModel;

namespace Framework.Core
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取对应枚举值的Description
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription<TEnum>(this Enum value) where TEnum : struct
        {
            if (value == null)
                return string.Empty;
            return GetDescription(typeof(TEnum), value.ToString());
        }

        /// <summary>
        /// 获取枚举值的Description
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string GetDescription(Type enumType, string value)
        {
            if (value == null)
                throw new Exception("the value of enum can not be null");
            var field = enumType?.GetField(value);
            DescriptionAttribute[] result;
            if (field != null & (result = (DescriptionAttribute[])field?.GetCustomAttributes(typeof(DescriptionAttribute), false))?.Length > 0)
            {
                return result[0].Description;
            }
            return null;
        }

        /// <summary>
        /// 获取枚举所有打上的Description特性
        /// </summary>
        /// <returns></returns>
        public static string GetDescription<TEnum>(this int value) where TEnum : Enum
        {
            var result = string.Empty;
            var fields = typeof(TEnum)?.GetFields();
            if (fields.Length == 0)
            {
                throw new BusinessException($"the enum type named {typeof(TEnum).Name} does not containes any public element");
            }
            foreach (var field in fields)
            {
                if (field != null && field.Name != "value__")
                {
                    var enumValue = (int)field.GetValue(null);
                    if (value == enumValue)
                    {
                        var descriptions = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        if (descriptions.Length == 0)
                            throw new BusinessException($"the enum value named {field.Name} do not have Description DescriptionAttribute");
                        result=descriptions[0].Description;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 根据枚举description值获取对应的枚举值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="descriptionStr">枚举的description值</param>
        /// <returns></returns>
        public static int? GetEnumValue<TEnum>(this string descriptionStr) where TEnum : Enum
        {
            int? result = null;
            var fields = typeof(TEnum)?.GetFields();
            if (fields.Length == 0)
            {
                throw new BusinessException($"the enum type named {typeof(TEnum).Name} does not containes any public element");
            }
            foreach (var field in fields)
            {
                if (field != null && field.Name != "value__")
                {
                    var descriptions = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                    foreach (var description in descriptions)
                    {
                        if (description.Description.Equals(descriptionStr))
                        {
                            result=(int)field.GetValue(null);
                            break;
                        }
                    }
                }
                if (result != null)
                    break;
            }
            return result;
        }
    }
}
