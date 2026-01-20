// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2ConnectionInfo.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Contains information required to establish an OAuth 2.0 connection with a provider,
    /// including client credentials, redirect URL, and optional environment configuration.
    /// </summary>
    public partial class OAuth2ConnectionInfo : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ConnectionInfo"/> class.
        /// </summary>
        /// <param name="credentials">The client credentials used to authenticate with the OAuth 2.0 provider.</param>
        /// <param name="redirectUrl">The URL that the OAuth 2.0 provider will redirect to after the user grants or denies authorization. Must be registered with the provider.</param>
        /// <param name="environment">OPTIONAL target environment for authorization (e.g., "sandbox" or "production"). This could be used to determine which OAuth 2.0 endpoints are used. DEFAULT is null, indicating the environment is not applicable.</param>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "1#", Justification = "Prefer URIs as strings.")]
        public OAuth2ConnectionInfo(
            OAuth2ClientCredentials credentials,
            string redirectUrl,
            string environment = null)
        {
            new { credentials }.AsArg().Must().NotBeNull();
            new { redirectUrl }.AsArg().Must().NotBeNullNorWhiteSpace();

            this.Credentials = credentials;
            this.RedirectUrl = redirectUrl;
            this.Environment = environment;
        }

        /// <summary>
        /// Gets the client credentials used to authenticate with the OAuth 2.0 provider.
        /// </summary>
        public OAuth2ClientCredentials Credentials { get; private set; }

        /// <summary>
        /// Gets the URL that the OAuth 2.0 provider will redirect to after the user
        /// grants or denies authorization.
        /// </summary>
        /// <remarks>
        /// This URL must be registered with the OAuth 2.0 provider as an authorized redirect URI.
        /// </remarks>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Prefer URIs as strings.")]
        public string RedirectUrl { get; private set; }

        /// <summary>
        /// Gets the target environment for authorization, or null if not applicable.
        /// </summary>
        /// <remarks>
        /// Some OAuth 2.0 providers (e.g., QuickBooks, PayPal) support separate sandbox and production
        /// environments with different endpoints. This value can be used to determine which endpoints to use.
        /// </remarks>
        public string Environment { get; private set; }
    }
}
