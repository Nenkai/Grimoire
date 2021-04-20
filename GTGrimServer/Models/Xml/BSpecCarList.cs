using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot("bspec_car_list")]
    public class BSpecCarList
    {
        [XmlElement("car")]
        public List<BSpecCar> Cars { get; set; }
    }

    public class BSpecCar
    {
        [XmlAttribute("car_id")]
        public long CarId { get; set; }

        [XmlAttribute("user_id")]
        public string UserId { get; set; }

        [XmlAttribute("car_label")]
        public string CarLabel { get; set; }

        [XmlAttribute("car_parameter")]
        public byte[] CarParameter { get; set; }

        [XmlAttribute("status")]
        public int Status { get; set; }

        [XmlAttribute("lock_time")]
        public string LockTimeString { get; set; }
        [XmlIgnore]
        public DateTime LockTime
        {
            get => DateTimeExtensions.FromRfc3339String(LockTimeString);
            set => LockTimeString = value.ToRfc3339String();
        }

        [XmlAttribute("lock_user_id")]
        public string LockUserId { get; set; }

        [XmlAttribute("thumbnail_photo_id")]
        public string ThumbnailPhotoId { get; set; }
    }
}
