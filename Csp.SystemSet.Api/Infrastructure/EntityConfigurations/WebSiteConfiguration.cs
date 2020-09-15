using Csp.SystemSet.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.SystemSet.Api.Infrastructure.EntityConfigurations
{
    public class WebSiteConfiguration : IEntityTypeConfiguration<WebSite>
    {
        public void Configure(EntityTypeBuilder<WebSite> builder)
        {
            builder.ToTable("website");

            builder.HasKey(a => a.Id);
        }
    }
}
