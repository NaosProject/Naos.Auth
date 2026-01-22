// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RevokeOAuth2TokenOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Revokes a token.
    /// </summary>
    /// <remarks>
    /// Note that if you revoke the refresh token, it's functionally equivalent to disconnecting from
    /// the provider because you can no longer get an access token.  Also, revoking a refresh token might
    /// also revoke any active access tokens.
    /// </remarks>
    public partial class RevokeOAuth2TokenOp : ReturningOperationBase<OAuth2TokenPair>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RevokeOAuth2TokenOp"/> class.
        /// </summary>
        /// <param name="connectionInfo">Information required to establish an OAuth 2.0 connection with a provider.</param>
        /// <param name="token">The token to revoke.</param>
        public RevokeOAuth2TokenOp(
            OAuth2ConnectionInfo connectionInfo,
            TokenInfo token)
        {
            new { connectionInfo }.AsArg().Must().NotBeNull();
            new { token }.AsArg().Must().NotBeNull();

            this.ConnectionInfo = connectionInfo;
            this.Token = token;
        }

        /// <summary>
        /// Gets information required to establish an OAuth 2.0 connection with a provider.
        /// </summary>
        public OAuth2ConnectionInfo ConnectionInfo { get; private set; }

        /// <summary>
        /// Gets the refresh token to use.
        /// </summary>
        public TokenInfo Token { get; private set; }
    }
}
