using AutoMapper;
using Framework.Core;
using System;

namespace Framework.AutoMapper
{
    /// <summary>
    /// 枚举值转换器 将枚举value转换成对应的Description特性值
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumValueTransfter<TEnum> : IValueConverter<int?, string> where TEnum : Enum
    {
        public string Convert(int? sourceMember, ResolutionContext context)
        {
            if (!sourceMember.HasValue) return string.Empty;
            return sourceMember.Value.GetDescription<TEnum>();
        }
    }

    /// <summary>
    /// 枚举Description值转换器 将枚举Description转换成对应枚举值
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    public class EnumDescriptionTransfter<TEnum> : IValueConverter<string, int?> where TEnum : Enum
    {
        public int? Convert(string sourceMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(sourceMember)) return null;
            return sourceMember.GetEnumValue<TEnum>();
        }
    }
}
