using System;

namespace ig.log4net.Extensions
{
    public static class DateTimeExtensions
    {

        public static bool HasAgeXAtDayY(this DateTime date, int x, DateTime y)
        {
            var xthBirthday = date.AddYears(x);
            return xthBirthday <= y;
        }

        public static int GetAgeAtDayX(this DateTime gebDat, DateTime dayX)
        {
            int alter = (dayX.Year - gebDat.Year);
            return gebDat.AddYears(alter) > dayX ? --alter : alter;
        }

        /// <summary>
        /// Get number of months from day Y to day X
        /// </summary>
        /// <param name="dayX"></param>
        /// <param name="dayY"></param>
        /// <returns></returns>
        public static int GetMonthsUntilX(this DateTime dayX, DateTime dayY) =>     dayY.Year * 12
                                                                                            + dayY.Month
                                                                                            - dayX.Year * 12
                                                                                            - dayX.Month;

    }
}
