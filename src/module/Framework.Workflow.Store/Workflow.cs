using System;

namespace Framework.Workflow.Store
{
    public class Workflow : Workflow<string>
    { 
        
    }

    public class Workflow<TKey>  where TKey:IEquatable<TKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        public override string ToString()
        {
            return $"workflow:{Name}";
        }
    }
}
