// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Recipes.Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using FakeItEasy;

    using FluentAssertions;

    using OBeautifulCode.Assertion.Recipes;
    using OBeautifulCode.AutoFakeItEasy;
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
            actual1.Message.Should().Contain("'minimumItems' < '1'");

            actual2.Should().BeOfType<ArgumentOutOfRangeException>();
            actual2.Message.Should().Contain("'minimumItems' < '1'");
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
        public static void GetLongestCommonPrefix___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => EnumerableExtensions.GetLongestCommonPrefix(null));

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void GetLongestCommonPrefix___Should_throw_ArgumentException___When_parameter_values_is_empty()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new string[0].GetLongestCommonPrefix());

            // Assert
            actual.Should().BeOfType<ArgumentException>();
            actual.Message.Should().Contain("values is empty");
        }

        [Fact]
        public static void GetLongestCommonPrefix___Should_return_single_element___When_parameter_values_has_one_element()
        {
            // Arrange
            var expected = new[]
            {
                null,
                string.Empty,
                A.Dummy<string>(),
            };

            // Act
            var actual = expected.Select(_ => new[] { _ }.GetLongestCommonPrefix()).ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void GetLongestCommonPrefix___Should_return_null___When_parameter_values_contains_one_or_more_null_elements()
        {
            // Arrange
            var expected = new[]
            {
                A.Dummy<string>(),
                A.Dummy<string>(),
                null,
            };

            // Act
            var actual = expected.GetLongestCommonPrefix();

            // Assert
            actual.AsTest().Must().BeNull();
        }

        [Fact]
        public static void GetLongestCommonPrefix___Should_return_empty_string___When_parameter_values_contains_no_common_prefix()
        {
            // Arrange
            var values = new[]
            {
                new[]
                {
                    "abc",
                    "Abc",
                },
                new[]
                {
                    "abc",
                    "def",
                    "ghi",
                },
            };

            // Act
            var actual = values.Select(_ => _.GetLongestCommonPrefix()).ToList();

            // Assert
            actual.AsTest().Must().Each().BeEqualTo(string.Empty);
        }

        [Fact]
        public static void GetLongestCommonPrefix___Should_return_longest_common_prefix___When_there_is_some_common_prefix()
        {
            // Arrange
            var values = new[]
            {
                new[]
                {
                    "aA",
                    "aa",
                },
                new[]
                {
                    "h:/a/b/c",
                    "h:/a/b/d",
                    "h:/a/b/e",
                    "h:/a/c",
                },
            };

            var expected = new[]
            {
                "a",
                "h:/a/",
            };

            // Act
            var actual = values.Select(_ => _.GetLongestCommonPrefix()).ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(expected);
        }

        [Fact]
        public static void RandomizeElements___Should_throw_ArgumentNullException___When_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => EnumerableExtensions.RandomizeElements<int>(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
        }

        [Fact]
        public static void RandomizeElements___Should_return_empty_enumerable___When_value_is_empty()
        {
            // Arrange
            var value = new bool[0];

            // Act
            var actual = value.RandomizeElements();

            // Assert
            actual.AsTest().Must().BeEmptyEnumerable();
        }

        [Fact]
        public static void RandomizeElements___Should_return_same_enumerable___When_value_has_one_element()
        {
            // Arrange
            var value = Some.ReadOnlyDummies<string>(1).ToList();

            // Act
            var actual = value.RandomizeElements().ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(value);
        }

        [Fact]
        public static void RandomizeElements___Should_elements_in_random_order___When_value_has_more_than_one_element()
        {
            // Arrange
            var values = new IReadOnlyCollection<string>[]
            {
                Some.ReadOnlyDummies<string>(2),
                Some.ReadOnlyDummies<string>(3),
                Some.ReadOnlyDummies<string>(4),
            };

            // Act
            var actual = values.Select(_ => (IReadOnlyCollection<string>)_.RandomizeElements().ToList()).ToArray();

            // Assert
            actual.AsTest().Must().BeEqualTo(values);
        }

        [Fact]
        public static void RemoveRange___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => EnumerableExtensions.RemoveRange(null, new[] { 1 }));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("value");
        }

        [Fact]
        public static void RemoveRange___Should_throw_ArgumentNullException___When_parameter_itemsToRemove_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => new[] { 1 }.RemoveRange(null));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Must().ContainString("itemsToRemove");
        }

        [Fact]
        public static void RemoveRange___Should_remove_items_in_itemsToRemove_using_specified_comparer___When_throwIfNotFound_is_false()
        {
            // Arrange, Act
            var scenarios = new[]
            {
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "B" }, Expected = new[] { "A", "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "b", }, Expected = new[] { "A", "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "B", "b" }, Expected = new[] { "A", "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "b", "B" }, Expected = new[] { "A", "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "a" }, Expected = new[] { "B", "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "c" }, Expected = new[] { "A", "B" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new[] { "D" }, Expected = new[] { "A", null, "C" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new string[] { null }, Expected = new[] { "A", "C" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new[] { null, "a" }, Expected = new[] { "C" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new[] { null, "c" }, Expected = new[] { "A" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new[] { "a", null }, Expected = new[] { "C" } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new[] { "c", null }, Expected = new[] { "A" } },
                new { Value = new string[] { }, ItemsToRemove = new[] { "c", null, "B" }, Expected = new string[] { } },
                new { Value = new[] { "A", null, "C" }, ItemsToRemove = new string[] { }, Expected = new[] { "A", null, "C" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "a", "b", "c" }, Expected = new string[] { } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "C", "B", "A" }, Expected = new string[] { } },
                new { Value = new[] { "A", "b", "C", "B", "a" }, ItemsToRemove = new[] { "B" }, Expected = new[] { "A", "C", "B", "a" } },
                new { Value = new[] { "A", "b", "C", "B", "a" }, ItemsToRemove = new[] { "a" }, Expected = new[] { "b", "C", "B", "a" } },
            };

            // Assert
            var actual = scenarios.Select(_ => _.Value.RemoveRange(_.ItemsToRemove, StringComparer.OrdinalIgnoreCase)).ToList();

            // Assert
            actual.AsTest().Must().BeEqualTo(scenarios.Select(_ => (IEnumerable<string>)_.Expected).ToList());
        }

        [Fact]
        public static void RemoveRange___Should_throw_InvalidOperationException___When_throwIfNotFound_is_true_and_an_item_to_remove_was_not_found()
        {
            // Arrange, Act
            var scenarios = new[]
            {
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "d" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "B", "d" } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new string[] { null } },
                new { Value = new[] { "A", "B", "C" }, ItemsToRemove = new[] { "B", "B" } },
                new { Value = new[] { "A", "B", null, "C" }, ItemsToRemove = new string[] { null, null } },
                new { Value = new[] { "A", "B", "C", "A" }, ItemsToRemove = new[] { "A", "A", "a" } },
            };

            // Assert
            var actual = scenarios.Select(_ => Record.Exception(() => _.Value.RemoveRange(_.ItemsToRemove, StringComparer.OrdinalIgnoreCase, throwIfNotFound: true))).ToList();

            // Assert
            actual.AsTest().Must().Each().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            var lengthPerChunk = A.Dummy<int>().ThatIs(_ => _ > 0);

            // Act
            var actual = Record.Exception(() => EnumerableExtensions.SplitIntoChunksOfLength<string>(null, lengthPerChunk));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.AsTest().Must().ContainString("value");
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_throw_ArgumentOutOfRangeException___When_parameter_lengthPerChunk_is_less_than_or_equal_to_0()
        {
            // Arrange
            var value = A.Dummy<List<int>>();
            var lengthPerChunk = A.Dummy<NegativeInteger>();

            // Act
            var actual1 = Record.Exception(() => value.SplitIntoChunksOfLength(0));
            var actual2 = Record.Exception(() => value.SplitIntoChunksOfLength(lengthPerChunk));

            // Assert
            actual1.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual1.Message.AsTest().Must().ContainString("lengthPerChunk");

            actual2.AsTest().Must().BeOfType<ArgumentOutOfRangeException>();
            actual2.Message.AsTest().Must().ContainString("lengthPerChunk");
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_empty_list___When_value_is_an_empty_collection()
        {
            // Arrange
            var value = new List<int>();
            var lengthPerChunk = A.Dummy<PositiveInteger>();

            // Act
            var actual = value.SplitIntoChunksOfLength(lengthPerChunk);

            // Assert
            actual.AsTest().Must().BeEmptyEnumerable();
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_chunks___When_length_of_value_divides_evenly_by_lengthPerChunk()
        {
            // Arrange
            var value = new List<string>
            {
                " ",
                "s",
                "o",
                "m",
                "e",
                " ",
                "b",
                "r",
                "o",
                "w",
                "n",
                " ",
                "c",
                "o",
                "w",
                "s",
                " ",
                " ",
            };

            var lengthPerChunk1 = 1;
            IReadOnlyList<IReadOnlyList<string>> expected1 = new List<List<string>>
            {
                new List<string> { " " },
                new List<string> { "s" },
                new List<string> { "o" },
                new List<string> { "m" },
                new List<string> { "e" },
                new List<string> { " " },
                new List<string> { "b" },
                new List<string> { "r" },
                new List<string> { "o" },
                new List<string> { "w" },
                new List<string> { "n" },
                new List<string> { " " },
                new List<string> { "c" },
                new List<string> { "o" },
                new List<string> { "w" },
                new List<string> { "s" },
                new List<string> { " " },
                new List<string> { " " },
            };

            var lengthPerChunk2 = 6;
            IReadOnlyList<IReadOnlyList<string>> expected2 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                    "m",
                    "e",
                    " ",
                },
                new List<string>
                {
                    "b",
                    "r",
                    "o",
                    "w",
                    "n",
                    " ",
                },
                new List<string>
                {
                "c",
                "o",
                "w",
                "s",
                " ",
                " ",
                },
            };

            var lengthPerChunk3 = 9;
            IReadOnlyList<IReadOnlyList<string>> expected3 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                    "m",
                    "e",
                    " ",
                    "b",
                    "r",
                    "o",
                },
                new List<string>
                {
                    "w",
                    "n",
                    " ",
                    "c",
                    "o",
                    "w",
                    "s",
                    " ",
                    " ",
                },
            };

            var lengthPerChunk4 = 18;
            IReadOnlyList<IReadOnlyList<string>> expected4 = new[] { value };

            // Act
            var actual1 = value.SplitIntoChunksOfLength(lengthPerChunk1);
            var actual2 = value.SplitIntoChunksOfLength(lengthPerChunk2);
            var actual3 = value.SplitIntoChunksOfLength(lengthPerChunk3);
            var actual4 = value.SplitIntoChunksOfLength(lengthPerChunk4);

            // Assert
            actual1.AsTest().Must().BeEqualTo(expected1);
            actual2.AsTest().Must().BeEqualTo(expected2);
            actual3.AsTest().Must().BeEqualTo(expected3);
            actual4.AsTest().Must().BeEqualTo(expected4);
        }

        [Fact]
        public static void SplitIntoChunksOfLength___Should_return_chunks___When_length_of_value_does_not_divide_evenly_by_lengthPerChunk()
        {
            // Arrange
            var value = new List<string>
            {
                " ",
                "s",
                "o",
                "m",
                "e",
                " ",
                "b",
                "r",
                "o",
                "w",
                "n",
                " ",
                "c",
                "o",
                "w",
                "s",
                " ",
            };

            var lengthPerChunk1 = 3;
            IReadOnlyList<IReadOnlyList<string>> expected1 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                },
                new List<string>
                {
                    "m",
                    "e",
                    " ",
                },
                new List<string>
                {
                    "b",
                    "r",
                    "o",
                },
                new List<string>
                {
                    "w",
                    "n",
                    " ",
                },
                new List<string>
                {
                    "c",
                    "o",
                    "w",
                },
                new List<string>
                {
                    "s",
                    " ",
                },
            };

            var lengthPerChunk2 = 5;
            IReadOnlyList<IReadOnlyList<string>> expected2 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                    "m",
                    "e",
                },
                new List<string>
                {
                    " ",
                    "b",
                    "r",
                    "o",
                    "w",
                },
                new List<string>
                {
                    "n",
                    " ",
                    "c",
                    "o",
                    "w",
                },
                new List<string>
                {
                    "s",
                    " ",
                },
            };

            var lengthPerChunk3 = 9;
            IReadOnlyList<IReadOnlyList<string>> expected3 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                    "m",
                    "e",
                    " ",
                    "b",
                    "r",
                    "o",
                },
                new List<string>
                {
                    "w",
                    "n",
                    " ",
                    "c",
                    "o",
                    "w",
                    "s",
                    " ",
                },
            };

            var lengthPerChunk4 = 16;
            IReadOnlyList<IReadOnlyList<string>> expected4 = new List<List<string>>
            {
                new List<string>
                {
                    " ",
                    "s",
                    "o",
                    "m",
                    "e",
                    " ",
                    "b",
                    "r",
                    "o",
                    "w",
                    "n",
                    " ",
                    "c",
                    "o",
                    "w",
                    "s",
                },
                new List<string>
                {
                    " ",
                },
            };

            var lengthPerChunk5 = 18;
            IReadOnlyList<IReadOnlyList<string>> expected5 = new[] { value };

            var lengthPerChunk6 = int.MaxValue;
            IReadOnlyList<IReadOnlyList<string>> expected6 = new[] { value };

            // Act
            var actual1 = value.SplitIntoChunksOfLength(lengthPerChunk1);
            var actual2 = value.SplitIntoChunksOfLength(lengthPerChunk2);
            var actual3 = value.SplitIntoChunksOfLength(lengthPerChunk3);
            var actual4 = value.SplitIntoChunksOfLength(lengthPerChunk4);
            var actual5 = value.SplitIntoChunksOfLength(lengthPerChunk5);
            var actual6 = value.SplitIntoChunksOfLength(lengthPerChunk6);

            // Assert
            actual1.AsTest().Must().BeEqualTo(expected1);
            actual2.AsTest().Must().BeEqualTo(expected2);
            actual3.AsTest().Must().BeEqualTo(expected3);
            actual4.AsTest().Must().BeEqualTo(expected4);
            actual5.AsTest().Must().BeEqualTo(expected5);
            actual6.AsTest().Must().BeEqualTo(expected6);
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
        public static void ToNonGenericDictionary___Should_throw_ArgumentNullException___When_parameter_value_is_null()
        {
            // Arrange
            IDictionary<string, string> value = null;

            // Act
            var actual = Record.Exception(() => value.ToNonGenericDictionary());

            // Assert
            actual.Should().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("value");
        }

        [Fact]
        public static void ToNonGenericDictionary___Should_throw_ArgumentException___When_parameter_value_is_contains_duplicate_keys()
        {
            // Arrange
            var value = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("duplicate", A.Dummy<string>()),
                new KeyValuePair<string, string>(A.Dummy<string>(), A.Dummy<string>()),
                new KeyValuePair<string, string>("duplicate", A.Dummy<string>()),
            };

            // Act
            var actual = Record.Exception(() => value.ToNonGenericDictionary());

            // Assert
            actual.Should().BeOfType<ArgumentException>();
            actual.Message.Should().Contain("value contains duplicate keys");
        }

        [Fact]
        public static void ToNonGenericDictionary___Should_return_empty_IDictionary___When_parameter_value_is_empty()
        {
            // Arrange
            var value1 = new Dictionary<string, string>();
            var value2 = new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
            IDictionary<string, string> value3 = value1;
            IReadOnlyDictionary<string, string> value4 = value2;

            // Act
            var actual1 = value1.ToNonGenericDictionary();
            var actual2 = value2.ToNonGenericDictionary();
            var actual3 = value3.ToNonGenericDictionary();
            var actual4 = value4.ToNonGenericDictionary();

            // Assert
            actual1.Should().BeEmpty();
            actual2.Should().BeEmpty();
            actual3.Should().BeEmpty();
            actual4.Should().BeEmpty();
        }

        [Fact]
        public static void ToNonGenericDictionary___Should_return_IDictionary_with_key_value_pairs_equivalent_to_parameter_value___When_parameter_value_is_not_empty()
        {
            // Arrange
            var value1 = A.Dummy<Dictionary<string, string>>();
            var value2 = new ReadOnlyDictionary<int, string>(A.Dummy<Dictionary<int, string>>());
            IDictionary<string, string> value3 = value1;
            IReadOnlyDictionary<int, string> value4 = value2;

            // Act
            var actual1 = value1.ToNonGenericDictionary();
            var actual2 = value2.ToNonGenericDictionary();
            var actual3 = value3.ToNonGenericDictionary();
            var actual4 = value4.ToNonGenericDictionary();

            // Assert
            actual1.Keys.Should().BeEquivalentTo(value1.Keys);
            foreach (var key in actual1.Keys)
            {
                actual1[key].Should().Be(value1[(string)key]);
            }

            actual2.Keys.Should().BeEquivalentTo(value2.Keys);
            foreach (var key in actual2.Keys)
            {
                actual2[key].Should().Be(value2[(int)key]);
            }

            actual3.Keys.Should().BeEquivalentTo(value3.Keys);
            foreach (var key in actual3.Keys)
            {
                actual3[key].Should().Be(value3[(string)key]);
            }

            actual4.Keys.Should().BeEquivalentTo(value4.Keys);
            foreach (var key in actual4.Keys)
            {
                actual4[key].Should().Be(value4[(int)key]);
            }
        }
    }
}
