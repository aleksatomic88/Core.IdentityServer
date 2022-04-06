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
                new ApiResource("api1", "FULL API 1" )
                {
                    Scopes = new [] { "api-user", "api-address" },
                    ApiSecrets = new List<Secret>{ new Secret("secret".Sha256()) }
                },
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>  new List<ApiScope>
            {
                new ApiScope("sub", "sub", new [] { JwtClaimTypes.Id, JwtClaimTypes.PreferredUserName, JwtClaimTypes.Email, JwtClaimTypes.Name, JwtClaimTypes.Role } ),
                new ApiScope("api-user", "API - User Endpoints"),
                new ApiScope("api-address", "API - Address Endpoints"),
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
                                            "api-user",
                                            "api-address"
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
                                            "api-user",
                                            "api-address"
                                           }
                }
            };
        }
}
