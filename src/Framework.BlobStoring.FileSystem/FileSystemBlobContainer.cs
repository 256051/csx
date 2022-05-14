namespace Framework.BlobStoring.FileSystem
{
    /// <summary>
    /// 文件系统容器
    /// </summary>
    [BlobContainer(name:"本地文件容器")]
    public class FileSystemBlobContainer
    {
        public const string Name = "FileSystemBlobContainer";
    }

    /// <summary>
    /// 模板文件容器
    /// </summary>
    [BlobContainer(name: "模板文件容器")]
    public class TemplateFilesBlobContainer
    {
        public const string Name = "TemplateFilesBlobContainer";
    }
}
