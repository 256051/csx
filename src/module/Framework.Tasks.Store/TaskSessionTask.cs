using System;

namespace Framework.Tasks.Store
{
    public class TaskSessionTask<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public TKey TaskId { get; set; }

        /// <summary>
        /// 任务会话Id
        /// </summary>
        public TKey TaskSessionId { get; set; }
    }
}
