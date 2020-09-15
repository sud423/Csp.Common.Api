using Csp.EF;

namespace Csp.SystemSet.Api.Models
{
    public class WebSite : Entity
    {
        /// <summary>
        /// 站点编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 租户编号
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 站点名称
        /// </summary>
        public string Name { get; set; }

    }
}
