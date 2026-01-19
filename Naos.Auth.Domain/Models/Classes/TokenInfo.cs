// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenInfo.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// A secure, digital credential that verifies a user's identity and grants them access
    /// to specific resources or APIs or the ability to generate another token (e.g. refresh token => access token),
    /// along with some information about the token.
    /// </summary>
    public partial class TokenInfo : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TokenInfo"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="validity">The period of time within which the token is valid.</param>
        public TokenInfo(
            string token,
            UtcDateTimeRangeInclusive validity)
        {
            new { token }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { validity }.AsArg().Must().NotBeNull();

            this.Token = token;
            this.Validity = validity;
        }

        /// <summary>
        /// Gets the the token.
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Gets the period of time within which the token is valid.
        /// </summary>
        public UtcDateTimeRangeInclusive Validity { get; private set; }
    }
}
