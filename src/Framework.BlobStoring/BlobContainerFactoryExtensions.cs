namespace Framework.BlobStoring
{
    public static class BlobContainerFactoryExtensions
    {
        public static IBlobContainer Create<TContainer>(
            this IBlobContainerFactory blobContainerFactory
        )
        {
            return blobContainerFactory.Create(BlobContainerAttribute.GetContainerName<TContainer>());
        }
    }
}
