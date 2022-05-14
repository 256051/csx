using System;
using System.Collections.Generic;

namespace Framework.DDD.Domain.Entities
{
    public abstract class Entity : IEntity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Keys = {GetKeys().JoinAsString(", ")}";
        }

        public Guid[] GetKeys()
        {
            return new Guid[] { Id };
        }
    }

    public abstract class Entity<TKey> :IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public virtual TKey Id { get; set; }

        protected Entity()
        {

        }

        protected Entity(TKey id)
        {
            Id = id;
        }

        public TKey[] GetKeys()
        {
            return new TKey[] { Id };
        }

        public override string ToString()
        {
            return $"[ENTITY: {GetType().Name}] Id = {Id}";
        }
    }
}
