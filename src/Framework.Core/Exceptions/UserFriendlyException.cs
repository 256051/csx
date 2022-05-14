using System.Runtime.Serialization;

namespace System
{
    [Serializable]
    public class UserFriendlyException : BusinessException, IUserFriendlyException
    {
        public UserFriendlyException(
            string message,
            string code = null,
            string details = null,
            Exception innerException = null)
            : base(
                  code,
                  message,
                  details,
                  innerException)
        {
            Details = details;
        }

        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
        {

        }
    }
}
