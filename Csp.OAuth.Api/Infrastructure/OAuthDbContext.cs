using Csp.EF;
using Csp.OAuth.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Csp.OAuth.Api.Infrastructure
{
    public class OAuthDbContext : CspDbContext<OAuthDbContext>
    {
        public OAuthDbContext(DbContextOptions<OAuthDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }

        public DbSet<UserInfo> UserInfos { get; set; }

        public DbSet<ExternalLogin> ExternalLogins { get; set; }


        
    }
}
