using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.IO;
using Xunit;

namespace Framework.Excel.Npoi.Tests
{
    public class ReadTest: NpoiTest
    {
        private IExcelReader _excelReader;

        public ReadTest()
        {
            _excelReader = ServiceProvider.GetRequiredService<IExcelReader>();
        }

        [Fact]
        public void Test1()
        {
            using (var stream =File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}\test.xlsx"))
            {
                var list = _excelReader.Read<Person>(stream);
                list.Count.ShouldBe(3);
            } 
        }

        [Fact]
        public void Test2()
        {
            using (var stream = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}\test.xlsx"))
            {
                var list = _excelReader.Read<Person>(stream);
                list.Count.ShouldBe(3);
            }
        }

        [Fact]
        public void Test3()
        {
            using (var stream = File.OpenRead($@"{AppDomain.CurrentDomain.BaseDirectory}\人员导入模板 (3)(1).xlsx"))
            {
                var list = _excelReader.Read<UserCreateDto>(stream);
                list.Count.ShouldBe(3);
            }
        }
        
    }
}
