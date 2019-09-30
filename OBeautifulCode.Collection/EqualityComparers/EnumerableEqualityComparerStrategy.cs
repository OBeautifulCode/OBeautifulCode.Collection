﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableEqualityComparerStrategy.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Collection source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Recipes
{
    using System.Collections.Generic;

    /// <summary>
    /// Determines the strategy to use when comparing two <see cref="IEnumerable{T}"/> for equality.
    /// </summary>
#if !OBeautifulCodeCollectionRecipesProject
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Collection", "See package version number")]
    internal
#else
    public
#endif
        enum EnumerableEqualityComparerStrategy
    {
        /// <summary>
        /// Use <see cref="EnumerableExtensions.IsSequenceEqualTo{TSource}"/>.
        /// </summary>
        SequenceEqual,

        /// <summary>
        /// Use <see cref="EnumerableExtensions.IsSymmetricDifferenceEqualTo{TSource}"/>.
        /// </summary>
        SymmetricDifferenceEqual,

        /// <summary>
        /// Use <see cref="EnumerableExtensions.IsUnorderedEqualTo{TSource}"/>.
        /// </summary>
        UnorderedEqual,
    }
}
