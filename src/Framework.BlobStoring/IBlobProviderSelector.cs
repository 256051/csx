using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BlobStoring
{
    public interface IBlobProviderSelector
    {
        IBlobProvider Get(string containerName);
    }
}
