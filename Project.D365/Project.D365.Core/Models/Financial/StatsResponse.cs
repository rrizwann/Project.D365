namespace Risika.D365.Core.Models.Financial
{
    public class Auditor
    {
        public string name { get; set; }
        public string description { get; set; }
        public string type_of_assistance { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
    }

    public class StatsResponse : BaseResponse
    {
        public Period period { get; set; }
        public string pdf_link { get; set; }
        public string type { get; set; }
        public string class_of_reporting_entity { get; set; }
        public string approval_date { get; set; }
        public string general_meeting_date { get; set; }
        public Auditor auditor { get; set; }
        public string currency { get; set; }
        public bool? ifrs { get; set; }
    }
}
