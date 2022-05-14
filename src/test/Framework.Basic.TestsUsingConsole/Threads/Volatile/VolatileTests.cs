//using System;
using System;
using System.Threading;

namespace Framework.Basic.TestsUsingConsole
{
    public class VolatileTests
    {
        private int x = 0;
        private int flag = 0;

        ///// <summary>
        /// 线程一任务
        /// </summary>
        public void Task1()
        {
            x = 100;
            flag = 100000;
        }

        /// <summary>
        /// 线程二任务
        /// </summary>
        public void Task2()
        {
            Console.WriteLine(x);
            Console.WriteLine(flag);

        }
    }
}
