using Framework.Core;
using System;
using System.Runtime.Serialization;

namespace Framework.BlobStoring
{
    /// <summary>
    /// Blob已存在异常
    /// </summary>
    public class BlobAlreadyExistsException : FrameworkException
    {
        public BlobAlreadyExistsException()
        {

        }

        public BlobAlreadyExistsException(string message) : base(message)
        {

        }

        public BlobAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public BlobAlreadyExistsException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {

        }
    }
}
