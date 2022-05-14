using System;

namespace Framework.BackgroundJobs
{
    /// <summary>
    /// 后台工作项序列化
    /// </summary>
    public interface IBackgroundJobSerializer
    {
        string Serialize(object obj);

        object Deserialize(string value, Type type);
    }
}
