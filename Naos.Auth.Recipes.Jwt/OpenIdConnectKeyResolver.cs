﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpenIdConnectKeyResolver.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in Naos.Auth.Recipes.Jwt source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Recipes.Jwt
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IdentityModel.Tokens;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Threading;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Serialization.Json;

    /// <summary>
    /// Self contained implementation of <see cref="IssuerSigningKeyResolver" /> for use with <see cref="TokenValidationParameters" />.
    /// </summary>
#if !NaosAuthSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Auth.Recipes.Jwt", "See package version number")]
    internal
#else
    public
#endif
    sealed class OpenIdConnectKeyResolver : IDisposable
    {
        private const long DO_FORCE_REFRESH = 1;
        private const long DO_NOT_FORCE_REFRESH = 0;
        private readonly object providerKeyTuplesSync = new object();
        private readonly ObcJsonSerializer serializer = new ObcJsonSerializer();
        private readonly string issuer;
        private readonly TimeSpan cachedKeysTimeToLive;
        private readonly TimeSpan keysRequestTimeout;
        private readonly int retryErrorsCount;
        private readonly TimeSpan retryErrorWaitTime;
        private readonly Dictionary<string, Tuple<Key, RSA, SecurityKey>> providerKeyTuples = new Dictionary<string, Tuple<Key, RSA, SecurityKey>>();
        private DateTime providerKeyTuplesCacheExpiration;
        private long shouldForceRefresh = DO_NOT_FORCE_REFRESH;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenIdConnectKeyResolver"/> class.
        /// </summary>
        /// <param name="issuer">The issuer/authority.</param>
        /// <param name="cachedKeysTimeToLive">The time to keep signing keys cached before re-fetching them from the <paramref name="issuer" />.</param>
        /// <param name="keysRequestTimeout">The timeout for requesting the keys.</param>
        /// <param name="retryErrorsCount">The number of times to retry errors.</param>
        /// <param name="retryErrorWaitTime">The time to wait between errors when retrying.</param>
        public OpenIdConnectKeyResolver(
            string issuer,
            TimeSpan cachedKeysTimeToLive,
            TimeSpan keysRequestTimeout,
            int retryErrorsCount,
            TimeSpan retryErrorWaitTime)
        {
            issuer.MustForArg(nameof(issuer)).NotBeNullNorWhiteSpace();
            retryErrorsCount.MustForArg(nameof(retryErrorsCount)).BeGreaterThanOrEqualTo(0);

            this.issuer = issuer;
            this.cachedKeysTimeToLive = cachedKeysTimeToLive;
            this.keysRequestTimeout = keysRequestTimeout;
            this.retryErrorsCount = retryErrorsCount;
            this.retryErrorWaitTime = retryErrorWaitTime;
            this.providerKeyTuplesCacheExpiration = DateTime.UtcNow;
        }

        /// <summary>
        /// Gets the signing key (implementation of <see cref="IssuerSigningKeyResolver" />).
        /// </summary>
        /// <param name="token">The <see cref="T:System.String" /> representation of the token that is being validated.</param>
        /// <param name="securityToken">The <SecurityToken> that is being validated. It may be null.</SecurityToken>.</param>
        /// <param name="keyIdentifier">The <see cref="T:System.IdentityModel.Tokens.SecurityKeyIdentifier" /> found in the token. It may be null.</param>
        /// <param name="validationParameters"><see cref="T:System.IdentityModel.Tokens.TokenValidationParameters" /> required for validation.</param>
        /// <returns>Matching <see cref="SecurityKey" />.</returns>
        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Justification = "Correct in context.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "validationParameters", Justification = "Necessary part of signature/contract.")]
        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "securityToken", Justification = "Necessary part of signature/contract.")]
        public SecurityKey GetSigningKey(
            string token,
            SecurityToken securityToken,
            SecurityKeyIdentifier keyIdentifier,
            TokenValidationParameters validationParameters)
        {
            if (keyIdentifier == null)
            {
                throw new ArgumentNullException(nameof(keyIdentifier));
            }

            SecurityKey result = null;
            var attempts = this.retryErrorsCount + 1;

            while (result == null && attempts > 0)
            {
                attempts = attempts - 1;

                try
                {
                    this.RefreshProviderKeyTuplesIfNecessary(token);

                    var kid = keyIdentifier
                             .Where(_ => _ is NamedKeySecurityKeyIdentifierClause)
                             .Cast<NamedKeySecurityKeyIdentifierClause>()
                             .SingleOrDefault(_ => _.Name == nameof(Key.Kid).ToLowerInvariant())
                            ?.Id;

                    lock (this.providerKeyTuplesSync)
                    {
                        if (kid != null && this.providerKeyTuples.TryGetValue(kid, out var entry))
                        {
                            result = entry.Item3;
                        }
                        else
                        {
                            result = null;
                        }
                    }

                    if (result == null)
                    {
                        Thread.Sleep(this.retryErrorsCount);
                        Interlocked.Exchange(ref this.shouldForceRefresh, DO_FORCE_REFRESH);
                    }
                }
                catch (Exception)
                {
                    if (attempts == 0)
                    {
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(this.retryErrorWaitTime);
                        Interlocked.Exchange(ref this.shouldForceRefresh, DO_FORCE_REFRESH);
                    }
                }
            }

            return result;
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = NaosSuppressBecause.CA1506_AvoidExcessiveClassCoupling_DisagreeWithAssessment)]
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Need to keep in scope until finalized.")]
        private void RefreshProviderKeyTuplesIfNecessary(
            string token)
        {
            if (DateTime.UtcNow > this.providerKeyTuplesCacheExpiration)
            {
                lock (this.providerKeyTuplesSync)
                {
                    var localShouldForceRefresh = Interlocked.Read(ref this.shouldForceRefresh);
                    if (DateTime.UtcNow > this.providerKeyTuplesCacheExpiration || localShouldForceRefresh == DO_FORCE_REFRESH)
                    {
                        Interlocked.Exchange(ref this.shouldForceRefresh, DO_NOT_FORCE_REFRESH);
                        var baseUrl = this.issuer;
                        if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
                        {
                            baseUrl = baseUrl + "/";
                        }

                        var uri = new Uri(baseUrl + ".well-known/jwks.json");
                        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                        request.ContentType = "application/json";
                        request.Accept = "application/json";
                        request.Method = "GET";
                        request.Timeout = (int)this.keysRequestTimeout.TotalMilliseconds;
                        request.Headers.Add("Authorization", "bearer: " + token);
                        string jwkRaw = null;
                        using (var resp = request.GetResponse())
                        {
                            var responseStream = resp.GetResponseStream();
                            if (responseStream != null)
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    jwkRaw = reader.ReadToEnd();
                                }
                            }
                        }

                        var jwk = this.serializer.Deserialize<KeySet>(jwkRaw);

                        var newProviderKeyTuples = jwk.Keys.Select(
                                                           key =>
                                                           {
                                                               // Genius: https://stackoverflow.com/questions/40367279/rsasecuritykey-does-not-take-rsaparameters-as-arguments
                                                               // var exampleXml = provider.ToXmlString(false);
                                                               var provider = new RSACryptoServiceProvider(2048);
                                                               var newN = Convert.ToBase64String(Base64UrlEncoder.DecodeBytes(key.N));
                                                               var rsaXml =
                                                                   $"<RSAKeyValue><Modulus>{newN}</Modulus><Exponent>{key.E}</Exponent></RSAKeyValue>";
                                                               provider.FromXmlString(rsaXml);
                                                               var issuerSigningKey = new RsaSecurityKey(provider);
                                                               var tuple = new Tuple<Key, RSA, SecurityKey>(key, provider, issuerSigningKey);
                                                               return tuple;
                                                           })
                                                      .ToList();

                        foreach (var providerKeyTuple in this.providerKeyTuples)
                        {
                            providerKeyTuple.Value.Item2.Dispose();
                        }

                        this.providerKeyTuples.Clear();
                        foreach (var newProviderKeyTuple in newProviderKeyTuples)
                        {
                            this.providerKeyTuples.Add(
                                newProviderKeyTuple.Item1.Kid,
                                newProviderKeyTuple);
                        }

                        this.providerKeyTuplesCacheExpiration = DateTime.UtcNow.Add(this.cachedKeysTimeToLive);
                    }
                }
            }
        }

        /// <inheritdoc />
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly", Justification = "This is acceptable.")]
        [SuppressMessage("Microsoft.Usage", "CA1816:CallGCSuppressFinalizeCorrectly", Justification = "This is acceptable.")]
        public void Dispose()
        {
            var keys = this.providerKeyTuples?.Keys.ToList() ?? new List<string>();
            keys.ForEach(
                _ =>
                {
                    if (this.providerKeyTuples.TryGetValue(_, out var result))
                    {
                        result.Item2?.Dispose();
                    }
                });
        }
    }

    /// <summary>
    /// Set of signing keys for JWK usage.
    /// </summary>
