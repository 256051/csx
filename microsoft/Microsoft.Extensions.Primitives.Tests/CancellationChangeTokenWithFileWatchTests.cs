using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Xunit;

namespace Microsoft.Extensions.Primitives.Tests
{
    public class CancellationChangeTokenWithFileWatchTests
    {
        [Fact]
        public void Test1()
        {
            var tokenSource = new CancellationTokenSource();

            //注册changeToken的回调为输出test.json被修改方法,所以当tokenSource调用cancel时,会执行回调方法
            //OnChange内部会叫回调为输出test.json被修改的方法挂载到changeToken上面,通过CancellationToken.UnSafeRegister的方法注入,参考CancellationTokenTests测试类
            //所以当当tokenSource调用cancel方法,从而触发回调为输出test.json被修改的方法
            ChangeToken.OnChange(()=> CreateToken(tokenSource), () =>
            {
                Trace.WriteLine("test.json被修改");
            });

            //监控根目录
            var file = $"{AppDomain.CurrentDomain.BaseDirectory}";
            var ex = File.Exists(file);
            var watcher = new FileSystemWatcher(file);
            watcher.Changed += (object sender, FileSystemEventArgs e) =>
            {
                tokenSource.Cancel();
                watcher.EnableRaisingEvents = false;
            };
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
            while (true)
            {
                Thread.Sleep(2000);
            }
        }

        private IChangeToken CreateToken(CancellationTokenSource tokenSource)
        {
            return new CancellationChangeToken(tokenSource.Token);
        }
    }
}
