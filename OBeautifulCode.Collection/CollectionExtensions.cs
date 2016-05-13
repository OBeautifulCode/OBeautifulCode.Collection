// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="OBeautifulCode">
//   Copyright 2015 OBeautifulCode
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Conditions;

    /// <summary>
    /// Helper methods for operating on objects of type <see cref="ICollection"/>
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the elements of the specified <see cref="IEnumerable"/> to an <see cref="ICollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="valuesToAdd">
        /// The <see cref="IEnumerable"/> whose elements should be added to the <see cref="ICollection"/>.
        /// The <see cref="IEnumerable"/> itself cannot be a null reference, but it can contain elements that are a null reference.
        /// </param>
        /// <exception cref="ArgumentNullException">collection is null.</exception>
        /// <exception cref="ArgumentNullException">valuesToAdd is null.</exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> valuesToAdd)
        {
            Condition.Requires(collection, nameof(collection)).IsNotNull();
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(valuesToAdd, nameof(valuesToAdd)).IsNotNull();
            foreach (var item in valuesToAdd)
            {
                collection.Add(item);
            }
            // ReSharper restore PossibleMultipleEnumeration
        }
    }
}