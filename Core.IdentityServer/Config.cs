using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;

namespace Identity
{
    public static class Config
    {

        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
            { 
                //new IdentityResources.Profile(),
                //new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
            {
                new ApiResource("api1", "FULL API 1" ) {Scopes = new [] { "api-user", "api-address" } },
            };

        public static IEnumerable<ApiScope> ApiScopes =>  new List<ApiScope>
            {
                new ApiScope("sub", "sub" ),
                new ApiScope("api-user", "API - User Endpoints", new [] {"claim1", "claim2" } ),
                new ApiScope("api-address", "API - Address Endpoints", new [] {"claim3", "claim4" } ),
            };

        public static IEnumerable<Client> Clients => new List<Client>
            {
                new Client
                {
                    ClientId = "NonInteractiveApp",

                    AllowedGrantTypes = { GrantType.ResourceOwnerPassword },
                    AllowOfflineAccess = true,

                    ClientSecrets = { new Secret("secret".Sha256()) },
                    
                    // scopes that client has access to
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
                    
                    // scopes that client has access to
                    AllowedScopes = new []{
                                            "api-user",
                                            "api-address"
                                           }
                }
            };
        }
}
