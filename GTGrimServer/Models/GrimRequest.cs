using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName="grim")]
    public class GrimRequest
    {
        [XmlElement(ElementName = "command")]
        public string Command { get; set; }

        [XmlElement(ElementName = "params")]
        public GrimResultParam Params { get; set; }


        public class GrimResultParam
        {
            [XmlElement(ElementName = "param")]
            public string[] Param { get; set; }
        }

        public GrimRequest() { }

        public static async Task<GrimRequest> Deserialize(Stream inputStream)
        {
            var serializer = new XmlSerializer(typeof(GrimRequest));

            Stream ms = new MemoryStream();
            await inputStream.CopyToAsync(ms);
            ms.Position = 0;

            GrimRequest requestReq = serializer.Deserialize(ms) as GrimRequest;
            return requestReq;
        }
    }
}
