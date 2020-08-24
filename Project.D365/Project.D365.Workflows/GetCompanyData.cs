using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Risika.D365.Core.Managers;
using Risika.D365.Core.Models.Company;
using Risika.D365.Core.Models.Rating;
using System.Activities;

namespace Risika.D365.Workflows
{
    public class GetCompanyData : WebApiActivityBase
    {
        [RequiredArgument]
        [Input("CVR")]
        public InArgument<string> CVR { get; set; }

        //[RequiredArgument]
        //[Input("Update Financial Reports")]
        //[Default("False")]
        //public InArgument<bool> UpdateFinancialReport { get; set; }

        //[RequiredArgument]
        //[Input("Update Highlights")]
        //[Default("False")]
        //public InArgument<bool> UpdateHighlights { get; set; }

        //[RequiredArgument]
        //[Input("Update Rating")]
        //[Default("False")]
        //public InArgument<bool> UpdateScores { get; set; }

        public override void Execute(CodeActivityContext activityContext, IWorkflowContext workflowContext, IOrganizationService organizationService, ITracingService tracingService)
        {
            tracingService.Trace("GetCompanies CW started");
            string baseUrl = Url.Get(activityContext);
            string accessToken = Token.Get(activityContext);
            string baseLanguage = Language.Get(activityContext);
            string country = Country.Get(activityContext);

            string cvr = CVR.Get(activityContext);
            if (!(string.IsNullOrEmpty(cvr) || string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(accessToken)))
            {
                CompanyManager companyManager = new CompanyManager(organizationService);

                tracingService.Trace("Get Company data");
                CompanyResponse companyResponse = companyManager.GetCompanyData(cvr, baseUrl, accessToken, baseLanguage, country);
                tracingService.Trace("Company data received");

                tracingService.Trace("Get Credit data");
                CreditResponse creditResponse = companyManager.GetCreditData(cvr, baseUrl, accessToken, baseLanguage, country);
                tracingService.Trace("SetOutArgument");

                this.SetOutArgument(companyResponse, creditResponse, activityContext, tracingService);
            }
        }

        private void SetOutArgument(CompanyResponse companyResponse, CreditResponse creditResponse, CodeActivityContext context, ITracingService tracingService)
        {
            if (!string.IsNullOrEmpty(companyResponse.error))
            {
                error.Set(context, companyResponse.error);
                return;
            }

            if (companyResponse.address != null)
            {
                address_coname.Set(context, companyResponse.address.coname);
                address_number.Set(context, companyResponse.address.number);
                address_street.Set(context, companyResponse.address.street);
                address_city.Set(context, companyResponse.address.city);
                address_country.Set(context, companyResponse.address.country);
                if (companyResponse.address.zipcode.HasValue)
                { address_zipcode.Set(context, companyResponse.address.zipcode.ToString()); }
                address_postdistrict.Set(context, companyResponse.address.postdistrict);
            }

            status.Set(context, companyResponse.status);
            if (companyResponse.advertisement_protection.HasValue)
            { advertisement_protection.Set(context, companyResponse.advertisement_protection.Value); }
            if (companyResponse.financial_reports.HasValue)
            { financial_reports.Set(context, companyResponse.financial_reports.Value); }

            if (companyResponse.main_industry_code != null && companyResponse.main_industry_code.code.HasValue)
            { main_industry_code.Set(context, companyResponse.main_industry_code.code.Value.ToString()); }


            main_industry_description.Set(context, companyResponse.main_industry_code.description);

            number_of_employees.Set(context, (companyResponse.number_of_employees == null) ? null : companyResponse.number_of_employees.interval);

            last_report_date.Set(context, (companyResponse.last_report_date == null) ? null : companyResponse.last_report_date);

            date_of_incorporation.Set(context, companyResponse.date_of_incorporation);

            company_type_short.Set(context, (companyResponse.company_type == null) ? null : companyResponse.company_type.@short);
            company_type_long.Set(context, (companyResponse.company_type == null) ? null : companyResponse.company_type.@long);

            score.Set(context, (companyResponse.score.HasValue) ? new OptionSetValue(companyResponse.score.Value) : null);

            webpage.Set(context, companyResponse.webpage);
            company_name.Set(context, companyResponse.company_name);

            if (companyResponse.holding_company.HasValue)
            { holding_company.Set(context, companyResponse.holding_company.Value); }

            email.Set(context, (companyResponse.email == null) ? null : companyResponse.email.email);
            phone.Set(context, (companyResponse.phone == null) ? null : companyResponse.phone.phone_number);

            if (companyResponse.hidden.HasValue)
            { hidden.Set(context, companyResponse.hidden.Value); }


            if (!string.IsNullOrEmpty(companyResponse.risk_assessment_code) && companyResponse.risk_assessment_code == "HIGH")
            { risk_assessment_codeapi.Set(context, new OptionSetValue(1)); }

            if (!string.IsNullOrEmpty(companyResponse.risk_assessment_code) && companyResponse.risk_assessment_code == "MEDIUM")
            { risk_assessment_codeapi.Set(context, new OptionSetValue(2)); }

            if (!string.IsNullOrEmpty(companyResponse.risk_assessment_code) && companyResponse.risk_assessment_code == "LOW")
            { risk_assessment_codeapi.Set(context, new OptionSetValue(3)); }

            if (string.IsNullOrEmpty(creditResponse.error))                                                                                 
            {
                if (creditResponse != null)
                {
                    if (creditResponse.credit_days.HasValue)
                        credit_days.Set(context, creditResponse.credit_days);

                    credit_max.Set(context, creditResponse.credit_max.HasValue ? new Money(creditResponse.credit_max.Value) : null);

                    upfront.Set(context, creditResponse.upfront.HasValue ? new Money(creditResponse.upfront.Value) : null);

                    if (!string.IsNullOrEmpty(creditResponse.error))
                        error.Set(context, creditResponse.error);
                }
            }
        }

