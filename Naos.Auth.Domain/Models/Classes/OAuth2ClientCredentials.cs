// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2ClientCredentials.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stores an OAuth2 client-secret pair.
    /// </summary>
    public partial class OAuth2ClientCredentials : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2ClientCredentials"/> class.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        public OAuth2ClientCredentials(
            string clientId,
            string clientSecret)
        {
            clientId.MustForArg(nameof(clientId)).NotBeNullNorWhiteSpace();
            clientSecret.MustForArg(nameof(clientSecret)).NotBeNullNorWhiteSpace();

            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
        }

        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        public string ClientId { get; private set; }

        /// <summary>
        /// Gets the client secret.
        /// </summary>
        public string ClientSecret { get; private set; }
    }
}
