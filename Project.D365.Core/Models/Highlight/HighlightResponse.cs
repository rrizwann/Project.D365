using System.Collections.Generic;

namespace Risika.D365.Core.Models.Highlight
{
    interface IHighlightType
    {
        string title { get; set; }
        string message { get; set; }
        string description { get; set; }
        string classification { get; set; }
        int? weight { get; set; }
    }

    public class Age : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class Address : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class ChangeInManagement : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class PowersToBind : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class ChangeInEmployees : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class IntangibleAssets : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class TypeOfAuditorAssistance : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class ConnectedBankruptcies : IHighlightType
    {
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
        public object data { get; set; }
    }

    public class Data
    {
        public IList<int> value1 { get; set; }
        public IList<int> value2 { get; set; }
    }

    public class AuditorChanges : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ChangeInProfitloss : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Profitloss : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ChangeInRevenue : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Revenue : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Equity : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }

}

public class OneFinancialStatement : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ForeignCurrency : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class OldFinancialstatement : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ThreeYearsProfitloss : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class TimelyDelivery : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Industry : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class IndustryRisk : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class QuantileCompare : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class CompanyType : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ProprietorshipAge : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Auditors : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class CurrentRatio : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class ReturnOnAssets : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class SolidityRatio : IHighlightType
{
    public string title { get; set; }
    public string message { get; set; }
    public string description { get; set; }
    public string classification { get; set; }
    public int? weight { get; set; }
}

public class Highlights : BaseResponse
{
    public Age age { get; set; }
    public Address address { get; set; }
    public ChangeInManagement change_in_management { get; set; }
    public PowersToBind powers_to_bind { get; set; }
    public ChangeInEmployees change_in_employees { get; set; }
    public IntangibleAssets intangible_assets { get; set; }
    public TypeOfAuditorAssistance type_of_auditor_assistance { get; set; }
    public ConnectedBankruptcies connected_bankruptcies { get; set; }

    public AuditorChanges auditor_changes { get; set; }
    public ChangeInProfitloss change_in_profit_loss { get; set; }
    public ChangeInRevenue change_in_revenue { get; set; }
    public Equity equity { get; set; }
    public OneFinancialStatement one_financial_statement { get; set; }
    public ForeignCurrency foreign_currency { get; set; }
    public OldFinancialstatement old_financialstatement { get; set; }
    public ThreeYearsProfitloss three_years_profitloss { get; set; }
    public TimelyDelivery timely_delivery { get; set; }
    public Industry industry { get; set; }
    public QuantileCompare quantile_compare { get; set; }
    public CompanyType company_type { get; set; }
    public ProprietorshipAge proprietorship_age { get; set; }
    public Profitloss profit_loss { get; set; }
    public IndustryRisk industry_risk { get; set; }
    public CurrentRatio current_ratio { get; set; }
    public ReturnOnAssets return_on_assets { get; set; }
    public SolidityRatio solidity_ratio { get; set; }
    public Revenue revenue { get; set; }
    public Auditors auditors { get; set; }

}

public class HighlightResponse
{
    public Highlights highlights { get; set; }
}
}
