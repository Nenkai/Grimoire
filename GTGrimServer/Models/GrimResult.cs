using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot("grim")]
    public class GrimResult
    {
        [XmlElement(ElementName = "result")]
        public int Result { get; set; }

        public GrimResult() { }

        public GrimResult(int result)
            => Result = result;
    }
}
