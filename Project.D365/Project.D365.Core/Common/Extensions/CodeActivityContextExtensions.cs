using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Risika.D365.Core.Common.Extensions
{
    public static class CodeActivityContextExtensions
    {
        public static IOrganizationServiceFactory GetOrganizationServiceFactory(this CodeActivityContext context)
        {
            return context.GetExtension<IOrganizationServiceFactory>();
        }

        public static IOrganizationService GetOrganizationServiceForInitiatingUser(this CodeActivityContext context)
        {
            IOrganizationServiceFactory serviceFactory = context.GetOrganizationServiceFactory();
            return serviceFactory.CreateOrganizationService(context.GetWorkflowExecutionContext().InitiatingUserId);
        }

        public static IOrganizationService GetOrganizationServiceForUser(this CodeActivityContext context)
        {
            IOrganizationServiceFactory serviceFactory = context.GetOrganizationServiceFactory();
            return serviceFactory.CreateOrganizationService(context.GetWorkflowExecutionContext().UserId);
        }

        public static IWorkflowContext GetWorkflowExecutionContext(this CodeActivityContext context)
        {
            return context.GetExtension<IWorkflowContext>();
        }

        public static ITracingService GetTracingService(this CodeActivityContext context)
        {
            return context.GetExtension<ITracingService>();
        }
    }
}
