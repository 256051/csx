using Framework.Core;
using Framework.Test;
using Shouldly;
using System;
using System.ComponentModel;
using Xunit;

namespace Framework.AutoMapper.Tests
{
    public class EnumTest:TestBase
    {
        [Fact]
        public void Test1()
        {
            var man = 0;
            var woman = 1;
            man.GetDescription<Sex>().ShouldBe("男");
            man.GetDescription<Sex>().ShouldNotBe("女");
            woman.GetDescription<Sex>().ShouldBe("女");
            woman.GetDescription<Sex>().ShouldNotBe("男");
        }

        [Fact]
        public void Test2()
        {
            var man = "男";
            var woman = "女";
            man.GetEnumValue<Sex>().ShouldBe(0);
            man.GetEnumValue<Sex>().ShouldNotBe(1);
            woman.GetEnumValue<Sex>().ShouldBe(1);
            woman.GetEnumValue<Sex>().ShouldNotBe(0);
        }

        protected override void LoadModules()
        {
            
        }
    }

    public enum Sex
    { 
        [Description("男")]
        Man=0,
        [Description("女")]
        Woman = 1
    }
}
