using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_detail_test1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        List<Parameter> list = new List<Parameter>();
        Parameter paramter = new Parameter("image", Server.MapPath("1.gif"));
        list.Add(paramter);
        list.Add(paramter);
        try
        {
            string realUrl = UploadFile.HttpPostWithFile("http://mall.jia.com/site/upload_describe_image", "", list);
            Response.Write(realUrl);
        }
        catch (Exception ex)
        {
            LogHelper.LogInfo.Add("用户上传图片错误", ex.Message);
        }
    }
}