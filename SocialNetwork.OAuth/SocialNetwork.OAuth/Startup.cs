using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialNetwork.OAuth.Configuration;

namespace SocialNetwork.OAuth
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();// this process to grab information from appsettings json connection string

        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(
                    @"D:\AspMVC\Identity-Server4\SocialNetwork.OAuth\SocialNetwork.OAuth\socialnetwork.pfx",
                    "password123")) // to use our own certificate 
                //.AddDeveloperSigningCredential()  //AddTemporarySigningCredential
                .AddTestUsers(InMemoryConfiguration.Users().ToList())
                .AddInMemoryClients(InMemoryConfiguration.Clients())
                .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources())
                .AddInMemoryApiResources(InMemoryConfiguration.ApiResources());
               

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(); //to output logging information to our console

            app.UseDeveloperExceptionPage(); // gives us information when there is exception or a problem

            app.UseIdentityServer();//to use identity server

            app.UseStaticFiles(); // to use static files in wwwroot or at any other place like css and js

            app.UseMvcWithDefaultRoute();

        }
    }
}
//openssl req -newkey rsa:2048 -nodes -keyout socialnetwork.key -x509 -days 365 -out socialnetwork.cer==> to generate a certtificate containing two files.. 1) .cer 2).key
//winpty openssl pkcs12 -export -in socialnetwork.cer -inkey socialnetwork.key -out socialnetwork.pfx==> for bundling both .key and .cer file into one called .pfx
