using System;
using System.Diagnostics;
using System.IO;

namespace Ms.Extensions.FileProviders
{
    internal static class FileSystemInfoHelper
    {
        public static bool IsExcluded(FileSystemInfo fileSystemInfo, ExclusionFilters filters)
        {
            if (filters == ExclusionFilters.None)
            {
                return false;
            }
            //����ļ���.��ͷֱ�ӹ���
            else if (fileSystemInfo.Name.StartsWith(".", StringComparison.Ordinal) && (filters & ExclusionFilters.DotPrefixed) != 0)
            {
                return true;
            }
            //�ļ�����������ļ���ΪHidden����filter��ΪNode������ϵͳ�ļ�,ֱ�ӹ���
            else if (fileSystemInfo.Exists &&
                (((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 && (filters & ExclusionFilters.Hidden) != 0) ||
                 ((fileSystemInfo.Attributes & FileAttributes.System) != 0 && (filters & ExclusionFilters.System) != 0)))
            {
                return true;
            }

            return false;
        }
    }
}
