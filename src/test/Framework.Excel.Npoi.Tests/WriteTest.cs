using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace Framework.Excel.Npoi.Tests
{
    public class WriteTest : NpoiTest
    {
        private IExcelWriter _excelWriter;

        public WriteTest()
        {
            _excelWriter = ServiceProvider.GetRequiredService<IExcelWriter>();
        }

        [Fact]
        public void Test1()
        {
            var list = new List<Person>()
            {
                new Person(){ Name=null,Age=22 },
                new Person(){ Name="李四",Age=23 },
                new Person(){ Name="王五",Age=24 },
                new Person(){ Name=null,Age=25 },
            };
            _excelWriter.Write(list,$"{AppDomain.CurrentDomain.BaseDirectory}\\excel\\output\\{Guid.NewGuid().ToString()}.xlsx");

            var wList = new List<WorkRecord>()
            {
                new WorkRecord(){ WorkTime=DateTime.Now,Content="aaaaaaaaaaaaaaaaaaaaaaaaaaaa" },
                new WorkRecord(){ WorkTime=DateTime.Now,Content=null },
                new WorkRecord(){ WorkTime=DateTime.Now,Content="aaaaaaaaaaaaaaaaaaaaaaaaaaaa" },
                new WorkRecord(){ WorkTime=DateTime.Now,Content="aaaaaaaaaaaaaaaaaaaaaaaaaaaa" },
            };
            _excelWriter.Write(wList, $"{AppDomain.CurrentDomain.BaseDirectory}\\excel\\output\\{Guid.NewGuid().ToString()}.xlsx");
        }

        [Fact]
        public void Test2()
        {
            var list = new List<Person>()
            {
                new Person(){ Name="张三",Age=22 },
                new Person(){ Name="李四",Age=23 },
                new Person(){ Name="王五",Age=24 },
                new Person(){ Name="赵柳",Age=25 },
            };

            var wList = new List<WorkRecord>()
            {
                new WorkRecord(){ WorkTime=DateTime.Now,Content="aaaaaaaaaaaaaaaaaaaaaaaaaaaa" },
                new WorkRecord(){ WorkTime=DateTime.Now,Content="bbbbbbbbbbbbbbbbbbbbbbbbb" },
                new WorkRecord(){ WorkTime=DateTime.Now,Content="ccccccccccccccccccccccccccccccc" },
                new WorkRecord(){ WorkTime=DateTime.Now,Content="dddddddddddddddddddddddd" },
            };
            _excelWriter
                .Write(list)
                .Write(wList)
                .SaveAs($"{AppDomain.CurrentDomain.BaseDirectory}\\excel\\output\\{Guid.NewGuid().ToString()}.xlsx");
        }
    }
}
