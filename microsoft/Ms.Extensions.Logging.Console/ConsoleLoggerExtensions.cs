// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Ms.Extensions.Logging.Abstracts;
using Ms.Extensions.Logging.Configuration;
using Ms.Extensions.Options;
using Ms.Extensions.Options.ConfigurationExtensions;
using System;

namespace Ms.Extensions.Logging.Console
{
    public static class ConsoleLoggerExtensions
    {
        public static ILoggingBuilder AddConsole(this ILoggingBuilder builder)
        {
            builder.AddConfiguration();
            builder.AddConsoleFormatter<SimpleConsoleFormatter, SimpleConsoleFormatterOptions>();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());
            LoggerProviderOptions.RegisterProviderOptions<ConsoleLoggerOptions, ConsoleLoggerProvider>(builder.Services);
            return builder;
        }

        public static ILoggingBuilder AddConsoleFormatter<TFormatter,TOptions>(this ILoggingBuilder builder)
            where TOptions : ConsoleFormatterOptions
            where TFormatter : ConsoleFormatter
        {
            builder.AddConfiguration();
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ConsoleFormatter, TFormatter>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<TOptions>, ConsoleLoggerFormatterConfigureOptions<TFormatter, TOptions>>());
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IOptionsChangeTokenSource<TOptions>, ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions>>());
            return builder;
        }

        internal sealed class ConsoleLoggerFormatterConfigureOptions<TFormatter,TOptions> : ConfigureFromConfigurationOptions<TOptions>
            where TOptions : ConsoleFormatterOptions
            where TFormatter : ConsoleFormatter
        {
            public ConsoleLoggerFormatterConfigureOptions(ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration) :
                base(providerConfiguration.Configuration.GetSection("FormatterOptions"))
            {
            }
        }

        internal sealed class ConsoleLoggerFormatterOptionsChangeTokenSource<TFormatter, TOptions> : ConfigurationChangeTokenSource<TOptions>
            where TOptions : ConsoleFormatterOptions
            where TFormatter : ConsoleFormatter
        {
            public ConsoleLoggerFormatterOptionsChangeTokenSource(ILoggerProviderConfiguration<ConsoleLoggerProvider> providerConfiguration)
                : base(providerConfiguration.Configuration.GetSection("FormatterOptions"))
            {
            }
        }
    }
}
