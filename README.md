# Naos.Auth
[![Build status](https://ci.appveyor.com/api/projects/status/3wx7s39a9bx5j0a5?svg=true)](https://ci.appveyor.com/project/Naos-Project/naos-auth)

## Naos.Auth.Recipes.Jwt

Example to setup JWT Authentication Validation in OWIN using .NET Framework.

```
public static IAppBuilder UseJwtAuthentication(
    this IAppBuilder app)
{
    var audiences = new[]
                    {
                        "https://web.api.myCompany.com",
                    };

    var issuer = "https://myCompany.us.auth0.com/";
    var resolver = new OpenIdConnectKeyResolver(issuer, TimeSpan.Zero);

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