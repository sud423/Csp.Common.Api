using Csp.EF.Extensions;
using Csp.EF.Paging;
using Csp.SystemSet.Api.Infrastructure;
using Csp.SystemSet.Api.Models;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        /// <summary>
        /// 获取站点列表
        /// </summary>
        /// <param name="tenantId">所属租户编号</param>
        /// <param name="name">站点名称</param>
        /// <returns></returns>
        [HttpGet, Route("{tenantId:int}")]
        public async Task<IActionResult> GetWebSites(int tenantId,string name,int page,int size)
        {
            var predicate = PredicateExtension.True<WebSite>();

            predicate = predicate.And(a => a.TenantId == tenantId && a.Status);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(a => name.Contains(a.Name));

            var results = await _systemSetDbContext.WebSites.Where(predicate)
                .ToPagedAsync(page, size);

            return Ok(results);
        }


        /// <summary>
        /// 根据主键获取轮播图信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        [HttpGet, Route("find/{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            if (id == 0)
                return BadRequest(OptResult.Failed("id不能小于或为0"));

            var result = await _systemSetDbContext.WebSites.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 创建或更新轮播图
        /// </summary>
        /// <param name="article">轮播图对象信息</param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] WebSite webSite)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToOptResult());

            if (webSite.Id > 0)
            {
                var old = await _systemSetDbContext.WebSites.SingleOrDefaultAsync(a => a.Id == webSite.Id);
                
                old.Update(webSite.Name, webSite.Domain, webSite.SEO, webSite.Server, webSite.Ftp);

                _systemSetDbContext.WebSites.Update(old);
            }
            else
            {
                _systemSetDbContext.WebSites.Add(webSite);
            }

            await _systemSetDbContext.SaveChangesAsync();

            return Ok(OptResult.Success());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">根据主键删除</param>
        /// <returns></returns>
        [HttpDelete, Route("delete/{id:int}")]
        public async Task<IActionResult> Deprecated(int id)
        {
            var webSite = await _systemSetDbContext.WebSites.SingleOrDefaultAsync(a => a.Id == id);

            if (webSite == null || webSite.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            webSite.Status = false;

            _systemSetDbContext.WebSites.Update(webSite);

            await _systemSetDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());

        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">批量id列表</param>
        /// <returns></returns>
        [HttpPost, Route("delete")]
        public IActionResult Delete(IEnumerable<int> ids)
        {
            var webSites = _systemSetDbContext.WebSites.Where(a => ids.Any(s => s == a.Id)).ToList();
            webSites.ForEach(a =>
            {
                a.Status = false;
            });

            //_blogDbContext.Update(articles);
            _systemSetDbContext.SaveChanges();
            return Ok();
        }
    }
}
