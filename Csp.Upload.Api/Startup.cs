using Csp.EF.Extensions;
using Csp.Jwt;
using Csp.Jwt.Extensions;
using Csp.Upload.Api.Application;
using Csp.Upload.Api.Application.Services;
using Csp.Upload.Api.Infrastructure;
using Csp.Upload.Api.Models;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Csp.Upload.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddEF<OssDbContext>(Configuration);

            services.AddJwt(Configuration);
            services.AddOssServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler(err => err.Run(async context => await context.ExceptionResponse()));

            app.UseStatusCodePages(err => err.Run(async context => await context.StatusCodeResponse()));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Ok");
                });
                endpoints.MapControllers();
            });
        }
    }
    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOssServices(this IServiceCollection services)
        {
            //services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<IIdentityParser<AppUser>, IdentityParser>();

            return services;
        }
    }
}
