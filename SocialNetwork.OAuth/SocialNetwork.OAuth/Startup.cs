﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialNetwork.OAuth.Configuration;

namespace SocialNetwork.OAuth
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(@"D:\AspMVC\Identity-Server4\SocialNetwork.OAuth\SocialNetwork.OAuth","password123")) // to use our own certificate 
                //.AddDeveloperSigningCredential()  //AddTemporarySigningCredential
                .AddTestUsers(InMemoryConfiguration.Users().ToList())
                .AddInMemoryClients(InMemoryConfiguration.Clients())
                .AddInMemoryApiResources(InMemoryConfiguration.ApiResources());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(); //to output logging information to our console

            app.UseDeveloperExceptionPage(); // gives us information when there is exception or a problem

            app.UseIdentityServer();//to use identity server
        }
    }
}
//openssl req -newkey rsa:2048 -nodes -keyout socialnetwork.key -x509 -days 365 -out socialnetwork.cer==> to generate a certtificate containing two files.. 1) .cer 2).key
//openssl pkcs12 -export -in socialnetwork.cer -inkey socialnetwork.key -out socialnetwork.pfx==> for bundling both .key and .cer file into one called .pfx
