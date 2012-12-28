using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class NewCourseType
    {
        public int ID { set; get; }
        public String Title { set; get; }
        public NewCatalogue Catalogue { set; get; }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (this.Title == (obj as NewCatalogue).Title)
                return true;
            return false;
        }
    }
}
