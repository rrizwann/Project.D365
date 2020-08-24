using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Risika.D365.Core.Managers
{
    public class OpportunityManager : BaseManager
    {
        public OpportunityManager(IOrganizationService service)
            : base(service)
        {

        }

        public OpportunityManager(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService)
        {

        }

        public EntityCollection GetOpportunities(Guid accountid)
        {
            string query = string.Format(@"<fetch  no-lock=""true"">
                                              <entity name=""opportunity"">
                                                    <attribute name=""opportunityid"" />
                                                    <attribute name=""name"" />
                                                    <attribute name=""customerid"" />
                                                    <attribute name=""estimatedvalue"" />
                                                    <attribute name=""statuscode"" />
                                                    <filter type=""and"">
                                                      <condition attribute=""parentaccountid"" operator=""eq"" value= ""{0}""/>
                                                    </filter>
                                               </entity>
                                          </fetch>", accountid);

            EntityCollection collection = OrganizationService.RetrieveMultiple(new FetchExpression(query));
            return collection;
        }
    }
}
