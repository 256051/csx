using Framework.Core.Dependency;
using System.Runtime.InteropServices;

namespace System.Runtime
{
    public class OSPlatformProvider : IOSPlatformProvider, ITransient
    {
        public virtual OSPlatform GetCurrentOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return OSPlatform.OSX; //MAC
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return OSPlatform.Windows;
            }

            return OSPlatform.Linux;
        }
    }
}
