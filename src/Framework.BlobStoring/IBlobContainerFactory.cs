namespace Framework.BlobStoring
{
    public interface IBlobContainerFactory
    {
        /// <summary>
        /// 创建Container
        /// </summary>
        /// <typeparam name="TContainer"></typeparam>
        /// <returns></returns>
        IBlobContainer Create(string name);
    }
}
