// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CodeGenTest.cs" company="OBeautifulCode">
//   Copyright (c) OBeautifulCode 2018. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace OBeautifulCode.Collection.Recipes.Test
{
    using System.Collections.Generic;
    using System.Text;
    using OBeautifulCode.Math.Recipes;
    using Xunit;
    using Xunit.Abstractions;
    using static System.FormattableString;

    public class CodeGenTest
    {
        private readonly ITestOutputHelper testOutputHelper;

        public CodeGenTest(
            ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Fact(Skip = "Generate code only.")]
        public void GenerateConditionalGroupByCode()
        {
            var truthTable = MathHelper.GenerateTruthTable(8, false);
            var tokenPrefix = "selector";
            var intToNameMap = new Dictionary<int, string>
                               {
                                   { 1, "One" },
                                   { 2, "Two" },
                                   { 3, "Three" },
                                   { 4, "Four" },
                                   { 5, "Five" },
                                   { 6, "Six" },
                                   { 7, "Seven" },
                                   { 8, "Eight" },
                               };
            var result = new StringBuilder();
            result.AppendLine("IEnumerable<Grouping<IReadOnlyList<object>, TItem>> result;");
            foreach (var row in truthTable)
            {
                /*
                    // This will print the truth table:
                    var rowString = row.Select(_ => _ ? "1":"0").ToDelimitedString(",");
                    this.testOutputHelper.WriteLine(rowString);
                */
                result.Append("else if (");
                for (int idx = 1;
                    idx <= row.Count;
                    idx++)
                {
                    result.Append(Invariant($"({tokenPrefix}{intToNameMap[idx]}"));

                    if (row[idx - 1])
                    {
                        result.Append(" != null)");
                    }
                    else
                    {
                        result.Append(" == null)");
                    }

                    if (idx != row.Count)
                    {
                        result.Append(" && ");
                    }
                }

                result.AppendLine(")");
                result.AppendLine("{");
                result.AppendLine("result = values");
                result.Append(".GroupBy(_ => new");
                result.Append(" { ");
                for (int idx = 1;
                    idx <= row.Count;
                    idx++)
                {
                    if (row[idx - 1])
                    {
                        var stringNumber = intToNameMap[idx];
                        result.Append(Invariant($"Key{stringNumber} = {tokenPrefix}{stringNumber}(_), "));
                    }
                }

                result.Append("}");
                result.AppendLine(")");
                result.Append(".Select(_ => new Grouping<IReadOnlyList<object>, TItem>(");
                result.Append("new object[]");
                result.Append(" {");
                for (int idx = 1;
                    idx <= row.Count;
                    idx++)
                {
                    if (row[idx - 1])
                    {
                        var stringNumber = intToNameMap[idx];
                        result.Append(Invariant($"_.Key.Key{stringNumber},"));
                    }
                }

                result.Append("}");
                result.AppendLine(", _))");
                result.AppendLine(".ToList();");
                result.AppendLine("}");
            }

            this.testOutputHelper.WriteLine(result.ToString());
        }
    }
}
