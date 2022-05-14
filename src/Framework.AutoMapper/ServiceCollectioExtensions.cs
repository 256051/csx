using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutoMapper
{
    public static class ServiceCollectioExtensions
    {
        /// <summary>
        /// 写入Profile配置到DI中
        /// </summary>
        /// <typeparam name="TProfile"></typeparam>
        /// <param name="services"></param>
        public static void AddAutoMapper<TProfile>(this IServiceCollection services) where TProfile:Profile
        {
            services.AddAutoMapper(typeof(TProfile).Assembly);
        }
    }
}
