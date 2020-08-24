using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace BusinessLogic.Test
{
    public abstract class BaseManagerTest
    {
        protected IOrganizationService Service;

        protected string BaseUrl;
        protected string AccessToken;
        protected string BaseLanguage;
        protected string Country;

        [TestInitialize]
        public void Setup()
        {
            Service = GetOrganizationService();

            BaseUrl = "https://api.risika.dk/v1.2/";
            AccessToken = "jwt eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6NzA0MiwidHlwZSI6InNpbmdsZS11c2VyIiwiZ3JhbnRfcGVybWl0IjpudWxsLCJjb21wYW55IjoxMTQ5LCJpYXQiOjE1OTM2NzIwMjgsIm5iZiI6MTU5MzY3MjAyOCwiZXhwIjo0NzQ5MzQ1NjI4fQ.iL0nWP5FYufgjJfyaUkrSTTdV6fgKuo17-x-DGtAND4";
            //BaseLanguage = "en-UK";
            //BaseLanguage = "da-DK";
            Country = "dk";


        }

        private IOrganizationService GetOrganizationService()
        {
            string ConnectionString = "AuthType=Office365;Url=https://crm620002.crm.dynamics.com;Username=admin@CRM620002.onmicrosoft.com;Password=Xtr3m3L@bs";

            CrmServiceClient crmServiceClient = new CrmServiceClient(ConnectionString);
            if (crmServiceClient.IsReady)
            {
                IOrganizationService service = (IOrganizationService)crmServiceClient.OrganizationWebProxyClient != null ? (IOrganizationService)crmServiceClient.OrganizationWebProxyClient : (IOrganizationService)crmServiceClient.OrganizationServiceProxy;
                return service;
            }
            else
            {
                throw crmServiceClient.LastCrmException;
            }
        }
    }
}
