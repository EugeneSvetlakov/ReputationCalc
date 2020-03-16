using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeastHunterControllers.Interfaces;
using BeastHunterControllers.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace BeastHunterWebApps
{
    public class Startup
    {

        #region ClassLifeCycles

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #endregion


        #region Properties

        public IConfiguration Configuration { get; }

        #endregion


        #region Methods

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Singleton because of service working InMemory
            services.AddSingleton<IReputationServices, ReputationServices>();
            services.AddSingleton<IItemServices, ItemServices>();
            services.AddSingleton<IEnemyServices, EnemyServices>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        #endregion
    }
}
