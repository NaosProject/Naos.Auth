﻿// --------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//   Generated using OBeautifulCode.CodeGen.ModelObject (1.0.195.0)
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace Naos.Auth.Domain.Test
{
    using global::System;
    using global::System.CodeDom.Compiler;
    using global::System.Collections.Concurrent;
    using global::System.Collections.Generic;
    using global::System.Collections.ObjectModel;
    using global::System.Diagnostics.CodeAnalysis;

    using global::FakeItEasy;

    using global::Naos.Auth.Domain;

    using global::OBeautifulCode.AutoFakeItEasy;
    using global::OBeautifulCode.Math.Recipes;

    /// <summary>
    /// The default (code generated) Dummy Factory.
    /// Derive from this class to add any overriding or custom registrations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [GeneratedCode("OBeautifulCode.CodeGen.ModelObject", "1.0.195.0")]
#if !NaosAuthSolution
    internal
#else
    public
#endif
    abstract class DefaultAuthDummyFactory : IDummyFactory
    {
        public DefaultAuthDummyFactory()
        {
            AutoFixtureBackedDummyFactory.AddDummyCreator(
                () => new OAuth2ClientCredentials(
                                 A.Dummy<string>(),
                                 A.Dummy<string>()));
        }

        /// <inheritdoc />
        public Priority Priority => new FakeItEasy.Priority(1);

        /// <inheritdoc />
        public bool CanCreate(Type type)
        {
            return false;
        }

        /// <inheritdoc />
        public object Create(Type type)
        {
            return null;
        }
    }
}