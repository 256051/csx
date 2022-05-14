using System;

namespace Framework.Auditing
{
    public interface IFullAuditedObject : IAuditedObject, IDeletionAuditedObject
    {

    }
    public interface IFullAuditedObject<TKey> : IAuditedObject<TKey>, IDeletionAuditedObject<TKey>
        where TKey:IEquatable<TKey>
    {

    }
}
