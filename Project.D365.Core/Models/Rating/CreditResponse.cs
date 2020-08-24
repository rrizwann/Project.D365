namespace Risika.D365.Core.Models.Rating
{
    public class CreditResponse : BaseResponse
    {
        public decimal? upfront { get; set; }
        public int? credit_days { get; set; }
        public int? credit_max { get; set; }
        
    }
}
