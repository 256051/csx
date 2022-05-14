using Dapper;
using Dapper.Contrib.Extensions;
using Framework.Core.Configurations;
using Framework.Core.Data;
using Framework.Core.Dependency;
using Framework.Dapper;
using Framework.Data.MySql;
using Framework.Test;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.Uow.Tests
{
    public class UowOfWorkTestBase: TestBase
    {
        protected IUnitOfWorkManager UnitOfWorkManager 
        {
            get
            {
                return ApplicationConfiguration.Provider.GetRequiredService<IUnitOfWorkManager>();
            }
        }

        protected TestRepository TestRepository
        {
            get
            {
                return ApplicationConfiguration.Provider.GetRequiredService<TestRepository>();
            }
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration
                .UseUnitOfWork()
                .UseMySql()
                .UseDapper()
                .AddModule(Assembly.GetExecutingAssembly().FullName);
        }
    }

    public class TestRepository : ISingleton
    {
        private IDbProvider _dbProvider;

        public TestRepository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task Update(string a,bool isException=false)
        {
            var connection = await _dbProvider.GetConnectionAsync();
            var transaction = await _dbProvider.GetTransactionAsync();
            var id = "02c4e80e-1252-43df-af46-77bb641a795d";
            await connection.ExecuteAsync("update ac_user_copy set user_name=@a where id=@id ", new { a, id }, transaction: transaction);

            if (isException)
            {
                await connection.ExecuteAsync("update ac_user_copy set user_name1=@a where id=@id ", new { a = "asdas", id }, transaction: transaction);
            }
            
        }

        public async Task Insert(string index, string value)
        {
            var connection = await _dbProvider.GetConnectionAsync();
            var transaction = await _dbProvider.GetTransactionAsync();
            await connection.InsertAsync(new TestData() {id=Guid.NewGuid().ToString(),value= value,index= index }, transaction: transaction);

        }

        [Table("ac_user_copy1")]
        public class TestData
        { 
            [ExplicitKey]
            public string id { get; set; }

            public string value { get; set; }

            public string index { get; set; }
        }
    }
}
