using Csp.Blog.Api.Infrastructure;
using Csp.Blog.Api.Models;
using Csp.EF.Extensions;
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
    public class ArticleController : ControllerBase
    {
        private readonly BlogDbContext _blogDbContext;


        public ArticleController(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="page">查询页码</param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpGet,Route("{tenantId:int}/{webSiteId:int}/{categoryId:int}")]
        public async Task<IActionResult> Index(int tenantId,int webSiteId,int categoryId,int userId,int page,int size)
        {
            var predicate = PredicateExtension.True<Article>();

            predicate = predicate.And(a => a.TenantId == tenantId && a.Status == 1 );

            if (userId > 0)
                predicate = predicate.And(a => a.UserId == userId);

            if (webSiteId > 0)
                predicate = predicate.And(a => a.WebSiteId == webSiteId);

            if (categoryId > 0)
                predicate = predicate.And(a => a.CategoryId == categoryId);

            var result =await _blogDbContext.Articles
                .Include(a=>a.Category)
                .Where(predicate)
                .OrderBy(a => a.Sort)
                .ThenByDescending(a=>a.CreatedAt)
                .ToPagedAsync(page, size);

            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取文章信息
        /// </summary>
        /// <param name="id">主键编号</param>
        /// <returns></returns>
        [HttpGet, Route("find/{id:int}")]
        public async Task<IActionResult> FindById(int id)
        {
            if (id == 0)
                return BadRequest(OptResult.Failed("id不能小于或为0"));

            var result = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == id);

            return Ok(result);
        }

        /// <summary>
        /// 创建或更新文章
        /// </summary>
        /// <param name="article">文章对象信息</param>
        /// <returns></returns>
        [HttpPost,Route("create")]
        public async Task<IActionResult> Create([FromBody]Article article)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.First());

            if(article.Id>0)
            {
                var old = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == article.Id);
                old.SetUpdated(article);
                
                _blogDbContext.Articles.Update(old);
            }
            else
            {
                _blogDbContext.Articles.Add(article);
            }

            await _blogDbContext.SaveChangesAsync();

            return Ok(OptResult.Success(article.Id.ToString()));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">根据主键删除</param>
        /// <returns></returns>
        [HttpDelete, Route("delete/{id:int}")]
        public async Task<IActionResult> Deprecated(int id)
        {
            var artice = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == id);

            if (artice == null || artice.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            artice.Status = 0;

            _blogDbContext.Articles.Update(artice);

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());

        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">文章编号</param>
        /// <returns></returns>
        [HttpPut, Route("check/{id:int}")]
        public async Task<IActionResult> Check(int id)
        {
            var artice = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == id);

            if (artice == null || artice.Id <= 0)
                return BadRequest(OptResult.Failed("删除的数据不存在"));

            artice.Status = 1;


            /*
             记录审核动作，文章编号、状态，状态描述，审核人，审核原因，审核时间
             */

            _blogDbContext.Articles.Update(artice);

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());
        }

        /// <summary>
        /// 获取文章的评论列表
        /// </summary>
        /// <param name="articleId">文章编号</param>
        /// <param name="page">页码</param>
        /// <param name="size">查询记录数</param>
        /// <returns></returns>
        [HttpGet, Route("getreplies/{articleId:int}")]
        public async Task<IActionResult> GetReplies(int articleId, int page, int size)
        {
            var result = await _blogDbContext.Replies
                .Include(a => a.ReplyLikes)
                .Include(a => a.User)
                .ThenInclude(a=>a.ExternalLogin)
                .Where(a => a.SourceId == articleId && a.Source == "article")
                .OrderByDescending(a => a.CreatedAt)
                .AsNoTracking()
                .ToPagedAsync(page, size);

            return Ok(result);
        }

        /// <summary>
        /// 回复文章
        /// </summary>
        /// <param name="reply">回复信息</param>
        /// <returns></returns>
        [HttpPost, Route("reply")]
        public async Task<IActionResult> Reply([FromBody]Reply reply)
        {
            if (reply.Id > 0)
            {
                var old = await _blogDbContext.Replies.SingleOrDefaultAsync(a => a.Id == reply.Id);

                old.Content = reply.Content;

                _blogDbContext.Replies.Update(old);
            }
            else
            {
                var article = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == reply.SourceId);

                article.Replys += 1;

                _blogDbContext.Articles.Update(article);

                await _blogDbContext.Replies.AddAsync(reply);

            }

            await _blogDbContext.SaveChangesAsync();
            return Ok(OptResult.Success());
        }

        /// <summary>
        /// 删除回复
        /// </summary>
        /// <param name="replyId">回复编号</param>
        /// <returns></returns>
        [HttpDelete, Route("delreply/{replyId:int}")]
        public async Task<IActionResult> DeleteReply(int replyId)
        {
            var reply = await _blogDbContext.Replies.SingleOrDefaultAsync(a => a.Id == replyId);
            if (reply == null)
                return BadRequest(OptResult.Failed("删除的内容不存在"));

            var article = await _blogDbContext.Articles.SingleOrDefaultAsync(a => a.Id == reply.SourceId);

            article.Replys -= 1;

            _blogDbContext.Articles.Update(article);
            _blogDbContext.Replies.Remove(reply);

            await _blogDbContext.SaveChangesAsync();

            return Ok(OptResult.Success());

        }


        /// <summary>
        /// 同意或取消回复
        /// </summary>
        /// <param name="replyId">回复编号</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        [HttpPut, Route("agree/{replyId:int}/{userId:int}")]
        public async Task<IActionResult> Agree(int replyId,int userId)
        {
            var like = await _blogDbContext.ReplyLikes.SingleOrDefaultAsync(a => a.ReplyId == replyId && a.UserId == userId);
            
            var reply = await _blogDbContext.Replies.SingleOrDefaultAsync(a => a.Id == replyId);

            if (like == null)
            {
                reply.Likes += 1;

                _blogDbContext.Replies.Update(reply);

                _blogDbContext.ReplyLikes.Add(new ReplyLike(replyId, userId));

            }
            else
            {
                reply.Likes -= 1;
                _blogDbContext.ReplyLikes.Remove(like);
            }

            await _blogDbContext.SaveChangesAsync();

            return Ok(OptResult.Success());
        }
    }
}
