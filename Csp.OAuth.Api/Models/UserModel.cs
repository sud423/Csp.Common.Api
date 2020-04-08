using Csp.OAuth.Api.Application;
using System.ComponentModel.DataAnnotations;

namespace Csp.OAuth.Api.Models
{
    public class UserModel
    {
        public int TenantId { get; set; }

        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "用户名最少4个字符最大16个字符")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "密码最少4个字符最大16个字符")]
        public string Password { get; set; }

        [Required(ErrorMessage = "手机号不能为空")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "手机号长度为11个字符")]
        [RegularExpression(@"^0?1[3|4|5|7|8|6|9][0-9]\d{8}$", ErrorMessage = "手机号格式不正确")]
        public string Cell { get; set; }
        
        [Required(ErrorMessage = "昵称不能为空")]
        [StringLength(50, ErrorMessage = "昵称长度为50个字符")]
        public string NickName { get; set; }
        
        [StringLength(255, ErrorMessage = "头像长度为255个字符")]
        public string HeadImgUrl { get; set; }

        public int WebSiteId { get; set; }

        public UserLogin ToUserLogin()
        {
            return new UserLogin
            {
                WebSiteId = WebSiteId,
                UserName = UserName,
                Password = PasswordHasher.Hash(Password)
            };
        }

        public User ToUser()
        {
            return new User
            {
                UserLogin = ToUserLogin(),
                Status = 1,
                Cell = Cell,
                TenantId = TenantId,
                NickName = NickName,
                HeadImgUrl = HeadImgUrl
            };
        }
    }
}
