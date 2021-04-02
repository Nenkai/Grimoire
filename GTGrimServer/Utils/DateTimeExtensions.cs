using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

namespace GTGrimServer.Utils
{
    public static class DateTimeExtensions
    {
        public static string ToRfc3339String(this DateTime dt)
            => dt.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo);
    }
}
