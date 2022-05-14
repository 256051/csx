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
            man.GetDescription<Sex>().ShouldBe("��");
            man.GetDescription<Sex>().ShouldNotBe("Ů");
            woman.GetDescription<Sex>().ShouldBe("Ů");
            woman.GetDescription<Sex>().ShouldNotBe("��");
        }

        [Fact]
        public void Test2()
        {
            var man = "��";
            var woman = "Ů";
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
        [Description("��")]
        Man=0,
        [Description("Ů")]
        Woman = 1
    }
}
