using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Basic.Tests
{
    public class NullableTests
    {
        [Fact]
        public void Test1()
        {
            var param1 = "参数1";
            var param2 = 2;
            var instance= (EnumConverter<TestFlag>)Activator.CreateInstance(
                typeof(EnumConverter<>).MakeGenericType(typeof(TestFlag)),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                new object[] { param1, param2 }, culture: null)!;
            instance.Convert();
        }

        [Fact]
        public void Test2()
        {
            TestFlag a = new TestFlag() ;
            var result = (new Test2(a).Get());
        }
    }

    public class EnumConverter<T> where T : struct, Enum
    {
        private string _param1;
        private int _param2;
        public EnumConverter(string param1, int param2)
        {
            _param1 = param1;
            _param2 = param2;
        }

        public void Convert()
        {
            var param = _param1 + _param2;
        }
    }

    public enum TestFlag
    { 
        A=1,
        B=2
    }

    public class Test2
    {
        public Test2(TestFlag obj) { }

        public int? Get()
        {
            return null;
        }
    }
}
