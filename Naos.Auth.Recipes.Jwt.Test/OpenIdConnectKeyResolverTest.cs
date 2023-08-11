// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenIdConnectKeyResolverTest.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Recipes.Jwt.Test
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Principal;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.IdentityModel.Tokens;
    using Naos.Bootstrapper;
    using OBeautifulCode.Assertion.Recipes;

    using Xunit;
    using Xunit.Abstractions;
    using TokenValidationParameters = System.IdentityModel.Tokens.TokenValidationParameters;

    public class OpenIdConnectKeyResolverTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public OpenIdConnectKeyResolverTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "Debug only.")]
        public static void TestWithTokenValidationHandler()
        {
            var authToken = "X.Y.Z";

            var audiences = new[]
                            {
                                "https://web.api.myCompany.com",
                            };

            var issuer = "https://myCompany.us.auth0.com/";

            using (var resolver = new OpenIdConnectKeyResolver(issuer, TimeSpan.Zero, TimeSpan.FromSeconds(10), 3, TimeSpan.Zero))
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

        [Fact(Skip = "Debug only.")]
        public async Task TestTokenValidationRefreshRaceConditions()
        {
            var authToken = "X.Y.Z";
            var timer = Stopwatch.StartNew();

            var audiences = new[]
                            {
                                "https://web.api.myCompany.com",
                            };

            var issuer = "https://myCompany.us.auth0.com/";
            using (var resolver = new OpenIdConnectKeyResolver(issuer, TimeSpan.FromMinutes(10), TimeSpan.FromSeconds(10), 3, TimeSpan.Zero))
            {
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

                var handler = new JwtSecurityTokenHandler();
                var count = 100;

                var errors = new List<Exception>();
                var tasks = Enumerable.Range(0, count)
                                      .Select(
                                           _ =>
                                               Task.Factory.StartNew(
                                                   () =>
                                                   {
                                                       try
                                                       {
                                                           var principal = handler.ValidateToken(
                                                               authToken,
                                                               validationParameters,
                                                               out var outToken);
                                                           principal.MustForTest().NotBeNull();
                                                           outToken.MustForTest().NotBeNull();
                                                       }
                                                       catch (Exception ex)
                                                       {
                                                           errors.Add(ex);
                                                       }
                                                   }))
                                      .ToArray();

                await Task.WhenAll(tasks);

                if (errors.Count != 0)
                {
                    foreach (var error in errors)
                    {
                        this.testOutputHelper.WriteLine(error.ToString());
                    }

                    throw new TestFailedException(this.GetType());
                }
            }

            timer.Stop();
            this.testOutputHelper.WriteLine(timer.Elapsed.Milliseconds + "ms");
        }
    }
}