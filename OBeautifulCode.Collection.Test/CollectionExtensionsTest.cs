// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using OBeautifulCode.Collection.Recipes;

    using Xunit;

    public static class CollectionExtensionsTest
    {
        // ReSharper disable InconsistentNaming
        [Fact]
        public static void AddRange___Should_throw_ArgumentNullException___When_parameter_collection_is_null()
        {
            // Arrange
            // ReSharper disable CollectionNeverUpdated.Local
            var valuesToAdd = new List<int>();
            // ReSharper restore CollectionNeverUpdated.Local

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => CollectionExtensions.AddRange(null, valuesToAdd));
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Addis", Justification = "Code Analysis does not recognize the underscore as a word breaker.")]
        public static void AddRange___Should_throw_ArgumentNullException___When_parameter_valuesToAdd_is_null()
        {
            // Arrange
            var collection = new List<string>();

            // Act, Assert
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => collection.AddRange(null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Addis", Justification = "Code Analysis does not recognize the underscore as a word breaker.")]
        public static void AddRange___Should_not_add_any_elements_to_collection___When_collection_is_empty_and_valuesToAdd_is_empty()
        {
            // Arrange
            var collection = new HashSet<string>();
            // ReSharper disable CollectionNeverUpdated.Local
            var valuesToAdd = new List<string>();
            // ReSharper restore CollectionNeverUpdated.Local

            // Act
            collection.AddRange(valuesToAdd);

            // Assert
            Assert.Empty(collection);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Addis", Justification = "Code Analysis does not recognize the underscore as a word breaker.")]
        public static void AddRange___Should_add_all_elements_in_valuesToAdd_to_collection___When_collection_is_empty_and_valuesToAdd_is_not_empty()
        {
            // Arrange
            var collection1 = new HashSet<string>();
            var valuesToAdd1 = new List<string> { "a" };

            var collection2 = new HashSet<string>();
            var valuesToAdd2 = new List<string> { "a", "b" };

            // Act
            collection1.AddRange(valuesToAdd1);
            collection2.AddRange(valuesToAdd2);

            // Assert
            Assert.Equal(1, collection1.Count);
            Assert.Contains("a", collection1);

            Assert.Equal(2, collection2.Count);
            Assert.Contains("a", collection2);
            Assert.Contains("b", collection2);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Addis", Justification = "Code Analysis does not recognize the underscore as a word breaker.")]
        public static void AddRange___Should_not_add_any_elements_to_collection___When_collection_is_not_empty_and_valuesToAdd_is_empty()
        {
            // Arrange
            // ReSharper disable CollectionNeverUpdated.Local
            var collection1 = new HashSet<string> { "a" };
            var valuesToAdd1 = new List<string>();

            var collection2 = new HashSet<string> { "a", "b" };
            var valuesToAdd2 = new List<string>();
            // ReSharper restore CollectionNeverUpdated.Local

            // Act
            collection1.AddRange(valuesToAdd1);
            collection2.AddRange(valuesToAdd2);

            // Assert
            Assert.Equal(1, collection1.Count);
            Assert.Contains("a", collection1);

            Assert.Equal(2, collection2.Count);
            Assert.Contains("a", collection2);
            Assert.Contains("b", collection2);
        }

        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Addis", Justification = "Code Analysis does not recognize the underscore as a word breaker.")]
        public static void AddRange___Should_add_all_elements_in_valuesToAdd_to_collection___When_collection_is_not_empty_and_valuesToAdd_is_not_empty()
        {
            // Arrange
            var collection1 = new HashSet<string> { "d", "e", "f" };
            var valuesToAdd1 = new List<string> { "g" };

            var collection2 = new List<string> { "d", "e", "f", "g" };
            var valuesToAdd2 = new List<string> { "d", "e", "h" };

            var collection3 = new HashSet<string> { "d", "e", "f", "g" };
            var valuesToAdd3 = new List<string> { "d", "e", "h" };

            // Act
            collection1.AddRange(valuesToAdd1);
            collection2.AddRange(valuesToAdd2);
            collection3.AddRange(valuesToAdd3);

            // Assert
            Assert.Equal(4, collection1.Count);
            Assert.Contains("d", collection1);
            Assert.Contains("e", collection1);
            Assert.Contains("f", collection1);

            Assert.Equal(7, collection2.Count);
            Assert.Contains("d", collection2);
            Assert.Contains("e", collection2);
            Assert.Contains("f", collection2);
            Assert.Contains("g", collection2);
            Assert.Contains("h", collection2);
            Assert.Equal(2, collection2.Count(item => item == "d"));
            Assert.Equal(2, collection2.Count(item => item == "e"));

            Assert.Equal(5, collection3.Count);
            Assert.Contains("d", collection3);
            Assert.Contains("e", collection3);
            Assert.Contains("f", collection3);
            Assert.Contains("g", collection3);
            Assert.Contains("h", collection3);
        }

        // ReSharper restore InconsistentNaming
    }
}
