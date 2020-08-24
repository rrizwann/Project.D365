using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Risika.D365.Core.Models.Rating;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Risika.D365.Core.Managers
{
    public class ScoreManager : BaseManager
    {
        public ScoreManager(IOrganizationService service)
            : base(service)
        {

        }

        public ScoreManager(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService)
        {

        }

        public void CopyScores(EntityReference companyid, string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            Entity company = new CompanyManager(OrganizationService).GetCompany(companyid);
            if (company != null)
            {
                EntityCollection scores = this.GetScores(company.Id);
                OrganizationRequestCollection requests = new OrganizationRequestCollection();

                IList<ScoreResponse> scoreResponses = new RisikaServiceManager(OrganizationService)
                    .GetScores(cvr, baseUrl, accessToken, baseLanguage, country);
                foreach (ScoreResponse scoreResponse in scoreResponses)
                {
                    if (scoreResponse != null && !string.IsNullOrEmpty(scoreResponse.date))
                    {
                        Entity oEntity = this.GetEntityObject(scoreResponse, scores, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                }

                if (requests.Count >= 1)
                {
                    ExecuteMultipleResponse response = this.ExecuteMultiple(requests);
                    if (response.IsFaulted)
                    {
                        throw new InvalidPluginExecutionException(this.GetExecuteMultipleFaultLog(response));
                    }
                }
            }
            this.SetCompanyScore(company);
        }

        public EntityCollection GetScores(Guid accountid)
        {
            string query = string.Format(@"<fetch no-lock=""true"" >
                              <entity name=""nt_scores"" >
                                <all-attributes/>
                                <filter>
                                  <condition attribute=""nt_accountid"" operator=""eq"" value=""{0}"" />
                                  <condition attribute=""nt_date"" operator=""not-null"" />
                                </filter>
                              </entity>
                            </fetch>", accountid);

            EntityCollection collection = OrganizationService.RetrieveMultiple(new FetchExpression(query));
            return collection;
        }

      

        private Entity GetEntityObject(ScoreResponse scoreResponse, EntityCollection scores, Entity company)
        {
            Entity oEntity = new Entity("nt_scores");
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_score", scoreResponse.score));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_error", scoreResponse.error));

            DateTime date;
            if (DateTime.TryParse(scoreResponse.date, out date))
            {
                Entity existingScore = scores.Entities
                    .Where(row => ((DateTime)row.Attributes["nt_date"]).Date == date.Date)
                    .FirstOrDefault();

                if (existingScore != null)
                {
                    oEntity.Id = existingScore.Id;
                }
                else
                {
                    DateTime value = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                    oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_date", value));
                }
            }

            if (oEntity.Id == Guid.Empty)
            {
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_accountid", company.ToEntityReference()));
            }

            return oEntity;
        }

        private void SetCompanyScore(Entity company)
        {
            
            Entity oCompany = new Entity(company.LogicalName, company.Id);
            EntityReference latestscore = null;
            string Error = null; 
            if (company.Attributes.ContainsKey("nt_scores"))
            {
                latestscore = company.Attributes["nt_latestscore"] as EntityReference;
            }
            
            Entity score = this.GetScores(company.Id).Entities
                .OrderByDescending(row => row.Attributes["nt_date"])
                .FirstOrDefault();
            if (score != null && (latestscore == null || latestscore.Id != score.Id))
            {
                
              //  oCompany.Attributes.Add(new KeyValuePair<string, object>("nt_latestscore", score.ToEntityReference()));
                if (score.Attributes.Contains("nt_error"))
                {
                    Error = score.Attributes["nt_error"] as string;
                    oCompany.Attributes.Add(new KeyValuePair<string, object>("nt_error", Error));
                }
                
                OrganizationService.Update(oCompany);
            }

           
        }

    
    }
}
