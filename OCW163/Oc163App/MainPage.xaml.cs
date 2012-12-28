using openCourse163Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace Oc163App
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                //哈佛大学公开课：计算机科学cs50  http://v.163.com/special/opencourse/cs50.html
                //哈佛大学公开课：构建动态网站 http://v.163.com/special/opencourse/buildingdynamicwebsites.html
                String url = "http://v.163.com/special/opencourse/cs50.html";//"http://v.163.com/special/opencourse/cs50.html";
                //密西西比河谷州立大学：Android应用程序开发
                //String url = "http://v.163.com/special/opencourse/developingandroidapplications.html";
                String gb2312 = "gb2312";
                
                OcClient oc = new OcClient();
                //获取结果
                Course course = await oc.DoWork(url, gb2312);
                //设置课程标题
                openCourseTitle.Text = course.OpenCourseTitle;
                //设置课程列表
                CourseItem.ItemsSource = course.MCourseItem;

            }
        }

        private void onCourseItemDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("onCourseItemDoubleTapped");
            ListView lv =sender as ListView;
            
        }
    }
}
