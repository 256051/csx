using System;

namespace Framework.Timing
{
    /// <summary>
    /// 禁用时间标准化特性,配合json序列化组件
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class DisableDateTimeNormalizationAttribute : Attribute
    {

    }
}
