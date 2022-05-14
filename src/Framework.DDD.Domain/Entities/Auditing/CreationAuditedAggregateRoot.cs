using Framework.Auditing;
using Framework.Core.Configurations;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.DDD.Domain.Entities
{
    /// <summary>
    /// 定义一个需要创建功能的实体,该实体可以拥有组合主键，且包含创建时间和创建人的Id为string
    /// </summary>
    public abstract class CreationAuditedAggregateRoot : AggregateRoot, ICreationAuditedObject
    {
        public virtual DateTime? CreationTime { get; set; }

        public virtual Guid? CreatorId { get; set; }
    }

    /// <summary>
    /// 定义一个需要创建功能的实体,该实体可以拥有单个主键为Id，且包含创建时间和创建人的Id为泛型
    /// </summary>
    public abstract class CreationAuditedAggregateRoot<TKey> : AggregateRoot<TKey>, ICreationAuditedObject<TKey>
        where TKey:IEquatable<TKey>
    {
        public CreationAuditedAggregateRoot()
        {
            CreationTime = DateTime.Now;
        }

        public virtual DateTime CreationTime { get; set; }

        public virtual TKey CreatorId { get; set; }
    }
}
