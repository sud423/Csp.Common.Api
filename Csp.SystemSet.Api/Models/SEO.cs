using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Csp.SystemSet.Api.Models
{
    /// <summary>
    /// 站点优化收录信息
    /// </summary>
    [Owned]
    public class SEO
    {
        /// <summary>
        /// 网页标题
        /// </summary>
        [StringLength(255,ErrorMessage = "网页标题最大长度为255个字符")]
        public string Title { get; set; }

        /// <summary>
        /// 网页关键字
        /// </summary>
        [StringLength(255, ErrorMessage = "网页关键字最大长度为255个字符")]
        public string Keyword { get; set; }

        /// <summary>
        /// 网页描述
        /// </summary>
        [StringLength(255, ErrorMessage = "网页描述最大长度为255个字符")]
        public string Descript { get; set; }
    }
}
