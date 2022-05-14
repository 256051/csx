using System;

namespace Framework.BackgroundJobs.Quartz
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QuartzJobAttribute : Attribute
    {
        /// <summary>
        /// 默认每三分钟运行一次,从0分开始
        /// </summary>
        public string CornExpression { get; set; } = "0 0/3 * * * ?*";

        /// <summary>
        /// 触发器描述
        /// </summary>
        public string TriggerDescription { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 延时启动
        /// </summary>
        public int? Delay { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cornExpression">corn表达式(默认每3分钟执行一次)</param>
        /// <param name="description">job描述</param>
        /// <param name="delay">延时启动任务</param>
        public QuartzJobAttribute(string cornExpression="", string description="",int delay=0,string triggerDesc="")
        {
            CornExpression = cornExpression;
            Description = description;
            Delay = delay;
            TriggerDescription = triggerDesc;
        }

        public static QuartzJobAttribute Default => new QuartzJobAttribute();
    }
}
