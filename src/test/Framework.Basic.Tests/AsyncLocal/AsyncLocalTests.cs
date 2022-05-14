using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Basic.Tests
{
    public class AsyncLocalTests
    {
        private static AsyncLocal<string> _str = new AsyncLocal<string>();

        /// <summary>
        /// AsyncLocal 修改会变更执行上下文ExecuteContext,而async await状态机检测到上下文变更,会还原之前的捕获的上下文,所以await之后通过AsyncLocal修改的数据会丢失.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task TestBug()
        {
            _str.Value = "张三";
            Trace.WriteLine($"{ _str.Value}");
            await UpdateAsync();
            Trace.WriteLine($"{ _str.Value}");
        }

        private async Task UpdateAsync()
        {
            Trace.WriteLine($"{ _str.Value}");
            await Task.CompletedTask;
            _str.Value = "李四";
            Trace.WriteLine($"{ _str.Value}");
        }
    }

    public class Test
    { 
        public string Name { get; set; }
    }
}
