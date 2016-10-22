using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BigTree.Services;
using BigTree.Models;

namespace BigTree
{
    public class Startup
    {
        public IHostingEnvironment _env;
        public IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(_env.ContentRootPath)
                .AddJsonFile("config.json")
                .AddEnvironmentVariables();

            _config = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_config);

            if (_env.IsEnvironment("Development"))
            {
                services.AddScoped<IMailService, DebugMailService>();
                services.AddMvc();
            }            
            else
            {
             //implement PRO mail service
        
            }

            services.AddDbContext<WorldContext>();
            services.AddScoped<IWorldRepository, WorldRepository>(); //create once per request
            services.AddTransient<WorldContextSeedData>();
            services.AddLogging();
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {
            loggerFactory.AddConsole();

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddDebug(LogLevel.Information);

            }
            else
            {
                loggerFactory.AddDebug(LogLevel.Error);

            }
            //Take note of the order of usings,
            //UseStaticFiles needs to be called after the default files is attained
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseMvc(Configure =>
            {
                Configure.MapRoute(
                    name: "Default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                    );

            });

            seeder.EnsureSeedData().Wait(); //can't call async within Configure..so use .Wait();

        }
    }
}
