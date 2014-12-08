// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Helper methods for operating on objects of type <see cref="IEnumerable"/>
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Libs.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using CuttingEdge.Conditions;

    using OBeautifulCode.Libs.String;

    /// <summary>
    /// Helper methods for operating on objects of type <see cref="IEnumerable"/>
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Fields (Private)
        
        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList, IEqualityComparer<TSource> comparer)
        {
            if (value == compareList)
            {
                return true;
            }
            if (value == null || compareList == null)
            {
                return false;
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            IEnumerator<TSource> enumerator1 = value.GetEnumerator();
            IEnumerator<TSource> enumerator2 = compareList.GetEnumerator();

            bool enum1HasValue = enumerator1.MoveNext();
            bool enum2HasValue = enumerator2.MoveNext();

            try
            {
                while (enum1HasValue && enum2HasValue)
                {
                    if (!comparer.Equals(enumerator1.Current, enumerator2.Current))
                    {
                        return false;
                    }

                    enum1HasValue = enumerator1.MoveNext();
                    enum2HasValue = enumerator2.MoveNext();
                }

                return !(enum1HasValue || enum2HasValue);
            }
            finally
            {
                enumerator1.Dispose();
                enumerator2.Dispose();
            }
        }

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList)
        {
            return IsEqualTo(value, compareList, null);
        }

        /// <summary>
        /// Checks whether a collection is the same as another collection
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>True if the two collections contain all the same items in the same order</returns>
        public static bool IsEqualTo(this IEnumerable value, IEnumerable compareList)
        {
            return IsEqualTo(value.OfType<object>(), compareList.OfType<object>());
        }

        /// <summary>
        /// Concatenates the individual values in an IEnumerable with a given delimiter
        /// separating the individual values.
        /// </summary>
        /// <remarks>
        /// if IEnumerable is empty, returns null.  If an element of the IEnumerable is null,
        /// then its treated like an empty string.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when delimiter is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
        public static string ToDelimitedString(this IEnumerable<string> value, string delimiter)
        {
            Condition.Requires(delimiter, "delimiter").IsNotNull();
            try
            {
                string result = value.Aggregate((working, next) => working + delimiter + next);
                // if there is only one element and it is null, then value.Aggregate returns null instead of empty string
                // ReSharper disable ConvertIfStatementToNullCoalescingExpression
                if (result == null)
                // ReSharper restore ConvertIfStatementToNullCoalescingExpression
                {
                    result = "";
                }
                return result;
            }
            catch (InvalidOperationException)  // means there are no elements in the IEnumerable
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a Common Separated Values string from the individual strings in an IEnumerable,
        /// making csv treatments where needed (double quotes around strings with commas, etc.)
        /// </summary>
        /// <remarks>
        /// if IEnumerable is empty, returns null.  If an element of the IEnumerable is null,
        /// then its treated like an empty string.
        /// csv treatments: http://en.wikipedia.org/wiki/Comma-separated_values
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when delimiter is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
        public static string ToCsv(this IEnumerable<string> value)
        {
            try
            {
                // ReSharper disable PossibleMultipleEnumeration
                int items = value.Count();
                if (items == 0)
                {
                    return null;
                }
                string result = value.First().ToCsvSafe() ?? "";
                bool first = true;
                foreach (string item in value)
                {
                    if (first)
                    {
                        first = false;
                        continue;
                    }
                    result = result + "," + item.ToCsvSafe();
                }
                return result;
                // ReSharper restore PossibleMultipleEnumeration
            }
            catch (InvalidOperationException)  // means there are no elements in the IEnumerable
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a string with the values in a given IEnumerable, separated by a newline
        /// </summary>
        /// <remarks>
        /// if IEnumerable is empty, returns null.  If an element of the IEnumerable is null,
        /// then its treated like an empty string.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when delimiter is null</exception>
        /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
        public static string ToNewLineDelimited(this IEnumerable<string> value)
        {
            return value.ToDelimitedString(Environment.NewLine);
        }

        /// <summary>
        /// Converts the source to a hash set.
        /// </summary>
        /// <remarks>
        /// copied from byteflux:  http://byteflux.me/index.php/2009/08/13/some-useful-extension-methods/
        /// </remarks>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <returns>A typed hash set containing the items from the source enumerable.</returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source)
        {
            return new HashSet<TSource>(source);
        }

        /// <summary>
        /// Converts the source to a hash set.
        /// </summary>
        /// <remarks>
        /// copied from byteflux:  http://byteflux.me/index.php/2009/08/13/some-useful-extension-methods/
        /// </remarks>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <param name="source">The source enumerable.</param>
        /// <param name="comparer">
        /// The comparer used by the hash set. This is useful for example when building a case insensitive
        /// hash set.
        /// </param>
        /// <returns>A typed hash set containing the items from the source enumerable.</returns>
        /// <exception cref="ArgumentNullException">source is null.</exception>
        public static HashSet<TSource> ToHashSet<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            return new HashSet<TSource>(source, comparer);
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using an equality comparer.  
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <param name="comparer">The comparer object to use to compare each item in the collection.  If null uses EqualityComparer(T).Default</param>
        /// <returns>IEnumerable(T) with the symmetric difference of the two sets.</returns>
        public static IEnumerable<TSource> SymmetricDifference<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList, IEqualityComparer<TSource> comparer)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
            if (compareList == null)
            {
                throw new ArgumentNullException("compareList");
            }

            if (comparer == null)
            {
                comparer = EqualityComparer<TSource>.Default;
            }

            // ReSharper disable PossibleMultipleEnumeration
            return value.Except(compareList, comparer).Union(compareList.Except(value, comparer), comparer);
            // ReSharper restore PossibleMultipleEnumeration
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using the default equality comparer.  
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>IEnumerable(T) with the symmetric difference of the two sets.</returns>
        public static IEnumerable<TSource> SymmetricDifference<TSource>(this IEnumerable<TSource> value, IEnumerable<TSource> compareList)
        {
            return SymmetricDifference(value, compareList, null);
        }

        /// <summary>
        /// Gets the symmetric difference of two sets using the default equality comparer.  
        /// The symmetric difference is defined as the set of elements which are in one of the sets, but not in both.
        /// </summary>
        /// <param name="value">The current instance object</param>
        /// <param name="compareList">The collection to compare with</param>
        /// <returns>IEnumerable(T) with the symmetric difference of the two sets.</returns>
        public static IEnumerable SymmetricDifference(this IEnumerable value, IEnumerable compareList)
        {
            return SymmetricDifference(value.OfType<object>(), compareList.OfType<object>());
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        #endregion
    }
}
