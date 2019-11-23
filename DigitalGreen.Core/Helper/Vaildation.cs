using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GolfCentra.Core.Helper
{
    /// <summary>
    /// Common Vaildation Message For APP string,int email etc.
    /// </summary>
    public static class Vaildation
    {
        public static void TryValidate(this long value, string message)
        {
            if (value <= 0L)
                throw new Exception("" + message + " Value Cannot Be Zero Or Less Than Zero. Please Provide Valid Positive Integer.");
        }

        public static void TryValidate(this int value, string message)
        {
            if (value <= 0)
                throw new Exception("" + message + " Value Cannot Be Zero Or Less Than Zero. Please Provide Valid Positive Integer.");
        }

        public static string TryValidate(this string value, string message)
        {
            if (value == null)
                throw new Exception("" + message + " Value Cannot Be Blank. Please Provide The Value.");
            if (value.Trim().Equals(""))
                throw new Exception("Invalid " + message + " Value. Please Provide Valid Value.");
            return value.Trim();
        }

        public static string TryValidateNullSstring(this string value, string message)
        {
            if (value == null)
                throw new Exception("" + message + " Value Cannot Be Blank. Please Provide The Value.");
            return value.Trim();
        }

        public static string TryValidateEmail(this string value, string message)
        {
            if (value == null)
                throw new Exception("" + message + " Value Cannot Be Blank. Please Provide The Value.");


            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(value);
            if (!match.Success)
                throw new Exception("Invalid " + message + " Value. Please Provide Valid Value.");

            return value.Trim();
        }

        public static void TryValidate(this DateTime value, string message)
        {
            if (value == null)
                throw new Exception("" + message + " Value Cannot Be Blank. Please Provide The Date.");
            if (value.Year == 0001)
                throw new Exception("Invalid " + message + " Value. Please Provide Valid Date.");

        }


     
    }
}

