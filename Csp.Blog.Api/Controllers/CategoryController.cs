using Csp.Blog.Api.Infrastructure;
using Csp.Blog.Api.Models;
using Csp.EF.Extensions;
using Csp.EF.Paging;
using Csp.Web;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Csp.Blog.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly BlogDbContext _blogDbContext;


        public CategoryController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet, Route("{tenantId:int}/{type}")]
        public async Task<IActionResult> Index(int tenantId, string type,int userId, int page, int size)
        {
            var predicate = PredicateExtension.True<Category>();

            predicate = predicate.And(a => a.TenantId == tenantId && type == a.Type && a.Status == 1);

            if (userId > 0)
                predicate = predicate.And(a => a.UserId == userId);

            var result = await _blogDbContext.Categories
                .Where(predicate)
                .OrderBy(a => a.Sort)
                .ThenByDescending(a => a.CreatedAt)
                .ToPagedAsync(page, size);

            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取分类信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        [HttpGet, Route("find/{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            if (id == 0)
                return BadRequest(OptResult.Failed("id不能小于或为0"));

            var result = await _blogDbContext.Categories.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 创建或更新分类
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Category category)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            //_blogDbContext.Entry(category).Property(a => a.CreatedAt).IsModified = false;

            if (category.Id > 0)
            {
                var old = await _blogDbContext.Categories.SingleOrDefaultAsync(a => a.Id == category.Id);
                old.Name = category.Name;
                old.Descript = category.Descript;
                _blogDbContext.Categories.Update(old);
            }
            else
            {
                //category.UserId = _appUser.Id;
                //category.TenantId = _appUser.TenantId;
                _blogDbContext.Categories.Add(category);
            }

            await _blogDbContext.SaveChangesAsync();

            return Ok(OptResult.Success());
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">根据主键删除</param>
        /// <returns></returns>
        [HttpPut, Route("delete/{id:int}")]
        public async Task<IActionResult> Deprecated(int id)
        {
            var category = await _blogDbContext.Categories.Include(a => a.Articles).SingleOrDefaultAsync(a => a.Id == id);

            if (category == null || category.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            category.Remove();

            _blogDbContext.Categories.Update(category);

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());

        }
        
        /// <summary>
        /// 关注或取关
        /// </summary>
        /// <param name="id">根据主键关注或取关</param>
        /// <returns></returns>
        [HttpDelete, Route("attention/{id}")]
        public async Task<IActionResult> Attention(int id, int userId)
        {
            var category = await _blogDbContext.Categories.Include(a => a.CategoryLikes)
                .SingleOrDefaultAsync(a => a.Id == id);

            if (category == null || category.Id == 0)
                return BadRequest(OptResult.Failed("关注的分类不存在"));

            category.Attention(id, userId);

            _blogDbContext.Categories.Update(category);

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());
        }
    }
}
