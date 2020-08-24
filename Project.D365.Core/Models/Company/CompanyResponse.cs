using System.Collections.Generic;

namespace Risika.D365.Core.Models.Company
{
    public class LocalOrganizationId
    {
        public string country { get; set; }
        public long? id { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string coname { get; set; }
        public string number { get; set; }
        public string street { get; set; }
        public string country { get; set; }
        public int? zipcode { get; set; }
        public string postdistrict { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string description { get; set; }
        public string classification { get; set; }
        public int? weight { get; set; }
    }

    public class MainIndustryCode
    {
        public int? code { get; set; }
        public string description { get; set; }
    }

    public class SecondaryIndustryCode
    {
        public string section { get; set; }
        public int? priority { get; set; }
        public string group_name { get; set; }
        public int? industry_code { get; set; }
        public string industry_description { get; set; }
    }

    public class NumberOfEmployees
    {
        public string interval { get; set; }
        public object source { get; set; }
    }

    public class CompanyType
    {
        public string @short { get; set; }
        public string @long { get; set; }
    }

    public class RegisteredCapital
    {
        public double? value { get; set; }
        public object currency { get; set; }
    }

    public class CompanySecondaryName
    {
        public string name { get; set; }
        public string valid_to { get; set; }
        public string valid_from { get; set; }
    }

    public class Email
    {
        public string email { get; set; }
        public bool? hidden { get; set; }
    }

    public class Phone
    {
        public string phone_number { get; set; }
        public bool? hidden { get; set; }
    }

    public class Credits
    {
        public decimal upfront { get; set; }
        public int credit_days { get; set; }
        public decimal credit_max { get; set; }
    }

    public class CompanyResponse : BaseResponse
    {
        public LocalOrganizationId local_organization_id { get; set; }
        public Address address { get; set; }
        public string status { get; set; }
        public bool? advertisement_protection { get; set; }
        public bool? financial_reports { get; set; }
        public MainIndustryCode main_industry_code { get; set; }
        public List<SecondaryIndustryCode> secondary_industry_codes { get; set; }
        public NumberOfEmployees number_of_employees { get; set; }
        public string last_report_date { get; set; }
        public string date_of_incorporation { get; set; }
        public CompanyType company_type { get; set; }
        public int? score { get; set; }
        public RegisteredCapital registered_capital { get; set; }
        public string company_name { get; set; }
        public List<CompanySecondaryName> company_secondary_names { get; set; }
        public bool? holding_company { get; set; }
        public string powers_to_bind { get; set; }
        public Email email { get; set; }
        public Phone phone { get; set; }
        public Credits Credits { get; set; }
        public bool? hidden { get; set; }
        public string risk_assessment_code { get; set; }
        public string webpage { get; set; }
    }
}
