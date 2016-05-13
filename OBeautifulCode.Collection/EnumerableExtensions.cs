﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Conditions;

    using OBeautifulCode.String;

    /// <summary>
    /// Helper methods for operating on objects of type <see cref="IEnumerable"/>
    /// </summary>
    public static class EnumerableExtensions
    {
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
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentNullException">delimiter is null.</exception>
        public static string ToDelimitedString(this IEnumerable<string> value, string delimiter)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(value, nameof(value)).IsNotNull();
            Condition.Requires(delimiter, nameof(delimiter)).IsNotNull();
            try
            {
                // if there is only one element and it is null, then value.Aggregate returns null instead of empty string
                string result = value.Aggregate((working, next) => working + delimiter + next) ?? string.Empty;
                return result;
            }
            catch (InvalidOperationException)
            {
                // means there are no elements in the IEnumerable
                return null;
            }

            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Creates a common separated values (CSV) string from the individual strings in an <see cref="IEnumerable"/>,
        /// making CSV treatments where needed (double quotes around strings with commas, etc.)
        /// </summary>
        /// <param name="value">The enumerable to transform into a CSV string.</param>
        /// <remarks>
        /// If an element of the IEnumerable is null, then its treated like an empty string.
        /// CSV treatments: <a href="http://en.wikipedia.org/wiki/Comma-separated_values"/>
        /// </remarks>
        /// <returns>
        /// Returns a string that contains each element in the input enumerable, separated by a comma
        /// and with the proper escaping.
        /// If the enumerable is empty, returns null.
        /// </returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public static string ToCsv(this IEnumerable<string> value)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(value, nameof(value)).IsNotNull();
            return value.Select(item => item == null ? string.Empty : item.ToCsvSafe()).ToDelimitedString(",");
            // ReSharper restore PossibleMultipleEnumeration
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
        /// <exception cref="ArgumentNullException">value is null.</exception>
        public static string ToNewLineDelimited(this IEnumerable<string> value)
        {
            return value.ToDelimitedString(Environment.NewLine);
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
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default.</param>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> with the symmetric difference of the two sets.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentNullException">secondSet is null.</exception>
        public static IEnumerable<TSource> SymmetricDifference<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> secondSet, IEqualityComparer<TSource> comparer)
        {
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(value, nameof(value)).IsNotNull();
            Condition.Requires(secondSet, nameof(secondSet)).IsNotNull();
            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            return value.Except(secondSet, comparer).Union(secondSet.Except(value, comparer), comparer);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using the default equality comparer.
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <remarks>
        /// If one set has duplicate items when evaluated using the comparer, then the resulting symmetric difference will only
        /// contain one copy of the the duplicate item and only if it doesn't appear in the other set.
        /// </remarks>
        /// <typeparam name="TSource">The type of elements in the collection.</typeparam>
        /// <param name="value">The first enumerable.</param>
        /// <param name="secondSet">The second enumerable to compare against the first.</param>
        /// <returns>IEnumerable(T) with the symmetric difference of the two sets.</returns>
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentNullException">secondSet is null.</exception>
        public static IEnumerable<TSource> SymmetricDifference<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> secondSet)
        {
            return SymmetricDifference(value, secondSet, null);
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
        /// <exception cref="ArgumentNullException">value is null.</exception>
        /// <exception cref="ArgumentNullException">secondSet is null.</exception>
        public static IEnumerable SymmetricDifference(this IEnumerable value, IEnumerable secondSet)
        {
            return SymmetricDifference(value.OfType<object>(), secondSet.OfType<object>());
        }
    }
}
