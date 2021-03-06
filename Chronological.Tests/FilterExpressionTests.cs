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
        [MemberData(nameof(FilterPredicateTestDataProvider.TestCases), MemberType = typeof(FilterPredicateTestDataProvider))]
        public void FilterPredicateTests(Expression<Func<TestType1, bool>> predicate, string expected)
        {
            var filter = Filter.Create(predicate);
            var result = filter.ToPredicateJProperty();

            var predicateString = GetPredicateString(result);

            Assert.Equal(expected, predicateString);
        }
    }

    public class FilterPredicateTestDataProvider
    {
        public static IEnumerable<object[]> TestCases
        {
            get
            {
                var testComparison = "12345";

                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value > 5), "([data.value] > 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value < 5), "([data.value] < 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value == 5), "([data.value] = 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value != 5), "([data.value] != 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value >= 5), "([data.value] >= 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Value <= 5), "([data.value] <= 5)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 > x.Value), "(4 > [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 < x.Value), "(4 < [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 == x.Value), "(4 = [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 != x.Value), "(4 != [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 >= x.Value), "(4 >= [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 <= x.Value), "(4 <= [data.value])" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 > x.Value && x.DataType == "AString"), "((4 > [data.value]) and ([data.type] = 'AString'))" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.DeviceDate > DateTime.UtcNow), "([data.devicedate] > utcNow())" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Date > DateTime.UtcNow), "($ts > utcNow())" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Date > (DateTime.UtcNow - TimeSpan.FromMinutes(55))), "($ts > (utcNow() - ts'P0Y0M0DT0H55M0.0S'))" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.Date > new DateTime(2018, 01, 27, 0, 0, 0, DateTimeKind.Utc)), "($ts > dt'2018-01-27T00:00:00.0000000Z')" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.IsSimulated == true), "([data.isSimulated] = TRUE)" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => 4 > x.Value && (x.DataType == "AString" || x.DataType == "AnotherString")), "((4 > [data.value]) and (([data.type] = 'AString') or ([data.type] = 'AnotherString')))" };
                yield return new object[] { (Expression<Func<TestType1, bool>>)(x => x.DataType == testComparison), "([data.type] = '12345')" };
            }
        }
    }
}
