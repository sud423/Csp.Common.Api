using System.Text.Json.Serialization;

namespace Csp.Blog.Api.Models
{
    public class ReplyLike
    {
        public int ReplyId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Reply Reply { get; set; }


        public ReplyLike() { }

        public ReplyLike(int replyId,int userId)
        {
            ReplyId = replyId;
            UserId = userId;
        }
    }
}
