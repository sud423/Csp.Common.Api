using Csp.Blog.Api.Infrastructure;
using Csp.Blog.Api.Models;
using Csp.EF.Paging;
using Csp.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Csp.Blog.Api.Controllers
{
    [Route("api/v1")]
    public class ValuesController : ControllerBase
    {
        private readonly BlogDbContext _blogDbContext;
        public ValuesController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="tenantId">租户</param>
        /// <returns></returns>
        [HttpGet,Route("categories/{tenantId:int}/{webSiteId:int}/{type}")]
        public async Task<IActionResult> GetCategories(int tenantId,int webSiteId,string type)
        {
            var result = await _blogDbContext.Categories
                .Where(a => a.TenantId == tenantId && a.Status == 1 
                && webSiteId == a.WebSiteId 
                && a.Type==type)
                .OrderBy(a => a.Sort)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet, Route("categories/hot/{tenantId:int}/{webSiteId:int}/{type}")]
        public async Task<IActionResult> GetHotCategories(int tenantId, int webSiteId, string type)
        {
            var result = await _blogDbContext.Categories
                .Where(a => a.TenantId == tenantId && a.Status == 1
                && webSiteId == a.WebSiteId
                && a.Type == type
                && a.IsHot)
                .OrderBy(a => a.Sort)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取分类信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        [HttpGet, Route("categories/find/{id:int}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            if (id == 0)
                return BadRequest(OptResult.Failed("id不能小于或为0"));

            var result = await _blogDbContext.Categories.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="page">查询页码</param>
        /// <returns></returns>
        [HttpGet, Route("articles/{tenantId:int}/{categoryId:int}/{webSiteId:int}")]
        public async Task<IActionResult> GetArticles(int tenantId, int categoryId, int webSiteId)
        {
            var result = await _blogDbContext.Articles
                .Where(a => a.TenantId == tenantId && a.Status == 1 && categoryId == a.CategoryId && (a.WebSiteId == 0 || a.WebSiteId == webSiteId))
                .OrderBy(a => a.Sort)
                .ThenByDescending(a=>a.CreatedAt)
                .ToListAsync();

            return Ok(result);
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="page">查询页码</param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet, Route("articles/{tenantId:int}/{categoryId:int}/{webSiteId:int}/{size:int}")]
        public async Task<IActionResult> GetArticles(int tenantId,int categoryId, int webSiteId, int size)
        {
            var result = await _blogDbContext.Articles
                .Where(a => a.TenantId == tenantId && a.Status == 1 && categoryId==a.CategoryId && (a.WebSiteId==0 || a.WebSiteId==webSiteId))
                .OrderBy(a => a.Sort)
                .ThenByDescending(a => a.CreatedAt)
                .Take(size)
                .ToListAsync();

            return Ok(result);
        }


        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="page">查询页码</param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet, Route("articles/{tenantId:int}/{categoryId:int}/{webSiteId:int}/{page:int}/{size:int}")]
        public async Task<IActionResult> GetArticles(int tenantId, int categoryId, int webSiteId,int page, int size)
        {
            var result = await _blogDbContext.Articles
                .Where(a => a.TenantId == tenantId && a.Status == 1 && categoryId == a.CategoryId && (a.WebSiteId == 0 || a.WebSiteId == webSiteId))
                .OrderBy(a => a.Sort)
                .ThenByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToPagedAsync(page, size);

            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取文章信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="ip">客户端ip地址</param>
        /// <returns></returns>
        [HttpPost, Route("articles/browse")]
        public async Task<IActionResult> GetArticleById([FromBody]BrowseHistory browseHistory)
        {
            if (browseHistory==null || browseHistory.SourceId <= 0)
                return BadRequest(OptResult.Failed("浏览的信息不存在"));

            var article = await _blogDbContext.Articles
                .Include(a => a.User)
                .ThenInclude(a=>a.ExternalLogin)
                .SingleOrDefaultAsync(a=>a.Id==browseHistory.SourceId);

            var now = DateTime.Now.Date;

            //判断 ip是否访问过
            if(!await  _blogDbContext.BrowseHistories.AnyAsync(a=>(a.Ip==browseHistory.Ip 
            || a.UserId==browseHistory.UserId)
            && a.Source==browseHistory.Source 
            && a.SourceId==browseHistory.SourceId 
            && a.CreatedAt.Date == now))
            {
                article.Clicks += 1;

                _blogDbContext.Articles.Update(article);
            }

            await _blogDbContext.BrowseHistories.AddAsync(browseHistory);

            await _blogDbContext.SaveChangesAsync();

            return Ok(article);
        }


        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="page">查询页码</param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet, Route("resources/{tenantId:int}/{categoryId:int}/{webSiteId:int}/{page:int}/{size:int}")]
        public async Task<IActionResult> GetResources(int tenantId, int categoryId, int webSiteId, int page, int size)
        {
            var result = await _blogDbContext.Resources
                .Where(a => a.TenantId == tenantId && a.Status == 1 && categoryId == a.CategoryId && (a.WebSiteId == 0 || a.WebSiteId == webSiteId))
                .OrderBy(a => a.Sort)
                .ThenByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToPagedAsync(page, size);

            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <param name="ip">客户端ip地址</param>
        /// <returns></returns>
        [HttpPost, Route("resources/browse")]
        public async Task<IActionResult> GetResourceById([FromBody]BrowseHistory browseHistory)
        {
            if (browseHistory == null || browseHistory.SourceId <= 0)
                return BadRequest(OptResult.Failed("浏览的信息不存在"));

            var resource = await _blogDbContext.Resources
                .Include(a => a.User)
                .ThenInclude(a => a.ExternalLogin)
                .SingleOrDefaultAsync(a => a.Id == browseHistory.SourceId);

            var now = DateTime.Now.Date;

            //判断 ip是否访问过
            if (!await _blogDbContext.BrowseHistories.AnyAsync(a => (a.Ip == browseHistory.Ip
            || a.UserId == browseHistory.UserId)
            && a.Source == browseHistory.Source
            && a.SourceId == browseHistory.SourceId
            && a.CreatedAt.Date == now))
            {
                resource.Clicks += 1;

                _blogDbContext.Resources.Update(resource);
            }

            await _blogDbContext.BrowseHistories.AddAsync(browseHistory);

            await _blogDbContext.SaveChangesAsync();

            return Ok(resource);
        }

    }
}
