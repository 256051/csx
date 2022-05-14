using Framework.Core;
using System;
using System.Runtime.Serialization;

namespace Framework.BackgroundJobs.Abstractions
{
    [Serializable]
    public class BackgroundJobExecutionException : FrameworkException
    {
        public string JobType { get; set; }

        public object JobArgs { get; set; }

        public BackgroundJobExecutionException()
        {

        }

        public BackgroundJobExecutionException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        public BackgroundJobExecutionException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
