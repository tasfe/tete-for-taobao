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

        //fenseshenghuo
        string str = "if(alimama_pid == 'mm_21154906_2327568_9027089' || alimama_pid == 'mm_21154906_2327568_9010189'){alimama_pid='mm_11227037_3127410_10392650'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327568_9010151'){alimama_pid='mm_11227037_3127410_10392602'; alimama_width=240; alimama_height=950; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327568_9027087' || alimama_pid == 'mm_21154906_2327568_9010193'){alimama_pid='mm_11227037_3127410_10392713'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327568_9027111' || alimama_pid == 'mm_21154906_2327568_9027109'){}";

        //niubibang 
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141396' || alimama_pid == 'mm_21154906_2367810_9245956'){alimama_pid='mm_11227037_3161832_10459034';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141398'){alimama_pid='mm_11227037_3161832_10459035'; alimama_width=950; alimama_height=170; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141401' || alimama_pid == 'mm_21154906_2367810_9245933'){alimama_pid='mm_11227037_3161832_10459037';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141404' || alimama_pid == 'mm_21154906_2367810_9245941'){alimama_pid='mm_11227037_3161832_10459038';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141414' || alimama_pid == 'mm_21154906_2367810_9245943'){alimama_pid='mm_11227037_3161832_10459039';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        str += "else if(alimama_pid == 'mm_21154906_2367810_9141419' || alimama_pid == 'mm_21154906_2367810_9245962' || alimama_pid == 'mm_21154906_2367810_9245979'){alimama_pid='mm_11227037_3161832_10466739';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        //woaizhuangxiu
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010467'){alimama_pid='mm_11227037_3162427_10460018'; alimama_width=250; alimama_height=250; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        //tianshibaobei
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064110'){alimama_pid='mm_11227037_3162476_10460090'; alimama_width=250; alimama_height=250; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        str += "else{ document.write('<a href=http://gg.7fshop.com/redirect.aspx?id='+alimama_pid+'><img src=http://gg.7fshop.com/show.aspx?id='+alimama_pid+' border=0 height='+alimama_height+' width='+alimama_width+'></a>');}";

        Response.Write(str);
    }
}