using Microsoft.Xrm.Sdk;
using Risika.D365.Core.Common.Helpers;
using Risika.D365.Core.Models;
using Risika.D365.Core.Models.Company;
using Risika.D365.Core.Models.Financial;
using Risika.D365.Core.Models.Highlight;
using Risika.D365.Core.Models.Rating;
using System;
using System.Collections.Generic;

namespace Risika.D365.Core.Managers
{
    public class RisikaServiceManager : BaseManager
    {
        public RisikaServiceManager(IOrganizationService service)
            : base(service)
        {

        }

        public RisikaServiceManager(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService)
        {

        }

        public SearchResponse GetCompanies(string name, string baseUrl, string accessToken,string country)
        {
            try
            {
                var client = new RestClient(baseUrl, accessToken);

                string data = GetPostRawData(name);
                string token = GetWebApiToken(client);
                Uri uri = new Uri($"{country}/search/company", UriKind.Relative);

                SearchResponse result = client.Send<SearchResponse>(uri, "POST", data, token);
                return result;
            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException(ex.InnerException.ToString());
            }
        }

        public CompanyResponse GetCompanyData(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            var client = new RestClient(baseUrl, accessToken, baseLanguage);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/company/basics/{cvr}", UriKind.Relative);

             CompanyResponse result = client.Send<CompanyResponse>(uri, "GET", null, token);
            return result;
        }

        public CreditResponse GetCreditData(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            var client = new RestClient(baseUrl, accessToken);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/rating/credit/{cvr}", UriKind.Relative);

            CreditResponse result = client.Send<CreditResponse>(uri, "GET", null, token);

            return result;
        }

        public IList<StatsResponse> GetFinancialStats(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            var client = new RestClient(baseUrl, accessToken);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/financial/stats/{cvr}", UriKind.Relative);

            IList<StatsResponse> result = client.Send<IList<StatsResponse>>(uri, "GET", null, token);
            return result;
        }

        public IList<NumberResponse> GetFinancialNumbers(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            var client = new RestClient(baseUrl, accessToken);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/financial/numbers/{cvr}", UriKind.Relative);

            IList<NumberResponse> result = client.Send<IList<NumberResponse>>(uri, "GET", null, token);
            return result;
        }

        public HighlightResponse GetHighlights(string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            var client = new RestClient(baseUrl, accessToken, baseLanguage);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/highlights/{cvr}", UriKind.Relative);

            HighlightResponse result = client.Send<HighlightResponse>(uri, "GET", null, token);
            return result;
        }

        public IList<ScoreResponse> GetScores(string cvr, string baseUrl, string accessToken, string baseLanguage,string country)
        {
            var client = new RestClient(baseUrl, accessToken, baseLanguage);

            string token = GetWebApiToken(client);
            Uri uri = new Uri($"{country}/rating/scores/{cvr}", UriKind.Relative);

            IList<ScoreResponse> result = client.Send<IList<ScoreResponse>>(uri, "GET", null, token);
            return result;
        }

        private string GetPostRawData(string name)
        {
            CompanyParam parameter = new CompanyParam();
            parameter.Mode = "full";
            parameter.Filters = new CompanyFilter();
            parameter.Filters.Name = name;
            parameter.Filters.Active = true;

            string data = SerializerClient.WriteObject<CompanyParam>(parameter);
            return data;
        }

        private string GetWebApiToken(RestClient client)
        {
            Uri uri = new Uri($"access/refresh_token", UriKind.Relative);
            TokenResponse response = client.Send<TokenResponse>(uri, "GET");
            return response.Token;
        }
    }
}
