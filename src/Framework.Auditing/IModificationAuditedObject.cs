using System;

namespace Framework.Auditing
{
    /// <summary>
    /// 定义一个需要包含最后一次修改时间的实体
    /// </summary>
    public interface IModificationAuditedObject 
    {
        Guid? LastModifierId { get; set; }

        DateTime? LastModificationTime { get; set; }
    }

    public interface IModificationAuditedObject<TKey> : IHasModificationTime
    {
        TKey LastModifierId { get; set; }
    }

    public interface IHasModificationTime
    {
        DateTime? LastModificationTime { get; set; }
    }
}
