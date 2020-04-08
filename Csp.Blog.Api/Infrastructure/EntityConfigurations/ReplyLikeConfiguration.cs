using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class ReplyLikeConfiguration : IEntityTypeConfiguration<ReplyLike>
    {
        public void Configure(EntityTypeBuilder<ReplyLike> builder)
        {
            builder.ToTable("replylike");

            builder.HasKey(a =>new { a.ReplyId,a.UserId });
        }
    }
}
