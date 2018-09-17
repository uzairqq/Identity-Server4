using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApplication2
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
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();//to make sure theres nothing is tampered whatever comes from the authorization server

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies"; //we prepare our website for cookie authentication
                options.DefaultChallengeScheme = "oidc";// this is the the scheme when we are talking with the authorization server
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Scope.Add("socialnetwork");//to get access to these api or website . in this case i want to access the social network api.
                    options.Scope.Add("offline_access");//to get access to these api or website . in this case i want to access the social network api.
                    options.Scope.Add("email");//to get access to these api or website . in this case i want to access the social network api.

                    options.SignInScheme = "Cookies"; // identity the scheme
                    options.Authority = "http://localhost:59814";  //pointing to the authorization server
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "socialnetwork_code";
                    options.SaveTokens = true;
                    options.ClientSecret = "secret";
                    options.ResponseType = "id_token code"; //to allow the access token default is id_token
                    options.GetClaimsFromUserInfoEndpoint = true;//to get additional information about the user
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication(); //for using authentication cookie

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
