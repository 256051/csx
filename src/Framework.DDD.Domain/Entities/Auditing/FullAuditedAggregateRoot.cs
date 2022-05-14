using Framework.Auditing;
using System;

namespace Framework.DDD.Domain.Entities
{
    public abstract class FullAuditedAggregateRoot : AuditedAggregateRoot, IFullAuditedObject
    {
        public virtual bool IsDeleted { get; set; }

        public virtual Guid? DeleterId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }
    }

    public abstract class FullAuditedAggregateRoot<TKey> : AuditedAggregateRoot<TKey>, IFullAuditedObject<TKey>
        where TKey:IEquatable<TKey>
    {
        public virtual bool IsDeleted { get; set; }

        public virtual TKey DeleterId { get; set; }

        public virtual DateTime? DeletionTime { get; set; }

        public FullAuditedAggregateRoot()
        {
            
        }
    }
}
