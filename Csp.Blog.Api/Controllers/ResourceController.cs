using Csp.Blog.Api.Infrastructure;
using Csp.Blog.Api.Models;
using Csp.EF.Paging;
using Csp.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Csp.Blog.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly BlogDbContext _blogDbContext;


        public ResourceController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        /// <summary>
        /// 获取分类列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet, Route("{type}/{tenantId:int}/{userId:int}")]
        public async Task<IActionResult> Index(int tenantId, int userId,string type, int page, int size)
        {
            var result = await _blogDbContext.Resources
                .Where(a => a.TenantId == tenantId && userId == a.UserId && a.Status == 1 && type==a.Type)
                .OrderBy(a => a.Sort)
                .ThenByDescending(a=>a.CreatedAt)
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

            var result = await _blogDbContext.Resources.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 创建或更新分类
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Resource resource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            //_blogDbContext.Entry(category).Property(a => a.CreatedAt).IsModified = false;

            if (resource.Id > 0)
            {
                _blogDbContext.Resources.Update(resource);
            }
            else
            {
                //category.UserId = _appUser.Id;
                //category.TenantId = _appUser.TenantId;
                _blogDbContext.Resources.Add(resource);
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
            var resource = await _blogDbContext.Resources.SingleOrDefaultAsync(a => a.Id == id);

            if (resource == null || resource.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            resource.Status = 0;

            _blogDbContext.Resources.Update(resource);

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());

        }
    }
}
