using Framework.Basic.TestsUsingConsole.Threads.Interlockeds;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Basic.TestsUsingConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            VolatileTest();
            Console.ReadKey();
        }

        /// <summary>
        /// Volatile及volatile关键字相关测试
        /// </summary>
       public static bool _switch = false;
        public static void VolatileTest()
        {
            while (!_switch)
            {
                var t = new VolatileTests();
                Task.Run(() =>
                {
                    t.Task1();
                });
                Task.Run(() =>
                {
                    t.Task2();
                });
            }
        }



        public static void InterlockedTest()
        {
            new MultiWebRequests();
        }
    }
}
