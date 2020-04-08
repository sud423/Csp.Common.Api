using Csp.Jwt;
using Csp.OAuth.Api.Application;
using Csp.OAuth.Api.Infrastructure;
using Csp.OAuth.Api.Models;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Csp.OAuth.Api.Controllers
{
    [Route("api/v1/account")]
    public class AccountController : ControllerBase
    {
        private readonly OAuthDbContext _ctx;
        private readonly IWxService _wxService;
        readonly ILogger<AccountController> _logger;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AccountController(OAuthDbContext ctx, IWxService wxService, ILogger<AccountController> logger, IJwtTokenGenerator jwtTokenGenerator)
        {
            _ctx = ctx;
            _wxService = wxService;
            _logger = logger;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [Route("sigin")]
        [HttpPost]
        public async Task<IActionResult> SigIn([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            var user = await _ctx.Users.Include(a => a.UserLogin).SingleOrDefaultAsync(a => a.UserLogin.UserName == model.UserName);

            if (user == null || !PasswordHasher.Verify(model.Password, user.UserLogin?.Password))
                return BadRequest(OptResult.Failed("用户名和密码不正确"));

            var accessTokenResult = _jwtTokenGenerator.GenerateAccessTokenWithClaimsPrincipal(model.UserName, AddMyClaims(user));

            return Ok(accessTokenResult.AccessToken);
        }

        /// <summary>
        /// 根据用户名和密码查询
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("signin"), HttpPost]
        public async Task<IActionResult> SignInByPassword([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            var user = await _ctx.Users.Include(a=>a.UserLogin).SingleOrDefaultAsync(a => a.UserLogin.UserName == model.UserName && a.TenantId== model.TenantId);// && a.UserLogin.WebSiteId==model.WebSiteId
            if (user == null)
                return BadRequest(OptResult.Failed("用户名不存在"));

            if (!PasswordHasher.Verify(model.Password, user.UserLogin?.Password))
                return BadRequest(OptResult.Failed("密码不正确"));

            //记录登录信息

            return Ok(user);
        }

        /// <summary>
        /// 第三方登录
        /// </summary>
        /// <param name="code">第三方登录授权码</param>
        /// <param name="tenantId">租户编号</param>
        /// <returns></returns>
        [HttpPost,Route("wxlogin/{tenantId:int}/{webSiteId:int}/{code}")]
        public async Task<IActionResult> SignInByProvide(string code,int tenantId,int webSiteId)
        {
            var login =await _wxService.GetLogin(code,tenantId,webSiteId);
            if (login == null)
                return BadRequest(OptResult.Failed("授权码无效"));

            var user= await _ctx.Users.Include(a => a.ExternalLogin).SingleOrDefaultAsync(a => a.ExternalLogin.OpenId == login.ExternalLogin.OpenId);

            if(user == null)
            {
                await _ctx.Users.AddAsync(login);

                await _ctx.SaveChangesAsync();
            }
            else
            {
                //记录登录信息
                _logger.LogInformation($"{user.ExternalLogin?.OpenId}-{DateTimeOffset.UtcNow.LocalDateTime} 登录成功");
            }
            return Ok(user);
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("create"), HttpPost]
        public async Task<IActionResult> Create([FromBody]UserModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            var user =await _ctx.Users.Include(a => a.UserLogin).SingleOrDefaultAsync(a => a.Cell == model.Cell);

            if (user!=null && user.UserLogin != null && user.UserLogin.UserName == model.UserName)
                return BadRequest(OptResult.Failed("该用户已注册"));

            var userLogin = model.ToUserLogin();

            if (user == null)
            {
                user = model.ToUser();

                _ctx.Users.Add(user);
            }
            else
            {
                user.UserLogin = model.ToUserLogin();

                _ctx.Users.Update(user);

            }

            await _ctx.SaveChangesAsync();

            return Ok(OptResult.Success());
        }

        [HttpPut, Route("bind/{userId:int}/{cell}")]
        public async Task<IActionResult> BindCell(int userId,string cell)
        {
            if (string.IsNullOrEmpty(cell))
                return BadRequest(OptResult.Failed("手机号不能为空"));

            var user = await _ctx.Users.SingleOrDefaultAsync(a => a.Id==userId);
            user.Cell = cell;

            _ctx.Users.Update(user);

            await _ctx.SaveChangesAsync();

            return Ok(OptResult.Success());
        }

        private IEnumerable<Claim> AddMyClaims(User user)
        {
            var myClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserInfo?.Name??""),
                //new Claim(ClaimTypes.Role,dto.Role.ToString()),
                new Claim(ClaimTypes.Email,user.UserInfo?.Email??""),
                new Claim(ClaimTypes.MobilePhone,user.UserInfo?.Cell??""),
                new Claim(ClaimTypes.NameIdentifier,user.NickName??""),
                new Claim(ClaimTypes.GroupSid,$"{user.TenantId}"),
                new Claim(ClaimTypes.Sid,$"{user.Id}"),
                new Claim("HeadImgUrl",user.HeadImgUrl??""),
                new Claim("OpenId",user.ExternalLogin?.OpenId??""),
                new Claim("aud", "OAuth"),
                new Claim("aud", "Blog"),
                new Claim("aud", "Upload"),
                new Claim("aud", "AskApi"),
                new Claim("aud", "AskWeb"),
                new Claim("aud", "MtWeb")
            };

            return myClaims;
        }
    }
}
