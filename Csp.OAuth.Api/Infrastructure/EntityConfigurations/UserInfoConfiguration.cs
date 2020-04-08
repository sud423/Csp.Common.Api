using Csp.OAuth.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.OAuth.Api.Infrastructure.EntityConfigurations
{
    public class UserInfoConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("userinfo");

            builder.HasKey(a => a.Id);
        }
    }
}
