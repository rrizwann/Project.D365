using Microsoft.Xrm.Sdk;
using Risika.D365.Core.Common.Extensions;
using System;

namespace Project.D365.Plugins
{
    public static class ParameterName
    {
        public const string Target = "Target";
        public const string PostImage = "PostImage";

        public const string Url = "Url";
        public const string Token = "Token";
        public const string Name = "Name";
        public const string Response = "Response";
        public const string Country = "Country";
    }

    public static class MessageName
    {
        public const string Create = "Create";
        public const string Delete = "Delete";
        public const string Update = "Update";
    }

    public static class ProcessingMode
    {
        public const int Synchronous = 0;
        public const int Asynchronous = 1;
    }

    public static class ProcessingStage
    {
        public const int PreValidation = 10;
        public const int PreOperation = 20;
        public const int MainOperation = 30;
        public const int PostOperation = 40;
    }

    public static class IsolationMode
    {
        public const int None = 1;
        public const int Sandbox = 2;
    }

    public abstract class PluginBase : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracing = serviceProvider.GetTracingService();
            if (tracing == null)
                throw new InvalidPluginExecutionException($"Plugin '{GetPluginName()}' failed to initialize tracing service");

            try
            {
                IPluginExecutionContext context = serviceProvider.GetPluginExecutionContext();
                if (context == null)
                    throw new InvalidPluginExecutionException($"Plugin '{GetPluginName()}' failed to initialize plugin execution context");

                IOrganizationService service = serviceProvider.GetOrganizationServiceForUser();
                if (service == null)
                    throw new InvalidPluginExecutionException($"Plugin '{GetPluginName()}' failed to create the default organization service");

                if (!IsContextValid(context))
                {
                    tracing.Trace($"Invalid plugin execution context detected in Plugin '{GetPluginName()}'."); 
                    tracing.Trace("Plugin execution aborted.");
                    return;
                }
                Execute(serviceProvider, context, service, tracing, context.MessageName);
            }
            catch (InvalidPluginExecutionException)
            {
                throw;
            }
            catch (Exception e)
            {
                tracing.Trace("Plugin '{0}' threw an unexpected exception: {1}", GetPluginName(), e.ToString()); 

                throw new InvalidPluginExecutionException(OperationStatus.Failed, "A plugin failed to execute due to an unexpected error."); 
            }
        }  

        public abstract void Execute(IServiceProvider serviceProvider, IPluginExecutionContext pluginExecutionContext,
            IOrganizationService organizationService, ITracingService tracingService, string messageName);

        protected string GetPluginName()
        {
            return GetType().FullName;
        }

        protected virtual bool IsContextValid(IPluginExecutionContext context)
        {
            return true;
        }

        protected Entity GetTargetEntity(IPluginExecutionContext context)
        {
            return context != null && context.InputParameters != null && context.InputParameters.Contains(ParameterName.Target) 
                ? context.InputParameters[ParameterName.Target] as Entity 
                : null;
        }

        protected Entity GetPreEntityImage(IPluginExecutionContext context, string name)
        {
            return context != null && context.PreEntityImages != null && context.PreEntityImages.Contains(name) 
                ? context.PreEntityImages[name] 
                : null;
        }

        protected Entity GetPostEntityImage(IPluginExecutionContext context, string aliasName)
        {
            return context != null && context.PostEntityImages != null && context.PostEntityImages.Contains(aliasName) 
                ? context.PostEntityImages[aliasName] 
                : null;
        }

        protected T GetInputParameter<T>(IPluginExecutionContext context, string name)
        {
            return context != null && context.InputParameters != null && context.InputParameters.Contains(name) 
                ? (T)context.InputParameters[name] 
                : default(T); 
        }

        protected void SetOutputParameter<T>(IPluginExecutionContext context, string name, T value)
        {
            if (context != null && context.OutputParameters != null)
            {
                if (context.OutputParameters.Contains(name))
                {
                    context.OutputParameters[name] = value;
                }
                else
                {
                    context.OutputParameters.Add(name, value);
                }
            } 
        }
    } 
}
