using System;

namespace Framework.Auditing
{
    public interface ICreationAuditedObject 
    {
        Guid? CreatorId { get; set; }

        DateTime? CreationTime { get; set; }
    }

    public interface ICreationAuditedObject<TKey> : IHasCreationTime, IMayHaveCreator<TKey>
        where TKey:IEquatable<TKey>
    {

    }
}
