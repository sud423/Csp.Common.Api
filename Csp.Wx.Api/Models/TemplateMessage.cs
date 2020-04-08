using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Csp.Wx.Api.Models
{
    public class TemplateMessage
    {
        /// <summary>
        /// 接收者openid
        /// </summary>
        [Required(ErrorMessage = "{0}：不能为空")]
        public string ToUser { get; set; }

        /// <summary>
        /// 模板ID
        /// </summary>
        [Required(ErrorMessage = "{0}：不能为空")]
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板数据
        /// </summary>
        [Required(ErrorMessage = "{0}：不能为空")]
        public Dictionary<string,Dictionary<string,string>> Data { get; set; }

        /// <summary>
        /// 模板跳转链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模板内容字体颜色，不填默认为黑色
        /// </summary>
        public string Color { get; set; }
    }
}
