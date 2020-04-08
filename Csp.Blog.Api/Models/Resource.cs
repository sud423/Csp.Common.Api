using Csp.EF;

namespace Csp.Blog.Api.Models
{
    public class Resource : Entity
    {
        public int Id { get; set; }

        public int TenantId { get; set; }

        public int WebSiteId { get; set; }

        public int CategoryId { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string Descript { get; set; }

        public string Src { get; set; }

        public bool IsHot { get; set; }

        public bool IsRed { get; set; }

        public bool IsTop { get; set; }

        public string Author { get; set; }

        public byte Status { get; set; }

        public int Clicks { get; set; }

        public int Replys { get; set; }

        public int Likes { get; set; }

        public int Favorites { get; set; }

        public int Sort { get; set; }

        public string LastReplyUser { get; set; }

        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
