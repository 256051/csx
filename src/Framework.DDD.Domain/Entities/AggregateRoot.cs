using System;

namespace Framework.DDD.Domain.Entities
{
    public abstract class AggregateRoot : BasicAggregateRoot
    {
       
    }

    public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>
        where TKey : IEquatable<TKey>
    {
        
    }
}
