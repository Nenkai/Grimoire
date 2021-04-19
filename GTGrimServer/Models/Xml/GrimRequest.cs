using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

namespace GTGrimServer.Models
{
    [XmlRoot(ElementName = "grim")]
    public class GrimRequest
    { 
        [XmlElement(ElementName = "command")]
        public string Command { get; set; }
        [XmlElement(ElementName = "params")]
        public GrimRequestParams Params { get; set; }
        

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

        public bool TryGetParameterByIndex(int index, out GrimRequestParam param)
        {
            if (index < 0 || index >= Params.ParamList.Count)
            {
                param = null;
                return false;
            }

            param = Params.ParamList[index];
            return true;
        }

        public bool TryGetParameterByKey(string key, out GrimRequestParam param)
        {
            param = Params.ParamList.FirstOrDefault(p => p.Key == key);
            if (param is not null && param.Text is null)
                param.Text = string.Empty;

            return param is not null;
        }
    }

    [XmlRoot(ElementName = "params")]
    public class GrimRequestParams
    {
        [XmlElement(ElementName = "param")]
        public List<GrimRequestParam> ParamList { get; set; }
    }

    [XmlRoot(ElementName = "param")]
    public class GrimRequestParam
    {
        [XmlAttribute(AttributeName = "key")]
        public string Key { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
