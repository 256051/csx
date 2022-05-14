using System.ComponentModel.DataAnnotations;

namespace Framework.DDD.Application.Contracts.Dtos
{
    public class PagedResultRequestDto:LimitedResultRequestDto,IPagedResultRequest
    {
        /// <summary>
        /// 第几页
        /// </summary>
        [Range(0, int.MaxValue)]
        [Required(ErrorMessage = "第几页不能为空")]
        public virtual int SkipCount { get; set; }
    }
}
