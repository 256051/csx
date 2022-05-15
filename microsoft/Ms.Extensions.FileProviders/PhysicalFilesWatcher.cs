using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.FileSystemGlobbing;
using Ms.Extensions.FileProviders.Abstractions;

namespace Ms.Extensions.FileProviders
{
    public class PhysicalFilesWatcher : IDisposable
    {
        /// <summary>
        /// 根路径
        /// </summary>
        private readonly string _root;

        private readonly FileSystemWatcher _fileWatcher;

        private readonly ExclusionFilters _filters;

        private Func<Timer> _timerFactory;

        public PhysicalFilesWatcher(
           string root,
           FileSystemWatcher fileSystemWatcher,
           bool pollForChanges)
           : this(root, fileSystemWatcher, ExclusionFilters.Sensitive)
        {

        }

        public PhysicalFilesWatcher(
        string root,
        FileSystemWatcher fileSystemWatcher,
        ExclusionFilters filters)
        {
            if (fileSystemWatcher == null)
            {
                throw new ArgumentNullException(nameof(fileSystemWatcher), "");
            }

            //设置根路径
            _root = root;

            if (fileSystemWatcher != null)
            {
                _fileWatcher = fileSystemWatcher;
                _fileWatcher.IncludeSubdirectories = true;//扫描子目录
                _fileWatcher.Created += OnChanged;
                _fileWatcher.Changed += OnChanged;
                _fileWatcher.Renamed += OnRenamed;
                _fileWatcher.Deleted += OnChanged;
                _fileWatcher.Error += OnError;
            }

            _filters = filters;
            //_timerFactory = () => NonCapturingTimer.Create(RaiseChangeEvents, state: PollingChangeTokens, dueTime: TimeSpan.Zero, period: DefaultPollingInterval);
        }

        /// <summary>
        /// 监控的路径发生改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            OnFileSystemEntryChange(e.FullPath);
        }

        /// <summary>
        /// 监控的路径发生异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnError(object sender, ErrorEventArgs e)
        {
            foreach (string path in _filePathTokenLookup.Keys)
            {
                ReportChangeForMatchedEntries(path);
            }
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            OnFileSystemEntryChange(e.OldFullPath);
            OnFileSystemEntryChange(e.FullPath);

            if (Directory.Exists(e.FullPath))
            {
                try
                {
                    // If the renamed entity is a directory then notify tokens for every sub item.
                    foreach (
                        string newLocation in
                        Directory.EnumerateFileSystemEntries(e.FullPath, "*", SearchOption.AllDirectories))
                    {
                        // Calculated previous path of this moved item.
                        string oldLocation = Path.Combine(e.OldFullPath, newLocation.Substring(e.FullPath.Length + 1));
                        OnFileSystemEntryChange(oldLocation);
                        OnFileSystemEntryChange(newLocation);
                    }
                }
                catch (Exception ex) when (
                    ex is IOException ||
                    ex is SecurityException ||
                    ex is DirectoryNotFoundException ||
                    ex is UnauthorizedAccessException)
                {
                    // Swallow the exception.
                }
            }
        }

        private void OnFileSystemEntryChange(string fullPath)
        {
            try
            {
                var fileSystemInfo = new FileInfo(fullPath);
                //过滤文件
                if (FileSystemInfoHelper.IsExcluded(fileSystemInfo, _filters))
                {
                    return;
                }

                string relativePath = fullPath.Substring(_root.Length);
                ReportChangeForMatchedEntries(relativePath);
            }
            catch (Exception ex) when (
                ex is IOException ||
                ex is SecurityException ||
                ex is UnauthorizedAccessException)
            {
                //忽略异常
            }
        }

        private readonly ConcurrentDictionary<string, ChangeTokenInfo> _filePathTokenLookup = new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, ChangeTokenInfo> _wildcardTokenLookup = new ConcurrentDictionary<string, ChangeTokenInfo>(StringComparer.OrdinalIgnoreCase);
        private void ReportChangeForMatchedEntries(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                // System.IO.FileSystemWatcher may trigger events that are missing the file name,
                // which makes it appear as if the root directory is renamed or deleted. Moving the root directory
                // of the file watcher is not supported, so this type of event is ignored.
                return;
            }

            path = NormalizePath(path);
            bool matched = false;
            if (_filePathTokenLookup.TryRemove(path, out ChangeTokenInfo matchInfo))
            {
                CancelToken(matchInfo);
                matched = true;
            }

            foreach (KeyValuePair<string, ChangeTokenInfo> wildCardEntry in _wildcardTokenLookup)
            {
                PatternMatchingResult matchResult = wildCardEntry.Value.Matcher.Match(path);
                if (matchResult.HasMatches &&
                    _wildcardTokenLookup.TryRemove(wildCardEntry.Key, out matchInfo))
                {
                    CancelToken(matchInfo);
                    matched = true;
                }
            }

