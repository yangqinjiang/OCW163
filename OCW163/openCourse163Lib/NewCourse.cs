using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    /// <summary>
    /// 每一门的课程类
    /// </summary>
    public class NewCourse
    {
            /// <summary>
            /// 课程标题
            /// </summary>
            public String CourseTitle
            {
                set;
                get;
            }
            /// <summary>
            /// 课程的网络链接地址
            /// </summary>
            public String CourseHrefUrl
            {
                set;
                get;
            }
            /// <summary>
            /// 课程缩略图片地址
            /// </summary>
            public String CourseImgUrl
            {
                set;
                get;
            }
            /// <summary>
            /// 课程更新进度
            /// </summary>
            public String CourseUpdataProgress
            {
                set;
                get;
            }
        /// <summary>
        /// 所属课程类型
        /// </summary>
            public NewCourseType CourseType
            {
                set;
                get;
            }
    }
}
