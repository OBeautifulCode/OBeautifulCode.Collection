﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// <auto-generated>
//   Sourced from NuGet package. Will be overwritten with package update except in OBeautifulCode.Collection source.
// </auto-generated>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Recipes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Equality.Recipes;
    using OBeautifulCode.String.Recipes;
    using OBeautifulCode.Validation.Recipes;

    using static System.FormattableString;

    /// <summary>
    /// Helper methods for operating on objects of type <see cref="IEnumerable"/> and <see cref="IEnumerable{T}"/>.
    /// </summary>
#if !OBeautifulCodeCollectionRecipesProject
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [System.CodeDom.Compiler.GeneratedCode("OBeautifulCode.Collection", "See package version number")]
    internal
#else
    public
#endif
    static class EnumerableExtensions
    {
        /// <summary>
        /// Gets all combinations of items in a specified set of items.
        /// </summary>
        /// <remarks>
        /// Adapted from <a href="https://stackoverflow.com/a/41642733/356790" />.
        /// </remarks>
        /// <typeparam name="T">The type of items in the set.</typeparam>
        /// <param name="values">The set of values.</param>
        /// <param name="minimumItems">Optional minimum number of items in each combination.  Default is 1.</param>
        /// <param name="maximumItems">Optional maximum number of items in each combination.  Default is no maximum limit.</param>
        /// <returns>
        /// All possible combinations for the input set, constrained by the specified <paramref name="maximumItems"/> and <paramref name="minimumItems"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="minimumItems"/> is less than 1.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maximumItems"/> is less than <paramref name="minimumItems"/>"/>.</exception>
        public static IReadOnlyCollection<IReadOnlyCollection<T>> GetCombinations<T>(
            this IEnumerable<T> values,
            int minimumItems = 1,
            int maximumItems = int.MaxValue)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            new { values }.Must().NotBeNull();
            new { minimumItems }.Must().BeGreaterThanOrEqualTo(1);
            new { maximumItems }.Must().BeGreaterThanOrEqualTo(minimumItems, Invariant($"{nameof(maximumItems)} < {nameof(minimumItems)}."));

            // ReSharper disable once PossibleMultipleEnumeration
            var valuesList = values.Distinct().ToList();

            var nonEmptyCombinations = (int)Math.Pow(2, valuesList.Count) - 1;
            var result = new List<List<T>>(nonEmptyCombinations + 1);

            // Optimize generation of empty combination, if empty combination is wanted
            if (minimumItems == 0)
            {
                result.Add(new List<T>());
            }

            if ((minimumItems <= 1) && (maximumItems >= valuesList.Count))
            {
                // Simple case, generate all possible non-empty combinations
                for (var bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
                {
                    result.Add(GenerateCombination(valuesList, bitPattern));
                }
            }
            else
            {
                // Not-so-simple case, avoid generating the unwanted combinations
                for (var bitPattern = 1; bitPattern <= nonEmptyCombinations; bitPattern++)
                {
                    var bitCount = CountBits(bitPattern);
                    if (bitCount >= minimumItems && bitCount <= maximumItems)
                    {
                        result.Add(GenerateCombination(valuesList, bitPattern));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using the default equality comparer.
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <remarks>
        /// If one set has duplicate items when evaluated using the comparer, then the resulting symmetric difference will only
        /// contain one copy of the the duplicate item and only if it doesn't appear in the other set.
        /// </remarks>
        /// <param name="value">The first enumerable.</param>
        /// <param name="secondSet">The second enumerable to compare against the first.</param>
        /// <returns>IEnumerable(T) with the symmetric difference of the two sets.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="secondSet"/> is null.</exception>
        public static IEnumerable SymmetricDifference(
            this IEnumerable value,
            IEnumerable secondSet)
        {
            var result = SymmetricDifference(value.OfType<object>(), secondSet.OfType<object>());

            return result;
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using an equality comparer.
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <remarks>
        /// If one set has duplicate items when evaluated using the comparer, then the resulting symmetric difference will only
        /// contain one copy of the the duplicate item and only if it doesn't appear in the other set.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the collection.</typeparam>
        /// <param name="value">The first enumerable.</param>
        /// <param name="secondSet">The second enumerable to compare against the first.</param>
        /// <param name="comparer">Optional equality comparer to use to compare elements.  Default is to call <see cref="EqualityComparerHelper.GetEqualityComparerToUse{T}(IEqualityComparer{T})"/>.</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> with the symmetric difference of the two sets.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="secondSet"/> is null.</exception>
        public static IEnumerable<TSource> SymmetricDifference<TSource>(
            this IEnumerable<TSource> value,
            IEnumerable<TSource> secondSet,
            IEqualityComparer<TSource> comparer = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            new { value }.Must().NotBeNull();
            new { secondSet }.Must().NotBeNull();

            var equalityComparerToUse = EqualityComparerHelper.GetEqualityComparerToUse(comparer);

            var result = value.Except(secondSet, equalityComparerToUse).Union(secondSet.Except(value, equalityComparerToUse), equalityComparerToUse);

            return result;

            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Creates a common separated values (CSV) string from the individual strings in an <see cref="IEnumerable"/>,
        /// making CSV treatments where needed (double quotes around strings with commas, etc.).
        /// </summary>
        /// <param name="value">The enumerable to transform into a CSV string.</param>
        /// <param name="nullValueEncoding">Optional value to use when encoding null elements.  Defaults to the empty string.</param>
        /// <remarks>
        /// CSV treatments: <a href="http://en.wikipedia.org/wiki/Comma-separated_values"/>.
        /// </remarks>
        /// <returns>
        /// Returns a string that contains each element in the input enumerable,
        /// separated by a comma and with the proper escaping.
        /// If the enumerable is empty, returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static string ToCsv(
            this IEnumerable<string> value,
            string nullValueEncoding = "")
        {
            // ReSharper disable once PossibleMultipleEnumeration
            new { value }.Must().NotBeNull();

            // ReSharper disable once PossibleMultipleEnumeration
            var result = value.Select(item => item == null ? nullValueEncoding : item.ToCsvSafe()).ToDelimitedString(",");
            return result;
        }

        /// <summary>
        /// Concatenates the individual values in an <see cref="IEnumerable"/> with a given delimiter
        /// separating the individual values.
        /// </summary>
        /// <param name="value">The enumerable to concatenate.</param>
        /// <param name="delimiter">The delimiter to use between elements in the enumerable.</param>
        /// <remarks>
        /// If an element of the IEnumerable is null, then its treated like an empty string.
        /// </remarks>
        /// <returns>
        /// Returns a string that contains each element in the input enumerable, separated by the given delimiter.
        /// If the enumerable is empty, then this method returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="delimiter"/> is null.</exception>
        public static string ToDelimitedString(
            this IEnumerable<string> value,
            string delimiter)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            new { value }.Must().NotBeNull();
            new { delimiter }.Must().NotBeNull();

            // ReSharper disable once PossibleMultipleEnumeration
            var valueAsList = value.ToList();
            if (valueAsList.Count == 0)
            {
                return null;
            }

            // ReSharper disable once PossibleMultipleEnumeration
            var result = string.Join(delimiter, value);
            return result;
        }

        /// <summary>
        /// Creates a string with the values in a given <see cref="IEnumerable"/>, separated by a newline.
        /// </summary>
        /// <param name="value">The enumerable to concatenate.</param>
        /// <remarks>
        /// If an element of the IEnumerable is null, then its treated like an empty string.
        /// </remarks>
        /// <returns>
        /// Returns a string that contains each element in the input enumerable, separated by a newline.
        /// If the enumerable is empty, then this method returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public static string ToNewLineDelimited(
            this IEnumerable<string> value)
        {
            var result = value.ToDelimitedString(Environment.NewLine);
            return result;
        }

        /// <summary>
        /// Converts a generic dictionary to a non-generic dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="value">The dictionary to convert.</param>
        /// <returns>
        /// The specified generic dictionary converted to a non-generic dictionary.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="value"/> contains duplicate keys.</exception>
        public static IDictionary ToNonGenericDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> value)
        {
            // ReSharper disable PossibleMultipleEnumeration
            new { value }.Must().NotBeNull();

            var result = new Hashtable();
            foreach (var item in value)
            {
                if (result.ContainsKey(item.Key))
                {
                    throw new ArgumentException(Invariant($"{nameof(value)} contains duplicate keys."), nameof(value));
                }

                result.Add(item.Key, item.Value);
            }

            return result;

            // ReSharper restore PossibleMultipleEnumeration
        }

        private static List<T> GenerateCombination<T>(
            IReadOnlyList<T> inputList,
            int bitPattern)
        {
            var result = new List<T>(inputList.Count);
            for (var j = 0; j < inputList.Count; j++)
            {
                if ((bitPattern >> j & 1) == 1)
                {
                    result.Add(inputList[j]);
                }
            }

            return result;
        }

        private static int CountBits(
            int bitPattern)
        {
            var result = 0;
            while (bitPattern != 0)
            {
                result++;
                bitPattern &= bitPattern - 1;
            }

            return result;
        }
    }
}
