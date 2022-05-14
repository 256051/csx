using System;

namespace Framework.DDD.Domain.Entities
{
    //todo领域事件
    /// <summary>
    /// 聚合根基类 
    /// </summary>
    public abstract class BasicAggregateRoot:Entity, IAggregateRoot
    {

    }

    public abstract class BasicAggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    { 
        
    }
}
