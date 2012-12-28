using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace openCourse163Lib
{
    /// <summary>
    /// 分类
    /// </summary>
    public class Catalogue
    {
        /// <summary>
        /// 分类标题
        /// </summary>
        public String CatalogueTitle
        {
            set;
            get;
        }
        /// <summary>
        /// 所有课程分类
        /// </summary>
        public CourseType CourseTypes
        {
            set;
            get;
        }
    }
}
