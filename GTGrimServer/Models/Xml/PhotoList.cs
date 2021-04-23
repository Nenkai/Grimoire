using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    /// <summary>
    /// Represents a list of photo information.
    /// </summary>
    [XmlRoot("photos")]
    public class PhotoList
    {
        [XmlElement("photo")]
        public List<Photo> Photos { get; set; } = new();
    }

    [XmlRoot("photo")]
    public class Photo
    {
        /// <summary>
        /// Database Id
        /// </summary>
        [XmlAttribute("photo_id")]
        public long PhotoId { get; set; }

        /// <summary>
        /// Location of the photo.
        /// </summary>
        [XmlAttribute("place")]
        public string Place { get; set; }

        /// <summary>
        /// Car name of car inside the photo.
        /// </summary>
        [XmlAttribute("car_name")]
        public string CarName { get; set; }

        /// <summary>
        /// User Id of the creator of the photo.
        /// </summary>
        [XmlAttribute("user_id")]
        public string UserId { get; set; }

        /// <summary>
        /// Comment of this photo. (140 chars max)
        /// </summary>
        [XmlAttribute("comment")]
        public string Comment { get; set; }

        /// <summary>
        /// Creation date of this photo.
        /// </summary>
        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }
    }
}
