using Csp.EF.Extensions;
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
    public class CarouselController : ControllerBase
    {
        private readonly SystemSetDbContext _systemSetDbContext;


        public CarouselController(SystemSetDbContext systemSetDbContext)
        {
            _systemSetDbContext = systemSetDbContext;
        }

        /// <summary>
        /// 获取租户下的轮播图
        /// </summary>
        /// <param name="tenantId">租户编号</param>
        /// <param name="webSiteId">站点编号</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        [HttpGet, Route("{tenantId:int}")]
        public IActionResult Index(int tenantId,int webSiteId,string name)
        {
            var predicate = PredicateExtension.True<Carousel>();

            predicate = predicate.And(a => a.TenantId == tenantId && a.Status != 0);

            if (webSiteId > 0)
                predicate = predicate.And(a => a.WebSiteId == webSiteId);

            if (!string.IsNullOrWhiteSpace(name))
                predicate = predicate.And(a => name.Contains(a.Name));

            var results = _systemSetDbContext.Carousels.Where(predicate).OrderBy(a=>new { a.Sort,a.CreatedAt });

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

            var result = await _systemSetDbContext.Carousels.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 创建或更新轮播图
        /// </summary>
        /// <param name="article">轮播图对象信息</param>
        /// <returns></returns>
        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] Carousel carousel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.ToOptResult());

            if (carousel.Id > 0)
            {
                var old = await _systemSetDbContext.Carousels.SingleOrDefaultAsync(a => a.Id == carousel.Id);
                old.Update(carousel.Name,carousel.Url,carousel.Sort);

                _systemSetDbContext.Carousels.Update(old);
            }
            else
            {
                _systemSetDbContext.Carousels.Add(carousel);
            }

            await _systemSetDbContext.SaveChangesAsync();

            return Ok(OptResult.Success());
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="id">轮播图编号</param>
        /// <returns></returns>
        [HttpPost, Route("change/{id:int}")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            if (id == 0)
                return BadRequest(OptResult.Failed("id不能小于或为0"));

            var result = await _systemSetDbContext.Carousels.SingleOrDefaultAsync(a => a.Id == id);
            if (result.Status == 1)
                result.Disabled();
            else
                result.Enabled();

            _systemSetDbContext.Carousels.Update(result);
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
            var artice = await _systemSetDbContext.Carousels.SingleOrDefaultAsync(a => a.Id == id);

            if (artice == null || artice.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            artice.Status = 0;

            _systemSetDbContext.Carousels.Update(artice);

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
            var articles = _systemSetDbContext.Carousels.Where(a => ids.Any(s => s == a.Id)).ToList();
            articles.ForEach(a =>
            {
                a.Status = 0;
            });

            //_blogDbContext.Update(articles);
            _systemSetDbContext.SaveChanges();
            return Ok();
        }
    }
}
