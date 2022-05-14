using Framework.Core;
using System;
using System.Linq;

namespace Framework.BackgroundJobs.Abstractions
{
    public class BackgroundJobNameAttribute : Attribute, IBackgroundJobNameProvider
    {
        public string Name { get; }

        public BackgroundJobNameAttribute(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public static string GetName<TJobArgs>()
        {
            return GetName(typeof(TJobArgs));
        }

        public static string GetName(Type jobArgsType)
        {
            Check.NotNull(jobArgsType, nameof(jobArgsType));

            return jobArgsType
                       .GetCustomAttributes(true)
                       .OfType<IBackgroundJobNameProvider>()
                       .FirstOrDefault()
                       ?.Name
                   ?? jobArgsType.FullName;
        }
    }
}
