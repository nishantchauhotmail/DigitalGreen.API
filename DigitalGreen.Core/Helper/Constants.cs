using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalGreen.Core.Helper
{
    public class Constants
    {

        public static class Url
        {
            public static string WebApiUrlWithoutSlash = ConfigurationManager.AppSettings[nameof(WebApiUrlWithoutSlash)];
        }

        public static class Common
        {
            public static string AppName = ConfigurationManager.AppSettings[nameof(AppName)];
        }



        public static class StrMessage
        {
            public static string InValidAccess = "Invalid access details, Please log-out.";

        }



        public static class ErrorDisplay
        {
            public static bool IsErrorDisplay = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["IsErrorDisplay"]);
        }
    }
}
