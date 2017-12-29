﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Chronological.Tests
{
    public class FilterExpressionTests
    {
        private string GetPredicateString(JProperty jProperty)
        {
            return jProperty.SelectToken("$..predicateString").Value<string>();
        }

        [Theory]
        [MemberData(nameof(ConfigTestDataProvider.TestCases), MemberType = typeof(ConfigTestDataProvider))]
        public void Test1(Expression<Func<TestType1, bool>> predicate, string expected)
        {
            var filter = Filter.Create(predicate);
            var result = filter.ToPredicateJProperty();

            var predicateString = GetPredicateString(result);

            Assert.Equal(expected, predicateString);
        }
    }

    public class ConfigTestDataProvider
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value > 5), "[data.value] > 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value < 5), "[data.value] < 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value == 5), "[data.value] = 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value != 5), "[data.value] != 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value >= 5), "[data.value] >= 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value <= 5), "[data.value] <= 5" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 > x.Value), "4 > [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 < x.Value), "4 < [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 == x.Value), "4 = [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 != x.Value), "4 != [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 >= x.Value), "4 >= [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 <= x.Value), "4 <= [data.value]" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 > x.Value && x.DataType == "AString"), "4 > [data.value] and [data.type] = 'AString'" };
            }
        }
    }
}