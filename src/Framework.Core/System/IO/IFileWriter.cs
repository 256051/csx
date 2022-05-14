using System;
using System.IO;
using System.Threading.Tasks;

namespace Framework.Core
{
    public interface IFileWriter
    {
        Task WriteResourceAsync(Uri uri, Stream resourceStream);

        void WriteResource(Uri uri, Stream resourceStream);
    }
}
