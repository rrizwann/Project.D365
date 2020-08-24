namespace Risika.D365.Core.Models.Financial
{
    public class NumberResponse : BaseResponse
    {
        public Period period { get; set; }
        public decimal? revenue { get; set; }
        public decimal? other_income { get; set; }
        public decimal? costs { get; set; }
        public decimal? gross_result { get; set; }
        public decimal? operating_costs { get; set; }
        public decimal? other_operating_income { get; set; }
        public decimal? staff_expenses { get; set; }
        public decimal? ebitda { get; set; }
        public decimal? depreciation { get; set; }
        public decimal? ebit { get; set; }
        public decimal? interest_income { get; set; }
        public decimal? interest_expenses { get; set; }
        public decimal? other_net_financial_income { get; set; }
        public decimal? net_financial_income { get; set; }
        public decimal? ordinary_profit { get; set; }
        public decimal? extraordinary_item { get; set; }
        public decimal? profit_loss_before_tax { get; set; }
        public decimal? tax_expenses { get; set; }
        public decimal? profit_loss { get; set; }
        public decimal? goodwill { get; set; }
        public decimal? other_intangible_assets { get; set; }
        public decimal? intangible_assets { get; set; }
        public decimal? land_and_buildings { get; set; }
        public decimal? plant_equipment_and_fixtures { get; set; }
        public decimal? other_property_plant_and_equipment { get; set; }
        public decimal? property_plant_and_equipment { get; set; }
        public decimal? noncurrent_receivables { get; set; }
        public decimal? noncurrent_investments { get; set; }
        public decimal? other_noncurrent_financial_assets { get; set; }
        public decimal? noncurrent_financial_assets { get; set; }
        public decimal? noncurrent_assets { get; set; }
        public decimal? inventories { get; set; }
        public decimal? current_prepayments { get; set; }
        public decimal? short_term_receivables_from_sales_and_services { get; set; }
        public decimal? short_term_receivables_from_group_enterprises { get; set; }
        public decimal? other_short_term_receivables { get; set; }
        public decimal? short_term_receivables { get; set; }
        public decimal? current_financial_assets { get; set; }
        public decimal? cash { get; set; }
        public decimal? current_assets { get; set; }
        public decimal? assets { get; set; }
        public decimal? contributed_capital { get; set; }
        public decimal? reserves { get; set; }
        public decimal? dividend { get; set; }
        public decimal? retained_earnings { get; set; }
        public decimal? equity_before_minority_interests { get; set; }
        public decimal? minority_interests { get; set; }
        public decimal? equity { get; set; }
        public decimal? provisions { get; set; }
        public decimal? long_term_debt_to_group_enterprises { get; set; }
        public decimal? long_term_debt_to_banks { get; set; }
        public decimal? long_term_mortgage_debt { get; set; }
        public decimal? equity_loan { get; set; }
        public decimal? deferred_tax { get; set; }
        public decimal? other_long_term_debt { get; set; }
        public decimal? long_term_debt { get; set; }
        public decimal? short_term_debt_to_group_enterprises { get; set; }
        public decimal? short_term_debt_to_banks { get; set; }
        public decimal? short_term_mortgage_debt { get; set; }
        public decimal? short_term_trade_payables { get; set; }
        public decimal? short_term_tax_payables { get; set; }
        public decimal? other_short_term_debt { get; set; }
        public decimal? short_term_debt { get; set; }
        public decimal? debt { get; set; }
        public decimal? liabilities_and_equity { get; set; }
    }

    public class Period
    {
        public string start { get; set; }
        public string end { get; set; }
    }
}
