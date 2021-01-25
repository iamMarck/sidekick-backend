using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using SideKick.Examination.Data;
using SideKick.Examination.WS.Constants;
using SideKick.Examination.WS.Extensions;
using SideKick.Examination.WS.Handlers;
using SideKick.Examination.WS.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace SideKick.Examination.WS
{
    public class Startup
    {
        public Startup(IConfiguration config)
        {
            this.Configuration = config;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Db connections

            services.AddDbContext<ClientDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LocalNetworkConnection")));

            #endregion

            services.AddScoped<DbContext, ClientDbContext>();
            services.AddTransient<IAccountService, AccountService>();

            services.Configure<CookiePolicyOptions>(options => {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
           
            services.AddHttpContextAccessor();
            services.AddSession(opt => { opt.Cookie.IsEssential = true; });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            
            services.AddWebSocketManager();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;

            app.UseWebSockets();
            app.MapWebSocketManager("/ws", serviceProvider.GetService<AccountHandler>());
            app.UseSession();

            app.Run(context => {
                return context.Response.WriteAsync("Sidekick examination app websocket is running!...");
            });


        }

    }
}
