using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Ms.Extensions.FileProviders.Abstractions
{
    public class NotFoundFileInfo : IFileInfo
    {
        public NotFoundFileInfo(string name)
        {
            Name = name;
        }

        public bool Exists => false;

        public bool IsDirectory => false;

        public DateTimeOffset LastModified => DateTimeOffset.MinValue;

        public long Length => -1;

        public string Name { get; }

        public string PhysicalPath => null;

      
        public Stream CreateReadStream()
        {
            throw new FileNotFoundException($"{Name} not found");
        }
    }
}
