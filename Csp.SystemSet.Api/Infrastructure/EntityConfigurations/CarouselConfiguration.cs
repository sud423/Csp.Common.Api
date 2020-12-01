using Csp.SystemSet.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Csp.SystemSet.Api.Infrastructure.EntityConfigurations
{
    public class CarouselConfiguration : IEntityTypeConfiguration<Carousel>
    {
        public void Configure(EntityTypeBuilder<Carousel> builder)
        {
            builder.ToTable("carousel");

            builder.HasKey(a => a.Id);
        }
    }
}
