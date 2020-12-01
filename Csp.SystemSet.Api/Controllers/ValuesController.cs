using Csp.SystemSet.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Csp.SystemSet.Api.Controllers
{
    [Route("api/v1")]
    public class ValuesController : ControllerBase
    {
        private readonly SystemSetDbContext _systemSetDbContext;


        public ValuesController(SystemSetDbContext systemSetDbContext)
        {
            _systemSetDbContext = systemSetDbContext;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <returns></returns>
        [HttpGet, Route("carousel/{tenantId:int}/{webSiteId:int}")]
        public async Task<IActionResult> GetCarousels(int tenantId, int webSiteId)
        {
            var result = await _systemSetDbContext.Carousels
                .Where(a => a.TenantId == tenantId && a.Status == 1
                && webSiteId == a.WebSiteId)
                .OrderBy(a => a.Sort)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();

            return Ok(result);
        }
    }
}
