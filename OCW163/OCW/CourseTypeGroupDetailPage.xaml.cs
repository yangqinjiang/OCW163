using Oc163App;
using OCW.Data;
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

// “组详细信息页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234229 上提供

namespace OCW
{
    /// <summary>
    /// 显示单个组的概述的页，包括组内各项
    /// 的预览。
    /// </summary>
    public sealed partial class CourseTypeGroupDetailPage : OCW.Common.LayoutAwarePage
    {
        public CourseTypeGroupDetailPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {

            // TODO: 创建适用于问题域的合适数据模型以替换示例数据
            var courseTypeGroup = SampleDataSource.GetCourseItems((String)navigationParameter);
            // TODO: 将可绑定组分配到 this.DefaultViewModel["Group"]
            this.DefaultViewModel["Group"] = courseTypeGroup;
            // TODO: 将可绑定项集合分配到 this.DefaultViewModel["Items"]
            this.DefaultViewModel["Items"] = courseTypeGroup.Items;
        }


        private void itemGridView_Click(object sender, ItemClickEventArgs e)
        {
            //url
            var clickItem = (SampleDataItem)e.ClickedItem;
            var itemId = clickItem.Description;
            this.Frame.Navigate(typeof(MainPage), itemId);
        }
    }
}
