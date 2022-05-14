using Framework.AspNetCore.Mvc.Conventions;
using Framework.AspNetCore.Mvc.ModelBinding;
using Framework.AspNetCore.Mvc.Uow;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AspNetCore.Mvc
{
    public static class MvcOptionsExtensions
    {
        public static void AddCore(this MvcOptions options, IServiceCollection services)
        {
            AddConventions(options);
            AddActionFilters(options);
            AddModelBinders(options);
        }

        /// <summary>
        /// 注入约定
        /// </summary>
        /// <param name="options"></param>
        /// <param name="services"></param>
        private static void AddConventions(MvcOptions options)
        {
            options.Conventions.Add(new ApiExplorerGroupPerVersionConvention());
        }

        /// <summary>
        /// 注入模型绑定
        /// </summary>
        /// <param name="options"></param>
        private static void AddModelBinders(MvcOptions options)
        {
            options.ModelBinderProviders.Insert(0, new DateTimeModelBinderProvider());
        }

        /// <summary>
        /// 注入拦截器
        /// </summary>
        /// <param name="options"></param>
        private static void AddActionFilters(MvcOptions options)
        {
            options.Filters.AddService(typeof(UowOfWorkFilter));
        }
    }
}
