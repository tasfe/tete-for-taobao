using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Web_detail_dialog1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 上传客户图片到自己服务器
    /// </summary>
    private void UploadUserPic()
    {
        string fileName = string.Empty;
        //判断日期文件夹是否存在
        string dateName = "pic/" + DateTime.Now.ToString("yyyy-MM-dd");
        if (!Directory.Exists(Server.MapPath(dateName)))
        {
            Directory.CreateDirectory(Server.MapPath(dateName));
        }

        if (CheckFileIsSave(FileUpload1))
        {
            this.FileUpload1.PostedFile.SaveAs(Server.MapPath(dateName + "/" + Guid.NewGuid() + ".jpg"));
        }
        if (CheckFileIsSave(FileUpload2))
        {
            this.FileUpload2.PostedFile.SaveAs(Server.MapPath(dateName + "/" + Guid.NewGuid() + ".jpg"));
        }
        if (CheckFileIsSave(FileUpload3))
        {
            this.FileUpload3.PostedFile.SaveAs(Server.MapPath(dateName + "/" + Guid.NewGuid() + ".jpg"));
        }
        if (CheckFileIsSave(FileUpload4))
        {
            this.FileUpload4.PostedFile.SaveAs(Server.MapPath(dateName + "/" + Guid.NewGuid() + ".jpg"));
        }
        if (CheckFileIsSave(FileUpload5))
        {
            this.FileUpload5.PostedFile.SaveAs(Server.MapPath(dateName + "/" + Guid.NewGuid() + ".jpg"));
        }
    }

    /// <summary>
    /// 判断上传的文件是否合法
    /// </summary>
    /// <param name="FileUpload1"></param>
    /// <returns></returns>
    private bool CheckFileIsSave(FileUpload FileUpload1)
    {
        if (FileUpload1.PostedFile.ContentType.IndexOf("jpeg") != -1 || FileUpload1.PostedFile.ContentType.IndexOf("jpg") != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        UploadUserPic();

        Response.Write("<script>alert(parent);</script>");
        Response.End();
    }
}