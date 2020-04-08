using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class CategoryLikeConfiguration : IEntityTypeConfiguration<CategoryLike>
    {
        public void Configure(EntityTypeBuilder<CategoryLike> builder)
        {
            builder.ToTable("categorylike");

            builder.HasKey(a => new { a.CategoryId, a.UserId });
        }
    }
}
