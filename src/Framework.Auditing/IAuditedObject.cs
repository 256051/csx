using System;

namespace Framework.Auditing
{
    public interface IAuditedObject : ICreationAuditedObject, IModificationAuditedObject
    {

    }

    public interface IAuditedObject<TKey> : ICreationAuditedObject<TKey>, IModificationAuditedObject<TKey>
       where TKey : IEquatable<TKey>
    {

    }
}
