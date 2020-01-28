using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Collections.Generic;

namespace POC_Hangfire
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
            services.AddControllers();

            services.AddHangfire(configuration =>
                   configuration.UseRedisStorage(
                       ConnectionMultiplexer.Connect("redis:6379"),
                       new RedisStorageOptions()
                       {
                           Prefix = "poc:"
                       }))
               .AddHangfireServer();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseHangfireServer();
            app.UseHangfireDashboard(options: new DashboardOptions() { 
               Authorization = new IDashboardAuthorizationFilter[] { new HangFireAuthorizationFilter() }
            });
        }

        public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize([NotNull] DashboardContext context)
            {
                return true;
            }
        }
    }
}
