// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensions.cs" company="OBeautifulCode">
//   Copyright 2014 OBeautifulCode
// </copyright>
// <summary>
//   Helper methods for operating on objects of type <see cref="ICollection"/>
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
    /// Helper methods for operating on objects of type <see cref="ICollection"/>
    /// </summary>
    public static class CollectionExtensions
    {
        #region Fields (Private)

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds the elements of the specified IEnumerable to a collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="collection">The collection to add to.</param>
        /// <param name="values">The IEnumerable whose elements should be added to the Collection. The IEnumerable itself cannot be a null reference, but it can contain elements that are a null reference.</param>
        /// <exception cref="ArgumentNullException">collection or values is a null reference.</exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> values)
        {
            Condition.Requires(collection, "collection").IsNotNull();
            Condition.Requires(values, "values").IsNotNull();
            foreach (var item in values)
            {
                collection.Add(item);
            }
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
