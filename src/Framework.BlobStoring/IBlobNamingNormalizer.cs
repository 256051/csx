namespace Framework.BlobStoring
{
    /// <summary>
    /// 容器和块名称标准化 按照云服务商的要求
    /// </summary>
    public interface IBlobNamingNormalizer
    {
        string NormalizeContainerName(string containerName);

        string NormalizeBlobName(string blobName);
    }
}
