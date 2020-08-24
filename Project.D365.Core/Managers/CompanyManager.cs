using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Risika.D365.Core.Common.Helpers;
using Risika.D365.Core.Models.Company;
using Risika.D365.Core.Models.Rating;
using System.Linq;

namespace Project.D365.Core.Managers
{
    public class CompanyManager : BaseManager
    {
        public CompanyManager(IOrganizationService service)
            : base(service)
        {

        }

        public CompanyManager(IOrganizationService service, ITracingService tracer)
            : base(service, tracer)
        {

        }

        public Entity GetCompany(EntityReference companyid)
        {
            return OrganizationService.Retrieve(companyid.LogicalName, companyid.Id, new ColumnSet(true)); 
        }

        public Entity GetCompany(string cvr)
        {
            string query = string.Format(@"<fetch no-lock=""true"" >
                                              <entity name=""account"" >
                                                <all-attributes/>
                                                <filter>
                                                  <condition attribute=""accountnumber"" operator=""eq"" value=""{0}"" />
                                                </filter>
                                              </entity>
                                            </fetch>", cvr);

            EntityCollection collection = OrganizationService.RetrieveMultiple(new FetchExpression(query));
            return collection.Entities.FirstOrDefault();
        }

        public string GetCompanies(string name, string baseUrl, string accessToken,string country)
        {
            SearchResponse searchResponses = new RisikaServiceManager(OrganizationService)
                .GetCompanies(name, baseUrl, accessToken, country);

            return SerializerClient.WriteObject(searchResponses);
        }

        public CompanyResponse GetCompanyData(string cvr, string baseUrl, string accessToken, string baseLanguage,string country)
        {
            CompanyResponse companyResponses = new RisikaServiceManager(OrganizationService)
                .GetCompanyData(cvr, baseUrl, accessToken, baseLanguage, country);
            return companyResponses;
        }

        public CreditResponse GetCreditData(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            CreditResponse creditResponses = new RisikaServiceManager(OrganizationService)
                .GetCreditData(cvr, baseUrl, accessToken, baseLanguage,country);
            return creditResponses;
        }
    }
}
