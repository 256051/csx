using Framework.AspNetCore.Mvc;
using Framework.AspNetCore.Swagger;
using Framework.BlobStoring;
using Framework.BlobStoring.FileSystem;
using Framework.Core.Configurations;
using Framework.Dapper;
using Framework.Data.MySql;
using Framework.Excel.Npoi;
using Framework.ExceptionHandling;
using Framework.Json;
using Framework.Logging;
using Framework.Security;
using Framework.Uow;
using Framework.Web.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Framework.AspNetCore.Test
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureFrameworkModules(services);
            services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true));
            ConfigureServiceModules();
        }

        /// <summary>
        /// ���ÿ��ģ�� ���ر����ҵ�����
        /// </summary>
        private void ConfigureFrameworkModules(IServiceCollection services)
        {
            services
            .UseCore()
            .UseLogging()
            .UseSecurity()
            .UseException()
            .UseJson()
            .UseUnitOfWork()
            .UseAspNetCore()
            .UseAspNetCoreMvc()
            .UseMySql()
            .UseDapper()
            .UseNpoiExcel()
            .UseBlobStoring()
            .UseFileSystemBlobStoring();
        }

        /// <summary>
        /// ����ҵ��ģ��
        /// </summary>
        private void ConfigureServiceModules()
        {
            ApplicationConfiguration.Current
               .UseApplication()
               .UseAspNetCoreSwagger();

            //�Զ�����֤������ע��
            ApplicationConfiguration.Current.Container.Configure<MvcOptions>(mvcOptions =>
            {
                mvcOptions.Filters.AddService(typeof(AuthenticationFilter));
            });


            //��������ģ��
            ApplicationConfiguration.Current.LoadModules();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseExceptionHandling();
            app.UseUnitOfWork();
            app.UseConfiguredEndpoints();
            app.UseAspNetCoreSwagger();
        }
    }
}
