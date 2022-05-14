namespace Framework.BackgroundWorkers.Quartz
{
    public class BackgroundWorkerQuartzOptions
    {
        /// <summary>
        /// 自动注册是否开启,设置成true,那么可以在框架内自定义Quarz工作者,而不用走适配器转换
        /// </summary>
        public bool IsAutoRegisterEnabled { get; set; } = true;
    }
}
