using Csp.EF;
using Csp.Upload.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Csp.Upload.Api.Infrastructure
{
    public class OssDbContext : CspDbContext<OssDbContext>
    {
        public OssDbContext(DbContextOptions<OssDbContext> options) : base(options)
        {
        }

        public DbSet<FileModel> Files { get; set; }

    }
}
