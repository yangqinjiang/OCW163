using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace openCourse163Lib
{
    /// <summary>
    /// 课程类型
    /// </summary>
    public class CourseType
    {
        /// <summary>
        /// 课程类型的ID
        /// </summary>
        public String CourseTypeId
        {
            set;
            get;
        }
        /// <summary>
        /// 课程类型的标题
        /// </summary>
        public String CourseTypeTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 属于该课程类型的课程列表
        /// </summary>
        public List<OCourse> OCourses
        {
            set;
            get;
        }
    }
}
