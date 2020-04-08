using Csp.EF.Extensions;
using Csp.Jwt.Extensions;
using Csp.OAuth.Api.Application;
using Csp.OAuth.Api.Infrastructure;
using Csp.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace Csp.OAuth.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            API.Remote_Service_Base_Url = Configuration.GetValue<string>("OcelotUrl");

            services.AddControllers();

            //services.AddConsul(Configuration);
            services.AddJwt(Configuration);
            services.AddEF<OAuthDbContext>(Configuration);

            services.AddHttpClientServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler(err => err.Run(async context => await context.ExceptionResponse()));

            app.UseStatusCodePages(err => err.Run(async context => await context.StatusCodeResponse()));

            //Registers the agent with an IConfiguration instance:
            //app.UseElasticApm(Configuration);
            //loggerfactory.AddSeq(Configuration.GetSection("Seq"));

            //Ìí¼Óconsul
            //app.UseConsul(lifetime);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => await context.Response.WriteAsync("Ok"));
                endpoints.MapControllers();
            });
        }
    }

    static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services)
        {
            services.AddHttpClient("extendedhandlerlifetime").SetHandlerLifetime(TimeSpan.FromMinutes(5));
            services.AddHttpClient<IWxService, WxService>();

            return services;
        }
    }
}
