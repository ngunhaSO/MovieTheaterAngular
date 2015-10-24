using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using Core.Common.Extensions;

namespace MovieTheaterRating.Test.Core.Common
{
    [TestClass]
    public class ExtDateTime
    {
        DateTime futureTimeUS;
        CultureInfo us;
        DayOfWeek firstDayOfWeekUS;
        CalendarWeekRule calendarWeekRuleUS; //rule in us should be FirstDay

        [TestInitialize]
        public void Setup()
        {
            futureTimeUS = new DateTime(2015, 9, 1);
            us = CultureInfo.GetCultureInfo("en-us");
            firstDayOfWeekUS = us.DateTimeFormat.FirstDayOfWeek;
            calendarWeekRuleUS = us.DateTimeFormat.CalendarWeekRule;
        }

        [TestMethod]
        public void TestElapsed()
        {
            TimeSpan ts = futureTimeUS.Elapsed();
            Assert.AreEqual(-21, ts.Days);
        }

        [TestMethod]
        public void TestWeekOfYearParameterless()
        {
            int weeks = futureTimeUS.WeekOfYear();
            Assert.AreEqual(36, weeks);
        }

        [TestMethod]
        public void TestWeekOfYearOneParm()
        {
            CalendarWeekRule cwr = CalendarWeekRule.FirstDay; //indicate first week of the year starts on first day of the year
            int weeks = futureTimeUS.WeekOfYear(cwr);
            Assert.AreEqual(36, weeks);
            Assert.AreEqual(calendarWeekRuleUS, cwr);
        }

        [TestMethod]
        public void TestWeekOfYearOneParm_FirstFourDayWeekRule()
        {
            CalendarWeekRule cwr = CalendarWeekRule.FirstFourDayWeek; //indicate first week of the year is the first week with 4 or more days
            int weeks = futureTimeUS.WeekOfYear(cwr);
            Assert.AreEqual(35, weeks);
            Assert.AreNotEqual(cwr, calendarWeekRuleUS);
        }

        [TestMethod]
        public void TestWeekOfYearOneParm_FirstFullWeekRule()
        {
            CalendarWeekRule cwr = CalendarWeekRule.FirstFullWeek;
            int weeks = futureTimeUS.WeekOfYear(cwr);
            Assert.AreEqual(35, weeks);
            Assert.AreNotEqual(cwr, calendarWeekRuleUS);
        }
    }
}
