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

        [XmlAttribute(AttributeName = "user_id")]
        public long UserId { get; set; }

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

        [XmlAttribute(AttributeName = "source_user_id")]
        public long SourceUserId { get; set; }

        [XmlAttribute(AttributeName = "straight")]
        public int Straight { get; set; }

        [XmlAttribute(AttributeName = "height")]
        public int Height { get; set; }
    }
}
