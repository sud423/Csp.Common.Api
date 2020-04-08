using Csp.OAuth.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.OAuth.Api.Infrastructure.EntityConfigurations
{
    public class ExternalLoginConfiguration : IEntityTypeConfiguration<ExternalLogin>
    {
        public void Configure(EntityTypeBuilder<ExternalLogin> builder)
        {
            builder.ToTable("externallogin");

            builder.HasKey(a => new { a.Id, a.OpenId });
        }
    }
}
