// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOAuth2InitiationOpExecutedEvent.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using System;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Executed an <see cref="GetOAuth2InitiationOp"/>.
    /// </summary>
    public partial class GetOAuth2InitiationOpExecutedEvent : EventBase<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOAuth2InitiationOpExecutedEvent"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the authorization request.  Likely <see cref="OAuth2Initiation.State"/>.</param>
        /// <param name="timestampUtc">The UTC timestamp.</param>
        /// <param name="context">The context data from the executed <see cref="GetOAuth2InitiationOp"/>.</param>
        public GetOAuth2InitiationOpExecutedEvent(
            string id,
            DateTime timestampUtc,
            string context)
            : base(id, timestampUtc)
        {
            new { id }.AsArg().Must().NotBeNullNorWhiteSpace();

            this.Context = context;
        }

        /// <summary>
        /// Gets the context data from the executed <see cref="GetOAuth2InitiationOp"/>.
        /// </summary>
        public string Context { get; private set; }
    }
}