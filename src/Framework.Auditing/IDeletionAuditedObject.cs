using System;

namespace Framework.Auditing
{
    public interface IDeletionAuditedObject : IHasDeletionTime
    {
        Guid? DeleterId { get; set; }
    }

    public interface IHasDeletionTime
    {
        DateTime? DeletionTime { get; set; }
    }

    public interface IDeletionAuditedObject<TKey> : IHasDeletionTime
        where TKey:IEquatable<TKey>
    {
        TKey DeleterId { get; set; }
    }
}
