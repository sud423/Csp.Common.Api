using Csp.OAuth.Api.Application;
using Csp.OAuth.Api.Infrastructure;
using Csp.OAuth.Api.Models;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Csp.OAuth.Api.Controllers
{
    [Route("api/v1/{controller}")]
    public class UserInfoController : ControllerBase
    {
        private readonly OAuthDbContext _ctx;

        public UserInfoController(OAuthDbContext ctx)
        {
            _ctx = ctx;
        }

        [Route("changepwd")]
        public async Task<IActionResult> EditPwd([FromBody] ChangePwdModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToOptResult());

            var userLogin = await _ctx.UserLogins.SingleOrDefaultAsync(a => a.Id == model.Id);
            if (userLogin == null)
                return BadRequest(OptResult.Failed("用户不存在无法继续修改密码"));

            if(!PasswordHasher.Verify(model.OldPwd, userLogin.Password))
                return BadRequest(OptResult.Failed("旧密码不正确"));

            userLogin.SetNewPwd(model.NewPwd);

            _ctx.UserLogins.Update(userLogin);

            await _ctx.SaveChangesAsync();


            return Ok(OptResult.Success());

        }
    }
}
