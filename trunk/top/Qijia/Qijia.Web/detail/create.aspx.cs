using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Web_detail_create : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        UploadUserPic();

        Response.Write("<script>alert(1);window.parent.document.getElementById('showArea').style.display = 'none';</script>");
        Response.End();
    }

    /// <summary>
    /// 上传客户图片到自己服务器
    /// </summary>
    private void UploadUserPic()
    {
        
    }
}