using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class CourseItem:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void FirePropertyChanged(string propertyName)
        {
            if(PropertyChanged!=null)
            {
                PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
            }
        }
        private string lessonTitle;
        /// <summary>
        /// 每一节课的标题
        /// </summary>
        public String LessonTitle
        {
            set
            {
                this.lessonTitle = value;
                FirePropertyChanged("LessonTitle");
            }
            get
            {
                return this.lessonTitle;
            }
        }
        private string lessonDownloadLink;
        /// <summary>
        /// 每一节课的下载链接
        /// </summary>
        public String LessonDownloadLink
        {
            set
            {
                this.lessonDownloadLink = value;
                FirePropertyChanged("LessonDownloadLink");
            }
            get
            {
                return this.lessonDownloadLink;
            }
        }

        public CourseItem()
        {
        }
        public CourseItem(String LessonTitle,String LessonDownloadLink)
        {
            this.LessonTitle = LessonTitle;
            this.LessonDownloadLink = LessonDownloadLink;
        }
    }
}
