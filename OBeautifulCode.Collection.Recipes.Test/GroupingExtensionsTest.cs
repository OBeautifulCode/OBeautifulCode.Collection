// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupingExtensionsTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Recipes.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using OBeautifulCode.Assertion.Recipes;
    using Xunit;

    public static class GroupingExtensionsTest
    {
        [Fact]
        public static void ConditionalGroupBy_TKeyOne___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyTwo___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyThree___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyFour___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyFive___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeySix___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _, _ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeySeven___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _, _ => _, _ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyEight___Should_throw_ArgumentNullException___When_parameter_values_is_null()
        {
            // Arrange, Act
            var actual = Record.Exception(() => ((IEnumerable<string>)null).ConditionalGroupBy(_ => _, _ => _, _ => _, _ => _, _ => _, _ => _, _ => _, _ => _));

            // Assert
            actual.AsTest().Must().BeOfType<ArgumentNullException>();
            actual.Message.Should().Contain("values");
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyOne___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyTwo___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyThree___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyFour___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyFive___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeySix___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeySeven___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyEight___Should_return_single_group_with_all_elements_of_parameter_value___When_all_selectors_are_null()
        {
            // Arrange
            var inputs = new[]
            {
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            Func<Tuple<string, string, string>, string> nullFunc = null;
            var groups = inputs.ConditionalGroupBy(nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc, nullFunc);

            // Assert
            groups.Count().MustForTest().BeEqualTo(1);
            groups.Single().ToList().MustForTest().BeEqualTo(inputs.ToList());
        }

        [Fact]
        public static void ConditionalGroupBy_TKeyOne___Should_return_expected_groups___When_selectors_are_defined()
        {
            // Arrange
            var group1Key = Guid.NewGuid().ToString();

            var group2Key = Guid.NewGuid().ToString();

            var inputs = new[]
            {
                Tuple.Create(group1Key, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(group2Key, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(group1Key, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
                Tuple.Create(group2Key, Guid.NewGuid().ToString(), Guid.NewGuid().ToString()),
            };

            // Act
            var groups = inputs.ConditionalGroupBy(_ => _.Item1);

            // Assert
            groups.AsTest().Must().HaveCount(2);

            foreach (var group in groups)
            {
                group.ToList().AsTest().Must().HaveCount(2);

                var expected = inputs.Where(_ => _.Item1 == group.Key.Single().ToString()).ToList();

                group.ToList().AsTest().Must().BeEqualTo(expected);
            }
        }
    }
}
