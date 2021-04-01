using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName="grim")]
    public class GrimRequest
    {
        [XmlElement(ElementName = "command")]
        public string Command { get; set; }
    }
}
