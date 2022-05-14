namespace Framework.AspNetCore.ExceptionHandling
{
    public class ExceptionHandlingOptions
    {
        /// <summary>
        /// 是否展示异常详情给客户端
        /// </summary>
        public bool SendExceptionsDetailsToClients { get; set; } = false;

        /// <summary>
        /// 是否采用标准输出
        /// </summary>
        public bool UseStandardOutput { get; set; } = true;
    }
}
