// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetOAuth2InitiationOp.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Gets the initial data required to begin an OAuth 2.0 authorization code flow.
    /// </summary>
    public partial class GetOAuth2InitiationOp : ReturningOperationBase<OAuth2Initiation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOAuth2InitiationOp"/> class.
        /// </summary>
        /// <param name="connectionInfo">Information required to establish an OAuth 2.0 connection with a provider.</param>
        /// <param name="context">
        /// OPTIONAL context data.  The protocol that executes this operation can choose to persist the context
        /// alongside the generated state to get that context back when the authorization flow is completed.
        /// DEFAULT is no context.
        /// </param>
        public GetOAuth2InitiationOp(
            OAuth2ConnectionInfo connectionInfo,
            string context = null)
        {
            new { connectionInfo }.AsArg().Must().NotBeNull();

            this.ConnectionInfo = connectionInfo;
            this.Context = context;
        }

        /// <summary>
        /// Gets information required to establish an OAuth 2.0 connection with a provider.
        /// </summary>
        public OAuth2ConnectionInfo ConnectionInfo { get; private set; }

        /// <summary>
        /// Gets context data.
        /// </summary>
        /// <remarks>
        /// The protocol that executes this operation can choose to persist the context
        /// alongside the generated state to get that context back when the authorization flow is completed.
        /// </remarks>
        public string Context { get; private set; }
    }
}
