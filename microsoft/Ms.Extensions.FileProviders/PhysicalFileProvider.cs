using Microsoft.Extensions.Primitives;
using Ms.Extensions.FileProviders.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Ms.Extensions.FileProviders
{
    public class PhysicalFileProvider : IFileProvider, IDisposable
    {
        /// <summary>
        /// 文件根目录
        /// </summary>
        public string Root { get; }

        /// <summary>
        /// 文件过滤器
        /// </summary>
        private readonly ExclusionFilters _filters;

        /// <summary>
        /// 文件监听
        /// </summary>
        private readonly Func<PhysicalFilesWatcher> _fileWatcherFactory;

        private static readonly char[] _pathSeparators = new[]
           {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};


        public PhysicalFileProvider(string root): this(root, ExclusionFilters.Sensitive)
        {

        }

        public PhysicalFileProvider(string root, ExclusionFilters filters)
       {
            //路径必须是绝对路径
            if (!Path.IsPathRooted(root))
            {
                throw new ArgumentException("The path must be absolute.", nameof(root));
            }

            string fullRoot = Path.GetFullPath(root);
            Root = PathUtils.EnsureTrailingSlash(fullRoot);
            if (!Directory.Exists(Root))
            {
                throw new DirectoryNotFoundException(Root);
            }
            _filters = filters;
            _fileWatcherFactory = () => CreateFileWatcher();
        }

        private PhysicalFilesWatcher _fileWatcher;
        private bool _fileWatcherInitialized;
        private object _fileWatcherLock = new object();
        internal PhysicalFilesWatcher FileWatcher
        {
            get
            {
                return LazyInitializer.EnsureInitialized(
                    ref _fileWatcher,
                    ref _fileWatcherInitialized,
                    ref _fileWatcherLock,
                    _fileWatcherFactory);
            }
            set
            {
                Debug.Assert(!_fileWatcherInitialized);
                _fileWatcherInitialized = true;
                _fileWatcher = value;
            }
        }

        internal PhysicalFilesWatcher CreateFileWatcher()
        {
            string root = PathUtils.EnsureTrailingSlash(Path.GetFullPath(Root));
            FileSystemWatcher watcher =  new FileSystemWatcher(root);
            return new PhysicalFilesWatcher(root, watcher, _filters);
        }

        public IChangeToken Watch(string filter)
        {
            if (filter == null || PathUtils.HasInvalidFilterChars(filter))
            {
                return NullChangeToken.Singleton;
            }

            filter = filter.TrimStart(_pathSeparators);
            return FileWatcher.CreateFileChangeToken(filter);
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            if (string.IsNullOrEmpty(subpath) || PathUtils.HasInvalidPathChars(subpath))
            {
                throw new Exception($"{subpath} file not found");
            }

            subpath = subpath.TrimStart(_pathSeparators);

            if (Path.IsPathRooted(subpath))
            {
                return new NotFoundFileInfo(subpath);
            }

            string fullPath = GetFullPath(subpath);
            if (fullPath == null)
            {
                return new NotFoundFileInfo(subpath);
            }

            var fileInfo = new FileInfo(fullPath);
            if (FileSystemInfoHelper.IsExcluded(fileInfo, _filters))
            {
                return new NotFoundFileInfo(subpath);
            }

            return new PhysicalFileInfo(fileInfo);
        }

        private string GetFullPath(string path)
        {
            if (PathUtils.PathNavigatesAboveRoot(path))
            {
                return null;
            }

            string fullPath;
            try
            {
                fullPath = Path.GetFullPath(Path.Combine(Root, path));
            }
            catch
            {
                return null;
            }

            if (!IsUnderneathRoot(fullPath))
            {
                return null;
            }

            return fullPath;
        }

        private bool IsUnderneathRoot(string fullPath)
        {
            return fullPath.StartsWith(Root, StringComparison.OrdinalIgnoreCase);
        }

        public void Dispose()
        {
            
        }
    }
}
