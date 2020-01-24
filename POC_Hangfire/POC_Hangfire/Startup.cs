using Hangfire;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;

namespace POC_Hangfire
{
    public class Startup
    {
        public static ConnectionMultiplexer Redis;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            var redisConnection = "redis:6379";
            Redis = ConnectionMultiplexer.Connect(redisConnection);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseRedisStorage(Redis, new RedisStorageOptions
                {
                    Prefix = "poc:hangfire"
                }));

            services.AddHangfireServer();

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

            app.UseHangfireDashboard();
            app.UseHangfireServer();
            //app.UseMvc();

            backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire! 1"));
            backgroundJobs.Schedule(() => Console.WriteLine("Hello world from Hangfire! 2"), TimeSpan.FromSeconds(1));
            backgroundJobs.Schedule(() => Console.WriteLine("Hello world from Hangfire! 3"), DateTimeOffset.FromUnixTimeSeconds(1));
        }
    }
}
