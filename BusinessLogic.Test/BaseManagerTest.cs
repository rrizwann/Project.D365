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

            BaseUrl = "https://api.xyz.dk/v1.2/";
            AccessToken = "";
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
