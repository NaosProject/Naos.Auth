// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenAuthorizationState.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    /// <summary>
    /// The authorization state of a token.
    /// </summary>
    public enum TokenAuthorizationState
    {
        /// <summary>
        /// Unknown (default).
        /// </summary>
        Unknown,

        /// <summary>
        /// We have a token, but it hasn't been verified to work.
        /// </summary>
        Presumed,

        /// <summary>
        /// The token was recently used, successfully.
        /// </summary>
        Confirmed,

        /// <summary>
        /// The token was denied by the provider.
        /// </summary>
        /// <remarks>
        /// The provider rejected the token (e.g., invalid_grant).
        /// The specific reason may not be known—could be revocation at the provider,
        /// expiration, or other invalidation.
        /// </remarks>
        Denied,

        /// <summary>
        /// The token is known to have been revoked.
        /// </summary>
        /// <remarks>
        /// This state most likely captures a user performing a revocation via the application,
        /// versus it being revoked with the provider that the application is accessing
        /// (providers likely wouldn't communicate that the token is revoked, the token would just be denied).
        /// </remarks>
        Revoked,
    }
}
