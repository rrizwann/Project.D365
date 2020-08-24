using Microsoft.Xrm.Sdk;
using Project.D365.Core.Managers;
using System;

namespace Project.D365.Plugins
{
    public class UpdateCreditMax : PluginBase
    {
        protected override bool IsContextValid(IPluginExecutionContext context)
        {
            return context.PrimaryEntityName == "account"
                && context.MessageName == MessageName.Update
                && context.Stage == ProcessingStage.PostOperation 
                && context.Mode == ProcessingMode.Synchronous 
                && context.IsolationMode == IsolationMode.Sandbox;
        }

        public override void Execute(IServiceProvider serviceProvider, IPluginExecutionContext pluginExecutionContext, IOrganizationService organizationService, ITracingService tracingService, string messageName)
        {
            Entity postImage = (Entity)pluginExecutionContext.PostEntityImages["postImage"];

            Money val_credit_max = null;
            if (postImage.Attributes.Contains("nt_manualcreditlimit"))
            {
                val_credit_max = postImage.Attributes["nt_manualcreditlimit"] as Money;
            }
            else if(postImage.Attributes.Contains("nt_credit_max"))
            {
                val_credit_max = postImage.Attributes["nt_credit_max"] as Money; 
            }

            EntityCollection opportunities = new OpportunityManager(organizationService)
                .GetOpportunities(postImage.Id);
            foreach (Entity opportunity in opportunities.Entities)
            {
                Entity oEntity = new Entity(opportunity.LogicalName, opportunity.Id);
                oEntity.Attributes.Add("nt_creditmax", val_credit_max);
                organizationService.Update(oEntity);
            } 
        } 
    }
}
