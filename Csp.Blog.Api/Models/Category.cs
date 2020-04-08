using Csp.EF;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;

namespace Csp.Blog.Api.Models
{
    public class Category : Entity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int WebSiteId { get; set; }

        [Required(ErrorMessage = "分类名称不能为空")]
        [StringLength(60,ErrorMessage = "分类名称最大为60个字符")]
        public string Name { get; set; }

        [Required(ErrorMessage = "分类类型不能为空")]
        [StringLength(60, ErrorMessage = "分类类型最大为20个字符")]
        public string Type { get; set; }

        public string Code { get; set; }

        public int Followers { get; set; }

        public int Sort { get; set; } 

        public bool IsTop { get; set; }

        public bool IsRed { get; set; }

        public bool IsHot { get; set; }

        public byte Status { get; set; }


        [StringLength(255, ErrorMessage = "小图片最大为255个字符")]
        public string SmallPic { get; set; }

        [StringLength(255, ErrorMessage = "大图片最大为255个字符")]
        public string BigPic { get; set; }

        [StringLength(2000, ErrorMessage = "描述最大为2000个字符")]
        public string Descript { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public ICollection<Article> Articles { get; set; }
        
        public ICollection<CategoryLike> CategoryLikes { get; set; }

        public Category()
        {
            Articles = new List<Article>();
            CategoryLikes = new List<CategoryLike>();
            Status = 1;
            Sort = 10001;
        }

        /// <summary>
        /// 移除
        /// </summary>
        public void Remove()
        {
            Status = 0;
            Articles.ToList()?.ForEach(a => {
                a.Status = 0;
            });
        }

        /// <summary>
        /// 关注或取关
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        public void Attention(int id,int userId)
        {
            //存在删除
            if (CategoryLikes.Any(b => b.UserId == userId))
            {
                Followers -= 1;
                CategoryLikes.Remove(CategoryLikes.First(a => a.UserId == userId));
            }
            else
            {
                Followers += 1;
                CategoryLike cl = new CategoryLike(id, userId);
                CategoryLikes.Add(cl);
            }
        }
    }
}
