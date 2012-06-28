using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class taobaotetesoft_caiji_initads : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = "http://fuwu.taobao.com/ser/detail.htm?service_id=764";

        string str = "if(alimama_pid == 'mm_21154906_2327568_9027089'){alimama_pid='mm_11227037_3127410_10392650'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"> ')}";

        str += "else if(alimama_pid == 'mm_21154906_2327568_9010151'){alimama_pid='mm_11227037_3127410_10392602'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"> ')}";

        str += "else if(alimama_pid == 'mm_21154906_2327568_9027111'){alimama_pid='mm_11227037_3127410_10392650'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"> ')}";

        str += "else{ document.write('<a href=http://gg.7fshop.com/redirect.aspx?id='+alimama_pid+'><img src=http://gg.7fshop.com/show.aspx?id='+alimama_pid+' border=0 height='+alimama_height+' width='+alimama_width+'></a>');}";

        Response.Write(str);
    }
}