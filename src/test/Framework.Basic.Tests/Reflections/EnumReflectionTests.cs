using Framework.Test;
using Shouldly;
using System.Linq;
using Xunit;

namespace Framework.Basic.Tests
{
    public class EnumReflectionTests:TestBase
    {
       
        [Fact]
        public void Test1()
        {
            var type = typeof(TestEnum);
            var fields=type.GetFields().Where(w => w.Name != "value__").ToList();
            for (var i = 0; i < fields.Count; i++)
            {
                var field = fields[i];
                var enumValue = (int)field.GetValue(null);
                if (i == 0)
                    enumValue.ShouldBe(0);
                if (i == 1)
                    enumValue.ShouldBe(1);

            }
        }

        protected override void LoadModules()
        {
            
        }
    }

    public enum TestEnum
    { 
        Leader=0,
        Worker=1
    }
}
