using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Framework.RabbitMQ.Client.Tests
{
    public class CommonTest: RabbitClientTest
    {
        [Fact]
        public void Test1()
        {
            foreach (var item in TestDto.Data)
            {
                _rabbmitClient.Publish(item,()=> {
                    return "testqueuqename";
                });
            }
            
        }
    }

    public class TestDto
    { 
        public string Name { get; set; }

        public int Age { get; set; }

        public static List<TestDto> Data
        {
            get
            {
                var result = new List<TestDto>();
                for (var i = 0; i < 1000; i++)
                {
                    result.Add(new TestDto() { Name = "ĞÕÃû" + new Random().Next(0, 1000), Age = new Random().Next(0, 1000) });
                }
                return result;
            }
        }
    }


}
