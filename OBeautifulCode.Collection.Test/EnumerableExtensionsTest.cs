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
    using System.Collections.ObjectModel;
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
        public static void SymmetricDifference___Should_throw_ArgumentNullException___When_first_set_is_null()
        {
            // Arrange
            var secondSet = new ArrayList { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SymmetricDifference(null, secondSet));
        }

        [Fact]
        public static void SymmetricDifference___Should_throw_ArgumentNullException___When_second_set_is_null()
        {
            // Arrange
            var firstSet = new ArrayList { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => firstSet.SymmetricDifference(null));
        }

        [Fact]
        public static void SymmetricDifference___Should_return_empty_set___When_both_sets_are_empty()
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
        public static void SymmetricDifference___Should_return_empty_set___When_both_sets_are_the_same()
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
        public static void SymmetricDifference___Should_return_values_in_nonempty_set___When_one_set_is_empty()
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
        public static void SymmetricDifference___Should_return_the_symmetric_difference___When_both_sets_have_one_or_more_differences()
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
        public static void SymmetricDifference___Should_return_the_symmetric_difference_with_only_one_copy_of_duplicate_items___When_one_set_has_duplicate_items()
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
        public static void SymmetricDifference_of_TSource___Should_throw_ArgumentNullException___When_first_set_is_null()
        {
            // Arrange
            var secondSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => EnumerableExtensions.SymmetricDifference(null, secondSet, StringComparer.CurrentCulture));
        }

        [Fact]
        public static void SymmetricDifference_of_TSource___Should_throw_ArgumentNullException___When_second_set_is_null()
        {
            // Arrange
            var firstSet = new List<string> { "abc" };

            // Act, Assert
            Assert.Throws<ArgumentNullException>(() => firstSet.SymmetricDifference(null, StringComparer.CurrentCulture));
        }

        [Fact]
        public static void SymmetricDifference_of_TSource___Should_return_empty_set___When_both_sets_are_empty()
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
        public static void SymmetricDifference_of_TSource___Should_return_empty_set___When_sets_are_the_same()
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
        public static void SymmetricDifference_of_TSource___Should_return_values_in_nonempty_set___When_one_set_is_empty()
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
        public static void SymmetricDifference_of_TSource___Should_return_the_symmetric_difference___When_sets_have_one_or_more_differences()
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
        public static void SymmetricDifference_of_TSource___Should_return_symmetric_difference_using_case_insensitive_comparisons___When_called_with_case_insensitive_comparer()
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
        public static void SymmetricDifference_of_TSource___Should_return_symmetric_difference_with_only_one_copy_of_duplicate_items___When_one_set_has_duplicate_items()
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
        public static void SymmetricDifference_of_TSource___Should_return_symmetric_difference___When_sets_contains_IEnumerables_themselves()
        {
            // Arrange

            // sub-enumerables have same items in same order
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have items in different order and order matters
            var item2a = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item2b = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have same items in different order, but order doesn't matter
            var item3a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", null, "pqr" },
            };
            var item3b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr", null },
            };

            // sub-enumerables have different items in different order, but order doesn't matter
            var item4a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item4b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // Act
            var actual1 = item1a.SymmetricDifference(item1b);
            var actual2 = item2a.SymmetricDifference(item2b);
            var actual3 = item3a.SymmetricDifference(item3b);
            var actual4 = item4a.SymmetricDifference(item4b);

            // Assert
            actual1.Should().BeEmpty();

            actual2.Should().HaveCount(2);
            actual2.First().Should().Equal("ghi", "jkl");
            actual2.Last().Should().Equal("jkl", "ghi");

            actual3.Should().BeEmpty();

            actual4.Should().HaveCount(2);
            actual4.First().Should().Equal("ghi", "jkl");
            actual4.Last().Should().Equal("ghi", "jkl", "ghi");
        }

        [Fact]
        public static void SymmetricDifference_of_TSource___Should_return_symmetric_difference___When_sets_contains_dictionaries()
        {
            // Arrange

            // same dictionaries
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };

            // different dictionaries
            var item2a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "5" },
                },
            };
            var item2b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "4" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };

            // Act
            var actual1 = item1a.SymmetricDifference(item1b);
            var actual2 = item2a.SymmetricDifference(item2b);

            // Assert
            actual1.Should().BeEmpty();

            actual2.Should().HaveCount(2);
            actual2.First().Should().Equal(
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "5" },
                });
            actual2.Last().Should().Equal(
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "4" },
                });
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_both_sequences_are_null()
        {
            // Arrange, Act
            var actual = EnumerableExtensions.SymmetricDifferenceEqual<object>(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_one_but_not_both_sequences_are_null()
        {
            // Arrange
            var notNullSequence = A.Dummy<List<string>>();

            // Act
            var actual1 = EnumerableExtensions.SymmetricDifferenceEqual(notNullSequence, null);
            var actual2 = EnumerableExtensions.SymmetricDifferenceEqual(null, notNullSequence);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_both_sets_are_empty()
        {
            // Arrange
            var item1 = new List<string>();
            var item2 = new List<string>();

            // Act
            var actual = item1.SymmetricDifferenceEqual(item2, StringComparer.CurrentCulture);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_sets_are_the_same()
        {
            // Arrange
            var item1a = new List<string> { "abc" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "abc", "DEF" };

            var item3a = new List<string> { "DEF", "abc" };
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl", "abc", "ghi" };
            var item4b = new List<string> { "ghi", "abc", "jkl", "def", "jkl" };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b);
            var actual3 = item3a.SymmetricDifferenceEqual(item3b);
            var actual4 = item4a.SymmetricDifferenceEqual(item4b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
            actual4.Should().BeTrue();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_false___When_one_set_is_empty_and_the_other_is_not()
        {
            // Arrange
            var item1a = new List<string>();
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string>();

            var item3a = new List<string>();
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item4b = new List<string>();

            var item5a = new List<string>();
            var item5b = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b);
            var actual3 = item3a.SymmetricDifferenceEqual(item3b);
            var actual4 = item4a.SymmetricDifferenceEqual(item4b);
            var actual5 = item5a.SymmetricDifferenceEqual(item5b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_false___When_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<string> { "def" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "ghi" };

            var item3a = new List<string> { "DEF" };
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item4b = new List<string> { "pqr", "def", "jkl", "mno" };

            var item5a = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var item5b = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b);
            var actual3 = item3a.SymmetricDifferenceEqual(item3b);
            var actual4 = item4a.SymmetricDifferenceEqual(item4b);
            var actual5 = item5a.SymmetricDifferenceEqual(item5b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_false___When_called_with_case_insensitive_comparer_and_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<string> { "def" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "ghi" };

            var item3a = new List<string> { "DEF" };
            var item3b = new List<string> { "abc", "def" };

            var item4a = new List<string> { "abc", "DEF", "ghi", "jkl" };
            var item4b = new List<string> { "pqr", "def", "JKL", "mno" };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b, StringComparer.CurrentCultureIgnoreCase);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b, StringComparer.CurrentCultureIgnoreCase);
            var actual3 = item3a.SymmetricDifferenceEqual(item3b, StringComparer.CurrentCultureIgnoreCase);
            var actual4 = item4a.SymmetricDifferenceEqual(item4b, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_called_with_case_insensitive_comparer_and_sets_are_equal()
        {
            // Arrange
            var item1a = new List<string> { "ABC", "DEF", "GHI", "JKL", "abc" };
            var item1b = new List<string> { "abc", "def", "ghi", "jkl", "ghi" };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_false___When_sets_contain_IEnumerables_themselves_and_sets_have_one_or_more_differences()
        {
            // Arrange

            // sub-enumerables have items in different order and order matters
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have different items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_sets_contain_IEnumerables_themselves_and_sets_are_equal()
        {
            // Arrange

            // sub-enumerables have same items in same order, and order matters
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
                new List<string> { "mno", "pqr" },
                new List<string> { "ghi", "jkl" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
                new List<string> { "abc", null, "def" },
                new List<string> { "abc", null, "def" },
            };

            // sub-enumerables have same items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", null, "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr", null },
            };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);
            var actual2 = item2a.SymmetricDifferenceEqual(item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_false___When_sets_contain_dictionaries_and_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "5" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "4" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);

            // Assert
            actual1.Should().BeFalse();
        }

        [Fact]
        public static void SymmetricDifferenceEqual___Should_return_true___When_sets_contain_dictionaries_and_sets_are_equal()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "ghi", "jkl" },
                    { "abc", "def" },
                },
            };

            // Act
            var actual1 = item1a.SymmetricDifferenceEqual(item1b);

            // Assert
            actual1.Should().BeTrue();
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
        public static void SequenceEqualHandlingNulls___Should_return_true___When_both_sequences_are_null()
        {
            // Arrange, Act
            var actual = EnumerableExtensions.SequenceEqualHandlingNulls<object>(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualHandlingNulls___Should_return_true___When_one_but_not_both_sequences_are_null()
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
        public static void SequenceEqualHandlingNulls___Should_return_same_result_as_SequenceEqual___When_both_sequences_are_not_null()
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
        public static void SequenceEqualsHandlingNulls___Should_return_same_result_as_SequenceEqual___When_both_sequences_are_not_null_and_equality_comparer_is_specified()
        {
            // Arrange
            var sequence1 = new[] { "abc", "def" };
            var sequence2 = new[] { "abc", "def" };
            var sequence3 = new[] { "abc", "def", "ghi" };
            var sequence4 = new string[0];
            var sequence5 = new string[0];
            var sequence6 = new[] { "aBc", "dEf" };

            // Act
            var actual1 = sequence1.SequenceEqualHandlingNulls(sequence2, StringComparer.CurrentCultureIgnoreCase);
            var actual2 = sequence2.SequenceEqualHandlingNulls(sequence3, StringComparer.CurrentCultureIgnoreCase);
            var actual3 = sequence3.SequenceEqualHandlingNulls(sequence2, StringComparer.CurrentCultureIgnoreCase);
            var actual4 = sequence4.SequenceEqualHandlingNulls(sequence5, StringComparer.CurrentCultureIgnoreCase);
            var actual5 = sequence1.SequenceEqualHandlingNulls(sequence6, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeTrue();
            actual5.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualHandlingNulls___Should_return_false___When_sets_contains_IEnumerables_and_sets_are_not_equal()
        {
            // sub-enumerables have items in different order and order matters
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have different items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // Act
            var actual1 = item1a.SequenceEqualHandlingNulls(item1b);
            var actual2 = item2a.SequenceEqualHandlingNulls(item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void SequenceEqualHandlingNulls___Should_return_true___When_sets_contain_IEnumerables_and_sets_are_equal()
        {
            // Arrange

            // sub-enumerables have same items in same order
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have same items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", null, "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "def", "abc" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr", null },
            };

            // Act
            var actual1 = item1a.SequenceEqualHandlingNulls(item1b);
            var actual2 = item2a.SequenceEqualHandlingNulls(item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void SequenceEqualHandlingNulls___Should_return_false___When_sets_contain_dictionaries_and_sets_are_not_equal()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "5" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "4" },
                },
            };

            // Act
            var actual1 = item1a.SequenceEqualHandlingNulls(item1b);

            // Assert
            actual1.Should().BeFalse();
        }

        [Fact]
        public static void SequenceEqualHandlingNulls___Should_return_false___When_sets_contain_dictionaries_and_sets_are_equal()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "ghi", "jkl" },
                    { "abc", "def" },
                },
                new Dictionary<string, string>
                {
                    { "1", "2" },
                    { "3", "4" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "3", "4" },
                    { "1", "2" },
                },
            };

            // Act
            var actual1 = item1a.SequenceEqualHandlingNulls(item1b);

            // Assert
            actual1.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_both_dictionaries_are_null()
        {
            // Arrange
            IReadOnlyDictionary<string, string> item1 = null;
            IReadOnlyDictionary<string, string> item2 = null;

            // Act
            var actual = EnumerableExtensions.DictionaryEqual(item1, item2);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_one_but_not_both_dictionaries_are_null()
        {
            // Arrange
            var notNullDictionary = A.Dummy<IReadOnlyDictionary<string, string>>();

            // Act
            var actual1 = EnumerableExtensions.DictionaryEqual<string, string>(notNullDictionary, null);
            var actual2 = EnumerableExtensions.DictionaryEqual<string, string>(null, notNullDictionary);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_false___When_dictionaries_are_not_equal()
        {
            // Arrange
            IReadOnlyDictionary<string, string> dictionary1a = new Dictionary<string, string>();

            IReadOnlyDictionary<string, string> dictionary1b = new Dictionary<string, string>
            {
                { "abc", "abc" },
            };

            IReadOnlyDictionary<string, string> dictionary2a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary2b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "aaa", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary3a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary3b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary4a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary4b = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "aaa" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary5a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary5b = new Dictionary<string, string>
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
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_dictionaries_are_equal()
        {
            // Arrange
            IReadOnlyDictionary<string, string> dictionary1a = new Dictionary<string, string>();
            IReadOnlyDictionary<string, string> dictionary1b = new Dictionary<string, string>();

            IReadOnlyDictionary<string, string> dictionary2a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary2b = new Dictionary<string, string>
            {
                { "mno", "pqr" },
                { "ghi", "jkl" },
                { "abc", "def" },
            };

            IReadOnlyDictionary<string, string> dictionary3a = new Dictionary<string, string>
            {
                { "abc", "def" },
                { "ghi", "jkl" },
                { "mno", "pqr" },
            };

            IReadOnlyDictionary<string, string> dictionary3b = new Dictionary<string, string>
            {
                { "mno", "PQR" },
                { "ghi", "JKL" },
                { "abc", "DEF" },
            };

            // Act
            var actual1 = dictionary1a.DictionaryEqual(dictionary1b);
            var actual2 = dictionary2a.DictionaryEqual(dictionary2b);
            var actual3 = dictionary3a.DictionaryEqual(dictionary3b, StringComparer.OrdinalIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_false___When_dictionaries_are_not_equal_and_values_are_dictionaries_themselves()
        {
            // Arrange

            // inner dictionary has different values
            IReadOnlyDictionary<string, IDictionary<string, string>> item1a = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "6" },
                    }
                },
            };
            IReadOnlyDictionary<string, IDictionary<string, string>> item1b = new Dictionary<string, IDictionary<string, string>>()
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "7" },
                    }
                },
            };

            // inner dictionary has different keys
            IReadOnlyDictionary<string, IDictionary<string, string>> item2a = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "6" },
                    }
                },
            };
            IReadOnlyDictionary<string, IDictionary<string, string>> item2b = new Dictionary<string, IDictionary<string, string>>()
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "4", "6" },
                    }
                },
            };

            // inner dictionary is null in one but not the other
            IReadOnlyDictionary<string, IDictionary<string, string>> item3a = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    null
                },
            };
            IReadOnlyDictionary<string, IDictionary<string, string>> item3b = new Dictionary<string, IDictionary<string, string>>()
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "6" },
                    }
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);
            var actual3 = item3a.DictionaryEqual(item3b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_dictionaries_are_equal_and_values_are_dictionaries_themselves()
        {
            // Arrange
            // mix-up order of key/value pairs in dictionaries
            IReadOnlyDictionary<string, IDictionary<string, string>> item1a = new Dictionary<string, IDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "6" },
                    }
                },
            };
            IReadOnlyDictionary<string, IDictionary<string, string>> item1b = new Dictionary<string, IDictionary<string, string>>()
            {
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "5", "6" },
                        { "3", "4" },
                    }
                },
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "2", "3" },
                        { "1", "2" },
                    }
                },
            };

            // use different concrete types
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> item2a = new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    new Dictionary<string, string>
                    {
                        { "3", "4" },
                        { "5", "6" },
                    }
                },
            };
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> item2b = new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                {
                    "whatever1",
                    new ReadOnlyDictionary<string, string>(
                        new Dictionary<string, string>
                        {
                            { "1", "2" },
                            { "2", "3" },
                        })
                },
                {
                    "whatever2",
                    new ReadOnlyDictionary<string, string>(
                        new Dictionary<string, string>
                        {
                            { "3", "4" },
                            { "5", "6" },
                        })
                },
            };

            // null inner dictionaries
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> item3a = new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                {
                    "whatever1",
                    new Dictionary<string, string>
                    {
                        { "1", "2" },
                        { "2", "3" },
                    }
                },
                {
                    "whatever2",
                    null
                },
            };
            IReadOnlyDictionary<string, IReadOnlyDictionary<string, string>> item3b = new Dictionary<string, IReadOnlyDictionary<string, string>>
            {
                {
                    "whatever1",
                    new ReadOnlyDictionary<string, string>(
                        new Dictionary<string, string>
                        {
                            { "1", "2" },
                            { "2", "3" },
                        })
                },
                {
                    "whatever2",
                    null
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);
            var actual3 = item3a.DictionaryEqual(item3b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_false___When_dictionaries_are_not_equal_and_values_are_ordered_enumerables()
        {
            // Arrange

            // enumerables are ordered differently
            IReadOnlyDictionary<string, IList<string>> item1a = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, IList<string>> item1b = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "jkl", "ghi" }
                },
            };

            // enumerables have different elements
            IReadOnlyDictionary<string, IList<string>> item2a = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, IList<string>> item2b = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi" }
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_dictionaries_are_equal_and_values_are_ordered_enumerables()
        {
            // Arrange

            // different kinds of concrete ordered enumerables
            IReadOnlyDictionary<string, IList<string>> item1a = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, IList<string>> item1b = new Dictionary<string, IList<string>>
            {
                {
                    "whatever2",
                    new List<string> { "ghi", "jkl" }
                },
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
            };

            // null and empty enumerables
            IReadOnlyDictionary<string, IList<string>> item2a = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new string[0]
                },
                {
                    "whatever2",
                    null
                },
            };
            IReadOnlyDictionary<string, IList<string>> item2b = new Dictionary<string, IList<string>>
            {
                {
                    "whatever1",
                    new List<string>()
                },
                {
                    "whatever2",
                    null
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_false___When_dictionaries_are_not_equal_and_values_are_not_ordered_enumerables()
        {
            // Arrange

            // enumerables have different elements
            IReadOnlyDictionary<string, IReadOnlyCollection<string>> item1a = new Dictionary<string, IReadOnlyCollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, IReadOnlyCollection<string>> item1b = new Dictionary<string, IReadOnlyCollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi" }
                },
            };

            // enumerables have different elements
            IReadOnlyDictionary<string, ICollection<string>> item2a = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, ICollection<string>> item2b = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl", "ghi" }
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void DictionaryEqual_of_IReadOnlyDictionary___Should_return_true___When_dictionaries_are_equal_and_values_are_not_ordered_enumerables()
        {
            // Arrange

            // different kinds of concrete ordered enumerables
            IReadOnlyDictionary<string, ICollection<string>> item1a = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, ICollection<string>> item1b = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new List<string> { "ghi", "jkl" }
                },
            };

            // null and empty enumerables
            IReadOnlyDictionary<string, IReadOnlyCollection<string>> item2a = new Dictionary<string, IReadOnlyCollection<string>>
            {
                {
                    "whatever1",
                    new string[0]
                },
                {
                    "whatever2",
                    null
                },
            };
            IReadOnlyDictionary<string, IReadOnlyCollection<string>> item2b = new Dictionary<string, IReadOnlyCollection<string>>
            {
                {
                    "whatever1",
                    new List<string>()
                },
                {
                    "whatever2",
                    null
                },
            };

            // elements in different order
            IReadOnlyDictionary<string, ICollection<string>> item3a = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever1",
                    new[] { "abc", "def" }
                },
                {
                    "whatever2",
                    new[] { "ghi", "jkl" }
                },
            };
            IReadOnlyDictionary<string, ICollection<string>> item3b = new Dictionary<string, ICollection<string>>
            {
                {
                    "whatever2",
                    new[] { "jkl", "ghi" }
                },
                {
                    "whatever1",
                    new[] { "def", "abc" }
                },
            };

            // Act
            var actual1 = item1a.DictionaryEqual(item1b);
            var actual2 = item2a.DictionaryEqual(item2b);
            var actual3 = item3a.DictionaryEqual(item3b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_both_sequences_are_null()
        {
            // Arrange, Act
            var actual = EnumerableExtensions.UnorderedEqual<object>(null, null);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_one_but_not_both_sequences_are_null()
        {
            // Arrange
            var notNullSequence = A.Dummy<List<string>>();

            // Act
            var actual1 = EnumerableExtensions.UnorderedEqual(notNullSequence, null);
            var actual2 = EnumerableExtensions.UnorderedEqual(null, notNullSequence);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_both_sets_are_empty()
        {
            // Arrange
            var item1 = new List<string>();
            var item2 = new List<string>();

            // Act
            var actual = item1.UnorderedEqual(item2, StringComparer.CurrentCulture);

            // Assert
            actual.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_sets_are_the_same()
        {
            // Arrange
            var item1a = new List<string> { "abc" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "abc", "DEF" };

            var item3a = new List<string> { "DEF", "abc" };
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item4b = new List<string> { "ghi", "abc", "jkl", "def" };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);
            var actual2 = item2a.UnorderedEqual(item2b);
            var actual3 = item3a.UnorderedEqual(item3b);
            var actual4 = item4a.UnorderedEqual(item4b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
            actual3.Should().BeTrue();
            actual4.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_false___When_one_set_is_empty_and_the_other_is_not()
        {
            // Arrange
            var item1a = new List<string>();
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string>();

            var item3a = new List<string>();
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item4b = new List<string>();

            var item5a = new List<string>();
            var item5b = new List<string> { "abc", "def", "ghi", "jkl" };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);
            var actual2 = item2a.UnorderedEqual(item2b);
            var actual3 = item3a.UnorderedEqual(item3b);
            var actual4 = item4a.UnorderedEqual(item4b);
            var actual5 = item5a.UnorderedEqual(item5b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_false___When_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<string> { "def" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "ghi" };

            var item3a = new List<string> { "DEF" };
            var item3b = new List<string> { "abc", "DEF" };

            var item4a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item4b = new List<string> { "pqr", "def", "jkl", "mno" };

            var item5a = new List<string> { "ABC", "DEF", "GHI", "JKL" };
            var item5b = new List<string> { "abc", "def", "ghi", "jkl" };

            var item6a = new List<string> { "abc", "def", "ghi", "jkl" };
            var item6b = new List<string> { "abc", "def", "ghi", "jkl", "abc" };

            var item7a = new List<string> { "abc", "def", "ghi", "jkl", null };
            var item7b = new List<string> { "abc", "def", "ghi", "jkl" };

            var item8a = new List<string> { "abc", "def", "ghi", "jkl", null };
            var item8b = new List<string> { "abc", "def", "ghi", "jkl", null, null };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);
            var actual2 = item2a.UnorderedEqual(item2b);
            var actual3 = item3a.UnorderedEqual(item3b);
            var actual4 = item4a.UnorderedEqual(item4b);
            var actual5 = item5a.UnorderedEqual(item5b);
            var actual6 = item6a.UnorderedEqual(item6b);
            var actual7 = item7a.UnorderedEqual(item7b);
            var actual8 = item8a.UnorderedEqual(item8b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
            actual5.Should().BeFalse();
            actual6.Should().BeFalse();
            actual7.Should().BeFalse();
            actual8.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_false___When_called_with_case_insensitive_comparer_and_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<string> { "def" };
            var item1b = new List<string> { "abc" };

            var item2a = new List<string> { "abc", "DEF" };
            var item2b = new List<string> { "ghi" };

            var item3a = new List<string> { "DEF" };
            var item3b = new List<string> { "abc", "def" };

            var item4a = new List<string> { "abc", "DEF", "ghi", "jkl" };
            var item4b = new List<string> { "pqr", "def", "JKL", "mno" };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b, StringComparer.CurrentCultureIgnoreCase);
            var actual2 = item2a.UnorderedEqual(item2b, StringComparer.CurrentCultureIgnoreCase);
            var actual3 = item3a.UnorderedEqual(item3b, StringComparer.CurrentCultureIgnoreCase);
            var actual4 = item4a.UnorderedEqual(item4b, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
            actual3.Should().BeFalse();
            actual4.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_called_with_case_insensitive_comparer_and_sets_are_equal()
        {
            // Arrange
            var item1a = new List<string> { "ABC", "DEF", null, "GHI", "JKL", "abc", "ghI" };
            var item1b = new List<string> { "abc", "def", "gHi", "aBc", "jkl", "ghi", null };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b, StringComparer.CurrentCultureIgnoreCase);

            // Assert
            actual1.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_false___When_sets_contain_IEnumerables_themselves_and_sets_have_one_or_more_differences()
        {
            // Arrange

            // sub-enumerables have items in different order and order matters
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // sub-enumerables have different items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl", "ghi" },
                new List<string> { "mno", "pqr" },
            };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);
            var actual2 = item2a.UnorderedEqual(item2b);

            // Assert
            actual1.Should().BeFalse();
            actual2.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_sets_contain_IEnumerables_themselves_and_sets_are_equal()
        {
            // Arrange

            // sub-enumerables have same items in same order, and order matters
            var item1a = new List<List<string>>
            {
                new List<string> { "abc", null, "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", "pqr" },
                new List<string> { "ghi", "jkl" },
            };
            var item1b = new List<List<string>>
            {
                new List<string> { "mno", "pqr" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "abc", null, "def" },
            };

            // sub-enumerables have same items in different order, but order doesn't matter
            var item2a = new List<IReadOnlyCollection<string>>
            {
                new List<string> { "abc", "def" },
                new List<string> { "ghi", "jkl" },
                new List<string> { "mno", null, "pqr" },
            };
            var item2b = new List<IReadOnlyCollection<string>>
            {
                new List<string> { null, "mno", "pqr" },
                new List<string> { "jkl", "ghi" },
                new List<string> { "def", "abc" },
            };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);
            var actual2 = item2a.UnorderedEqual(item2b);

            // Assert
            actual1.Should().BeTrue();
            actual2.Should().BeTrue();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_false___When_sets_contain_dictionaries_and_sets_have_one_or_more_differences()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "5" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "3" },
                    { "2", "4" },
                },
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
            };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);

            // Assert
            actual1.Should().BeFalse();
        }

        [Fact]
        public static void UnorderedEqual___Should_return_true___When_sets_contain_dictionaries_and_sets_are_equal()
        {
            // Arrange
            var item1a = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "abc", "def" },
                    { "ghi", "jkl" },
                },
                new Dictionary<string, string>
                {
                    { "1", "2" },
                    { "3", "4" },
                },
            };
            var item1b = new List<IReadOnlyDictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "3", "4" },
                    { "1", "2" },
                },
                new Dictionary<string, string>
                {
                    { "ghi", "jkl" },
                    { "abc", "def" },
                },
            };

            // Act
            var actual1 = item1a.UnorderedEqual(item1b);

            // Assert
            actual1.Should().BeTrue();
        }
    }
}
