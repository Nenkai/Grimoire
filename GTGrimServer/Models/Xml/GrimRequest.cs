using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using System.IO;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot(ElementName = "grim")]
    public class GrimRequest
    { 
        [XmlElement("command")]
        public string Command { get; set; }
        [XmlElement("params")]
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

        public static GrimRequest Deserialize(string inputXml)
        {
            var grimSerializer = new XmlSerializer(typeof(GrimRequest));
            using var reader = new StringReader(inputXml);

            GrimRequest requestReq = grimSerializer.Deserialize(reader) as GrimRequest;
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

        public bool TryGetParameterIntByKey(string key, out int value)
        {
            value = 0;
            if (TryGetParameterByKey(key, out var param) && int.TryParse(param.Text, out value))
                return true;

            return false;
        }

        public bool TryGetParameterLongByKey(string key, out long value)
        {
            value = 0;
            if (TryGetParameterByKey(key, out var param) && long.TryParse(param.Text, out value))
                return true;

            return false;
        }
    }

    [XmlRoot("params")]
    public class GrimRequestParams
    {
        [XmlElement("param")]
        public List<GrimRequestParam> ParamList { get; set; }
    }

    [XmlRoot("param")]
    public class GrimRequestParam
    {
        [XmlAttribute("key")]
        public string Key { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
