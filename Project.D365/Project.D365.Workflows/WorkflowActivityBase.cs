using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Risika.D365.Core.Common.Extensions;
using System;
using System.Activities;

namespace Risika.D365.Workflows
{
    public abstract class WorkflowActivityBase : CodeActivity
    {
        protected override void Execute(CodeActivityContext context)
        {
            ITracingService tracing = context.GetTracingService();
            if (tracing == null)
                throw new InvalidPluginExecutionException($"Custom workflow activity '{GetWorkflowName()}' failed to initialize tracing service");

            try
            {
                IWorkflowContext executionContext = context.GetWorkflowExecutionContext();
                if (executionContext == null)
                    throw new InvalidPluginExecutionException($"Custom workflow activity '{GetWorkflowName()}' failed to initialize workflow execution context");

                IOrganizationService service = context.GetOrganizationServiceForUser();
                if (service == null)
                    throw new InvalidPluginExecutionException($"Custom workflow activity '{GetWorkflowName()}' failed to create the default organization service");

                tracing.Trace("Invoking WorkflowActivityBase.Execute() method.");
                Execute(context, executionContext, service, tracing);
            }
            catch (InvalidPluginExecutionException)
            {
                throw;
            }
            catch (Exception e)
            {
                tracing.Trace("Custom workflow activity '{0}' threw an unexpected exception: {1}", GetWorkflowName(), e.ToString()); 

                throw new InvalidPluginExecutionException(OperationStatus.Failed, "A workflow failed to execute due to an unexpected error.");
            }
        }

        private string GetWorkflowName()
        {
            return GetType().FullName;
        }

        public abstract void Execute(CodeActivityContext activityContext, IWorkflowContext workflowContext,
                                     IOrganizationService organizationService, ITracingService tracingService);
    }
}
