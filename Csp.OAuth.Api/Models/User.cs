using Csp.EF;

namespace Csp.OAuth.Api.Models
{
    public class User : Entity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public string Cell { get; set; }
        
        public string NickName { get; set; }

        public string HeadImgUrl { get; set; }

        public byte Status { get; set; }
        
        /// <summary>
        /// 最后审核原因
        /// </summary>
        public string Audit { get; set; }


        public virtual UserLogin UserLogin { get; set; }

        public virtual UserInfo UserInfo { get; set; }

        public virtual ExternalLogin ExternalLogin { get; set; }

    }
}
