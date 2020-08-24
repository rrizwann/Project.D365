using System.Runtime.Serialization;

namespace Risika.D365.Core.Models
{
    [DataContract]
    public class TokenResponse
    {
        [DataMember(Name = "token")]
        public string Token { get; set; }
    }
}
