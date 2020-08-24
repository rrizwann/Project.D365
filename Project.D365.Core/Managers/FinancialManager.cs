using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Risika.D365.Core.Models.Financial;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.D365.Core.Managers
{
    public class FinancialManager : BaseManager
    {
        public FinancialManager(IOrganizationService service)
            : base(service)
        {

        }

        public FinancialManager(IOrganizationService service, ITracingService tracingService)
            : base(service, tracingService)
        {

        }

        public void CopyFinancials(EntityReference companyid, string cvr, string baseUrl, string accessToken, string baseLanguage, string country)
        {
            Entity company = new CompanyManager(OrganizationService).GetCompany(companyid);
            if (company != null)
            {
                this.CopyStats(cvr, baseUrl, accessToken, baseLanguage, company,country);
                this.CopyNumbers(cvr, baseUrl, accessToken, baseLanguage, company, country);
            }
            this.SetCompanyFinancial(company);
        }

        private void CopyStats(string cvr, string baseUrl, string accessToken, string baseLanguage, Entity company,string country)
        {
            EntityCollection financials = this.GetFinancials(company.Id);

            OrganizationRequestCollection requests = new OrganizationRequestCollection();

            IList<StatsResponse> statsResponses = new RisikaServiceManager(OrganizationService)
                .GetFinancialStats(cvr, baseUrl, accessToken, baseLanguage, country);
            foreach (StatsResponse statsResponse in statsResponses)
            {
                if (statsResponse != null && statsResponse.period != null)
                {
                    Period period = statsResponse.period;
                    if (!(string.IsNullOrEmpty(period.start) || string.IsNullOrEmpty(period.end)))
                    {
                        Entity oEntity = this.GetEntityObject(statsResponse, financials, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
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

        private void CopyNumbers(string cvr, string baseUrl, string accessToken, string baseLanguage, Entity company,string country)
        {
            EntityCollection financials = this.GetFinancials(company.Id);

            OrganizationRequestCollection requests = new OrganizationRequestCollection();

            IList<NumberResponse> financialResponses = new RisikaServiceManager(OrganizationService)
                .GetFinancialNumbers(cvr, baseUrl, accessToken, baseLanguage,country);
            foreach (NumberResponse financialResponse in financialResponses)
            {
                if (financialResponse != null && financialResponse.period != null)
                {
                    Period period = financialResponse.period;
                    if (!(string.IsNullOrEmpty(period.start) || string.IsNullOrEmpty(period.end)))
                    {
                        Entity oEntity = this.GetEntityObject(financialResponse, financials, company);
                        requests.Add(this.GetOrganizationRequest(oEntity));
                    }
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

        public EntityCollection GetFinancials(Guid accountid)
        {
            string query = string.Format(@"<fetch no-lock=""true"" >
                              <entity name=""nt_financialreport"" >
                                <all-attributes/>
                                <filter>
                                  <condition attribute=""nt_company"" operator=""eq"" value=""{0}"" />
                                  <condition attribute=""nt_periodstart"" operator=""not-null"" />
                                  <condition attribute=""nt_periodend"" operator=""not-null"" />
                                </filter>
                              </entity>
                            </fetch>", accountid);

            EntityCollection collection = OrganizationService.RetrieveMultiple(new FetchExpression(query));
            return collection;
        }

        private Entity GetEntityObject(StatsResponse statsResponse, EntityCollection scores, Entity company)
        {
            Entity oEntity = new Entity("nt_financialreport");

            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_pdflink", statsResponse.pdf_link));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_type", statsResponse.type));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_class_of_reporting_entity", statsResponse.class_of_reporting_entity));

            DateTime? approvalDate = null;
            if (!string.IsNullOrEmpty(statsResponse.approval_date))
            {
                DateTime approval;
                if (DateTime.TryParse(statsResponse.approval_date, out approval))
                {
                    approvalDate = DateTime.SpecifyKind(approval, DateTimeKind.Utc);
                }
            }

            DateTime? general_meeting_date = null;
            if (!string.IsNullOrEmpty(statsResponse.general_meeting_date))
            {
                DateTime meeting_date;
                if (DateTime.TryParse(statsResponse.approval_date, out meeting_date))
                {
                    general_meeting_date = DateTime.SpecifyKind(meeting_date, DateTimeKind.Utc);
                }
            } 
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_approvaldate", approvalDate));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_general_meeting_date", general_meeting_date));

            if (statsResponse.auditor != null)
            {
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditor", statsResponse.auditor.name));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditor_description", statsResponse.auditor.description));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_audittype", statsResponse.auditor.type_of_assistance));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_company_id", statsResponse.auditor.company_id));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditorcompany", statsResponse.auditor.company_name));
            }
            else
            {
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditor", null));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditor_description", null));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_audittype", null));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_company_id", null));
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_auditorcompany", null));
            }

            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_currency", statsResponse.currency));
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_ifrs", statsResponse.ifrs)); 

            DateTime start, end;
            if (DateTime.TryParse(statsResponse.period.start, out start) &&
                DateTime.TryParse(statsResponse.period.end, out end))
            {
                Entity existingFinancial = scores.Entities
                    .Where(row => ((DateTime)row.Attributes["nt_periodstart"]).Date == start.Date)
                    .Where(row => ((DateTime)row.Attributes["nt_periodend"]).Date == end.Date)
                    .FirstOrDefault();

                if (existingFinancial != null)
                {
                    oEntity.Id = existingFinancial.Id;
                }
                else
                {
                    DateTime periodStart = DateTime.SpecifyKind(start, DateTimeKind.Utc);
                    oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_periodstart", periodStart));

                    DateTime periodEnd = DateTime.SpecifyKind(end, DateTimeKind.Utc);
                    oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_periodend", periodEnd)); 
                }
            }

            if (oEntity.Id == Guid.Empty)
            {
                string name = company.Attributes.ContainsKey("name") ? company.Attributes["name"].ToString() : null;
                if (start != default(DateTime))
                {
                    name += ((string.IsNullOrEmpty(name) ? null : " - ") + start.Year.ToString());
                } 
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_name", name));

                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_company", company.ToEntityReference()));
            }

            return oEntity;
        }

        private Entity GetEntityObject(NumberResponse financialResponse, EntityCollection scores, Entity company)
        {
            Entity oEntity = new Entity("nt_financialreport");

            Money revenue = financialResponse.revenue.HasValue ? new Money(financialResponse.revenue.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_revenue", revenue));

            Money other_income = financialResponse.other_income.HasValue ? new Money(financialResponse.other_income.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_otherincome", other_income));

            Money costs = financialResponse.costs.HasValue ? new Money(financialResponse.costs.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_costs", costs));

            Money gross_result = financialResponse.gross_result.HasValue ? new Money(financialResponse.gross_result.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_grossresult", gross_result));

            Money operating_costs = financialResponse.operating_costs.HasValue ? new Money(financialResponse.operating_costs.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_operatingcosts", operating_costs));

            Money other_operating_income = financialResponse.other_operating_income.HasValue ? new Money(financialResponse.other_operating_income.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_otheroperatingcosts", other_operating_income));

            Money staff_expenses = financialResponse.staff_expenses.HasValue ? new Money(financialResponse.staff_expenses.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_staffexpenses", staff_expenses));

            Money ebitda = financialResponse.ebitda.HasValue ? new Money(financialResponse.ebitda.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_ebitda", ebitda));

            Money depreciation = financialResponse.depreciation.HasValue ? new Money(financialResponse.depreciation.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_depreciation", depreciation));

            Money ebit = financialResponse.ebit.HasValue ? new Money(financialResponse.ebit.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_ebit", ebit));

            Money interest_income = financialResponse.interest_income.HasValue ? new Money(financialResponse.interest_income.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_interestincome", interest_income));

            Money interest_expenses = financialResponse.interest_expenses.HasValue ? new Money(financialResponse.interest_expenses.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_interestexpenses", interest_expenses));

            Money other_net_financial_income = financialResponse.other_net_financial_income.HasValue ? new Money(financialResponse.other_net_financial_income.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_othernetfinancialincome", other_net_financial_income));

            Money net_financial_income = financialResponse.net_financial_income.HasValue ? new Money(financialResponse.net_financial_income.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_netfinancialincome", net_financial_income));

            Money ordinary_profit = financialResponse.ordinary_profit.HasValue ? new Money(financialResponse.ordinary_profit.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_ordinary_profit", ordinary_profit));

            Money extraordinary_item = financialResponse.extraordinary_item.HasValue ? new Money(financialResponse.extraordinary_item.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_extraordinary_item", extraordinary_item));

            Money profit_loss_before_tax = financialResponse.profit_loss_before_tax.HasValue ? new Money(financialResponse.profit_loss_before_tax.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_profit_loss_before_tax", profit_loss_before_tax));

            Money tax_expenses = financialResponse.tax_expenses.HasValue ? new Money(financialResponse.tax_expenses.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_tax_expenses", new Money(financialResponse.tax_expenses.Value)));

            Money profit_loss = financialResponse.profit_loss.HasValue ? new Money(financialResponse.profit_loss.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_profit_loss", profit_loss));

            Money goodwill = financialResponse.goodwill.HasValue ? new Money(financialResponse.goodwill.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_goodwill", goodwill));

            Money other_intangible_assets = financialResponse.other_intangible_assets.HasValue ? new Money(financialResponse.other_intangible_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_intangible_assets", other_intangible_assets));

            Money intangible_assets = financialResponse.intangible_assets.HasValue ? new Money(financialResponse.intangible_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_intangible_assets", intangible_assets));

            Money land_and_buildings = financialResponse.land_and_buildings.HasValue ? new Money(financialResponse.land_and_buildings.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_land_and_buildings", land_and_buildings));

            Money plant_equipment_and_fixtures = financialResponse.plant_equipment_and_fixtures.HasValue ? new Money(financialResponse.plant_equipment_and_fixtures.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_plant_equipment_and_fixtures", plant_equipment_and_fixtures));

            Money other_property_plant_and_equipment = financialResponse.other_property_plant_and_equipment.HasValue ? new Money(financialResponse.other_property_plant_and_equipment.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_property_plant_and_equipment", other_property_plant_and_equipment));

            Money property_plant_and_equipment = financialResponse.property_plant_and_equipment.HasValue ? new Money(financialResponse.property_plant_and_equipment.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_property_plant_and_equipment", property_plant_and_equipment));

            Money noncurrent_receivables = financialResponse.noncurrent_receivables.HasValue ? new Money(financialResponse.noncurrent_receivables.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_noncurrent_receivables", noncurrent_receivables));

            Money noncurrent_investments = financialResponse.noncurrent_investments.HasValue ? new Money(financialResponse.noncurrent_investments.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_noncurrent_investments", noncurrent_investments));

            Money other_noncurrent_financial_assets = financialResponse.other_noncurrent_financial_assets.HasValue ? new Money(financialResponse.other_noncurrent_financial_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_noncurrent_financial_assets", other_noncurrent_financial_assets));

            Money noncurrent_financial_assets = financialResponse.noncurrent_financial_assets.HasValue ? new Money(financialResponse.noncurrent_financial_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_noncurrent_financial_assets", noncurrent_financial_assets));

            Money noncurrent_assets = financialResponse.noncurrent_assets.HasValue ? new Money(financialResponse.noncurrent_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_noncurrent_assets", noncurrent_assets));

            Money inventories = financialResponse.inventories.HasValue ? new Money(financialResponse.inventories.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_inventories", inventories));

            Money current_prepayments = financialResponse.current_prepayments.HasValue ? new Money(financialResponse.current_prepayments.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_current_prepayments", current_prepayments));

            Money short_term_receivables_from_sales_and_services = financialResponse.short_term_receivables_from_sales_and_services.HasValue ? new Money(financialResponse.short_term_receivables_from_sales_and_services.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_receivables_from_sales_and_ser", short_term_receivables_from_sales_and_services));

            Money short_term_receivables_from_group_enterprises = financialResponse.short_term_receivables_from_group_enterprises.HasValue ? new Money(financialResponse.short_term_receivables_from_group_enterprises.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_receivables_from_group_enterpr", short_term_receivables_from_group_enterprises));

            Money other_short_term_receivables = financialResponse.other_short_term_receivables.HasValue ? new Money(financialResponse.other_short_term_receivables.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_short_term_receivables", other_short_term_receivables));

            Money short_term_receivables = financialResponse.short_term_receivables.HasValue ? new Money(financialResponse.short_term_receivables.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_receivables", short_term_receivables));

            Money current_financial_assets = financialResponse.current_financial_assets.HasValue ? new Money(financialResponse.current_financial_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_current_financial_assets", current_financial_assets));

            Money cash = financialResponse.cash.HasValue ? new Money(financialResponse.cash.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_cash", cash));

            Money current_assets = financialResponse.current_assets.HasValue ? new Money(financialResponse.current_assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_current_assets", current_assets));

            Money assets = financialResponse.assets.HasValue ? new Money(financialResponse.assets.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_assets", assets));

            Money contributed_capital = financialResponse.contributed_capital.HasValue ? new Money(financialResponse.contributed_capital.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_contributed_capital", contributed_capital));

            Money reserves = financialResponse.reserves.HasValue ? new Money(financialResponse.reserves.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_reserves", reserves));

            Money dividend = financialResponse.dividend.HasValue ? new Money(financialResponse.dividend.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_dividend", dividend));

            Money retained_earnings = financialResponse.retained_earnings.HasValue ? new Money(financialResponse.retained_earnings.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_retained_earnings", retained_earnings));

            Money equity_before_minority_interests = financialResponse.equity_before_minority_interests.HasValue ? new Money(financialResponse.equity_before_minority_interests.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_equity_before_minority_interests", equity_before_minority_interests));

            Money minority_interests = financialResponse.minority_interests.HasValue ? new Money(financialResponse.minority_interests.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_minority_interests", minority_interests));

            Money equity = financialResponse.equity.HasValue ? new Money(financialResponse.equity.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_equity", equity));

            Money provisions = financialResponse.provisions.HasValue ? new Money(financialResponse.provisions.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_provisions", provisions));

            Money long_term_debt_to_group_enterprises = financialResponse.long_term_debt_to_group_enterprises.HasValue ? new Money(financialResponse.long_term_debt_to_group_enterprises.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_long_term_debt_to_group_enterprises", long_term_debt_to_group_enterprises));

            Money long_term_debt_to_banks = financialResponse.long_term_debt_to_banks.HasValue ? new Money(financialResponse.long_term_debt_to_banks.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_long_term_debt_to_banks", long_term_debt_to_banks));

            Money long_term_mortgage_debt = financialResponse.long_term_mortgage_debt.HasValue ? new Money(financialResponse.long_term_mortgage_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_long_term_mortgage_debt", long_term_mortgage_debt));

            Money equity_loan = financialResponse.equity_loan.HasValue ? new Money(financialResponse.equity_loan.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_equity_loan", equity_loan));

            Money deferred_tax = financialResponse.deferred_tax.HasValue ? new Money(financialResponse.deferred_tax.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_deferred_tax", deferred_tax));

            Money other_long_term_debt = financialResponse.other_long_term_debt.HasValue ? new Money(financialResponse.other_long_term_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_long_term_debt", other_long_term_debt));

            Money long_term_debt = financialResponse.long_term_debt.HasValue ? new Money(financialResponse.long_term_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_long_term_debt", long_term_debt));

            Money short_term_debt_to_group_enterprises = financialResponse.short_term_debt_to_group_enterprises.HasValue ? new Money(financialResponse.short_term_debt_to_group_enterprises.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_debt_to_group_enterprises", short_term_debt_to_group_enterprises));

            Money short_term_debt_to_banks = financialResponse.short_term_debt_to_banks.HasValue ? new Money(financialResponse.short_term_debt_to_banks.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_debt_to_banks", short_term_debt_to_banks));

            Money short_term_mortgage_debt = financialResponse.short_term_mortgage_debt.HasValue ? new Money(financialResponse.short_term_mortgage_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_mortgage_debt", short_term_mortgage_debt));

            Money short_term_trade_payables = financialResponse.short_term_trade_payables.HasValue ? new Money(financialResponse.short_term_trade_payables.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_trade_payables", short_term_trade_payables));

            Money short_term_tax_payables = financialResponse.short_term_tax_payables.HasValue ? new Money(financialResponse.short_term_tax_payables.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_tax_payables", short_term_tax_payables));

            Money other_short_term_debt = financialResponse.other_short_term_debt.HasValue ? new Money(financialResponse.other_short_term_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_other_short_term_debt", other_short_term_debt));

            Money short_term_debt = financialResponse.short_term_debt.HasValue ? new Money(financialResponse.short_term_debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_short_term_debt", short_term_debt));

            Money debt = financialResponse.debt.HasValue ? new Money(financialResponse.debt.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_debt", debt));

            Money liabilities_and_equity = financialResponse.liabilities_and_equity.HasValue ? new Money(financialResponse.liabilities_and_equity.Value) : null;
            oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_liabilities_and_equity", liabilities_and_equity));

            DateTime start, end;
            if (DateTime.TryParse(financialResponse.period.start, out start) &&
                DateTime.TryParse(financialResponse.period.end, out end))
            {
                Entity existingFinancial = scores.Entities
                    .Where(row => ((DateTime)row.Attributes["nt_periodstart"]).Date == start.Date)
                    .Where(row => ((DateTime)row.Attributes["nt_periodend"]).Date == end.Date)
                    .FirstOrDefault();

                if (existingFinancial != null)
                {
                    oEntity.Id = existingFinancial.Id;
                }
                else
                {
                    DateTime periodStart = DateTime.SpecifyKind(start, DateTimeKind.Utc);
                    oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_periodstart", periodStart));

                    DateTime periodEnd = DateTime.SpecifyKind(end, DateTimeKind.Utc);
                    oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_periodend", periodEnd));
                }
            }

            if (oEntity.Id == Guid.Empty)
            {
                string name = company.Attributes.ContainsKey("name") ? company.Attributes["name"].ToString() : null;
                if (start != default(DateTime))
                {
                    name += ((string.IsNullOrEmpty(name) ? null : " - ") + start.Year.ToString());
                }
                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_name", name));

                oEntity.Attributes.Add(new KeyValuePair<string, object>("nt_company", company.ToEntityReference()));
            }

            return oEntity;
        }

        private void SetCompanyFinancial(Entity company)
        {
            EntityReference financialReport = null;
            if (company.Attributes.ContainsKey("nt_financialreport"))
            {
                financialReport = company.Attributes["nt_financialreport"] as EntityReference;
            }

            Entity financial = this.GetFinancials(company.Id).Entities
                .OrderByDescending(row => row.Attributes["nt_periodend"])
                .FirstOrDefault(); 
            if (financial != null && (financialReport == null || financialReport.Id != financial.Id))
            {
                Entity oCompany = new Entity(company.LogicalName, company.Id);
                oCompany.Attributes.Add(new KeyValuePair<string, object>("nt_financialreport", financial.ToEntityReference()));
                OrganizationService.Update(oCompany);
            }
            else
            {
                Entity oCompany = new Entity(company.LogicalName, company.Id);
                string Error = "Dette firma har ikke indrapporterede regnskaber. Score kan ikke beregnes";
                if (financialReport == null)
                {
                    oCompany.Attributes.Add("nt_error", Error);
                    OrganizationService.Update(oCompany);
                }
            }
        }
    }
}
