using System;

namespace Framework.Auditing
{

    public interface IMayHaveCreator
    {
        Guid? CreatorId { get; }
    }

    /// <summary>
    /// 定义需要创建人Id的实体 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IMayHaveCreator<TKey>
        where TKey:IEquatable<TKey>
    {
        TKey CreatorId { get; set; }
    }
}
