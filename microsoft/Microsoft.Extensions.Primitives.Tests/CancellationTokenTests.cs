using System;
using System.Diagnostics;
using System.Threading;
using Xunit;

namespace Microsoft.Extensions.Primitives.Tests
{
    public class CancellationTokenTests
    {
        [Fact]
        public void Test1()
        {
            var tokenSource = new CancellationTokenSource();
            var callbackObj = "666";

            //ע��һ��ί�е�CancellationToken����Cancel����ʱ,�ص��ᱻִ��
            //����������Ѵ���ȡ��״̬�����������ͬ�����С�ί�����ɵ��κ��쳣����Ӹ÷��������д�����ȥ��ExecutionContext���ᱻ����������ص��ĵ��á�
            tokenSource.Token.UnsafeRegister(obj =>
            {
                Trace.WriteLine(obj.ToString());
            }, callbackObj);

            tokenSource.Cancel();
        }
    }
}
