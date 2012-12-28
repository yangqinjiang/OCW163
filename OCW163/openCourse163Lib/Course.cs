
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class Course : INotifyPropertyChanged
    {
        /// <summary>
        /// 公开课标题
        /// </summary>
        private string openCourseTitle;
        public String OpenCourseTitle
        {
            set
            {
                this.openCourseTitle = value;
                FirePropertyChanged("OpenCourseTitle");
            }
            get
            {
                return this.openCourseTitle;
            }
        }
        /// <summary>
        /// 课程条目
        /// </summary>
        private List<CourseItem> mCourseItem;
        public List<CourseItem> MCourseItem
        {
            set
            {
                this.mCourseItem = value;
               // FirePropertyChanged("MCourseItem");
            }
            get
            {
                return this.mCourseItem;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
