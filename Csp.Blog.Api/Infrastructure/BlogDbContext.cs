using Csp.Blog.Api.Models;
using Csp.EF;
using Microsoft.EntityFrameworkCore;

namespace Csp.Blog.Api.Infrastructure
{
    public class BlogDbContext : CspDbContext<BlogDbContext>
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryLike> CategoryLikes { get; set; }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<ReplyLike> ReplyLikes { get; set; }

        public DbSet<BrowseHistory> BrowseHistories { get; set; }

    }
}
