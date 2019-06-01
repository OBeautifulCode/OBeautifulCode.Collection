// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableEqualityComparerTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Test
{
    using FluentAssertions;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Collection.Recipes;

    using Xunit;

    public static class EnumerableEqualityComparerTest
    {
        [Fact]
        public static void Equals___Should_return_true___When_item1_and_item2_are_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();

            // Act
            var actual = systemUnderTest.Equals(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_false___When_item1_is_null_and_item2_is_not_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();
            var item2 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(null, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_item1_is_not_null_and_item2_is_null()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();
            var item1 = Some.ReadOnlyDummies<string>();

            // Act
            var actual = systemUnderTest.Equals(item1, null);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_false___When_item1_and_item2_are_not_null_and_not_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();
            var item1 = Some.ReadOnlyDummies<string>(3);
            var item2 = Some.ReadOnlyDummies<string>(3);

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeFalse();
        }

        [Fact]
        public static void Equals___Should_return_true___When_item1_and_item2_are_not_null_and_equal()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();
            var item1 = new[] { "abc", null, "def" };
            var item2 = new[] { "abc", null, "def" };

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void Equals___Should_return_true___When_item1_and_item2_are_empty()
        {
            // Arrange
            var systemUnderTest = new EnumerableEqualityComparer<string>();
            var item1 = new string[0];
            var item2 = new string[0];

            // Act
            var actual = systemUnderTest.Equals(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }
    }
}
