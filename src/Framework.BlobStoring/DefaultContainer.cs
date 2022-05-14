namespace Framework.BlobStoring
{
    /// <summary>
    /// 默认容器 当然可以设置一些默认参数,作用于所有容器
    /// </summary>
    [BlobContainer(Name)]
    public class DefaultContainer
    {
        public const string Name = "default";
    }
}
