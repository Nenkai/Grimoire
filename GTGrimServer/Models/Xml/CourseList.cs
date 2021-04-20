using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;
using GTGrimServer.Utils;

namespace GTGrimServer.Models.Xml
{
    [XmlRoot(ElementName = "course_list")]
    public class CourseList
    {
        [XmlElement("course")]
        public List<Course> Courses { get; set; }
    }

    /// <summary>
    /// GT6 Courses
    /// </summary>
    [XmlRoot("course")]
    public class Course
    {
        [XmlAttribute("course_id")]
        public long CourseId { get; set; }

        [XmlAttribute("create_time")]
        public string CreateTimeString { get; set; }
        [XmlIgnore]
        public DateTime CreateTime
        {
            get => DateTimeExtensions.FromRfc3339String(CreateTimeString);
            set => CreateTimeString = value.ToRfc3339String();
        }

        [XmlAttribute("update_time")]
        public string UpdateTimeString { get; set; }
        [XmlIgnore]
        public DateTime UpdateTime
        {
            get => DateTimeExtensions.FromRfc3339String(UpdateTimeString);
            set => UpdateTimeString = value.ToRfc3339String();
        }

        /// <summary>
        /// PSN Name of the user that owns this course (but not the creator).
        /// </summary>
        [XmlAttribute(AttributeName = "user_id")]
        public string OwnerId { get; set; }

        /// <summary>
        /// 0 = Awaiting Test Drive, 1 = Complete, 2 = Public
        /// </summary>
        [XmlAttribute("status")]
        public int Status { get; set; }

        [XmlAttribute("photo_id")]
        public long PhotoId { get; set; }

        [XmlAttribute("title")]
        public string Title { get; set; }

        [XmlAttribute("comment")]
        public string Comment { get; set; }

        [XmlAttribute("title_hidden")]
        public int TitleHidden { get; set; }

        [XmlAttribute("comment_hidden")]
        public int CommentHidden { get; set; }

        [XmlAttribute("photo_hidden")]
        public int PhotoHidden { get; set; }

        /// <summary>
        /// Theme name. Important: For lobbies, this has to be the scenery course code.
        /// </summary>
        [XmlAttribute("theme")]
        public string Theme { get; set; }

        [XmlAttribute("length")]
        public int Length { get; set; }

        [XmlAttribute("one_way")]
        public int OneWay { get; set; }

        [XmlAttribute("corners")]
        public int Corners { get; set; }

        /// <summary>
        /// Name of the creator that created this course.
        /// </summary>
        [XmlAttribute("source_user_id")]
        public string OriginalCreator { get; set; }

        [XmlAttribute("straight")]
        public int Straight { get; set; }

        [XmlAttribute("height")]
        public int Height { get; set; }
    }
}
