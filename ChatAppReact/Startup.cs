using ChatAppReact.Hubs;
using ChatAppReact.Services;
using ChatAppReact.User;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ChatAppReact
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			services.AddMvc();
			services.AddCors();
			services.AddSignalR();

			//Set Redis Cache for Tracking Online Users
			RedisConnector.redisHost = Configuration.GetSection("Redis:redisHost").Value;
			RedisConnector.redisPw = Configuration.GetSection("Redis:redisPw").Value;

			services.AddTransient<IUserTracker, UserTracker>();
			services.AddTransient<IChatMessageRepository, ChatMessageRepository>();
			services.AddTransient<IChatService, ChatService>();

			// In production, the React files will be served from this directory
			services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });
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
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

			app.UseSignalR(routes =>
			{
				routes.MapHub<ChatHub>("/chat");
			});
			app.UseCors(builder =>
			builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

			app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
