using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class NewCatalogue
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public String Title { set; get; }

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
