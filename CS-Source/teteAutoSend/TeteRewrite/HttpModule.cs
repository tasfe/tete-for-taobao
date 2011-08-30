using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Data;
using System.Text.RegularExpressions;

namespace TeteRewrite
{
    public class HttpModule : IHttpModule
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public HttpModule()
        { }

        /// <summary>
        /// 继承释放
        /// </summary>
        public void Dispose()
        {
            //GC.Collect();
        }

        /// <summary>
        /// 事件托管
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += new EventHandler(this.ReUrl_BeginRequest);
        }

        /// <summary>
        /// 请求事件托管 Modify By 吕鑫 2008-11-14
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            //验证URL是否合法请求
            string path = string.Empty;
            path = HttpContext.Current.Request.Path.ToLower();
            string url = path;
            string newUrl = path.Replace("svn.7fshop.com", "www.7fshop.com:8181");

            HttpContext.Current.RewritePath(newUrl);
        }
    }
}
