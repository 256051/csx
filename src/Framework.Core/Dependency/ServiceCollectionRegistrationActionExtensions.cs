using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Dependency
{
    public static class ServiceCollectionRegistrationActionExtensions
    {
        public static void OnRegistred(this IServiceCollection services, Action<IOnServiceRegistredContext> registrationAction)
        {
            GetOrCreateRegistrationActionList(services).Add(registrationAction);
        }

        public static ServiceRegistrationActionList GetRegistrationActionList(this IServiceCollection services)
        {
            return GetOrCreateRegistrationActionList(services);
        }

        public static ServiceExposingActionList GetExposingActionList(this IServiceCollection services)
        {
            return GetOrCreateExposingList(services);
        }

        private static ServiceRegistrationActionList GetOrCreateRegistrationActionList(IServiceCollection services)
        {
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceRegistrationActionList>>()?.Value;
            if (actionList == null)
            {
                actionList = new ServiceRegistrationActionList();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }

        private static ServiceExposingActionList GetOrCreateExposingList(IServiceCollection services)
        {
            var actionList = services.GetSingletonInstanceOrNull<IObjectAccessor<ServiceExposingActionList>>()?.Value;
            if (actionList == null)
            {
                actionList = new ServiceExposingActionList();
                services.AddObjectAccessor(actionList);
            }

            return actionList;
        }
    }
}
