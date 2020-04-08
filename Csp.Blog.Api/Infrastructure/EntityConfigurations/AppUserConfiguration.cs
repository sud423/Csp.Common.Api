using Csp.Blog.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.Blog.Api.Infrastructure.EntityConfigurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.UserLogin).WithOne(a => a.User).HasForeignKey<UserLogin>(a => a.Id);
            builder.HasOne(a => a.ExternalLogin).WithOne(a => a.User).HasForeignKey<ExternalLogin>(a => a.Id);


        }
    }
}
