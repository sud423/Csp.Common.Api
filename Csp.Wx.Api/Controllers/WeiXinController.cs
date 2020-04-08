using Csp.Web;
using Csp.Web.Extensions;
using Csp.Wx.Api.Models;
using Csp.Wx.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Csp.Wx.Api.Controllers
{
    [Route("api/v1/wx")]
    public class WeiXinController : ControllerBase
    {
        readonly IWxService _wxService;


        public WeiXinController(IWxService wxService)
        {
            _wxService = wxService;
        }


        /// <summary>
        /// 获取微信用户同意授权地址
        /// </summary>
        /// <param name="url">回调地址</param>
        /// <param name="state">状态码，为空时值为：csp</param>
        /// <returns></returns>
        [Route("getauth"),HttpGet]
        public IActionResult GetAuthUrl(string url,string state=null)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest(OptResult.Failed("回调地址不能为空"));

            return Content(_wxService.GetAuthUrl(url, state));
        }


        [Route("getconfig"), HttpGet]
        public async Task<IActionResult> GetConfig(string url)
        {
            if (string.IsNullOrEmpty(url))
                return BadRequest(OptResult.Failed("当前页面地址不能为空"));

            return Ok(await _wxService.GetConfig(url));
        }

        /// <summary>
        /// 拉取用户信息
        /// </summary>
        /// <param name="code">授权码</param>
        /// <returns></returns>
        [Route("getsnsuser/{code}")]
        [HttpGet]
        public async Task<ActionResult<WxUser>> GetWxUser(string code)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest(OptResult.Failed("授权码不能为空"));

            return await _wxService.GetSnsApiUserInfo(code);
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="model">模板信息</param>
        /// <returns></returns>
        [HttpPost,Route("send")]
        public async Task<IActionResult> SendTemp([FromBody]TemplateMessage model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

#if DEBUG
            await Task.CompletedTask;
#else
            await _wxService.SendTempMsg(model.ToUser, model.TemplateId, model.Data, model.Url, model.Color);
#endif
            return Ok(OptResult.Success("发送成功"));
        }
    }
}
