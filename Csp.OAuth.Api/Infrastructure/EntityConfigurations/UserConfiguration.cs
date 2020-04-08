using Csp.OAuth.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.OAuth.Api.Infrastructure.EntityConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(a => a.Id);


            builder.HasOne(a => a.UserLogin).WithOne(a => a.User).HasForeignKey<UserLogin>(a=>a.Id);
            builder.HasOne(a => a.ExternalLogin).WithOne(a => a.User).HasForeignKey<ExternalLogin>(a => a.Id);
            builder.HasOne(a => a.UserInfo).WithOne(a => a.User).HasForeignKey<UserInfo>(a => a.Id);
        }
    }
}
