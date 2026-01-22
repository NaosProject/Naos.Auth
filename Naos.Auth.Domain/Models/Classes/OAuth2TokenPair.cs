// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2TokenPair.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A pair of tokens: a refresh and an access token.
    /// </summary>
    public partial class OAuth2TokenPair : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2TokenPair"/> class.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <param name="accessToken">The access token.</param>
        public OAuth2TokenPair(
            TokenInfo refreshToken,
            TokenInfo accessToken)
        {
            new { refreshToken }.AsArg().Must().NotBeNull();
            new { accessToken }.AsArg().Must().NotBeNull();

            this.RefreshToken = refreshToken;
            this.AccessToken = accessToken;
        }

        /// <summary>
        /// Gets the refresh token.
        /// </summary>
        public TokenInfo RefreshToken { get; private set; }

        /// <summary>
        /// Gets the access token.
        /// </summary>
        public TokenInfo AccessToken { get; private set; }
    }
}
