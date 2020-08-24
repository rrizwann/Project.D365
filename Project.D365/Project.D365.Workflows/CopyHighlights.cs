﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Risika.D365.Core.Managers;
using System.Activities;

namespace Risika.D365.Workflows
{
    public class CopyHighlights : WebApiActivityBase
    {
        [RequiredArgument]
        [Input("CVR")]
        public InArgument<string> CVR { get; set; }

        [RequiredArgument]
        [Input("Company")]
        [ReferenceTarget("account")]
        public InArgument<EntityReference> Company { get; set; }

        public override void Execute(CodeActivityContext activityContext, IWorkflowContext workflowContext, IOrganizationService organizationService, ITracingService tracingService)
        {
            tracingService.Trace("CopyHighlights Workflow Started");
            string baseUrl = Url.Get(activityContext);
            string accessToken = Token.Get(activityContext);
            string baseLanguage = Language.Get(activityContext);
            string country = Country.Get(activityContext);

            string cvr = CVR.Get(activityContext);
            EntityReference companyid = Company.Get(activityContext);
            if (!(companyid == null || string.IsNullOrEmpty(cvr) || string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(accessToken)))
            {
                new HighlightManager(organizationService).CopyHighlights(companyid, cvr, baseUrl, accessToken, baseLanguage,tracingService, country);
            } 
        }
    }
}
