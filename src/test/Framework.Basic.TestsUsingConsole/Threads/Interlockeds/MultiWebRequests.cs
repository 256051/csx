using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Basic.TestsUsingConsole.Threads.Interlockeds
{
    public class MultiWebRequests
    {
        private AsyncCoordinator _asyncCoordinator = new AsyncCoordinator();
        private Dictionary<string, object> _servers = new Dictionary<string, object>() {
            {"https://www.baidu.com/",null },
            {"https://cn.bing.com/",null },
            {"https://github.com/",null }
        };

        public MultiWebRequests(int timeOut = Timeout.Infinite)
        {
            using (var httpClient = new HttpClient())
            {
                foreach (var server in _servers.Keys)
                {
                    _asyncCoordinator.Begin();
                    httpClient.GetByteArrayAsync(server)
                        .ContinueWith(task => CompleteResult(server, task));
                }
                _asyncCoordinator.AllBegin(AllDone,10000);
            }  
        }

        private void CompleteResult(string server,Task<byte[]> task)
        {
            object result;
            if (task.Exception != null)
            {
                result = task.Exception.InnerException;
            }
            else {
                result = "访问成功";
            }
            _servers[server] = result;
            _asyncCoordinator.Stop();
        }

        public void AllDone(CoordinatorStatus status)
        {
            switch (status)
            {
                case CoordinatorStatus.Cancel:Console.WriteLine("操作取消");break;
                case CoordinatorStatus.Timeout:Console.WriteLine("操作超时");break;
                case CoordinatorStatus.AllDone:
                    Console.WriteLine("操作全部完成,结果如下:");
                    foreach (var server in _servers)
                    {
                        Console.WriteLine($"{server.Key}");
                        var result = server.Value;
                        if (result is Exception)
                        {
                            Console.WriteLine($"访问失败,原因:{((Exception)result).Message}");
                        }
                        else {
                            Console.WriteLine($"访问成功,返回值:{result}");
                        }
                    }
                    break;
            }
        }
    }

    public enum CoordinatorStatus
    { 
        Cancel,Timeout,AllDone
    }

    public class AsyncCoordinator
    {
        private int _operateCount = 0;
        private int _reportedStatus = 0;
        private Action<CoordinatorStatus> _callBack;
        private Timer _timer;

        public void Begin(int num=1)
        {
            Interlocked.Add(ref _operateCount, num);
        }

        public void Stop()
        {
            if (Interlocked.Decrement(ref _operateCount) == 0)
                ReportStatus(CoordinatorStatus.AllDone);
        }

        public void AllBegin(Action<CoordinatorStatus> callBack,int timeout=Timeout.Infinite)
        {
            _callBack = callBack;
            if(timeout!=Timeout.Infinite)
                _timer=new Timer(TimeExpired, null,timeout, Timeout.Infinite);
        }

        private void TimeExpired(object state) { ReportStatus(CoordinatorStatus.Timeout); }

        public void ReportStatus(CoordinatorStatus status)
        {
            //第一次进来会触发回调函数
            if (Interlocked.Exchange(ref _reportedStatus, 1) == 0)
                _callBack(status);
        }
    }
}
