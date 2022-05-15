using Microsoft.Extensions.Primitives;
using System.IO;
using System.Linq;

namespace Ms.Extensions.FileProviders
{
    internal static class PathUtils
    {
        /// <summary>
        /// 确保路径最后带上\或者/根据操作系统的不同
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static string EnsureTrailingSlash(string path)
        {
            if (!string.IsNullOrEmpty(path) && path[path.Length - 1] != Path.DirectorySeparatorChar)
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        private static readonly char[] _invalidFileNameChars = Path.GetInvalidFileNameChars()
            .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();
        private static readonly char[] _invalidFilterChars = _invalidFileNameChars
           .Where(c => c != '*' && c != '|' && c != '?').ToArray();
        /// <summary>
        /// 判断文件名的非法字符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool HasInvalidFilterChars(string path)
        {
            return path.IndexOfAny(_invalidFilterChars) != -1;
        }

        private static readonly char[] _pathSeparators = new[]
           {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};
        internal static bool PathNavigatesAboveRoot(string path)
        {
            var tokenizer = new StringTokenizer(path, _pathSeparators);
            int depth = 0;

            foreach (StringSegment segment in tokenizer)
            {
                if (segment.Equals(".") || segment.Equals(""))
                {
                    continue;
                }
                else if (segment.Equals(".."))
                {
                    depth--;

                    if (depth == -1)
                    {
                        return true;
                    }
                }
                else
                {
                    depth++;
                }
            }

            return false;
        }

        /// <summary>
        /// 是否存在无效的路径字符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static bool HasInvalidPathChars(string path)
        {
            return path.IndexOfAny(_invalidFileNameChars) != -1;
        }
    }
}
