using Microsoft.Xrm.Sdk;
using Risika.D365.Core.Managers;
using System;

namespace Risika.D365.Plugins
{
    public class RetrieveCompanies : PluginBase
    {
        protected override bool IsContextValid(IPluginExecutionContext context)
        {
            return context.Stage == ProcessingStage.PostOperation
                && context.Mode == ProcessingMode.Synchronous
                && context.IsolationMode == IsolationMode.Sandbox;
        }

        public override void Execute(IServiceProvider serviceProvider, IPluginExecutionContext pluginExecutionContext, IOrganizationService organizationService, ITracingService tracingService, string messageName)
        {
            string baseUrl = GetInputParameter<string>(pluginExecutionContext, ParameterName.Url); 
            string accessToken = GetInputParameter<string>(pluginExecutionContext, ParameterName.Token); 
            string name = GetInputParameter<string>(pluginExecutionContext, ParameterName.Name);
            string country = GetInputParameter<string>(pluginExecutionContext, ParameterName.Country);
            tracingService.Trace($"InputParameters Url: {baseUrl}, Token: {accessToken}, Name: {name}"); 

            string response = string.Empty;
            if (!(string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(name)))
            {
                response = new CompanyManager(organizationService).GetCompanies(name, baseUrl, accessToken, country);
            }

            tracingService.Trace($"WebApi result: {response}");
            this.SetOutputParameter<string>(pluginExecutionContext, ParameterName.Response, response);
        }
    }
}
