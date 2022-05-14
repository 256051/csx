namespace System
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// 获取一个时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTimeOffset dateTime) 
        {
            return dateTime.ToUnixTimeSeconds();
        }

        /// <summary>
        /// 根据时间戳返回时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTimeOffset ConvertToDateTimeOffset(this long timeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(timeStamp);
        }
    }
}
