﻿using System;
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
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141396'){alimama_width=250; alimama_height=250; alimama_pid='mm_11227037_3161832_10459034';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        //gg1
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141398'){alimama_pid='mm_11227037_3161832_10459035'; alimama_width=950; alimama_height=170; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2367810_9245956'){alimama_pid='mm_11227037_3161832_10459035';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        //gg2
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141401' || alimama_pid == 'mm_21154906_2367810_9245933'){alimama_pid='mm_11227037_3161832_10459037';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        //left xiao
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141404' || alimama_pid == 'mm_21154906_2367810_9245941'){alimama_pid='mm_11227037_3161832_10459038';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        //right xiao
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141414' || alimama_pid == 'mm_21154906_2367810_9245943'){alimama_pid='mm_11227037_3161832_10459039';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        //bottom
        str += "else if(alimama_pid == 'mm_21154906_2367810_9141419' || alimama_pid == 'mm_21154906_2367810_9245962' || alimama_pid == 'mm_21154906_2367810_9245979'){alimama_pid='mm_11227037_3161832_10466739';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";



        //tianshibaobei
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010467' || alimama_pid == 'mm_11523861_2344778_9254331'){alimama_pid='mm_11227037_3162427_10460018'; alimama_width=250; alimama_height=250; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064059' ||  alimama_pid == 'mm_11523861_2344778_9254320'){alimama_pid='mm_11227037_3162476_10466866'; alimama_width=950; alimama_height=170; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064116' || alimama_pid == 'mm_11523861_2344778_9254316'){alimama_pid='mm_11227037_3162476_10466867';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064118' || alimama_pid == 'mm_11523861_2344778_9254323'){alimama_pid='mm_11227037_3162476_10466870';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064121' || alimama_pid == 'mm_11523861_2344778_9254326'){alimama_pid='mm_11227037_3162476_10466873';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064123' || alimama_pid == 'mm_11523861_2344778_9254329'){alimama_pid='mm_11227037_3162476_10466878';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        //woaizhuangxiu 
        str += "else if(alimama_pid == 'mm_11523861_2344778_9064110'){alimama_pid='mm_11227037_3162476_10460090'; alimama_width=250; alimama_height=250; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        str += "else if(alimama_pid == 'mm_21154906_2327674_9010570' ||  alimama_pid == 'mm_21154906_2327674_9059236'){alimama_pid='mm_11227037_3162427_10467122'; alimama_width=950; alimama_height=170; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010574' || alimama_pid == 'mm_21154906_2327674_9059228'){alimama_pid='mm_11227037_3162427_10467123';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010585' || alimama_pid == 'mm_21154906_2327674_9059231'){alimama_pid='mm_11227037_3162427_10467127';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010587' || alimama_pid == 'mm_21154906_2327674_9059234'){alimama_pid='mm_11227037_3162427_10467129';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_21154906_2327674_9010581' || alimama_pid == 'mm_21154906_2327674_9059237'){alimama_pid='mm_11227037_3162427_10467130';  document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";

        //go12go
        str += "else if(alimama_pid == 'mm_10128191_208985_2478481'){alimama_pid='mm_11227037_3167205_10468715'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_10128191_208985_7882501'){alimama_pid='mm_11227037_3167205_10468717'; alimama_width=250; alimama_height=250; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_10128191_208985_6823512'){alimama_pid='mm_11227037_3167205_10468719'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_10128191_208985_1622657'){alimama_pid='mm_11227037_3167205_10468720'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";
        str += "else if(alimama_pid == 'mm_10128191_208985_1622774'){alimama_pid='mm_11227037_3167205_10468723'; document.write('<script src=\"http://a.alimama.cn/inf.js\" type=\"text/javascript\"></script> ')}";


        str += "else{ document.write('<a href=http://gg.7fshop.com/redirect.aspx?id='+alimama_pid+'><img src=http://gg.7fshop.com/show.aspx?id='+alimama_pid+' border=0 height='+alimama_height+' width='+alimama_width+'></a>');}";

        Response.Write(str);
    }
}