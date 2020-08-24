﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Risika.D365.Core.Common.Extensions;
using System;
using System.Activities;

namespace Project.D365.Workflows
{
    public abstract class WebApiActivityBase : CodeActivity
    {
        [RequiredArgument]
        [Input("WebApi Access Token")]
        
        public InArgument<string> Token { get; set; }

        [RequiredArgument]
        [Input("WebApi Url")]
        [Default("https://api.xyz.dk/v1.2/")]
        public InArgument<string> Url { get; set; }

        [RequiredArgument]
        [Input("WebApi Accept-Language")]
        [Default("en-UK")]
        public InArgument<string> Language { get; set; }

        [RequiredArgument]
        [Input("Country")]
        public InArgument<string> Country { get; set; }

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

                tracing.Trace("Invoking WebApiActivityBase.Execute() method.");
                Execute(context, executionContext, service, tracing);
                tracing.Trace("Completed WebApiActivityBase.Execute() method.");
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
