using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class ReplyConfiguration : IEntityTypeConfiguration<Reply>
    {
        public void Configure(EntityTypeBuilder<Reply> builder)
        {
            builder.ToTable("reply");

            builder.HasKey(a => a.Id);

            builder.HasMany(a => a.ReplyLikes).WithOne(a => a.Reply).HasForeignKey(a => a.ReplyId);
            builder.HasOne(a => a.User).WithOne(a => a.Reply).HasForeignKey<Reply>(a => a.UserId);
        }
    }
}
