using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Json.Tests
{
    public class Person
    {
        public string Name { get; set; } = "张三";

        public int Age { get; set; } = 20;

        public DateTime CreateTime { get; set; } = new DateTime(1970,1,1,12,0,0);

        public DateTime? ModifyTime { get; set; }

        public bool IsMan { get; set; } = true;

        public PositionType Position { get; set; } = PositionType.Leader;

        public float Money { get; set; } = 1111111111.1111111111f;

        public decimal High { get; set; } = 22222222222.222222222222m;

        public double Weight { get; set; } = 333333333333.333333333333;
    }

    public enum PositionType
    { 
        Leader=0,

        Worker=1
    }
}
