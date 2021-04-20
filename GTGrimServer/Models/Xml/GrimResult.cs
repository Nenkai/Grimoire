using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;

using System.Xml.Serialization;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("grim")]
    public class GrimResult
    {
        [XmlElement("result")]
        public string Result { get; set; }

        public GrimResult() { }

        public GrimResult(string result)
            => Result = result;

        public static GrimResult FromString(string result)
            => new(result);

        public static GrimResult FromBool(bool result)
            => new(result ? "1" : "0");

        public static GrimResult FromByte(byte result)
            => new(result.ToString());

        public static GrimResult FromSByte(sbyte result)
           => new(result.ToString());

        public static GrimResult FromShort(short result)
            => new(result.ToString());

        public static GrimResult FromUShort(ushort result)
           => new(result.ToString());

        public static GrimResult FromInt(int result)
            => new(result.ToString());

        public static GrimResult FromUInt(uint result)
            => new(result.ToString());

        public static GrimResult FromLong(long result)
            => new(result.ToString());

        public static GrimResult FromULong(ulong result)
            => new(result.ToString());

        public static GrimResult FromSingle(float result)
            => new(result.ToString());

        public static GrimResult FromDouble(double result)
            => new(result.ToString());

        public static GrimResult FromDateTimeRfc3339(DateTime result)
            => new(result.ToString("yyyy-MM-dd'T'HH:mm:ss.fffzzz", DateTimeFormatInfo.InvariantInfo)); // Rfc3339

    }
}
