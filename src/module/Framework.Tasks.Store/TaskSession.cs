using System;

namespace Framework.Tasks.Store
{
    public class TaskSession : TaskSession<string>
    { 
        
    }

    public class TaskSession<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 会话id
        /// </summary>
        public virtual TKey Id { get; set; }

        public override string ToString()
        {
            return $"task session id:{Id}";
        }
    }
}
