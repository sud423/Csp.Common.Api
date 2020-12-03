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


            builder.OwnsOne(a => a.SEO,seo=> {
                seo.Property(s => s.Title).HasColumnName("Title");
                seo.Property(s => s.Keyword).HasColumnName("Keyword");
                seo.Property(s => s.Descript).HasColumnName("Descript");
            });
            builder.OwnsOne(a => a.Server, server => {
                server.Property(s => s.Ip).HasColumnName("Ip");
                server.Property(s => s.IntranetIp).HasColumnName("IntranetIp");
                server.Property(s => s.RemoteAccount).HasColumnName("RemoteAccount");
                server.Property(s => s.RemotePwd).HasColumnName("RemotePwd");
                server.Property(s => s.Port).HasColumnName("Port");
                server.Property(s => s.Os).HasColumnName("Os");
            }); 
            builder.OwnsOne(a => a.Ftp, ftp => {
                ftp.Property(s => s.FtpAddress).HasColumnName("FtpAddress");
                ftp.Property(s => s.FtpAccount).HasColumnName("FtpAccount");
                ftp.Property(s => s.FtpPwd).HasColumnName("FtpPwd");
                ftp.Property(s => s.FtpPort).HasColumnName("FtpPort");
            });
        }
    }
}