        #region OutArgument

        [Output("Outputcall")]
        public OutArgument<string> outputcall { get; set; }

        [Output("Address:City")]
        public OutArgument<string> address_city { get; set; }


        [Output("Address:Coname")]
        public OutArgument<string> address_coname { get; set; }

        [Output("Address:Number")]
        public OutArgument<string> address_number { get; set; }

        [Output("Address:Street")]
        public OutArgument<string> address_street { get; set; }

        [Output("Address:Country")]
        public OutArgument<string> address_country { get; set; }

        [Output("Address:Zipcode")]
        public OutArgument<string> address_zipcode { get; set; }

        [Output("Address:Postdistrict")]
        public OutArgument<string> address_postdistrict { get; set; }

        [Output("Status")]
        public OutArgument<string> status { get; set; }

        [Output("Advertisement Protection")]
        public OutArgument<bool> advertisement_protection { get; set; }

        [Output("Financial Reports")]
        public OutArgument<bool> financial_reports { get; set; }

        [Output("Main Industry Code")]
        public OutArgument<string> main_industry_code { get; set; }

        [Output("Main Industry Description")]
        public OutArgument<string> main_industry_description{ get; set; }

        [Output("Number of employees")]
        public OutArgument<string> number_of_employees { get; set; }

        [Output("Last report date")]
        public OutArgument<string> last_report_date { get; set; }

        [Output("Date of incorporation")]
        public OutArgument<string> date_of_incorporation { get; set; }

        [Output("Company Type - short")]
        public OutArgument<string> company_type_short { get; set; }

        [Output("Company Type - long")]
        public OutArgument<string> company_type_long { get; set; }

        [Output("Score")]
        [AttributeTarget("account", "nt_score")]
        public OutArgument<OptionSetValue> score { get; set; }

        [Output("Company name")]
        public OutArgument<string> company_name { get; set; }

        [Output("Holding Company")]
        public OutArgument<bool> holding_company { get; set; }

        [Output("Email")]
        public OutArgument<string> email { get; set; }

        [Output("phone")]
        public OutArgument<string> phone { get; set; }

        [Output("Upfront")]
        public OutArgument<Money> upfront { get; set; }

        [Output("Credit Days")]
        public OutArgument<int> credit_days { get; set; }

        [Output("Credit Max")]
        public OutArgument<Money> credit_max { get; set; }

        [Output("Hidden")]
        public OutArgument<bool> hidden { get; set; }

        [Output("error")]
        public OutArgument<string> error { get; set; }

        [Output("risk_assessment_codeapi")]
        [AttributeTarget("account", "nt_risk_assessment_code")]
        public OutArgument<OptionSetValue> risk_assessment_codeapi { get; set; }

        [Output("webpage")]
        public OutArgument<string> webpage { get; set; }


        #endregion
    }
}
