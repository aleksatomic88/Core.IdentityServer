using IdentityModel;
using IdentityServer4.Models;
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

        public static IEnumerable<Client> GetClients() => new List<Client>
        {
            new Client
            {
                ClientId = "NonInteractiveApp",

                AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = new []{
                                        "sub",
                                        "users-api",
                                        "addresses-api"
                                        }
            },
            new Client
            {
                ClientId = "ClientApp",

                AllowedGrantTypes = { GrantType.ClientCredentials },
                AllowOfflineAccess = true,

                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedScopes = new []{
                                        "sub",
                                        "users-api",
                                        "addresses-api"
                                        }
            }
        };
    }
}
