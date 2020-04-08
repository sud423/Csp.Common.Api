using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class AtricleConfiguration : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.ToTable("article");

            builder.HasKey(a => a.Id);


            builder.HasOne(a => a.Category).WithMany(a => a.Articles).HasForeignKey("CategoryId");

            builder.HasOne(a => a.User).WithOne(a => a.Article).HasForeignKey<Article>(a=>a.UserId);

            //builder.HasMany(a => a.Replies).WithOne(a => a.Article).HasForeignKey("ReplyId");
        }
    }
}
