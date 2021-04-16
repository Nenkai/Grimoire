using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Xml.Serialization;

namespace GTGrimServer.Models
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
    [XmlRoot(ElementName = "course")]
    public class Course
    {
        [XmlAttribute(AttributeName = "course_id")]
        public long CourseId { get; set; }

        [XmlAttribute(AttributeName = "create_time")]
        public string CreateTime { get; set; }

        [XmlAttribute(AttributeName = "update_time")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// PSN Name of the user that owns this course (but not the creator).
        /// </summary>
        [XmlAttribute(AttributeName = "user_id")]
        public string OwnerId { get; set; }

        /// <summary>
        /// 1 = Complete, 2 = Public
        /// </summary>
        [XmlAttribute(AttributeName = "status")]
        public int Status { get; set; }

        [XmlAttribute(AttributeName = "photo_id")]
        public long PhotoId { get; set; }

        [XmlAttribute(AttributeName = "title")]
        public string Title { get; set; }

        [XmlAttribute(AttributeName = "comment")]
        public string Comment { get; set; }

        [XmlAttribute(AttributeName = "title_hidden")]
        public int TitleHidden { get; set; }

        [XmlAttribute(AttributeName = "comment_hidden")]
        public int CommentHidden { get; set; }

        [XmlAttribute(AttributeName = "photo_hidden")]
        public int PhotoHidden { get; set; }

        [XmlAttribute(AttributeName = "theme")]
        public string Theme { get; set; }

        [XmlAttribute(AttributeName = "length")]
        public int Length { get; set; }

        [XmlAttribute(AttributeName = "one_way")]
        public int OneWay { get; set; }

        [XmlAttribute(AttributeName = "corners")]
        public int Corners { get; set; }

        /// <summary>
        /// Name of the creator that created this course.
        /// </summary>
        [XmlAttribute(AttributeName = "source_user_id")]
        public string OriginalCreator { get; set; }

        [XmlAttribute(AttributeName = "straight")]
        public int Straight { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
    }
}
