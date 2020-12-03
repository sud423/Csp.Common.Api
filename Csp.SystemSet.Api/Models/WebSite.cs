using Csp.EF;
using System.ComponentModel.DataAnnotations;

namespace Csp.SystemSet.Api.Models
{
    /// <summary>
    /// 站点信息
    /// </summary>
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
        [Required(ErrorMessage ="站点名称不能为空")]
        [StringLength(100,ErrorMessage ="站点名称最大长度为100个字符")]
        public string Name { get; set; }

        /// <summary>
        /// 状态 0=禁用 1=启用
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 网站域名
        /// </summary>
        [StringLength(100, ErrorMessage = "网站域名最大长度为100个字符")]
        public string Domain { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public int UserId { get; set; }


        public SEO SEO { get; set; }


        public Server Server { get; set; }


        public Ftp Ftp { get; set; }


        public WebSite()
        {
            SEO = new SEO();
            Server = new Server();
            Ftp = new Ftp();
        }

        public void Update(string name,string domain,SEO seo,Server server,Ftp ftp)
        {
            Name = name;
            Domain = domain;
            SEO = seo;
            Server = server;
            Ftp = ftp;
        }
    }
}
