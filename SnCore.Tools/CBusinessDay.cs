using System;
using System.Collections.Generic;
using System.Text;

// http://www.codeproject.com/csharp/datetimelib.asp

namespace SnCore.Tools
{

    /// <summary>
    /// 
    /// Developed By : Gaurang Desai
    /// 
    /// Created On	: 29th May, 2005
    /// 
    /// Class Library that contains, Most Usefull function related to 
    /// DateTime manipulation. Used DateTime and TimeSpan class.
    /// All functions are Static and so the Class in Sealed.
    /// </summary>
    public sealed class CBusinessDay
    {

        /// <summary>
        /// Calulates Business Days within the given range of days.
        /// Start date and End date inclusive.
        /// Note : Startdate is Exclusive.
        /// </summary>
        /// <param name="startDate">Datetime object containing Starting Date</param>
        /// <param name="EndDate">Datetime object containing End Date</param>
        /// <param name="NoOfDayWeek">integer denoting No of Business Day in a week</param>
        /// <param name="DayType"> DayType=0 for Business Day and DayType=1 for WeekEnds </param>
        /// <returns></returns>
        public static double CalculateBDay(
         DateTime startDate,
         DateTime EndDate,
         int NoOfDayWeek, /* No of Working Day per week*/
         int DayType
         )
        {
            double iWeek, iDays, isDays, ieDays;
            TimeSpan T;
            T = EndDate - startDate;

            //*  Find the number of weeks between the dates. Subtract 1 */
            //  since we do not want to count the current week.	 * /

            iDays = (double)T.Days;

            iWeek = GetWeeks(startDate, EndDate) - 1;

            iDays = iWeek * NoOfDayWeek;
            //
            if (NoOfDayWeek == 5)
            {
                //-- If Saturday, Sunday is holiday
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                    isDays = 7 - (int)startDate.DayOfWeek;
                else
                    isDays = 7 - (int)startDate.DayOfWeek - 1;
            }
            else
            {
                //-- If Sunday is only Holiday
                isDays = 7 - (int)startDate.DayOfWeek;
            }

            //-- Calculate the days in the last week. These are not included in the
            //-- week calculation. Since we are starting with the end date, we only
            //-- remove the Sunday (datepart=1) from the number of days. If the end
            //-- date is Saturday, correct for this.


            if (NoOfDayWeek == 5)
            {
                if (EndDate.DayOfWeek == DayOfWeek.Saturday)
                    ieDays = (int)EndDate.DayOfWeek - 2;
                else
                    ieDays = (int)EndDate.DayOfWeek - 1;
            }
            else
            {
                ieDays = (int)EndDate.DayOfWeek - 1;
            }


            //-- Sum everything together.
            iDays = iDays + isDays + ieDays;

            if (DayType == 0)
                return iDays;
            else
                return T.Days - iDays;
        }

        /// <summary>
        /// Calculate Age on given date.
        /// Calculates as Years, Months and Days and return as concatenated string.
        /// </summary>
        /// <param name="DOB">Datetime object containing DOB value</param>
        /// <param name="OnDate">Datetime object containing given date, for which we need to calculate the age</param>
        /// <returns></returns>
        public static string Age(DateTime DOB, DateTime OnDate)
        {


            //Get the difference in terms of Month and Year.
            int sMonth, eMonth, sYear, eYear;
            double Months, Years;

            sMonth = DOB.Month;
            eMonth = OnDate.Month;
            sYear = DOB.Year;
            eYear = OnDate.Year;

            // calculate Year
            if (eMonth >= sMonth)
                Years = eYear - sYear;
            else
                Years = eYear - sYear - 1;

            //calculate Months
            if (eMonth >= sMonth)
                Months = eMonth - sMonth;
            else
                if (OnDate.Day > DOB.Day)
                    Months = (12 - sMonth) + eMonth - 1;
                else
                    Months = (12 - sMonth) + eMonth - 2;

            double tDays = 0;

            //calculate Days
            if (eMonth != sMonth && OnDate.Day != DOB.Day)
            {
                if (OnDate.Day > DOB.Day)
                    tDays = DateTime.DaysInMonth(OnDate.Year, OnDate.Month) - DOB.Day;
                else
                    tDays = DateTime.DaysInMonth(OnDate.Year, OnDate.Month - 1) - DOB.Day + OnDate.Day;
            }


            string strAge = Years + "/" + Months + "/" + tDays;
            return strAge;
        }


        /// <summary>
        /// Calculate weeks between starting date and ending date
        /// </summary>
        /// <param name="stdate"></param>
        /// <param name="eddate"></param>
        /// <returns></returns>
        public static int GetWeeks(DateTime stdate, DateTime eddate)
        {
            TimeSpan t = eddate - stdate;

            int iDays;

            if (t.Days < 7)
            {
                if (stdate.DayOfWeek > eddate.DayOfWeek)
                    return 1; //It is accross the week 
                else
                    return 0; // same week
            }
            else
            {
                iDays = t.Days - 7 + (int)stdate.DayOfWeek;
                int i = 0;
                int k = 0;
                for (i = 1; k < iDays; i++)
                {
                    k += 7;
                }
                if (i > 1 && eddate.DayOfWeek != DayOfWeek.Sunday) i -= 1;
                return i;
            }

        }

        /// <summary>
        /// Mimic the Implementation of DateDiff function of VB.Net.
        /// Note : Number of Year/Month is calculated as how many times you have crossed the boundry.
        /// e.g. if you say starting date is 29/01/2005 and 01/02/2005 the year will be 0,month will be 1.
        /// 
        /// </summary>
        /// <param name="datePart">specifies on which part of the date to calculate the difference </param>
        /// <param name="startDate">Datetime object containing the beginning date for the calculation</param>
        /// <param name="endDate">Datetime object containing the ending date for the calculation</param>
        /// <returns></returns>
        public static double DateDiff(string datePart, DateTime startDate, DateTime endDate)
        {
            //Get the difference in terms of TimeSpan
            TimeSpan T;
            T = endDate - startDate;

            //Get the difference in terms of Month and Year.
            int sMonth, eMonth, sYear, eYear;
            sMonth = startDate.Month;
            eMonth = endDate.Month;
            sYear = startDate.Year;
            eYear = endDate.Year;

            double Months, Years = 0;
            Months = eMonth - sMonth;

            Years = eYear - sYear;

            Months = Months + (Years * 12);
            switch (datePart.ToUpper())
            {
                case "WW":
                case "DW":
                    return (double)GetWeeks(startDate, endDate);

                case "MM":

                    return Months;

                case "YY":
                case "YYYY":
                    return Years;

                case "QQ":
                case "QQQQ":
                    //Difference in Terms of Quater
                    return Math.Ceiling((double)T.Days / 90.0);
                case "MI":
                case "N":
                    return T.TotalMinutes;
                case "HH":
                    return T.TotalHours;
                case "SS":
                    return T.TotalSeconds;
                case "MS":
                    return T.TotalMilliseconds;
                case "DD":
                default:
                    return T.Days;
            }
        }

        /// <summary>
        /// Occurrence of the day of week this month. 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static int GetDayOfWeekOccurrenceThisMonth(DateTime dt)
        {
            int result = 1;
            DateTime qt = dt.AddDays(-7);
            while (qt.Month == dt.Month)
            {
                result++;
                qt = qt.AddDays(-7);
            }
            return result;
        }

        /// <summary>
        /// Is last day of week occurrence this month.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsLastDayOfWeekOccurrenceThisMonth(DateTime dt)
        {
            return (dt.AddDays(7).Month != dt.Month);
        }
    }
}
