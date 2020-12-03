using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Csp.SystemSet.Api.Models
{
    /// <summary>
    /// Ftp信息
    /// 如果网站是部署部虚拟空间，需提供ftp相关信息
    /// </summary>
    [Owned]
    public class Ftp
    {
        /// <summary>
        /// ftp地址
        /// </summary>
        [StringLength(255,ErrorMessage ="ftp地址最大长度为255个字符")]
        public string FtpAddress { get; set; }

        /// <summary>
        /// ftp连接账号
        /// </summary>
        [StringLength(60, ErrorMessage = "ftp连接账号最大长度为60个字符")]
        public string FtpAccount { get; set; }

        /// <summary>
        /// ftp密码
        /// </summary>
        [StringLength(60, ErrorMessage = "ftp密码最大长度为60个字符")]
        public string FtpPwd { get; set; }

        /// <summary>
        /// ftp端口号
        /// </summary>
        [StringLength(6, ErrorMessage = "ftp端口号最大长度为6个字符")]
        public string FtpPort { get; set; }
    }
}
