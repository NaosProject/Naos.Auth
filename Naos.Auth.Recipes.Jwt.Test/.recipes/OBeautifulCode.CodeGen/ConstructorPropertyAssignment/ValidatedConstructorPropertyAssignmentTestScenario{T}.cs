﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidatedConstructorPropertyAssignmentTestScenario{T}.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.CodeGen.ModelObject.Recipes source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.CodeGen.ModelObject.Recipes
{
    using global::System;
    using global::System.Linq;
    using global::System.Reflection;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.Reflection.Recipes;

    /// <summary>
    /// Specifies a scenario for testing when a constructor sets a property values.
    /// </summary>
    /// <typeparam name="T">The type of the object being tested.</typeparam>
#if !OBeautifulCodeCodeGenSolution
    [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [global::System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.CodeGen.ModelObject.Recipes", "See package version number")]
    internal
#else
    public
#endif
    class ValidatedConstructorPropertyAssignmentTestScenario<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatedConstructorPropertyAssignmentTestScenario{T}"/> class.
        /// </summary>
        /// <param name="id">The identifier of the scenario.</param>
        /// <param name="propertyName">The name of the property that is assigned a value by the constructor.</param>
        /// <param name="systemUnderTestExpectedPropertyValueFunc">A func that returns the object to test and the expected value of the property being tested.</param>
        /// <param name="compareActualToExpectedUsing">Specifies how to compare the actual property value to the expected property value.</param>
        public ValidatedConstructorPropertyAssignmentTestScenario(
            string id,
            string propertyName,
            Func<SystemUnderTestExpectedPropertyValue<T>> systemUnderTestExpectedPropertyValueFunc,
            CompareActualToExpectedUsing compareActualToExpectedUsing)
        {
            new { id }.AsTest().Must().NotBeNullNorWhiteSpace();

            new { propertyName }.AsTest().Must().NotBeNullNorWhiteSpace(id);

            T systemUnderTest = null;

            PropertyInfo property = null;

            object expectedPropertyValue = null;

            if ((propertyName != ConstructorPropertyAssignmentTestScenario.ForceGeneratedTestsToPassAndWriteMyOwnScenarioPropertyName)
                && (propertyName != ConstructorPropertyAssignmentTestScenario.NoPropertiesAssignedInConstructorScenarioPropertyName))
            {
                new { systemUnderTestExpectedPropertyValueFunc }.AsTest().Must().NotBeNull(id);
                var systemUnderTestExpectedPropertyValue = systemUnderTestExpectedPropertyValueFunc();

                systemUnderTest = systemUnderTestExpectedPropertyValue.SystemUnderTest;
                new { systemUnderTest }.AsTest().Must().NotBeNull(id);

                expectedPropertyValue = systemUnderTestExpectedPropertyValue.ExpectedPropertyValue;

                property = typeof(T).GetPropertyFiltered(propertyName, MemberRelationships.DeclaredOrInherited, MemberOwners.Instance, MemberAccessModifiers.Public, throwIfNotFound: false);

                new { property }.AsTest().Must().NotBeNull(id);

                if (compareActualToExpectedUsing == CompareActualToExpectedUsing.DefaultStrategy)
                {
                    if (expectedPropertyValue == null)
                    {
                        compareActualToExpectedUsing = CompareActualToExpectedUsing.ReferenceEquality;
                    }
                    else if (expectedPropertyValue.GetType().IsValueType)
                    {
                        compareActualToExpectedUsing = CompareActualToExpectedUsing.ValueEquality;
                    }
                    else
                    {
                        compareActualToExpectedUsing = CompareActualToExpectedUsing.ReferenceEquality;
                    }
                }
            }

            this.Id = id;
            this.PropertyName = propertyName;
            this.SystemUnderTest = systemUnderTest;
            this.ExpectedPropertyValue = expectedPropertyValue;
            this.Property = property;
            this.CompareActualToExpectedUsing = compareActualToExpectedUsing;
        }

        /// <summary>
        /// Gets the identifier of the scenario.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the name of the property that is assigned a value by the constructor.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the object to test.
        /// </summary>
        public T SystemUnderTest { get; }

        /// <summary>
        /// Gets the expected property value.
        /// </summary>
        public object ExpectedPropertyValue { get; }

        /// <summary>
        /// Gets the property that is assigned a value by the constructor.
        /// </summary>
        public PropertyInfo Property { get; }

        /// <summary>
        /// Gets a specification of how to compare the actual property value to the expected property value.
        /// </summary>
        public CompareActualToExpectedUsing CompareActualToExpectedUsing { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            var result = this.Id;

            return result;
        }
    }
}
