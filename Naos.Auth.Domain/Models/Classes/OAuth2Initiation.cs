// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OAuth2Initiation.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using System.Diagnostics.CodeAnalysis;
    using Naos.CodeAnalysis.Recipes;
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Represents the initial data required to begin an OAuth 2.0 authorization code flow,
    /// including the authorization URL and state for CSRF validation and request correlation.
    /// </summary>
    public partial class OAuth2Initiation : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OAuth2Initiation"/> class.
        /// </summary>
        /// <param name="authorizationUrl">The URL to redirect the user to for authorization.</param>
        /// <param name="state">
        /// The state value for validating against CSRF (cross-site request forgery) attacks
        /// and correlating requests when handling the authorization callback.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#", Justification = "Prefer URIs as strings.")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "csrf", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public OAuth2Initiation(
            string authorizationUrl,
            string state)
        {
            new { authorizationUrl }.AsArg().Must().NotBeNullNorWhiteSpace();
            new { state }.AsArg().Must().NotBeNullNorWhiteSpace();

            this.AuthorizationUrl = authorizationUrl;
            this.State = state;
        }

        /// <summary>
        /// Gets the URL to redirect the user to for authorization.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Justification = "Prefer URIs as strings.")]
        public string AuthorizationUrl { get; private set; }

        /// <summary>
        /// Gets the state value for validating against CSRF (cross-site request forgery) attacks
        /// and correlating requests when handling the authorization callback.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Csrf", Justification = NaosSuppressBecause.CA1704_IdentifiersShouldBeSpelledCorrectly_SpellingIsCorrectInContextOfTheDomain)]
        public string State { get; private set; }
    }
}
