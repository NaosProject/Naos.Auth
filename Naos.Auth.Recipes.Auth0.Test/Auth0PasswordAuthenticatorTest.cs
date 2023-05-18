// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Auth0PasswordAuthenticatorTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Recipes.Auth0.Test
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Security.Principal;
    using Naos.Auth.Recipes.Jwt;
    using OBeautifulCode.Assertion.Recipes;

    using Xunit;

    public static class Auth0PasswordAuthenticatorTest
    {
        [Fact(Skip = "Debug only.")]
        public static void TestAuthenticate()
        {
            var clientId = "client_id_from_application_on_auth0_dashboard";
            var clientSecret = "client_secret_from_application_on_auth0_dashboard";
            var requestTimeout = TimeSpan.FromSeconds(10);
            var audience = "https://legacy.myCompany.com";
            var connection = "Username-Password-Authentication";
            var issuer = "https://myCompany.us.auth0.com/";

            var username = "username";
            var password = "password";

            var authenticator = new Auth0PasswordAuthenticator(issuer, clientId, clientSecret, audience, requestTimeout, connection);

            var authToken = authenticator.Authenticate(username, password);

            using (var resolver = new OpenIdConnectKeyResolver(issuer, TimeSpan.Zero, TimeSpan.FromSeconds(10)))
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                                           {
                                               ValidateLifetime = true,
                                               ValidateAudience = true,
                                               ValidateIssuer = true,
                                               ValidateActor = true,
                                               ValidateIssuerSigningKey = true,
                                               ValidAudiences = new[]
                                                                {
                                                                    audience,
                                                                },
                                               ValidIssuer = issuer,
                                               IssuerSigningKeyResolver = resolver.GetSigningKey,
                                           };

                IPrincipal principal = tokenHandler.ValidateToken(authToken.AccessToken, validationParameters, out var validatedToken);
                validatedToken.MustForTest().NotBeNull();
                principal.MustForTest().NotBeNull();
            }
        }
    }
}