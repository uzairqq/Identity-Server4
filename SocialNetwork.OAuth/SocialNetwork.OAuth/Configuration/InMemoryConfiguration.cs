using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using StackExchange.Redis;

namespace SocialNetwork.OAuth.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new[]
            {
                new ApiResource("socialnetwork", "Social Network")
            };
        }

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(), 
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<Client> Clients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId = "socialnetwork", //client id and client secret will always retreive with the token
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    AllowedScopes = new[] {"socialnetwork"}
                },
                new Client()
                {
                    ClientId = "socialnetwork_implicit", //mvc client id and client secret will always retreive with the token
                    ClientSecrets = new[] {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:53013/signin-oidc" },//it needs to point to your application project

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:53013/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }


                }
            };
        }

        public static IEnumerable<TestUser> Users()
        {
            return new[]
            {
                new TestUser()
                {
                    SubjectId = "1",
                    Username = "uzair.qq@outlook.com",
                    Password = "password123"
                },
                new TestUser()
                {
                    SubjectId = "2",
                    Username = "uzair.qq@gmail.com",
                    Password = "password12345"
                }
            };
        }
    }
}
//=====We need to add this code in api project in startup.cs file In Configure service
//services.AddAuthentication("Bearer")
//.AddIdentityServerAuthentication(options =>
//{
//options.Authority = "http://localhost:59814/";
//options.RequireHttpsMetadata = false;
//options.ApiName = "socialnetwork";
//});


//== Add this in Configure method in api project
// app.UseAuthentication();

//==Add Authorize attribute in Controller in api project

//for downloading the identity quick start UI
//iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/release/get.ps1'))