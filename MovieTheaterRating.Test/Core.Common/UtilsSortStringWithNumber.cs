using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core.Common.Utils;
using System.Linq;

namespace MovieTheaterRating.Test.Core.Common
{
    [TestClass]
    public class UtilsSortStringWithNumber
    {
        string[] codes = new[]
        {
            "NP/417A",
            "NP 416",
            "NP/418F",
            "NP/418C",
            "NP111",
            "NP112",
        };

        [TestMethod]
        public void TestStringWithSlashAndNumber()
        {
            var ordered = codes.OrderBy(c => new SortStringWithNumber(c).ToString()).ToArray();
            Assert.AreEqual("NP111", ordered.FirstOrDefault());
            Assert.AreEqual("NP/418F", ordered.LastOrDefault());
        }
    }
}
