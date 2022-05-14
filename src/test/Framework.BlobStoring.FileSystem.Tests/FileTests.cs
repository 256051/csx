using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Framework.BlobStoring.FileSystem.Tests
{
    public class FileTests: BlobStoringFileSystemTest
    {
        private IBlobContainer<FileSystemBlobContainer> _fileContainer;
        private IBlobContainer<TemplateFilesBlobContainer> _templatefileContainer;
        public FileTests()
        {
            _fileContainer=ServiceProvider.GetRequiredService<IBlobContainer<FileSystemBlobContainer>>();
            _templatefileContainer= ServiceProvider.GetRequiredService<IBlobContainer<TemplateFilesBlobContainer>>();
        }

        [Fact]
        public async Task Test1()
        {
            var content = "111";
            var path = Services.GetConfiguration().GetSection(FileSystemBlobProviderConfigurationNames.BasePath).Value;
            var fileName = Guid.NewGuid().ToString() + ".txt";
            var fullPath = $"{path}{fileName}";
            using (var ts = File.Create(fullPath))
            {
                await ts.WriteAsync(Encoding.UTF8.GetBytes(content));
                await ts.FlushAsync();
            }
            using (var fs = File.OpenRead(fullPath))
            {
                await _fileContainer.SaveAsync(fileName, fs);
            }

            var stream= await _fileContainer.GetAsync(fileName);
            stream.Length.ShouldBeGreaterThanOrEqualTo(content.Length);

            if (await _fileContainer.ExistsAsync(fileName))
            {
                await _fileContainer.DeleteAsync(fileName);
            }
            (await _fileContainer.ExistsAsync(fileName)).ShouldBe(false);
        }

        /// <summary>
        /// 模板文件容器
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TemplateFileTest1()
        {
            var stream = await _templatefileContainer.GetAsync("测试模板文件.txt");
            stream.Length.ShouldBe(3);
        }
    }
}
