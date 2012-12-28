using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace openCourse163Lib
{
    public class OcClient
    {
        public async Task<Course> DoWork(String url,String encode)
        {
            String htmlString = await readHtmlString(url, encode);
            System.Diagnostics.Debug.WriteLine("返回的HTML是:\n{0}", htmlString.Substring(0, 20));
            Course course = ParseHtml(htmlString);
            return course;
        }

        /// <summary>
        /// 从url地址中读取数据,返回HTML字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        private async Task<String> readHtmlString(String url, String encode)
        {
            HttpClient HttpClient = new HttpClient();
            //读取字节数组
            byte[] bytes = await HttpClient.GetByteArrayAsync(url);
            //从字节数组中解释哪种编码 如gb2312 UTF-8等
            Encoding encoding = Encoding.GetEncoding(encode);
            String htmlString = encoding.GetString(bytes, 0, bytes.Length);
            return htmlString;
        }

        private Course ParseHtml(String htmlString)
        {
            //解释htmlString 使用HtmlAgilityPack工具

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlString);
            
            Course course = new Course();
            //获取课程标题
            var titleNode =htmlDoc.DocumentNode.Descendants("title").First();
            course.OpenCourseTitle = titleNode.InnerText;

            List<CourseItem> items = new List<CourseItem>();
            //遍历
            var tableNodes = htmlDoc.DocumentNode.Descendants("table");
            #region
            foreach (HtmlNode tableNode in tableNodes)
            {
                String list2 = tableNode.GetAttributeValue("id", "list1");
                if (list2 == "list2")
                {
                    System.Diagnostics.Debug.WriteLine("得到的课程列表");
                    //得到table id=2的tr
                    var trNode = tableNode.Descendants("tr");
                    foreach (var tr in trNode)
                    {
                        String u_evenORodd = tr.GetAttributeValue("class", null);
                        if (u_evenORodd == "u-even" || u_evenORodd == "u-odd")//奇数行
                        {
                            var tdNode = tr.Descendants("td");
                            CourseItem item = new CourseItem();
                            foreach (var td in tdNode)
                            {
                                //视频名称
                                String u_ctitle = "";
                                if (td.GetAttributeValue("class", null) == "u-ctitle")
                                {
                                    u_ctitle = td.InnerText;
                                    //去掉 \n 和空格
                                    u_ctitle = u_ctitle.Replace("\n", "").Replace(" ", "");
                                    item.LessonTitle = u_ctitle;
                                    System.Diagnostics.Debug.WriteLine("视频名称:" + u_ctitle);

                                }
                                //视频下载url
                                String u_cdown = "";
                                if (td.GetAttributeValue("class", null) == "u-cdown")
                                {

                                    var aNodes =td.Descendants("a");
                                    if (aNodes.Count() != 0)
                                    {
                                        HtmlNode urlNode = aNodes.ElementAt(0);
                                        u_cdown = urlNode.GetAttributeValue("href", "no url");
                                        item.LessonDownloadLink = u_cdown;
                                        System.Diagnostics.Debug.WriteLine("视频下载URL:" + u_cdown);
                                    }
                                    else
                                    {
                                        item.LessonDownloadLink = String.Empty;
                                    }
                                    items.Add(item);
                                }
                            }

                        }
                        System.Diagnostics.Debug.WriteLine("-------------");
                    }
                }
            }
            #endregion
            course.MCourseItem = items;
            return course;
        }
    }
}
