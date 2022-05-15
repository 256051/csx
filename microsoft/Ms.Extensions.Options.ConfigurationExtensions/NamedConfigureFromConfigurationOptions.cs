// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Ms.Configuration.Abstracts;
using Ms.Extensions.Configuration;
using System;

namespace Ms.Extensions.Options.ConfigurationExtensions
{
    public class NamedConfigureFromConfigurationOptions<TOptions> : ConfigureNamedOptions<TOptions>
        where TOptions : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">选项的名称</param>
        /// <param name="config">配置节点</param>
        /// <param name="configureBinder">绑定回调</param>
        public NamedConfigureFromConfigurationOptions(string name, IConfiguration config, Action<BinderOptions> configureBinder):base(name, options => BindFromOptions(options, config, configureBinder))
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }
        }


        private static void BindFromOptions(TOptions options, IConfiguration config, Action<BinderOptions> configureBinder) => config.Bind(options, configureBinder);
    }
}
