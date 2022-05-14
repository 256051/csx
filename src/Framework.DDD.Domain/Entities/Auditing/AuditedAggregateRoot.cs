using Framework.Auditing;
using Framework.Timing;
using System;

namespace Framework.DDD.Domain.Entities
{

    public abstract class AuditedAggregateRoot : CreationAuditedAggregateRoot, IAuditedObject
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual Guid? LastModifierId { get; set; }
    }

    public abstract class AuditedAggregateRoot<TKey> : CreationAuditedAggregateRoot<TKey>, IAuditedObject<TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual TKey LastModifierId { get; set; }

        protected AuditedAggregateRoot()
        {

        }
    }
}
