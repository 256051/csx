using Framework.Core.Data;
using Framework.Core.Dependency;
using Framework.DDD.Domain.Entities;

namespace Framework.AspNetCore.Test
{
    public interface IUserRepository 
    {

    }

    public class Test : FullAuditedAggregateRoot
    {
        public string Name { get; set; }
    }

    public class UserRepository : IUserRepository,ITransient
    {
      

    }
}
