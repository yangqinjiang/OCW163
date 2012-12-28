using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class Client
    {

        public All AllSet { set; get; }
        public  async Task<All> GetAllSet(){
            HtmlWeb webClient = new HtmlWeb();
            //国际名校公开课 http://open.163.com/ocw/
            HtmlDocument docNode = await webClient.LoadFromWebAsync("http://localhost:8080/OpenCourse163Test.html");//
            System.Diagnostics.Debug.WriteLine("after LoadFromWebAsync");
            HtmlNode node = docNode.DocumentNode;
            // 1.<div class="m-t-bg">...</div>
            List<Catalogue> catalogues = this.GetCatalogueList(node);
            //获取一级标题列表
            HashSet<NewCatalogue> newCatalogues = new HashSet<NewCatalogue>();
            // Guid g = new Guid();
            //获取二级标题列表
            HashSet<NewCourseType> newCourseTypes = new HashSet<NewCourseType>();
            //获取课程列表
            HashSet<NewCourse> newCourses = new HashSet<NewCourse>();

            foreach (var catalogue in catalogues)
            {
                //一级
                NewCatalogue nc = new NewCatalogue
                {
                    ID = catalogue.CatalogueTitle.GetHashCode(),
                    Title = catalogue.CatalogueTitle
                };
                newCatalogues.Add(nc);
                //二级
                CourseType ct = catalogue.CourseTypes;
                NewCourseType nct = new NewCourseType
                {
                    ID = ct.CourseTypeId.GetHashCode(),
                    Title = ct.CourseTypeTitle,
                    Catalogue = nc
                };
                newCourseTypes.Add(nct);
                //课程列表
                foreach (var oc in catalogue.CourseTypes.OCourses)
                {
                    NewCourse newCourse =
                        new NewCourse
                        {
                            CourseTitle = oc.CourseTitle,
                            CourseHrefUrl = oc.CourseHrefUrl,
                            CourseImgUrl = oc.CourseImgUrl,
                            CourseUpdataProgress = oc.CourseUpdataProgress,
                            CourseType = nct

                        };
                    newCourses.Add(newCourse);
                }
            }

            return new All 
            { 
                NewCatalogueSet = newCatalogues ,
                newCourseTypeSet = newCourseTypes,
                NewCourseSet = newCourses
            };
        }

        public  List<Catalogue> GetCatalogueList(HtmlNode node)
        {
            List<Catalogue> catalogues = new List<Catalogue>();
           
            //获取根节点下所有的  div 子孙节点
            //divNode1 是主要内容的一级div
            foreach (var divNode1 in node.Descendants("div"))
            {
                AppendCatalogueList(divNode1, catalogues);
            }
            return catalogues;
        }
        /// <summary>
        /// 填充Catalogue列表
        /// </summary>
        /// <param name="divNode1"></param>
        /// <param name="catalogues"></param>
        private void AppendCatalogueList(HtmlNode divNode1, List<Catalogue> catalogues)
        {
            Catalogue catalogue = new Catalogue();
            if (divNode1.GetAttributeValue("class", null) == "m-t-bg")
            {
                var divNode2 = divNode1.ChildNodes;
                String catalogueTitle = "";
                CourseType courseType = new CourseType();
                foreach (var divNode22 in divNode2)
                {

                    //第一个div是一级标题
                    if (divNode22.GetAttributeValue("class", null) == "m-t-innerbg")
                    {
                        catalogueTitle = GetCatalogueTitle(divNode22);
                        catalogue.CatalogueTitle = catalogueTitle;
                        //System.Diagnostics.Debug.WriteLine("-------catalogueTitle {0}", catalogueTitle);
                    }

                    if (divNode22.GetAttributeValue("class", null) == "m-conmt")
                    {
                        //第二个div是二级标题
                        courseType = GetCourseType(divNode22);
                        catalogue = new Catalogue { CatalogueTitle = catalogueTitle, CourseTypes = courseType };
                        catalogues.Add(catalogue);
                    }
                }
            }
        }

        /// <summary>
        /// //●获取分类 一级标题
        /// <div class="m-t-innerbg">...</div>
        /// </summary>
        /// <param name="divNode2"></param>
        /// <returns></returns>
        private  String GetCatalogueTitle(HtmlNode divNode2)
        {
            //获取分类标题 一级标题信息 ▲如人文科学,社会科学,自然科学
            String catalogueTitle = divNode2.Descendants("h2").First().InnerText;
            //System.Diagnostics.Debug.WriteLine("一级分类标题:{0}", catalogueTitle);
            return catalogueTitle;
        }
        //
        private  CourseType GetCourseType(HtmlNode divNode2)
        {
            //courseTypeId课程类型ID     courseTypeTitle 课程类型标题
            String courseTypeId = "", courseTypeTitle = "";

            //获取某个二级课程名称和ID 如文学,艺术等
            #region
            //▲获取课程种类的ID
            courseTypeId = divNode2.GetAttributeValue("id", null);
            //System.Diagnostics.Debug.WriteLine("\t\t二级 课程种类的ID{0}", courseTypeId);
            #endregion
            #region
            //获取课程种类信息
            var h3Node = divNode2.Descendants("h3").First();
            if (h3Node.GetAttributeValue("class", null) == "f-fs1 f-cb")
            {
                //▲获取课程种类的名称
                courseTypeTitle = h3Node.InnerText;
            }
            //课程列表
            var ulNode = divNode2.Descendants("div").First().Descendants("ul").First();
            List<OCourse> ocs = GetSingleCourseList(ulNode);
            #endregion

            return new CourseType { CourseTypeId = courseTypeId, CourseTypeTitle = courseTypeTitle, OCourses = ocs };
        }


        /// <summary>
        /// 获取某类课程列表 如文学,艺术等
        /// <div class ="m-clscnt m-clscnt-3"></div>
        /// </summary>
        /// <param name="ulNode"></param>
        private  List<OCourse> GetSingleCourseList(HtmlNode ulNode)
        {
            List<OCourse> oCourses = new List<OCourse>();

            //获取课程列表
            if (ulNode.GetAttributeValue("class", null) == "f-cb")
            {
                var liNodes = ulNode.Descendants("li");
                //遍历所有课程信息
                foreach (var liNode in liNodes)
                {
                    //增加一门课程到OCourse集合中
                    OCourse oc = GetSingleCourse(liNode);
                    oCourses.Add(oc);
                }
            }
            return oCourses;
        }
        /// <summary>
        /// 获取单门课程的信息
        /// </summary>
        /// <param name="liNode"></param>
        /// <returns></returns>
        private  OCourse GetSingleCourse(HtmlNode liNode)
        {
            String hrefUrl = "", imgUrl = "", courseTitle = "", courseUpdataProgress = "";
            //读取一门课程信息
            //读取网络链接URL和图片URL
            var aNode = liNode.Descendants("div").First().Descendants("a").First();
            if (aNode.GetAttributeValue("class", null) == "cimg")
            {
                //▲网络链接URL
                hrefUrl = aNode.GetAttributeValue("href", "no url");

                var ImgNode = aNode.Descendants("img").First();
                //▲图片url
                imgUrl = ImgNode.GetAttributeValue("data-original", "http://img1.cache.netease.com/v/open/index/images/banner-bg.png");

            }
            var h5Node = liNode.Descendants("div").First().Descendants("h5").First();
            //▲课程标题和更新进度
            courseTitle = h5Node.Descendants("a").First().InnerText;
            //▲课程更新进度
            courseUpdataProgress = liNode.Descendants("div").First().Descendants("h6").First().InnerText;
            //返回一门课程
            return new OCourse
            {
                CourseHrefUrl = hrefUrl,
                CourseImgUrl = imgUrl,
                CourseTitle = courseTitle,
                CourseUpdataProgress = courseUpdataProgress
            };
        }
    }
}
