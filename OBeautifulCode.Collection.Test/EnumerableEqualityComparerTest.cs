// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableEqualityComparerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Test
{
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Collection.Recipes;

    using Xunit;

    public static class EnumerableEqualityComparerTest
    {
        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_and_item2_are_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_is_null_and_item2_is_not_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);
            var item2 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(null, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_is_not_null_and_item2_is_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);
            var item1 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(item1, null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_and_item2_are_not_null_and_not_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);

            var item1a = Some.ReadOnlyDummies<string>(3);
            var item1b = Some.ReadOnlyDummies<string>(3);

            var item2a = new[] { "abc", null, "def" };
            var item2b = new[] { "def", null, "abc" };

            // Act
            var actual1 = systemUnderTest.Equals(item1a, item1b);
            var actual2 = systemUnderTest.Equals(item2a, item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_and_item2_are_not_null_and_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);
            var item1 = new[] { "abc", null, "def" };
            var item2 = new[] { "abc", null, "def" };

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_item1_and_item2_are_empty()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SequenceEqual);
            var item1 = new string[0];
            var item2 = new string[0];

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_and_item2_are_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_is_null_and_item2_is_not_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);
            var item2 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(null, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_is_not_null_and_item2_is_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);
            var item1 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(item1, null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_and_item2_are_not_null_and_not_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);

            var item1 = Some.ReadOnlyDummies<string>(3);
            var item2 = Some.ReadOnlyDummies<string>(3).Concat(new[] { A.Dummy<string>() }).ToList();

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_and_item2_are_not_null_and_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);

            var item1a = new[] { "abc", null, "def" };
            var item1b = new[] { "abc", null, "def" };

            var item2a = new[] { "abc", null, "def", "ghi", null };
            var item2b = new[] { null, "ghi", "abc", "def", "abc" };

            // Act
            var actual1 = systemUnderTest.Equals(item1a, item1b);
            var actual2 = systemUnderTest.Equals(item2a, item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_SymmetricDifferenceEqual_and_item1_and_item2_are_empty()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.SymmetricDifferenceEqual);
            var item1 = new string[0];
            var item2 = new string[0];

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_and_item2_are_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_is_null_and_item2_is_not_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);
            var item2 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(null, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_is_not_null_and_item2_is_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);
            var item1 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(item1, null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_and_item2_are_not_null_and_not_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);

            var item1 = new[] { "abc", "def", "ghi" };
            var item2 = new[] { "abc", "def", "ghi", "abc" };

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_and_item2_are_not_null_and_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);

            var item1a = new[] { "abc", null, "def" };
            var item1b = new[] { "def", "abc", null };

            var item2a = new[] { "abc", null, "def", "abc", "ghi", null };
            var item2b = new[] { null, "ghi", null, "abc", "def", "abc" };

            // Act
            var actual1 = systemUnderTest.Equals(item1a, item1b);
            var actual2 = systemUnderTest.Equals(item2a, item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_enumerableEqualityComparerStrategy_is_UnorderedEqual_and_item1_and_item2_are_empty()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>(EnumerableEqualityComparerStrategy.UnorderedEqual);
            var item1 = new string[0];
            var item2 = new string[0];

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }
    }
}
