using System.Text.Json.Serialization;

namespace Csp.Blog.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public int TenantId { get; set; }


        public string NickName { get; set; }

        public string HeadImgUrl { get; set; }

        public virtual ExternalLogin ExternalLogin { get; set; }

        public virtual UserLogin UserLogin { get; set; }

        [JsonIgnore]
        public virtual Reply Reply { get; set; }

        [JsonIgnore]
        public virtual Article Article { get; set; }

        [JsonIgnore]
        public virtual Resource Resource { get; set; }

    }

    public class ExternalLogin
    {
        public int Id { get; set; }

        public int WebSiteId { get; set; }

        public string OpenId { get; set; }


        [JsonIgnore]
        public virtual User User { get; set; }

    }

    public class UserLogin
    {
        public int Id { get; set; }
        public int WebSiteId { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
