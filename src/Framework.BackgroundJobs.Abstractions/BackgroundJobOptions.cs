using Framework.Core;
using System;
using System.Collections.Generic;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 后台工作项配置类
    /// </summary>
    public class BackgroundJobOptions
    {
        private readonly Dictionary<Type, BackgroundJobConfiguration> _jobConfigurationsByArgsType;
        private readonly Dictionary<string, BackgroundJobConfiguration> _jobConfigurationsByName;

        public bool IsJobExecutionEnabled { get; set; } = true;

        public BackgroundJobOptions()
        {
            _jobConfigurationsByArgsType = new Dictionary<Type, BackgroundJobConfiguration>();
            _jobConfigurationsByName = new Dictionary<string, BackgroundJobConfiguration>();
        }

        /// <summary>
        /// 从模块配置中获取工作项
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public BackgroundJobConfiguration GetJob(string name)
        {
            var jobConfiguration = _jobConfigurationsByName.GetOrDefault(name);

            if (jobConfiguration == null)
            {
                throw new FrameworkException("Undefined background job for the job name: " + name);
            }

            return jobConfiguration;
        }

        /// <summary>
        /// 从模块配置中获取工作项
        /// </summary>
        /// <param name="argsType"></param>
        /// <returns></returns>
        public BackgroundJobConfiguration GetJob(Type argsType)
        {
            var jobConfiguration = _jobConfigurationsByArgsType.GetOrDefault(argsType);

            if (jobConfiguration == null)
            {
                throw new FrameworkException("Undefined background job for the job args type: " + argsType.AssemblyQualifiedName);
            }

            return jobConfiguration;
        }

        public void AddJob<TJob>()
        {
            AddJob(typeof(TJob));
        }

        public void AddJob(Type jobType)
        {
            AddJob(new BackgroundJobConfiguration(jobType));
        }

        public void AddJob(BackgroundJobConfiguration jobConfiguration)
        {
            _jobConfigurationsByArgsType[jobConfiguration.ArgsType] = jobConfiguration;
            _jobConfigurationsByName[jobConfiguration.JobName] = jobConfiguration;
        }
    }
}
