using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Csp.SystemSet.Api.Models
{
    /// <summary>
    /// 站点部署的服务器信息
    /// </summary>
    [Owned]
    public class Server
    {
        /// <summary>
        /// 外网ip
        /// </summary>
        [StringLength(64,ErrorMessage = "外网Ip最大长度为64个字符")]
        public string Ip { get; set; }

        /// <summary>
        /// 内网IP
        /// </summary>
        [StringLength(64, ErrorMessage = "内网IP最大长度为64个字符")]
        public string IntranetIp { get; set; }

        /// <summary>
        /// 远程登录账号
        /// </summary>
        [StringLength(60, ErrorMessage = "远程登录账号最大长度为60个字符")]
        public string RemoteAccount { get; set; }

        /// <summary>
        /// 远程登录密码
        /// </summary>
        [StringLength(60, ErrorMessage = "远程登录密码最大长度为60个字符")]
        public string RemotePwd { get; set; }

        /// <summary>
        /// 远程登录端口号
        /// </summary>
        [StringLength(6, ErrorMessage = "远程登录端口号最大长度为6个字符")]
        public string Port { get; set; }

        /// <summary>
        /// 服务器操作系统
        /// </summary>
        [StringLength(30, ErrorMessage = "服务器操作系统最大长度为30个字符")]
        public string Os { get; set; }
    }
}
