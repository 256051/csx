using System.ComponentModel.DataAnnotations;

namespace Framework.DDD.Application.Contracts.Dtos
{
    public class LimitedResultRequestDto:ILimitedResultRequest
    {
        /// <summary>
        /// 默认最大返回结果记录条数
        /// </summary>
        public static int DefaultMaxResultCount { get; set; } = 10;

        /// <summary>
        /// 最大允许返回的分页结果记录数
        /// </summary>
        public static int MaxMaxResultCount { get; set; } = 1000;

        /// <summary>
        /// 每页显示多少条
        /// </summary>
        [Range(1, int.MaxValue)]
        [Required(ErrorMessage ="每页记录数不能为空")]
        public virtual int MaxResultCount { get; set; } = DefaultMaxResultCount;
    }
}
