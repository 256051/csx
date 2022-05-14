using System;

namespace Framework.DDD.Domain.Entities
{
    /// <summary>
    /// 定义实体 
    /// </summary>
    public interface IEntity
    {
        Guid Id { get; set; }

        Guid[] GetKeys();
    }

    /// <summary>
    /// 定义一个带单个主键的实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>  where TKey:IEquatable<TKey>
    {
        TKey Id { get; set; }

        TKey[] GetKeys();
    }
}
