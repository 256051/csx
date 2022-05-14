using Framework.DDD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Test
{
    public class Person:FullAuditedAggregateRoot
    {
        public string Name { get; set; }

        public int Age { get; set; }
    }
}
