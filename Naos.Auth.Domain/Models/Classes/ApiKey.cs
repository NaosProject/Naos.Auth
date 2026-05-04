// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiKey.cs" company="Naos Project">
//    Copyright (c) Naos Project 2019. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain
{
    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Type;

    /// <summary>
    /// Stores an API key.
    /// </summary>
    public partial class ApiKey : IModelViaCodeGen
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKey"/> class.
        /// </summary>
        /// <param name="value">The value of the API key.</param>
        public ApiKey(
            string value)
        {
            value.MustForArg(nameof(value)).NotBeNullNorWhiteSpace();

            this.Value = value;
        }

        /// <summary>
        /// Gets the value of the API key.
        /// </summary>
        public string Value { get; private set; }
    }
}