#if !NaosAuthSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Auth.Recipes.Jwt", "See package version number")]
    internal
#else
    public
#endif
    class KeySet
    {
        /// <summary>
        /// Gets or sets the signing keys.
        /// </summary>
        public IReadOnlyCollection<Key> Keys { get; set; }
    }

    /// <summary>
    /// Single signing key for JWK usage.
    /// </summary>
#if !NaosAuthSolution
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("Naos.Auth.Recipes.Jwt", "See package version number")]
    internal
#else
    public
#endif
        class Key
    {
        /// <summary>
        /// Gets or sets the algorithm.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Alg", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string Alg { get; set; }

        /// <summary>
        /// Gets or sets the key type.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Kty", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string Kty { get; set; }

        /// <summary>
        /// Gets or sets the public key use.
        /// </summary>
        public string Use { get; set; }

        /// <summary>
        /// Gets or sets the RSA - Modulus.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "N", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string N { get; set; }

        /// <summary>
        /// Gets or sets the RSA - Exponent.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "E", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string E { get; set; }

        /// <summary>
        /// Gets or sets the key identifier.
        /// </summary>
        public string Kid { get; set; }

        /// <summary>
        /// Gets or sets the X.509 Certificate SHA1 thumbprint.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "t", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string X5t { get; set; }

        /// <summary>
        /// Gets or sets the X.509 Certificate chain.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "c", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public IReadOnlyCollection<string> X5c { get; set; }
    }
}