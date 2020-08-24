using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Risika.D365.Core.Models.Company
{
    [DataContract]
    public class CompanyParam
    {
        [DataMember(Name = "mode")]
        public string Mode { get; set; }
        [DataMember(Name = "filters")]
        public CompanyFilter Filters { get; set; }
    }

    [DataContract]
    public class CompanyFilter
    {
        [DataMember(Name = "company_name")]
        public string Name { get; set; }
        [DataMember(Name = "active")]
        public bool Active { get; set; }
    }

    public class SearchResult
    {
        public LocalOrganizationId local_organization_id { get; set; }
        public bool active { get; set; }
        public string company_name { get; set; }
        public int? score { get; set; }
        public string company_type { get; set; }
        public string employees_interval { get; set; }
       // public bool advertisement_protected { get; set; } = false;
        public Address address { get; set; }
    }

    public class Rows
    {
        public int from { get; set; }
        public int to { get; set; }
    }

    public class SearchResponse
    {
        public List<SearchResult> search_result { get; set; }
        public Rows rows { get; set; }
        public int count { get; set; }
    }
}
