﻿// --------------------------------------------------------------------------------------------------------------------
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

    using CuttingEdge.Conditions;

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
            Condition.Requires(collection, "collection").IsNotNull();
            // ReSharper disable PossibleMultipleEnumeration
            Condition.Requires(valuesToAdd, "valuesToAdd").IsNotNull();
            foreach (var item in valuesToAdd)
            {
                collection.Add(item);
            }
            // ReSharper restore PossibleMultipleEnumeration
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