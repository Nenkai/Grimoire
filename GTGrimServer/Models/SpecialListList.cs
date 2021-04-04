using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "user_special_list")]
    public class SpecialListList
    {
        public List<SpecialList> Items { get; set; }
    }
}
