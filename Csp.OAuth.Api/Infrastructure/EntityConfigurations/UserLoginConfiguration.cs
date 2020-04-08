using Csp.OAuth.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.OAuth.Api.Infrastructure.EntityConfigurations
{
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("userlogin");

            builder.HasKey(a => new { a.Id,a.UserName });

        }
    }
}
