using Framework.DDD.Application.Contracts.Dtos;
using System.Collections.Generic;

namespace Framework.ExceptionHandling.Domain.Shared
{

    public class ExceptionPagedResponseDto: PagedResultDto<ExceptionResponseDto>
    {
        public ExceptionPagedResponseDto(long totalCount, IReadOnlyList<ExceptionResponseDto> items): base(totalCount,items)
        {
            TotalCount = totalCount;
        }
    }
}
