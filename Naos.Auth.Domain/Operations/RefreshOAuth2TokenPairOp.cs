// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RefreshOAuth2TokenPairOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Using a refresh token, gets a new refresh and access token.
    /// </summary>
    public partial class RefreshOAuth2TokenPairOp : ReturningOperationBase<OAuth2TokenPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshOAuth2TokenPairOp"/> class.
        /// </summary>
        /// <param name="connectionInfo">Information required to establish an OAuth 2.0 connection with a provider.</param>
        /// <param name="refreshToken">The refresh token to use.</param>
        public RefreshOAuth2TokenPairOp(
            OAuth2ConnectionInfo connectionInfo,
            TokenInfo refreshToken)
        {
            new { connectionInfo }.AsArg().Must().NotBeNull();
            new { refreshToken }.AsArg().Must().NotBeNull();

            this.ConnectionInfo = connectionInfo;
            this.RefreshToken = refreshToken;
        }

        /// <summary>
        /// Gets information required to establish an OAuth 2.0 connection with a provider.
        /// </summary>
        public OAuth2ConnectionInfo ConnectionInfo { get; private set; }

        /// <summary>
        /// Gets the refresh token to use.
        /// </summary>
        public TokenInfo RefreshToken { get; private set; }
    }
}
