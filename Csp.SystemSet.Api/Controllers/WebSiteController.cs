using Csp.SystemSet.Api.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Csp.SystemSet.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class WebSiteController : ControllerBase
    {
        private readonly SystemSetDbContext _systemSetDbContext;


        public WebSiteController(SystemSetDbContext systemSetDbContext)
        {
            _systemSetDbContext = systemSetDbContext;
        }

        /// <summary>
        /// 获取该租户下的站点名称
        /// </summary>
        /// <param name="tenantId">租户编号</param>
        /// <returns></returns>
        [HttpGet, Route("getdroplist/{tenantId:int}")]
        public IActionResult GetDroplist(int tenantId)
        {
            var results = _systemSetDbContext.WebSites.Where(a => a.TenantId == tenantId).Select(a => new { a.Id, a.Name });

            return Ok(results);
        }

    }
}
