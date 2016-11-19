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
using Newtonsoft.Json.Serialization;
using AutoMapper;
using BigTree.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            services.AddTransient<GeoCoordsService>();
            services.AddLogging();
            services.AddMvc(config =>
                {
                    if(_env.IsProduction())
                    {
                        config.Filters.Add(new RequireHttpsAttribute()); //redirect to secure Https if PRO environment

                    }
                })
                .AddJsonOptions(opt =>
                {
                    opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddIdentity<WorldUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login"; //TODO: explore options to integrate with OAuth token
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if(ctx.Request.Path.StartsWithSegments("/api") &&
                        ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }

                        await Task.Yield();
                    }

                };
            })
            .AddEntityFrameworkStores<WorldContext>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, 
            ILoggerFactory loggerFactory, WorldContextSeedData seeder)
        {

            Mapper.Initialize(config =>
            {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();

            });

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

            app.UseIdentity();

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
