using Microsoft.Extensions.Logging;

namespace Framework.Core.Logging
{
    /// <summary>
    /// 解决异常中包含一个异常属性()
    /// </summary>
    public interface IExceptionWithSelfLogging
    {
        void Log(ILogger logger);
    }
}
