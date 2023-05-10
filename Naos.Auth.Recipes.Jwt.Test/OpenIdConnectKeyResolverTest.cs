// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenIdConnectKeyResolverTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Recipes.Jwt.Test
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Security.Principal;
    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public static class OpenIdConnectKeyResolverTest
    {
        [Fact(Skip = "Debug only.")]
        public static void TestWithTokenValidationHandler()
        {
            var authToken = "X.Y.Z";

            var audiences = new[]
                            {
                                "https://web.api.myCompany.com",
                            };

            var issuer = "https://myCompany.us.auth0.com/";

            using (var resolver = new OpenIdConnectKeyResolver(issuer, TimeSpan.Zero))
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters()
                                           {
                                               ValidateLifetime = true,
                                               ValidateAudience = true,
                                               ValidateIssuer = true,
                                               ValidateActor = true,
                                               ValidateIssuerSigningKey = true,
                                               ValidAudiences = audiences,
                                               ValidIssuer = issuer,
                                               IssuerSigningKeyResolver = resolver.GetSigningKey,
                                           };

                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out var validatedToken);
                validatedToken.MustForTest().NotBeNull();
                principal.MustForTest().NotBeNull();
            }
        }
    }
}