using Microsoft.Xrm.Sdk;
using System;

namespace Risika.D365.Core.Common.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static IOrganizationServiceFactory GetOrganizationServiceFactory(this IServiceProvider serviceProvider)
        {
            return (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
        }

        public static IOrganizationService GetOrganizationServiceForInitiatingUser(this IServiceProvider serviceProvider)
        {
            IOrganizationServiceFactory serviceFactory = serviceProvider.GetOrganizationServiceFactory();
            return serviceFactory.CreateOrganizationService(serviceProvider.GetPluginExecutionContext().InitiatingUserId);
        }

        public static IOrganizationService GetOrganizationServiceForUser(this IServiceProvider serviceProvider)
        {
            IOrganizationServiceFactory serviceFactory = serviceProvider.GetOrganizationServiceFactory();
            return serviceFactory.CreateOrganizationService(serviceProvider.GetPluginExecutionContext().UserId);
        }

        public static IPluginExecutionContext GetPluginExecutionContext(this IServiceProvider serviceProvider)
        {
            return (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
        }

        public static ITracingService GetTracingService(this IServiceProvider serviceProvider)
        {
            return (ITracingService)serviceProvider.GetService(typeof(ITracingService));
        }
    }
}
