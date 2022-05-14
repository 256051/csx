using System;

namespace Framework.Auditing
{
    /// <summary>
    /// 定义需要创建时间的实体
    /// </summary>
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; }
    }
}
