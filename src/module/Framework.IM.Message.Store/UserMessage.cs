using System;

namespace Framework.IM.Message.Store
{
    public class UserMessage : UserMessage<string>
    {

    }

    public class UserMessage<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// 消息发送者
        /// </summary>
        public virtual TKey SenderId { get; set; }

        /// <summary>
        /// 消息接收者
        /// </summary>
        public virtual TKey ReceiverId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息是否确认
        /// </summary>
        public bool? Confirmed { get; set; }

        /// <summary>
        /// 消息确认时间
        /// </summary>
        public DateTime? ConfirmedTime { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
