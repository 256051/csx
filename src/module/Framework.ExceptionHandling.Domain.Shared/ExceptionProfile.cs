using AutoMapper;
using Framework.ExceptionHandling.Dapper;
using Microsoft.Extensions.Logging;

namespace Framework.ExceptionHandling.Domain.Shared
{
    public class ExceptionProfile : Profile
    {
        public ExceptionProfile()
        {
            CreateMap<ExceptionInfo, ExceptionResponseDto>()
                .ForMember(m => m.LogLevel, m => m.MapFrom<LogLevelValueResolver>());
        }
    }

    public class LogLevelValueResolver : IValueResolver<ExceptionInfo, ExceptionResponseDto, string>
    {
        public string Resolve(ExceptionInfo source, ExceptionResponseDto destination, string destMember, ResolutionContext context)
        {
            switch (source.LogLevel)
            {
                case (int)LogLevel.Debug:
                    return "调试";       
                case (int)LogLevel.Information:
                    return "信息"; 
                case (int)LogLevel.Warning:
                    return "警告"; 
                case (int)LogLevel.Error:
                    return "异常"; 
                case (int)LogLevel.Trace:
                    return "追踪"; 
                case (int)LogLevel.Critical:
                    return "不稳定";
                default:return "未知级别";
            }
        }
    }
}
