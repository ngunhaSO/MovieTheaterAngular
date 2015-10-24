using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Utils;
using System.Linq;
using System.Text.RegularExpressions;

namespace MovieTheaterRating.Test.Core.Common
{
    [TestClass]
    public class UtilsEnumerableComparer
    {
        private string[] testItems = { "NP 417 ", "NP 418", "NP111", "NP112", "NP 31" };
        public UtilsEnumerableComparer()
        {

        }

        [TestMethod]
        public void TestSortStringWithNumber()
        {
            Func<string, object> convert = str =>
            {
                try { return int.Parse(str); }
                catch { return str; }
            };
            var sorted = testItems.OrderBy(
                str => Regex.Split(str.Replace(" ", ""), "([0-9]+)").Select(convert),
                new EnumerableComparer<object>());

            Assert.AreEqual("NP 31", sorted.FirstOrDefault());
            Assert.AreEqual("NP 418", sorted.LastOrDefault());
        }
    }
}
