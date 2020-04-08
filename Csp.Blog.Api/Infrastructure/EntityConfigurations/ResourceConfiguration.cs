using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("resource");

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.User).WithOne(a => a.Resource).HasForeignKey<Resource>(a => a.UserId);
        }
    }
}
