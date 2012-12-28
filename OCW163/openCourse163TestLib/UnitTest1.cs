using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace openCourse163Lib
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void OpenSite2()
        {
            //哈佛大学公开课：计算机科学cs50  http://v.163.com/special/opencourse/cs50.html
            //哈佛大学公开课：构建动态网站 http://v.163.com/special/opencourse/buildingdynamicwebsites.html
            //String url = "http://v.163.com/special/opencourse/cs50.html";//"http://v.163.com/special/opencourse/cs50.html";
            //密西西比河谷州立大学：Android应用程序开发
            String url = "http://v.163.com/special/opencourse/developingandroidapplications.html";
            String gb2312 = "gb2312";
            OcClient oc = new OcClient();
            Task<Course> items =oc.DoWork(url,gb2312);
            Course result =items.Result;
        }
        ////一级标题集合
        //public HashSet<NewCatalogue> NewCatalogues
        //{
        //    set;
        //    get;
        //}
        ////二级标题
        //public HashSet<NewCourseType> NewCourseTypes { set; get; }
        ////课程列表
        //public HashSet<NewCourse> NewCourses { set; get; }

        [TestMethod]
        public void ReadSite()
        {
            HtmlWeb webClient = new HtmlWeb();
            //国际名校公开课 http://open.163.com/ocw/
            Task<HtmlDocument> doc = webClient.LoadFromWebAsync("http://localhost:8080/OpenCourse163Test.html");//http://open.163.com/ocw/
            HtmlDocument docNode =doc.Result;
            HtmlNode node = docNode.DocumentNode;
            // 1.<div class="m-t-bg">...</div>
            List<Catalogue> catalogues=GetCatalogueList(node);
            printCatalogues(catalogues);
            //断言
            Assert.AreEqual(18,catalogues.Count());


        }

        [TestMethod]
        public void ReadSite2()
        {
            HtmlWeb webClient = new HtmlWeb();
            //国际名校公开课 http://open.163.com/ocw/
            Task<HtmlDocument> doc = webClient.LoadFromWebAsync("http://localhost:8080/OpenCourse163Test.html");//http://open.163.com/ocw/
            HtmlDocument docNode = doc.Result;
            HtmlNode node = docNode.DocumentNode;
            // 1.<div class="m-t-bg">...</div>
            Client client = new Client();
            List<Catalogue> catalogues = client.GetCatalogueList(node);
            printCatalogues(catalogues);
            //断言
            Assert.AreEqual(18, catalogues.Count());
        }

        [TestMethod]
        public void ReadSite3()
        {
            Client client = new Client();
            Task<All> alltask = client.GetAllSet();
            All all = alltask.Result;
            foreach(var cs in all.NewCatalogueSet)
            {
                System.Diagnostics.Debug.WriteLine("NewCatalogueSet ID {0}\t Title {1}",cs.ID,cs.Title);
            }
            foreach (var nct in all.newCourseTypeSet)
            {
                System.Diagnostics.Debug.WriteLine("NewCatalogueSet ID {0}\t Title {1} CatalogueTitle {2}",
                    nct.ID, nct.Title, nct.Catalogue.Title);
            }
            foreach(var ncs in all.NewCourseSet)
            {
                System.Diagnostics.Debug.WriteLine("CourseTitle {0}  CourseType {1}", ncs.CourseTitle, ncs.CourseType.Title);
            }
            Assert.AreEqual(3, all.NewCatalogueSet.Count());
            Assert.AreEqual(18, all.newCourseTypeSet.Count());
            Assert.AreEqual(237, all.NewCourseSet.Count());
        }

        private static void printCatalogues(List<Catalogue> catalogues)
        {
            System.Diagnostics.Debug.WriteLine("catalogues count {0}", catalogues.Count());
            foreach (var c in catalogues)
            {
                System.Diagnostics.Debug.WriteLine("c.CatalogueTitle {0}\n\t c.CourseTypes.CourseTypeTitle {1}", c.CatalogueTitle, c.CourseTypes.CourseTypeTitle);
                foreach (var oc in c.CourseTypes.OCourses)
                {
                    System.Diagnostics.Debug.WriteLine("CourseTitle {0} | UpdataProgress {1}",
                        oc.CourseTitle, oc.CourseUpdataProgress);
                }
            }
        }

        private static List<Catalogue> GetCatalogueList(HtmlNode node)
        {
            List<Catalogue> catalogues = new List<Catalogue>();
            
            //获取根节点下所有的  div 子孙节点
            //divNode1 是主要内容的一级div
            foreach (var divNode1 in node.Descendants("div"))
            {
                AppendCatalogueList(divNode1,catalogues);
            }
            return catalogues;
        }
        /// <summary>
        /// 填充Catalogue列表
        /// </summary>
        /// <param name="divNode1"></param>
        /// <param name="catalogues"></param>
        private static void AppendCatalogueList(HtmlNode divNode1, List<Catalogue> catalogues)
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
         private static String GetCatalogueTitle(HtmlNode divNode2)
         {
             //获取分类标题 一级标题信息 ▲如人文科学,社会科学,自然科学
             String catalogueTitle = divNode2.Descendants("h2").First().InnerText;
             //System.Diagnostics.Debug.WriteLine("一级分类标题:{0}", catalogueTitle);
            return catalogueTitle;
        }
        //
         private static CourseType GetCourseType(HtmlNode divNode2)
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
        private static List<OCourse> GetSingleCourseList(HtmlNode ulNode)
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
        private static OCourse GetSingleCourse(HtmlNode liNode)
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
