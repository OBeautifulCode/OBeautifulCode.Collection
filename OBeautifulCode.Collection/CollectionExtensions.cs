// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using Spritely.Recipes;

    /// <summary>
    /// Helper methods for operating on objects of type <see cref="ICollection{T}"/>
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
            collection.Named(nameof(collection)).Must().NotBeNull().OrThrow();
            valuesToAdd.Named(nameof(valuesToAdd)).Must().NotBeNull().OrThrow();

            foreach (var item in valuesToAdd)
            {
                collection.Add(item);
            }
        }
    }
}