using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Risika.D365.Core.Models.Highlight;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Risika.D365.Core.Managers
{
    public class HighlightManager : BaseManager
    {
        public HighlightManager(IOrganizationService service)
            : base(service)
        {

        }

        public HighlightManager(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService)
        {
            
        }

        public void CopyHighlights(EntityReference companyid, string cvr, string baseUrl, string accessToken, string baseLanguage, ITracingService tracingService, string country)
        {
            Entity company = new CompanyManager(OrganizationService).GetCompany(companyid);
            if (company != null)
            {
                EntityCollection highlights = GetHighlights(company.Id);

                HighlightResponse highlightResponse = new RisikaServiceManager(OrganizationService)
                    .GetHighlights(cvr, baseUrl, accessToken, baseLanguage,country);

                HighlightResponse highlightsModel = new HighlightResponse();
                
                if (highlightResponse != null && highlightResponse.highlights != null)
                {
                    OrganizationRequestCollection requests = new OrganizationRequestCollection();
                    if (highlightResponse.highlights.age != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.age, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        Age age = new Age();
                        bool isDeactivated = this.DeactivateHighlight(age, highlights);
                    }
                    if (highlightResponse.highlights.address != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.address, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        Address address = new Address();
                        bool isDeactivated = this.DeactivateHighlight(address, highlights);
                    }
                    if (highlightResponse.highlights.change_in_management != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.change_in_management, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ChangeInManagement change_in_management = new ChangeInManagement();
                        bool isDeactivated = this.DeactivateHighlight(change_in_management, highlights);
                    }
                    if (highlightResponse.highlights.powers_to_bind != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.powers_to_bind, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        PowersToBind powers_to_bind = new PowersToBind();
                        bool isDeactivated = this.DeactivateHighlight(powers_to_bind, highlights);
                    }

                    if (highlightResponse.highlights.change_in_employees != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.change_in_employees, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ChangeInEmployees change_in_employees = new ChangeInEmployees();
                        bool isDeactivated = this.DeactivateHighlight(change_in_employees, highlights);
                    }

                    if (highlightResponse.highlights.intangible_assets != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.intangible_assets, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        IntangibleAssets intangible_assets = new IntangibleAssets();
                        bool isDeactivated = this.DeactivateHighlight(intangible_assets, highlights);
                    }

                    if (highlightResponse.highlights.type_of_auditor_assistance != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.type_of_auditor_assistance, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        TypeOfAuditorAssistance type_of_auditor_assistance = new TypeOfAuditorAssistance();
                        bool isDeactivated = this.DeactivateHighlight(type_of_auditor_assistance, highlights);
                    }

                    if (highlightResponse.highlights.connected_bankruptcies != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.connected_bankruptcies, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ConnectedBankruptcies connected_bankruptcies = new ConnectedBankruptcies();
                        bool isDeactivated = this.DeactivateHighlight(connected_bankruptcies, highlights);
                    }

                    if (highlightResponse.highlights.auditor_changes != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.auditor_changes, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else if (highlightResponse.highlights.auditors != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.auditors, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        AuditorChanges auditor_changes = new AuditorChanges();
                        bool isDeactivated = this.DeactivateHighlight(auditor_changes, highlights);
                    }

                    if (highlightResponse.highlights.change_in_profit_loss != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.change_in_profit_loss, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else if (highlightResponse.highlights.profit_loss != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.profit_loss, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ChangeInProfitloss change_in_profit_loss = new ChangeInProfitloss();
                        bool isDeactivated = this.DeactivateHighlight(change_in_profit_loss, highlights);
                    }

                    if (highlightResponse.highlights.change_in_revenue != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.change_in_revenue, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else if (highlightResponse.highlights.revenue != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.revenue, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ChangeInRevenue change_in_revenue = new ChangeInRevenue();
                        bool isDeactivated = this.DeactivateHighlight(change_in_revenue, highlights);
                    }

                    if (highlightResponse.highlights.equity != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.equity, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        Equity equity = new Equity();
                        bool isDeactivated = this.DeactivateHighlight(equity, highlights);
                    }

                    if (highlightResponse.highlights.one_financial_statement != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.one_financial_statement, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        OneFinancialStatement one_financial_statement = new OneFinancialStatement();
                        bool isDeactivated = this.DeactivateHighlight(one_financial_statement, highlights);
                    }

                    if (highlightResponse.highlights.foreign_currency != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.foreign_currency, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ForeignCurrency foreign_currency = new ForeignCurrency();
                        bool isDeactivated = this.DeactivateHighlight(foreign_currency, highlights);
                    }

                    if (highlightResponse.highlights.old_financialstatement != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.old_financialstatement, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        OldFinancialstatement old_financialstatement = new OldFinancialstatement();
                        bool isDeactivated = this.DeactivateHighlight(old_financialstatement, highlights);
                    }

                    if (highlightResponse.highlights.three_years_profitloss != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.three_years_profitloss, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ThreeYearsProfitloss three_years_profitloss = new ThreeYearsProfitloss();
                        bool isDeactivated = this.DeactivateHighlight(three_years_profitloss, highlights);
                    }

                    if (highlightResponse.highlights.timely_delivery != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.timely_delivery, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        TimelyDelivery timely_delivery = new TimelyDelivery();
                        bool isDeactivated = this.DeactivateHighlight(timely_delivery, highlights);
                    }

                    if (highlightResponse.highlights.industry != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.industry, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        Industry industry = new Industry();
                        bool isDeactivated = this.DeactivateHighlight(industry, highlights);
                    }

                    if (highlightResponse.highlights.industry_risk != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.industry_risk, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        IndustryRisk industry_risk = new IndustryRisk();
                        bool isDeactivated = this.DeactivateHighlight(industry_risk, highlights);
                    }

                    if (highlightResponse.highlights.quantile_compare != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.quantile_compare, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        QuantileCompare quantile_compare = new QuantileCompare();
                        bool isDeactivated = this.DeactivateHighlight(quantile_compare, highlights);
                    }

                    if (highlightResponse.highlights.company_type != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.company_type, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        CompanyType company_type = new CompanyType();
                        bool isDeactivated = this.DeactivateHighlight(company_type, highlights);
                    }

                    if (highlightResponse.highlights.proprietorship_age != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.proprietorship_age, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ProprietorshipAge proprietorship_age = new ProprietorshipAge();
                        bool isDeactivated = this.DeactivateHighlight(proprietorship_age, highlights);
                    }

                    //current_ratio
                    if (highlightResponse.highlights.current_ratio != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.current_ratio, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        CurrentRatio current_ratio = new CurrentRatio();
                        bool isDeactivated = this.DeactivateHighlight(current_ratio, highlights);
                    }

                    //return_on_assets
                    if (highlightResponse.highlights.return_on_assets != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.return_on_assets, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        ReturnOnAssets return_on_assets = new ReturnOnAssets();
                        bool isDeactivated = this.DeactivateHighlight(return_on_assets, highlights);
                    }

                    //solidity_ratio
                    if (highlightResponse.highlights.solidity_ratio != null)
                    {
                        Entity oEntity = this.GetEntityObject(highlightResponse.highlights.solidity_ratio, highlights, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
                    else
                    {
                        SolidityRatio solidity_ratio = new SolidityRatio();
                        bool isDeactivated = this.DeactivateHighlight(solidity_ratio, highlights);
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
            }
        }

        public EntityCollection GetHighlights(Guid accountid)
        {
            string query = string.Format(@"<fetch no-lock=""true"" >
                              <entity name=""nt_highlight"" >
                                <all-attributes/>
                                <filter>
                                  <condition attribute=""nt_company"" operator=""eq"" value=""{0}"" />
                                  <condition attribute=""nt_type"" operator=""not-null"" />
                                  <condition attribute=""statecode"" operator=""eq"" value=""0"" />                              
                                </filter>
                              </entity>
                            </fetch>", accountid);

            EntityCollection collection = OrganizationService.RetrieveMultiple(new FetchExpression(query));
            return collection;
        }

        private Entity GetEntityObject(IHighlightType highlightType, EntityCollection highlights, Entity company)
        {
            Entity oEntity = new Entity("nt_highlight");

            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_title", highlightType.title));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_message", highlightType.message));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_description", highlightType.description));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_weight", highlightType.weight));

            int classificationCode = this.GetClassificationCode(highlightType.classification);
            OptionSetValue classificationValue = classificationCode == -1 ? null : new OptionSetValue(classificationCode);
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_classification", classificationValue));

            int highlightTypeCode = GetHighlightTypeCode(highlightType);
            if (highlightTypeCode > 0)
            {
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_type", new OptionSetValue(highlightTypeCode)));

                Entity existingHighlight = highlights.Entities
                    .Where(row => ((OptionSetValue)row.Attributes["nt_type"]).Value == highlightTypeCode)
                    .FirstOrDefault();
                if (existingHighlight != null)
                {
                    oEntity.Id = existingHighlight.Id;
                }
            }

            if (oEntity.Id == Guid.Empty)
            {
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_company", company.ToEntityReference()));
            }

            return oEntity;
        }

        private int GetClassificationCode(string classification)
        {
            int classificationCode = -1;

            switch (classification.ToLower())
            {
                case "positive":
                    classificationCode = 206400000;
                    break;
                case "negative":
                    classificationCode = 206400002;
                    break;
                case "neutral":
                    classificationCode = 206400001;
                    break;
            }
            return classificationCode;
        }

        private int GetHighlightTypeCode(IHighlightType highlightType)
        {
            int highlightTypeCode = -1;

            Type type = highlightType.GetType();
            if (type == typeof(Age))
            {
                highlightTypeCode = 206400000;
            }
            else if (type == typeof(Address))
            {
                highlightTypeCode = 206400001;
            }
            else if (type == typeof(ChangeInManagement))
            {
                highlightTypeCode = 206400014;
            }
            else if (type == typeof(PowersToBind))
            {
                highlightTypeCode = 206400020;
            }
            else if (type == typeof(ChangeInEmployees))
            {
                highlightTypeCode = 206400017;
            }
            else if (type == typeof(IntangibleAssets))
            {
                highlightTypeCode = 206400008;
            }
            else if (type == typeof(TypeOfAuditorAssistance))
            {
                highlightTypeCode = 206400010;
            }
            else if (type == typeof(ConnectedBankruptcies))
            {
                highlightTypeCode = 206400015;
            }

            else if (type == typeof(AuditorChanges) || type == typeof(Auditors)) //auditor_changes or auditors
            {
                highlightTypeCode = 206400002;
            }
            else if (type == typeof(ChangeInProfitloss) || type == typeof(Profitloss))
            {
                highlightTypeCode = 206400003;
            }
            else if (type == typeof(ChangeInRevenue) || (type == typeof(Revenue))) //change_in_revenue or revenue
            {
                highlightTypeCode = 206400004;
            }
            else if (type == typeof(Equity))
            {
                highlightTypeCode = 206400005;
            }
            else if (type == typeof(OneFinancialStatement))
            {
                highlightTypeCode = 206400006;
            }
            else if (type == typeof(ForeignCurrency))
            {
                highlightTypeCode = 206400007;
            }
            else if (type == typeof(OldFinancialstatement))
            {
                highlightTypeCode = 206400009;
            }
            else if (type == typeof(ThreeYearsProfitloss))
            {
                highlightTypeCode = 206400011;
            }
            else if (type == typeof(TimelyDelivery))
            {
                highlightTypeCode = 206400012;
            }
            else if (type == typeof(Industry))
            {
                highlightTypeCode = 206400013;
            }
            else if (type == typeof(QuantileCompare))
            {
                highlightTypeCode = 206400016;
            }
            else if (type == typeof(CompanyType))
            {
                highlightTypeCode = 206400018;
            }
            else if (type == typeof(ProprietorshipAge))
            {
                highlightTypeCode = 206400019;
            }
            else if (type == typeof(IndustryRisk))
            {
                highlightTypeCode = 206400024; //industry_risk
            }
            else if (type == typeof(CurrentRatio))
            {
                highlightTypeCode = 206400021; //current_ratio
            }
            else if (type == typeof(ReturnOnAssets))
            {
                highlightTypeCode = 206400022; //return_on_assets
            }
            else if (type == typeof(SolidityRatio))
            {
                highlightTypeCode = 206400023; //solidity_ratio
            }


            return highlightTypeCode;
        }

        private bool DeactivateHighlight(IHighlightType highlightType, EntityCollection highlights)
        {
            bool isDeactivated = false;

            int highlightTypeCode = GetHighlightTypeCode(highlightType);
            if (highlightTypeCode > 0)
            {
                Entity existingHighlight = highlights.Entities
                    .Where(row => ((OptionSetValue)row.Attributes["nt_type"]).Value == highlightTypeCode)
                    .FirstOrDefault();

                if (existingHighlight != null)
                {
                    SetStateRequest request = new SetStateRequest
                    {
                        EntityMoniker = new EntityReference("nt_highlight", existingHighlight.Id),
                        State = new OptionSetValue(1),
                        Status = new OptionSetValue(2)
                    };

                    OrganizationService.Execute(request);
                    isDeactivated = true;
                }
            }

            return isDeactivated;
        }

    }
}
