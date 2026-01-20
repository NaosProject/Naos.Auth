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
        /// The token was denied.
        /// </summary>
        Denied,
    }
}
