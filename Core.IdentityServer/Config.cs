using IdentityModel;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> GetIdentityResources() => new IdentityResource[]
        { 
            // new IdentityResources.Profile(),
            // new IdentityResources.OpenId()
        };

        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>
        {
            new ApiResource("core.users.api", "Core.Users.API" )
            {
                Scopes = new [] { "users-api", "addresses-api" }
            },
        };

        public static IEnumerable<ApiScope> GetApiScopes() => new List<ApiScope>
        {
            new ApiScope("sub", "sub", new [] { JwtClaimTypes.Id, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Name, JwtClaimTypes.Role } ),
            new ApiScope("users-api", "User Endpoints"),
            new ApiScope("addresses-api", "Address Endpoints"),
        };

        public static IEnumerable<Client> GetClients(IConfigurationSection clientConfigs) => new List<Client>
        {
            new Client
            {
                ClientId = clientConfigs.GetSection("NonInteractiveApp")["ClientId"],

                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret(clientConfigs.GetSection("NonInteractiveApp")["Secret"].Sha256()) },

                AllowedScopes = new []{
                                        "sub",
                                        "users-api",
                                        "addresses-api"
                                        }
            },
            new Client
            {
                ClientId = clientConfigs.GetSection("ClientApp")["ClientId"],

                AllowedGrantTypes = { GrantType.ClientCredentials },
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret(clientConfigs.GetSection("ClientApp")["Secret"].Sha256()) },

                AllowedScopes = new []{
                                        "sub",
                                        "users-api",
                                        "addresses-api"
                                        }
            }
        };
    }
}