            if (matched)
            {
                //关闭监视
                TryDisableFileSystemWatcher();
            }
        }

        private static string NormalizePath(string filter) => filter = filter.Replace('\\', '/');

        private static readonly Action<object> _cancelTokenSource = state => ((CancellationTokenSource)state).Cancel();

        private static void CancelToken(ChangeTokenInfo matchInfo)
        {
            if (matchInfo.TokenSource.IsCancellationRequested)
            {
                return;
            }

            matchInfo.TokenSource.Cancel();

            //Task.Factory.StartNew(
            //    _cancelTokenSource,
            //    matchInfo.TokenSource,
            //    CancellationToken.None,
            //    TaskCreationOptions.DenyChildAttach,
            //    TaskScheduler.Default);
        }

        private readonly object _fileWatcherLock = new object();
        private void TryDisableFileSystemWatcher()
        {
            if (_fileWatcher != null)
            {
                lock (_fileWatcherLock)
                {
                    if (_filePathTokenLookup.IsEmpty &&
                        _wildcardTokenLookup.IsEmpty &&
                        _fileWatcher.EnableRaisingEvents)
                    {
                        // 没有要监视的文件，请关闭文件监视。
                        _fileWatcher.EnableRaisingEvents = false;
                    }
                }
            }
        }

        public IChangeToken CreateFileChangeToken(string filter)        {            if (filter == null)            {                throw new ArgumentNullException(nameof(filter));            }            filter = NormalizePath(filter);

            if (Path.IsPathRooted(filter) || PathUtils.PathNavigatesAboveRoot(filter))            {                return NullChangeToken.Singleton;            }            IChangeToken changeToken = GetOrAddChangeToken(filter);


            // We made sure that browser/iOS/tvOS never uses FileSystemWatcher.
#pragma warning disable CA1416 // Validate platform compatibility            TryEnableFileSystemWatcher();


#pragma warning restore CA1416 // Validate platform compatibility
            return changeToken;        }


        private IChangeToken GetOrAddChangeToken(string pattern)
        {
            IChangeToken changeToken;
            bool isWildCard = pattern.IndexOf('*') != -1;
            if (isWildCard || IsDirectoryPath(pattern))
            {
                changeToken = GetOrAddWildcardChangeToken(pattern);
            }
            else
            {
                changeToken = GetOrAddFilePathChangeToken(pattern);
            }

            return changeToken;
        }

        private void TryEnableFileSystemWatcher()
        {
            if (_fileWatcher != null)
            {
                lock (_fileWatcherLock)
                {
                    if ((!_filePathTokenLookup.IsEmpty || !_wildcardTokenLookup.IsEmpty) &&
                        !_fileWatcher.EnableRaisingEvents)
                    {
                        // Perf: Turn off the file monitoring if no files to monitor.
                        _fileWatcher.EnableRaisingEvents = true;
                    }
                }
            }
        }

        internal IChangeToken GetOrAddWildcardChangeToken(string pattern)
        {
            if (!_wildcardTokenLookup.TryGetValue(pattern, out ChangeTokenInfo tokenInfo))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                var matcher = new Matcher(StringComparison.OrdinalIgnoreCase);
                matcher.AddInclude(pattern);
                tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken, matcher);
                tokenInfo = _wildcardTokenLookup.GetOrAdd(pattern, tokenInfo);
            }

            IChangeToken changeToken = tokenInfo.ChangeToken;
            return changeToken;
        }

        internal IChangeToken GetOrAddFilePathChangeToken(string filePath)
        {
            if (!_filePathTokenLookup.TryGetValue(filePath, out ChangeTokenInfo tokenInfo))
            {
                var cancellationTokenSource = new CancellationTokenSource();
                var cancellationChangeToken = new CancellationChangeToken(cancellationTokenSource.Token);
                tokenInfo = new ChangeTokenInfo(cancellationTokenSource, cancellationChangeToken);
                tokenInfo = _filePathTokenLookup.GetOrAdd(filePath, tokenInfo);
            }

            IChangeToken changeToken = tokenInfo.ChangeToken;
            return changeToken;
        }

        /// <summary>
        /// 判断路径是否是一个目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsDirectoryPath(string path)
        {
            return path.Length > 0 &&
                (path[path.Length - 1] == Path.DirectorySeparatorChar ||
                path[path.Length - 1] == Path.AltDirectorySeparatorChar);
        }

        public void Dispose()
        {
            
        }

        private readonly struct ChangeTokenInfo
        {
            public ChangeTokenInfo(
                CancellationTokenSource tokenSource,
                CancellationChangeToken changeToken)
                : this(tokenSource, changeToken, matcher: null)
            {
            }

            public ChangeTokenInfo(
                CancellationTokenSource tokenSource,
                CancellationChangeToken changeToken,
                Matcher matcher)
            {
                TokenSource = tokenSource;
                ChangeToken = changeToken;
                Matcher = matcher;
            }

            public CancellationTokenSource TokenSource { get; }

            public CancellationChangeToken ChangeToken { get; }

            public Matcher Matcher { get; }
        }
    }
}
