using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace GTGrimServer.Utils
{
    public static class DateTimeExtensions
    {
        private static readonly string[] _formats = new string[] { "yyyy-MM-ddTHH:mm:ssK", "yyyy-MM-ddTHH:mm:ss.ffK", "yyyy-MM-ddTHH:mm:ssZ", "yyyy-MM-ddTHH:mm:ss.ffZ", "yyyy-MM-dd'T'HH:mm:ss.fffzzz" };

        public static string ToRfc3339String(this DateTime dt)
            => dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);

        public static DateTime FromRfc3339String(string dtString)
        {
            DateTime.TryParseExact(dtString, _formats,
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out DateTime dt); // Will be defaulted if incorrect 
            return dt;
        }
    }
}
