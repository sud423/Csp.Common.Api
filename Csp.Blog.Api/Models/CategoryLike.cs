using System.Text.Json.Serialization;

namespace Csp.Blog.Api.Models
{
    public class CategoryLike
    {
        public int CategoryId { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        public CategoryLike() { }

        public CategoryLike(int id,int userId) {
            CategoryId = id;
            UserId = userId;
        }
    }
}
