using System.ComponentModel.DataAnnotations;

namespace Csp.OAuth.Api.Models
{
    public class ChangePwdModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "旧密码不能为空")]
        public string OldPwd { get; set; }

        [Required(ErrorMessage ="新密码不能为空")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "密码最小长度为6个字符且不能超过18个字符")]
        [RegularExpression(@"(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).{6,18}", ErrorMessage ="密码由大小写字母开头，且必需有特殊字符及数字组成")]
        public string NewPwd { get; set; }


        [Required(ErrorMessage = "确认密码不能为空")]
        [Compare("NewPwd",ErrorMessage ="两次密码不一致")]
        public string ConfirmPwd { get; set; }
    }
}
