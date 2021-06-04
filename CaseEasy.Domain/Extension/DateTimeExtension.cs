using System;

namespace CaseEasy.Domain.Extension
{
    public static class DateTimeExtension
    {
        public static double MonthDifference(this DateTime date, DateTime otherDate)
        {
            return 12 * (date.Year - otherDate.Year) + date.Month - otherDate.Month;
        }
    }
}
