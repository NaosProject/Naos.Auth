# Naos.Auth
[![Build status](https://ci.appveyor.com/api/projects/status/3wx7s39a9bx5j0a5?svg=true)](https://ci.appveyor.com/project/Naos-Project/naos-auth)

## Naos.Auth.Recipes.Jwt

Example to setup JWT Authentication Validation in OWIN using .NET Framework.

```
private const string Issuer = "https://myCompany.us.auth0.com/";

// This has RSA Providers for the signing keys that need to stay in scope (i.e. not get disposed) to perform the validation.
private static readonly OpenIdConnectKeyResolver resolver = new OpenIdConnectKeyResolver(Issuer, TimeSpan.FromHours(1));

public static IAppBuilder UseJwtAuthentication(
    this IAppBuilder app)
{
    var audiences = new[]
                    {
                        "https://web.api.myCompany.com",
                    };

    var validationParameters = new TokenValidationParameters()
                            {
                                ValidateLifetime = true,
                                ValidateAudience = true,
                                ValidateIssuer = true,
                                ValidateActor = true,
                                ValidateIssuerSigningKey = true,
                                ValidAudiences = audiences,
                                ValidIssuer = Issuer,
                                IssuerSigningKeyResolver = resolver.GetSigningKey,
                            };

    var tokenHandler = new JwtSecurityTokenHandler();
    app.UseJwtBearerAuthentication(
        new JwtBearerAuthenticationOptions
        {
            AuthenticationMode = AuthenticationMode.Active,
            AllowedAudiences = audiences,
            TokenHandler = tokenHandler,
            TokenValidationParameters = validationParameters,
        });

    return app;
}
```