using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Hangfire;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.Redis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC_Hangfire.Configurations;
using POC_Hangfire.Repositories;
using StackExchange.Redis;

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
            
           


            services.AddSingleton<IAmazonDynamoDB>(x =>
            {
                var clientConfig = new AmazonDynamoDBConfig { ServiceURL = "http://dynamodb:8000" };

                return new AmazonDynamoDBClient("root", "secret", clientConfig);
            });

            services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
            services.AddSingleton<IRegisterTables, RegisterTables>();
            services.AddSingleton<ICarRepository, CarRepository>();


            services.AddAsyncInitializer<MyAsyncInitializer>();

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
