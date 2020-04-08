using Csp.EF;
using System.Text.Json.Serialization;

namespace Csp.OAuth.Api.Models
{
    public class ExternalLogin : Entity
    {
        public int Id { get; set; }

        public int WebSiteId { get; set; }

        public string Provide { get; set; }

        public string OpenId { get; set; }


        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
