using System;

namespace Framework.DDD.Domain.Entities
{
    /// <summary>
    /// 定义一个聚合根 包含组合主键
    /// </summary>
    public interface IAggregateRoot:IEntity
    {

    }

    /// <summary>
    /// 定义一个聚合根 包含单个主键 主键为Id
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IAggregateRoot<TKey> : IEntity<TKey> where TKey:IEquatable<TKey>
    {

    }
}
