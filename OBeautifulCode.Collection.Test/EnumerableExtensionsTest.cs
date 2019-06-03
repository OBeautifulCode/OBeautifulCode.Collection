// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.AutoFakeItEasy;
    using OBeautifulCode.Collection.Recipes;
    using OBeautifulCode.String.Recipes;

    using Xunit;

    public static class EnumerableExtensionsTest
    {
        [Fact]
        public static void GetCombinations___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => EnumerableExtensions.GetCombinations<string>(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void GetCombinations___Should_throw_ArgumentOutOfRangeException___When_parameter_minimumItems_is_less_than_1()
        {
            // Arrange
            var values = Some.ReadOnlyDummies<string>();

            // Act
            var actual1 = Record.Exception(() => values.GetCombinations(minimumItems: 0));
            var actual2 = Record.Exception(() => values.GetCombinations(minimumItems: A.Dummy<NegativeInteger>()));

            // Assert
            actual1.Should().BeOfType<ArgumentOutOfRangeException>();
            actual1.Message.Should().Contain("minimumItems");
            actual1.Message.Should().Contain("'comparisonValue' is '1'");

            actual2.Should().BeOfType<ArgumentOutOfRangeException>();
            actual2.Message.Should().Contain("minimumItems");
            actual2.Message.Should().Contain("'comparisonValue' is '1'");
        }

        [Fact]
        public static void GetCombinations___Should_throw_ArgumentOutOfRangeException___When_parameter_maximumItems_is_less_than_minimumItems()
        {
            // Arrange
            var values = Some.ReadOnlyDummies<string>();
            var minimumItems = A.Dummy<PositiveInteger>().ThatIs(_ => _ > 10000);
            var maximumItems = A.Dummy<PositiveInteger>().ThatIs(_ => _ < minimumItems);

            // Act
            var actual = Record.Exception(() => values.GetCombinations(minimumItems, maximumItems));

            // Assert
            actual.Should().BeOfType<ArgumentOutOfRangeException>();
            actual.Message.Should().Contain("maximumItems < minimumItems");
        }

        [Fact]
        public static void GetCombinations___Should_return_empty_collection___When_values_is_an_empty_collection()
        {
            // Arrange
            var values = new string[] { };

            // Act
            var actual = values.GetCombinations();

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void GetCombinations___Should_return_empty_collection___When_minimumItems_is_greater_than_the_size_of_the_values_collection()
        {
            // Arrange
            var values = Some.ReadOnlyDummies<string>();

            // Act
            var actual = values.GetCombinations(minimumItems: values.Count + 1);

            // Assert
            actual.Should().BeEmpty();
        }

        [Fact]
        public static void GetCombinations___Should_return_all_combinations___When_called()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var expected = new[] { new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 }, new[] { 1, 2, 3 } };

            // Act
            var actual = values.GetCombinations();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public static void GetCombinations___Should_return_all_combinations_that_satisfy_minimumItems_count___When_specifying_non_default_minimumItems()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var expected = new[] { new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 }, new[] { 1, 2, 3 } };

            // Act
            var actual = values.GetCombinations(minimumItems: 2);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public static void GetCombinations___Should_return_all_combinations_that_satisfy_maximumItems_count___When_specifying_non_default_maximumItems()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var expected = new[] { new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 } };

            // Act
            var actual = values.GetCombinations(maximumItems: 2);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public static void GetCombinations___Should_return_all_combinations_that_satisfy_both_minimumItems_and_maximumItems_count___When_specifying_non_default_minimumItems_and_maximumItems()
        {
            // Arrange
            var values = new[] { 1, 2, 3 };
            var expected = new[] { new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 } };

            // Act
            var actual = values.GetCombinations(minimumItems: 2, maximumItems: 2);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public static void GetCombinations___Should_return_all_unique_combinations___When_values_contains_duplicate_elements()
        {
            // Arrange
            var values = new[] { 1, 2, 3, 2 };
            var expected = new[] { new[] { 1 }, new[] { 2 }, new[] { 3 }, new[] { 1, 2 }, new[] { 1, 3 }, new[] { 2, 3 }, new[] { 1, 2, 3 } };

            // Act
            var actual = values.GetCombinations();

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_throw_ArgumentNullException___When_first_set_is_null()
        {
            // Arrange
            var secondSet = new ArrayList { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SymmetricDifference(null, secondSet));
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_throw_ArgumentNullException___When_second_set_is_null()
        {
            // Arrange
            var firstSet = new ArrayList { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => firstSet.SymmetricDifference(null));
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_return_empty_set___When_both_sets_are_empty()
        {
            // Arrange
            var firstSet = new ArrayList();
            var secondSet = new ArrayList();

            // Act
            var actual = firstSet.SymmetricDifference(secondSet);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_return_empty_set___When_both_sets_are_the_same()
        {
            // Arrange
            var firstSet1 = new ArrayList { "abc" };
            var secondSet1 = new ArrayList { "abc" };

            var firstSet2 = new ArrayList { "abc", "DEF" };
            var secondSet2 = new ArrayList { "abc", "DEF" };

            var firstSet3 = new ArrayList { "DEF", "abc" };
            var secondSet3 = new ArrayList { "abc", "DEF" };

            var firstSet4 = new ArrayList { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new ArrayList { "ghi", "abc", "jkl", "def" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);

            // Assert
            Assert.Empty(actual1);
            Assert.Empty(actual2);
            Assert.Empty(actual3);
            Assert.Empty(actual4);
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_return_values_in_nonempty_set___When_one_set_is_empty()
        {
            // Arrange
            var firstSet1 = new ArrayList();
            var secondSet1 = new ArrayList { "abc" };

            var firstSet2 = new ArrayList { "abc", "DEF" };
            var secondSet2 = new ArrayList();

            var firstSet3 = new ArrayList();
            var secondSet3 = new ArrayList { "abc", "DEF" };

            var firstSet4 = new ArrayList { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new ArrayList();

            var firstSet5 = new ArrayList();
            var secondSet5 = new ArrayList { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);
            var actual5 = firstSet5.SymmetricDifference(secondSet5);

            // Assert
            Assert.True(secondSet1.Cast<object>().SequenceEqual(actual1.Cast<object>()));
            Assert.True(firstSet2.Cast<object>().SequenceEqual(actual2.Cast<object>()));
            Assert.True(secondSet3.Cast<object>().SequenceEqual(actual3.Cast<object>()));
            Assert.True(firstSet4.Cast<object>().SequenceEqual(actual4.Cast<object>()));
            Assert.True(secondSet5.Cast<object>().SequenceEqual(actual5.Cast<object>()));
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_return_the_symmetric_difference___When_both_sets_have_one_or_more_differences()
        {
            // Arrange
            var firstSet1 = new ArrayList { "def" };
            var secondSet1 = new ArrayList { "abc" };

            var firstSet2 = new ArrayList { "abc", "DEF" };
            var secondSet2 = new ArrayList { "ghi" };

            var firstSet3 = new ArrayList { "DEF" };
            var secondSet3 = new ArrayList { "abc", "DEF" };

            var firstSet4 = new ArrayList { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new ArrayList { "pqr", "def", "jkl", "mno" };

            var firstSet5 = new ArrayList { "ABC", "DEF", "GHI", "JKL" };
            var secondSet5 = new ArrayList { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1).Cast<object>();
            var actual2 = firstSet2.SymmetricDifference(secondSet2).Cast<object>();
            var actual3 = firstSet3.SymmetricDifference(secondSet3).Cast<object>();
            var actual4 = firstSet4.SymmetricDifference(secondSet4).Cast<object>();
            var actual5 = firstSet5.SymmetricDifference(secondSet5).Cast<object>();

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            Assert.Equal(8, actual5.Count());
            Assert.Contains("abc", actual5);
            Assert.Contains("def", actual5);
            Assert.Contains("ghi", actual5);
            Assert.Contains("jkl", actual5);
            Assert.Contains("ABC", actual5);
            Assert.Contains("DEF", actual5);
            Assert.Contains("GHI", actual5);
            Assert.Contains("JKL", actual5);
        }

        [Fact]
        public static void SymmetricDifference_value_secondSet___Should_return_the_symmetric_difference_with_only_one_copy_of_duplicate_items___When_one_set_has_duplicate_items()
        {
            // Arrange
            var firstSet1 = new ArrayList { "def", "def" };
            var secondSet1 = new ArrayList { "abc" };

            var firstSet2 = new ArrayList { "abc", "DEF" };
            var secondSet2 = new ArrayList { "ghi", "ghi" };

            var firstSet3 = new ArrayList { "DEF" };
            var secondSet3 = new ArrayList { "DEF", "abc", "DEF", "abc" };

            var firstSet4 = new ArrayList { "abc", "def", "ghi", "jkl", "ghi", "def" };
            var secondSet4 = new ArrayList { "mno", "pqr", "jkl", "def", "jkl", "mno" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1).Cast<object>();
            var actual2 = firstSet2.SymmetricDifference(secondSet2).Cast<object>();
            var actual3 = firstSet3.SymmetricDifference(secondSet3).Cast<object>();
            var actual4 = firstSet4.SymmetricDifference(secondSet4).Cast<object>();

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_throw_ArgumentNullException___When_first_set_is_null()
        {
            // Arrange
            var secondSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SymmetricDifference(null, secondSet));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_throw_ArgumentNullException___When_second_set_is_null()
        {
            // Arrange
            var firstSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => firstSet.SymmetricDifference(null));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_empty_set___When_both_sets_are_empty()
        {
            // Arrange
            var firstSet = new List<string>();
            var secondSet = new List<string>();

            // Act
            var actual = firstSet.SymmetricDifference(secondSet);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_empty_set___When_sets_are_the_same()
        {
            // Arrange
            var firstSet1 = new List<string> { "abc" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "abc", "DEF" };

            var firstSet3 = new List<string> { "DEF", "abc" };
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string> { "ghi", "abc", "jkl", "def" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);

            // Assert
            Assert.Empty(actual1);
            Assert.Empty(actual2);
            Assert.Empty(actual3);
            Assert.Empty(actual4);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_values_in_nonempty_set___When_one_set_is_empty()
        {
            // Arrange
            var firstSet1 = new List<string>();
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string>();

            var firstSet3 = new List<string>();
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string>();

            var firstSet5 = new List<string>();
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);
            var actual5 = firstSet5.SymmetricDifference(secondSet5);

            // Assert
            Assert.True(secondSet1.SequenceEqual(actual1));
            Assert.True(firstSet2.SequenceEqual(actual2));
            Assert.True(secondSet3.SequenceEqual(actual3));
            Assert.True(firstSet4.SequenceEqual(actual4));
            Assert.True(secondSet5.SequenceEqual(actual5));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_the_symmetric_difference___When_sets_have_one_or_more_differences()
        {
            // Arrange
            var firstSet1 = new List<string> { "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string> { "pqr", "def", "jkl", "mno" };

            var firstSet5 = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);
            var actual5 = firstSet5.SymmetricDifference(secondSet5);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            Assert.Equal(8, actual5.Count());
            Assert.Contains("abc", actual5);
            Assert.Contains("def", actual5);
            Assert.Contains("ghi", actual5);
            Assert.Contains("jkl", actual5);
            Assert.Contains("ABC", actual5);
            Assert.Contains("DEF", actual5);
            Assert.Contains("GHI", actual5);
            Assert.Contains("JKL", actual5);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_symmetric_difference_using_default_comparer___When_called()
        {
            // Arrange
            var firstSet1 = new List<string> { "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string> { "pqr", "def", "jkl", "mno" };

            var firstSet5 = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, null);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, null);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, null);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, null);
            var actual5 = firstSet5.SymmetricDifference(secondSet5, null);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            Assert.Equal(8, actual5.Count());
            Assert.Contains("abc", actual5);
            Assert.Contains("def", actual5);
            Assert.Contains("ghi", actual5);
            Assert.Contains("jkl", actual5);
            Assert.Contains("ABC", actual5);
            Assert.Contains("DEF", actual5);
            Assert.Contains("GHI", actual5);
            Assert.Contains("JKL", actual5);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet___Should_return_symmetric_difference_with_only_one_copy_of_duplicate_items___When_one_set_has_duplicate_items()
        {
            // Arrange
            var firstSet1 = new List<string> { "def", "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi", "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "DEF", "abc", "DEF", "abc" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl", "ghi", "def" };
            var secondSet4 = new List<string> { "mno", "pqr", "jkl", "def", "jkl", "mno" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1);
            var actual2 = firstSet2.SymmetricDifference(secondSet2);
            var actual3 = firstSet3.SymmetricDifference(secondSet3);
            var actual4 = firstSet4.SymmetricDifference(secondSet4);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_throw_ArgumentNullException___When_first_set_is_null()
        {
            // Arrange
            var secondSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SymmetricDifference(null, secondSet, StringComparer.CurrentCulture));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_throw_ArgumentNullException___When_second_set_is_null()
        {
            // Arrange
            var firstSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => firstSet.SymmetricDifference(null, StringComparer.CurrentCulture));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_empty_set___When_both_sets_are_empty()
        {
            // Arrange
            var firstSet = new List<string>();
            var secondSet = new List<string>();

            // Act
            var actual = firstSet.SymmetricDifference(secondSet, StringComparer.CurrentCulture);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_empty_set___When_sets_are_the_same()
        {
            // Arrange
            var firstSet1 = new List<string> { "abc" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "abc", "DEF" };

            var firstSet3 = new List<string> { "DEF", "abc" };
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string> { "ghi", "abc", "jkl", "def" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, StringComparer.CurrentCulture);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, StringComparer.CurrentCulture);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, StringComparer.CurrentCulture);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, StringComparer.CurrentCulture);

            // Assert
            Assert.Empty(actual1);
            Assert.Empty(actual2);
            Assert.Empty(actual3);
            Assert.Empty(actual4);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_values_in_nonempty_set___When_one_set_is_empty()
        {
            // Arrange
            var firstSet1 = new List<string>();
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string>();

            var firstSet3 = new List<string>();
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string>();

            var firstSet5 = new List<string>();
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, StringComparer.CurrentCulture);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, StringComparer.CurrentCulture);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, StringComparer.CurrentCulture);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, StringComparer.CurrentCulture);
            var actual5 = firstSet5.SymmetricDifference(secondSet5, StringComparer.CurrentCulture);

            // Assert
            Assert.True(secondSet1.SequenceEqual(actual1));
            Assert.True(firstSet2.SequenceEqual(actual2));
            Assert.True(secondSet3.SequenceEqual(actual3));
            Assert.True(firstSet4.SequenceEqual(actual4));
            Assert.True(secondSet5.SequenceEqual(actual5));
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_the_symmetric_difference___When_sets_have_one_or_more_differences()
        {
            // Arrange
            var firstSet1 = new List<string> { "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "abc", "DEF" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl" };
            var secondSet4 = new List<string> { "pqr", "def", "jkl", "mno" };

            var firstSet5 = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, StringComparer.CurrentCulture);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, StringComparer.CurrentCulture);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, StringComparer.CurrentCulture);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, StringComparer.CurrentCulture);
            var actual5 = firstSet5.SymmetricDifference(secondSet5, StringComparer.CurrentCulture);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            Assert.Equal(8, actual5.Count());
            Assert.Contains("abc", actual5);
            Assert.Contains("def", actual5);
            Assert.Contains("ghi", actual5);
            Assert.Contains("jkl", actual5);
            Assert.Contains("ABC", actual5);
            Assert.Contains("DEF", actual5);
            Assert.Contains("GHI", actual5);
            Assert.Contains("JKL", actual5);
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_symmetric_difference_using_case_insensitive_comparisons___When_called_with_case_insensitive_comparer()
        {
            // Arrange
            var firstSet1 = new List<string> { "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "abc", "def" };

            var firstSet4 = new List<string> { "abc", "DEF", "ghi", "jkl" };
            var secondSet4 = new List<string> { "pqr", "def", "JKL", "mno" };

            var firstSet5 = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var secondSet5 = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, StringComparer.CurrentCultureIgnoreCase);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, StringComparer.CurrentCultureIgnoreCase);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, StringComparer.CurrentCultureIgnoreCase);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, StringComparer.CurrentCultureIgnoreCase);
            var actual5 = firstSet5.SymmetricDifference(secondSet5, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            actual5.Should().BeEmpty();
        }

        [Fact]
        public static void SymmetricDifference_TSource_value_secondSet_comparer___Should_return_symmetric_difference_with_only_one_copy_of_duplicate_items___When_one_set_has_duplicate_items()
        {
            // Arrange
            var firstSet1 = new List<string> { "def", "def" };
            var secondSet1 = new List<string> { "abc" };

            var firstSet2 = new List<string> { "abc", "DEF" };
            var secondSet2 = new List<string> { "ghi", "ghi" };

            var firstSet3 = new List<string> { "DEF" };
            var secondSet3 = new List<string> { "DEF", "abc", "DEF", "abc" };

            var firstSet4 = new List<string> { "abc", "def", "ghi", "jkl", "ghi", "def" };
            var secondSet4 = new List<string> { "mno", "pqr", "jkl", "def", "jkl", "mno" };

            var firstSet5 = new List<string> { "ABC", "MNO", "DEF", "GHI", "JKL", "mno" };
            var secondSet5 = new List<string> { "abc", "pqr", "def", "ghi", "jkl", "PQR" };

            // Act
            var actual1 = firstSet1.SymmetricDifference(secondSet1, StringComparer.CurrentCulture);
            var actual2 = firstSet2.SymmetricDifference(secondSet2, StringComparer.CurrentCulture);
            var actual3 = firstSet3.SymmetricDifference(secondSet3, StringComparer.CurrentCulture);
            var actual4 = firstSet4.SymmetricDifference(secondSet4, StringComparer.CurrentCulture);
            var actual5 = firstSet5.SymmetricDifference(secondSet5, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            Assert.Equal(2, actual1.Count());
            Assert.Contains("def", actual1);
            Assert.Contains("abc", actual1);

            Assert.Equal(3, actual2.Count());
            Assert.Contains("DEF", actual2);
            Assert.Contains("abc", actual2);
            Assert.Contains("ghi", actual2);

            actual3.Should().ContainSingle();
            Assert.Contains("abc", actual3);

            Assert.Equal(4, actual4.Count());
            Assert.Contains("abc", actual4);
            Assert.Contains("ghi", actual4);
            Assert.Contains("mno", actual4);
            Assert.Contains("pqr", actual4);

            Assert.Equal(2, actual5.Count());
            Assert.Contains("MNO", actual5);
            Assert.Contains("pqr", actual5);
        }

        [Fact]
        public static void ToCsv___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToCsv(null));
        }

        [Fact]
        public static void ToCsv___Should_return_null___When_enumerable_is_empty()
        {
            // Arrange
            var values = new List<string>();

            // Act
            var actual = values.ToCsv();

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public static void ToCsv___Should_return_same_one_element___When_enumerable_contains_one_elements()
        {
            // Arrange
            var values = new List<string> { "fir    st" };

            // Act
            var actual = values.ToCsv();

            // Assert
            Assert.Equal(values[0], actual);
        }

        [Fact]
        public static void ToCsv___Should_return_all_elements_separated_by_comma___When_enumerable_has_multiple_elements()
        {
            // Arrange
            var values1a = new List<string> { "first", "second" };
            const string Expected1a = "first,second";
            var values1b = new List<string> { "first", "second", "third" };
            const string Expected1b = "first,second,third";

            // Act
            var actual1a = values1a.ToCsv();
            var actual1b = values1b.ToCsv();

            // Assert
            Assert.Equal(Expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
        }

        [Fact]
        public static void ToCsv___Should_return_all_elements_separated_by_comma_with_null_elements_treated_as_empty_string___When_enumerable_has_null_elements_and_nullValueEncoding_is_the_empty_string()
        {
            // Arrange
            var values1a = new List<string> { null };
            string expected1a = string.Empty;
            var values1b = new List<string> { "first", "second", null };
            const string Expected1b = "first,second,";

            var values2a = new List<string> { null, "second" };
            const string Expected2a = ",second";
            var values2b = new List<string> { "first", null, "third" };
            const string Expected2b = "first,,third";

            // Act
            var actual1a = values1a.ToCsv();
            var actual1b = values1b.ToCsv();
            var actual2a = values2a.ToCsv();
            var actual2b = values2b.ToCsv();

            // Assert
            Assert.Equal(expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
        }

        [Fact]
        public static void ToCsv___Should_return_all_elements_separated_by_comma_with_a_well_known_token_used_for_null_elements___When_enumerable_has_null_elements_and_nullValueEncoding_is_a_well_known_token()
        {
            // Arrange
            const string nullValueEncoding = "<null>";

            var values1a = new List<string> { null };
            string expected1a = "<null>";
            var values1b = new List<string> { "first", "second", null };
            const string Expected1b = "first,second,<null>";

            var values2a = new List<string> { null, "second" };
            const string Expected2a = "<null>,second";
            var values2b = new List<string> { "first", null, "third" };
            const string Expected2b = "first,<null>,third";

            // Act
            var actual1a = values1a.ToCsv(nullValueEncoding: nullValueEncoding);
            var actual1b = values1b.ToCsv(nullValueEncoding: nullValueEncoding);
            var actual2a = values2a.ToCsv(nullValueEncoding: nullValueEncoding);
            var actual2b = values2b.ToCsv(nullValueEncoding: nullValueEncoding);

            // Assert
            Assert.Equal(expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
        }

        [Fact]
        public static void ToCsv___Should_make_all_non_CSV_safe_elements_safe_before_adding_to_string___When_enumerable_has_elements_that_are_not_CSV_safe()
        {
            // Arrange
            var values1 = new List<string> { "first", null, "se\"c\"ond" };
            const string Expected1 = "first,,\"se\"\"c\"\"ond\"";

            var values2 = new List<string> { "  first  ", "sec,ond" };
            const string Expected2 = "\"  first  \",\"sec,ond\"";

            var values3 = new List<string> { "first", null, "se\"c\"ond" };
            const string Expected3 = "first,<null>,\"se\"\"c\"\"ond\"";

            // Act
            var actual1 = values1.ToCsv();
            var actual2 = values2.ToCsv();
            var actual3 = values3.ToCsv(nullValueEncoding: "<null>");

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2, actual2);
            Assert.Equal(Expected3, actual3);
        }

        [Fact]
        public static void ToCsv___Should_result_in_a_string_that_parses_into_the_original_collection_using_FromCsv___When_both_methods_called_using_same_nullValueEncoding()
        {
            // Arrange
            var expected = new List<string> { string.Empty, "ed133556-a398-4e15-a234-5b3b182e0ff5", null, string.Empty, null, "7235b88a-71b8-4f8a-92a7-03b90f3af393" };

            // Act
            var csv = expected.ToCsv(nullValueEncoding: "<null>");
            var actual = csv.FromCsv(nullValueEncoding: "<null>");

            // Assert
            actual.Should().Equal(expected);
        }

        [Fact]
        public static void ToDelimitedString___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            const string Delimiter = ",";

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToDelimitedString(null, Delimiter));
        }

        [Fact]
        public static void ToDelimitedString___Should_throw_ArgumentNullException___When_parameter_delimiter_is_null()
        {
            // Arrange
            var values = new List<string> { "abc", "def" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => values.ToDelimitedString(null));
        }

        [Fact]
        public static void ToDelimitedString___Should_return_null___When_parameter_values_is_empty()
        {
            // Arrange
            var values = new List<string>();
            const string Delimiter = ",";

            // Act
            var actual = values.ToDelimitedString(Delimiter);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public static void ToDelimitedString___Should_return_empty_string___When_parameter_values_contains_one_element_that_is_null()
        {
            // Arrange
            var values = new List<string> { null };
            const string Delimiter = ",";

            // Act
            var actual = values.ToDelimitedString(Delimiter);

            // Assert
            actual.Should().Be(string.Empty);
        }

        [Fact]
        public static void ToDelimitedString___Should_return_same_one_element___When_parameter_values_contains_one_element()
        {
            // Arrange
            var values = new List<string> { "fir    st" };
            const string Delimiter = ",";

            // Act
            var actual = values.ToDelimitedString(Delimiter);

            // Assert
            Assert.Equal(values[0], actual);
        }

        [Fact]
        public static void ToDelimitedString___Should_return_string_with_all_elements_of_specified_enumerable_concatenated___When_delimiter_is_empty_string()
        {
            // Arrange
            var values1 = new List<string> { "fir    st" };
            const string Expected1 = "fir    st";
            var values2 = new List<string> { "first", "second", "third" };
            const string Expected2 = "firstsecondthird";
            string delimiter = string.Empty;

            // Act
            var actual1 = values1.ToDelimitedString(delimiter);
            var actual2 = values2.ToDelimitedString(delimiter);

            // Assert
            Assert.Equal(Expected1, actual1);
            Assert.Equal(Expected2, actual2);
        }

        [Fact]
        public static void ToDelimitedString___Should_return_string_with_all_elements_separated_by_delimiter___When_enumerable_has_multiple_elements_with_nonempty_delimiter()
        {
            // Arrange
            const string Delimiter1 = ",";
            var values1a = new List<string> { "first", "second" };
            const string Expected1a = "first,second";
            var values1b = new List<string> { "first", "second", "third" };
            const string Expected1b = "first,second,third";

            const string Delimiter2 = " *A--> ";
            var values2a = new List<string> { "first", "second" };
            const string Expected2a = "first *A--> second";
            var values2b = new List<string> { "first", "second", "third" };
            const string Expected2b = "first *A--> second *A--> third";

            // Act
            var actual1a = values1a.ToDelimitedString(Delimiter1);
            var actual1b = values1b.ToDelimitedString(Delimiter1);
            var actual2a = values2a.ToDelimitedString(Delimiter2);
            var actual2b = values2b.ToDelimitedString(Delimiter2);

            // Assert
            Assert.Equal(Expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
        }

        [Fact]
        public static void ToDelimitedString__Should_return_string_with_all_elements_separated_by_delimiter_and_null_elements_treated_as_empty_string___When_enumerable_has_null_elements()
        {
            // Arrange
            const string Delimiter1 = ",";
            var values1a = new List<string> { null };
            string expected1a = string.Empty;
            var values1b = new List<string> { "first", "second", null };
            const string Expected1b = "first,second,";

            const string Delimiter2 = " *A--> ";
            var values2a = new List<string> { null, "second" };
            const string Expected2a = " *A--> second";
            var values2b = new List<string> { "first", null, "third" };
            const string Expected2b = "first *A-->  *A--> third";

            // Act
            var actual1a = values1a.ToDelimitedString(Delimiter1);
            var actual1b = values1b.ToDelimitedString(Delimiter1);
            var actual2a = values2a.ToDelimitedString(Delimiter2);
            var actual2b = values2b.ToDelimitedString(Delimiter2);

            // Assert
            Assert.Equal(expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
        }

        [Fact]
        public static void ToNewLineDelimited___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.ToNewLineDelimited(null));
        }

        [Fact]
        public static void ToNewLineDelimited___Should_return_null___When_parameter_value_is_null()
        {
            // Arrange
            var values = new List<string>();

            // Act
            var actual = values.ToNewLineDelimited();

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public static void ToNewLineDelimited___Should_return_same_one_element___When_enumerable_contains_one_element()
        {
            // Arrange
            var values = new List<string> { "fir    st" };

            // Act
            var actual = values.ToNewLineDelimited();

            // Assert
            Assert.Equal(values[0], actual);
        }

        [Fact]
        public static void ToNewLineDelimited___Should_return_all_elements_separated_by_new_line___When_enumerable_has_multiple_elements()
        {
            // Arrange
            var values1a = new List<string> { "first", "second" };
            const string Expected1a = "first\r\nsecond";
            var values1b = new List<string> { "first", "second", "third" };
            const string Expected1b = "first\r\nsecond\r\nthird";

            // Act
            var actual1a = values1a.ToNewLineDelimited();
            var actual1b = values1b.ToNewLineDelimited();

            // Assert
            Assert.Equal(Expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
        }

        [Fact]
        public static void ToNewLineDelimited___Should_return_all_elements_separated_by_new_line_with_null_elements_treated_as_empty_string___When_enumerable_has_empty_elements()
        {
            // Arrange
            var values1a = new List<string> { null };
            string expected1a = string.Empty;
            var values1b = new List<string> { "first", "second", null };
            const string Expected1b = "first\r\nsecond\r\n";

            var values2a = new List<string> { null, "second" };
            const string Expected2a = "\r\nsecond";
            var values2b = new List<string> { "first", null, "third" };
            const string Expected2b = "first\r\n\r\nthird";

            // Act
            var actual1a = values1a.ToNewLineDelimited();
            var actual1b = values1b.ToNewLineDelimited();
            var actual2a = values2a.ToNewLineDelimited();
            var actual2b = values2b.ToNewLineDelimited();

            // Assert
            Assert.Equal(expected1a, actual1a);
            Assert.Equal(Expected1b, actual1b);
            Assert.Equal(Expected2a, actual2a);
            Assert.Equal(Expected2b, actual2b);
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second___Should_return_true___When_both_sequences_are_null()
        {
            // Arrange, Act
            var actual = EnumerableExtensions.SequenceEqualHandlingNulls<object>(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second___Should_return_true___When_one_but_not_both_sequences_are_null()
        {
            // Arrange
            var notNullSequence = A.Dummy<List<string>>();

            // Act
            var actual1 = EnumerableExtensions.SequenceEqualHandlingNulls<object>(notNullSequence, null);
            var actual2 = EnumerableExtensions.SequenceEqualHandlingNulls<object>(null, notNullSequence);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second___Should_return_same_result_as_SequenceEqual___When_both_sequences_are_not_null()
        {
            // Arrange
            var sequence1 = new[] { "abc", "def" };
            var sequence2 = new[] { "abc", "def" };
            var sequence3 = new[] { "abc", "def", "ghi" };
            var sequence4 = new string[0];
            var sequence5 = new string[0];
            var sequence6 = new[] { "aBc", "dEf" };

            // Act
            var actual1 = sequence1.SequenceEqual(sequence2);
            var actual2 = sequence2.SequenceEqual(sequence3);
            var actual3 = sequence3.SequenceEqual(sequence2);
            var actual4 = sequence4.SequenceEqual(sequence5);
            var actual5 = sequence1.SequenceEqual(sequence6);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeTrue();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual___Should_return_true___When_both_dictionaries_are_null()
        {
            // Arrange, Act
            var actual = EnumerableExtensions.DictionaryEqual<string, string>(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual___Should_return_true___When_one_but_not_both_dictionaries_are_null()
        {
            // Arrange
            var notNullDictionary = A.Dummy<Dictionary<string, string>>();

            // Act
            var actual1 = EnumerableExtensions.DictionaryEqual<string, string>(notNullDictionary, null);
            var actual2 = EnumerableExtensions.DictionaryEqual<string, string>(null, notNullDictionary);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual___Should_return_false___When_dictionaries_are_not_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string>();

            var dictionary1b = new Dictionary<string, string>
            {
                { "abc", "abc" },
            };

            var dictionary2a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary2b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "aaa", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary3a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary3b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "mno", "pqr" },
            };

            var dictionary4a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary4b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "aaa" },
                { "mno", "pqr" },
            };

            var dictionary5a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary5b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "GHI", "jkl" },
                { "mno", "pqr" },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqual(dictionary1b);
            var actual2 = dictionary2a.DictionaryEqual(dictionary2b);
            var actual3 = dictionary3a.DictionaryEqual(dictionary3b);
            var actual4 = dictionary4a.DictionaryEqual(dictionary4b);
            var actual5 = dictionary5a.DictionaryEqual(dictionary5b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual___Should_return_true___When_dictionaries_are_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string>();
            var dictionary1b = new Dictionary<string, string>();

            var dictionary2a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary2b = new Dictionary<string, string>
            {
                { "mno", "pqr" },
                { "ghi", "jkl" },
                { "abc", "def" },
            };

            var dictionary3a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            var dictionary3b = new Dictionary<string, string>
            {
                { "MNO", "PQR" },
                { "GHI", "JKL" },
                { "ABC", "DEF" },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqual(dictionary1b);
            var actual2 = dictionary2a.DictionaryEqual(dictionary2b);
            var actual3 = dictionary3a.DictionaryEqual(dictionary3b, StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_true___When_both_dictionaries_are_null()
        {
            // Arrange
            Dictionary<string, string[]> nullDictionary = null;

            // Act
            var actual = EnumerableExtensions.DictionaryEqualHavingEnumerableValues(nullDictionary, nullDictionary);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_true___When_one_but_not_both_dictionaries_are_null()
        {
            // Arrange
            var notNullDictionary = A.Dummy<Dictionary<string, string[]>>();

            // Act
            var actual1 = EnumerableExtensions.DictionaryEqualHavingEnumerableValues(notNullDictionary, null);
            var actual2 = EnumerableExtensions.DictionaryEqualHavingEnumerableValues(null, notNullDictionary);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_false___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_dictionaries_are_not_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string[]>();

            var dictionary1b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "abc" } },
            };

            var dictionary2a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary2b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "aaa", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary4a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary4b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "aaa" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary5a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary5b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "GHI", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary6a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr", "stu" } },
            };

            var dictionary6b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "stu", "pqr" } },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqualHavingEnumerableValues(dictionary1b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual2 = dictionary2a.DictionaryEqualHavingEnumerableValues(dictionary2b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual3 = dictionary3a.DictionaryEqualHavingEnumerableValues(dictionary3b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual4 = dictionary4a.DictionaryEqualHavingEnumerableValues(dictionary4b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual5 = dictionary5a.DictionaryEqualHavingEnumerableValues(dictionary5b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual6 = dictionary6a.DictionaryEqualHavingEnumerableValues(dictionary6b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
            actual6.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_true___When_enumerableEqualityComparerStrategy_is_SequenceEqual_and_dictionaries_are_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string[]>();
            var dictionary1b = new Dictionary<string, string[]>();

            var dictionary2a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary2b = new Dictionary<string, string[]>
            {
                { "mno", new[] { "pqr" } },
                { "ghi", new[] { "jkl" } },
                { "abc", new[] { "def" } },
            };

            var dictionary3a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3b = new Dictionary<string, string[]>
            {
                { "MNO", new[] { "pqr" } },
                { "GHI", new[] { "jkl" } },
                { "ABC", new[] { "def" } },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqualHavingEnumerableValues(dictionary1b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual2 = dictionary2a.DictionaryEqualHavingEnumerableValues(dictionary2b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.SequenceEqual);
            var actual3 = dictionary3a.DictionaryEqualHavingEnumerableValues(dictionary3b, StringComparer.OrdinalIgnoreCase, EnumerableEqualityComparerStrategy.SequenceEqual);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_false___When_enumerableEqualityComparerStrategy_is_NoSymmetricDifference_and_dictionaries_are_not_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string[]>();

            var dictionary1b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "abc" } },
            };

            var dictionary2a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary2b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "aaa", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary4a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary4b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "aaa" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary5a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary5b = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "GHI", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqualHavingEnumerableValues(dictionary1b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual2 = dictionary2a.DictionaryEqualHavingEnumerableValues(dictionary2b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual3 = dictionary3a.DictionaryEqualHavingEnumerableValues(dictionary3b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual4 = dictionary4a.DictionaryEqualHavingEnumerableValues(dictionary4b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual5 = dictionary5a.DictionaryEqualHavingEnumerableValues(dictionary5b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqualHavingEnumerableValues___Should_return_true___When_enumerableEqualityComparerStrategy_is_NoSymmetricDifference_and_dictionaries_are_equal()
        {
            // Arrange
            var dictionary1a = new Dictionary<string, string[]>();
            var dictionary1b = new Dictionary<string, string[]>();

            var dictionary2a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary2b = new Dictionary<string, string[]>
            {
                { "mno", new[] { "pqr" } },
                { "ghi", new[] { "jkl" } },
                { "abc", new[] { "def" } },
            };

            var dictionary3a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "pqr" } },
            };

            var dictionary3b = new Dictionary<string, string[]>
            {
                { "MNO", new[] { "pqr" } },
                { "GHI", new[] { "jkl" } },
                { "ABC", new[] { "def" } },
            };

            var dictionary4a = new Dictionary<string, string[]>
            {
                { "abc", new[] { "def" } },
                { "ghi", new[] { "jkl" } },
                { "mno", new[] { "vwx", null, "pqr", null, "stu", "pqr" } },
            };

            var dictionary4b = new Dictionary<string, string[]>
            {
                { "mno", new[] { "stu", "pqr", null, "vwx" } },
                { "ghi", new[] { "jkl" } },
                { "abc", new[] { "def" } },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqualHavingEnumerableValues(dictionary1b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual2 = dictionary2a.DictionaryEqualHavingEnumerableValues(dictionary2b, enumerableEqualityComparerStrategy: EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual3 = dictionary3a.DictionaryEqualHavingEnumerableValues(dictionary3b, StringComparer.OrdinalIgnoreCase, EnumerableEqualityComparerStrategy.NoSymmetricDifference);
            var actual4 = dictionary4a.DictionaryEqualHavingEnumerableValues(dictionary4b, StringComparer.OrdinalIgnoreCase, EnumerableEqualityComparerStrategy.NoSymmetricDifference);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
            actual4.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second_comparer___Should_return_true___When_both_sequences_are_null()
        {
            // Arrange, Act
            var actual1 = EnumerableExtensions.SequenceEqualHandlingNulls<object>(null, null, null);
            var actual2 = EnumerableExtensions.SequenceEqualHandlingNulls(null, null, EqualityComparer<object>.Default);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second_comparer___Should_return_true___When_one_but_not_both_sequences_are_null()
        {
            // Arrange
            var notNullSequence = A.Dummy<List<string>>();

            // Act
            var actual1 = EnumerableExtensions.SequenceEqualHandlingNulls<object>(notNullSequence, null, null);
            var actual2 = EnumerableExtensions.SequenceEqualHandlingNulls<object>(null, notNullSequence, EqualityComparer<object>.Default);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second_comparer___Should_return_same_result_as_SequenceEqual_using_default_EqualityComparer___When_both_sequences_are_not_null_and_comparer_is_null()
        {
            // Arrange
            var sequence1 = new[] { "abc", "def" };
            var sequence2 = new[] { "abc", "def" };
            var sequence3 = new[] { "abc", "def", "ghi" };
            var sequence4 = new string[0];
            var sequence5 = new string[0];
            var sequence6 = new[] { "aBc", "dEf" };

            // Act
            var actual1 = sequence1.SequenceEqual(sequence2, null);
            var actual2 = sequence2.SequenceEqual(sequence3, null);
            var actual3 = sequence3.SequenceEqual(sequence2, null);
            var actual4 = sequence4.SequenceEqual(sequence5, null);
            var actual5 = sequence1.SequenceEqual(sequence6, null);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeTrue();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void SequenceEqualsHandlingNulls_first_second_comparer___Should_return_same_result_as_SequenceEqual___When_both_sequences_are_not_null()
        {
            // Arrange
            var sequence1 = new[] { "abc", "def" };
            var sequence2 = new[] { "abc", "def" };
            var sequence3 = new[] { "abc", "def", "ghi" };
            var sequence4 = new string[0];
            var sequence5 = new string[0];
            var sequence6 = new[] { "aBc", "dEf" };

            // Act
            var actual1 = sequence1.SequenceEqual(sequence2, StringComparer.CurrentCultureIgnoreCase);
            var actual2 = sequence2.SequenceEqual(sequence3, StringComparer.CurrentCultureIgnoreCase);
            var actual3 = sequence3.SequenceEqual(sequence2, StringComparer.CurrentCultureIgnoreCase);
            var actual4 = sequence4.SequenceEqual(sequence5, StringComparer.CurrentCultureIgnoreCase);
            var actual5 = sequence1.SequenceEqual(sequence6, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeTrue();
            actual5.Should().BeTrue();
        }
    }
}
