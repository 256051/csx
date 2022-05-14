using AutoMapper;
using Framework.AspNetCore.Mvc;
using Framework.DDD.Application.Contracts.Dtos;
using Framework.ExceptionHandling.Dapper;
using Framework.ExceptionHandling.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.ExceptionHandling.WebApi
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = Consts.ModuleName)]
    public class ExceptionController : WebApiControllerBase
    {
        private readonly IExceptionRepository _exceptionRepository;
        private IMapper _mapper;
        public ExceptionController(
            IExceptionRepository exceptionRepository, 
            IMapper mapper)
        {
            _exceptionRepository = exceptionRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// 异常信息列表
        /// </summary>
        [HttpPost]
        public async Task<ExceptionPagedResponseDto> PageList(ExceptionPagedRequestDto dto)
        {
            (var entities, var count) = await _exceptionRepository.
                GetPageListAsync(dto.SkipCount, dto.MaxResultCount, dto.Handled, dto.LogLevel,dto.StartTime,dto.EndTime);
            var response=_mapper.Map<List<ExceptionInfo>,List<ExceptionResponseDto>>(entities.ToList());
            return new ExceptionPagedResponseDto(count, response);
        }
    }
}
