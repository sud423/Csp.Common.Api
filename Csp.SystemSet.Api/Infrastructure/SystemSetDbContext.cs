using Csp.EF;
using Csp.SystemSet.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Csp.SystemSet.Api.Infrastructure
{
    public class SystemSetDbContext : CspDbContext<SystemSetDbContext>
    {
        public SystemSetDbContext(DbContextOptions<SystemSetDbContext> options) : base(options)
        {
        }

        public DbSet<WebSite> WebSites { get; set; }

        public DbSet<Carousel> Carousels { get; set; }
    }
}
