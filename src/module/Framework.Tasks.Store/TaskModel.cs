using Framework.Tasks.Domain;
using System;

namespace Framework.Tasks.Store
{
    public class TaskModel : TaskModel<string>
    { 
        
    }

    public class TaskModel<TKey> where TKey:IEquatable<TKey>
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public TKey Id { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public TaskState TaskState { get; set; }

        /// <summary>
        /// 任务执行失败事件
        /// </summary>
        public event Action<string> FailedEvent;

        /// <summary>
        /// 执行失败
        /// </summary>
        /// <param name="failedMessage"></param>
        public void Failed(string failedMessage)
        {
            FailedEvent?.Invoke(failedMessage);
        }

        public event Action<string> SuceedEvent;

        public void Succeed(string message)
        {
            SuceedEvent?.Invoke(message);
        }

        public override string ToString()
        {
            return $"id:{Id},name:{Name} ";
        }
    }
}
