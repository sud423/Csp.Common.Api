using Csp.EF;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Csp.Blog.Api.Models
{
    public class Reply :Entity
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "来源不能为空")]
        [StringLength(100, ErrorMessage = "来源最大为1000个字符")]
        public string Source { get; set; }

        public int SourceId { get; set; }

        [Required(ErrorMessage ="回复内容不能为空")]
        [StringLength(1000,ErrorMessage ="回复内容最大为1000个字符")]
        public string Content { get; set; }

        public int Likes { get; set; }

        public int UserId { get; set; }
        
        public virtual ICollection<ReplyLike> ReplyLikes { get; set; }

        public virtual User User { get; set; }

    }
}
