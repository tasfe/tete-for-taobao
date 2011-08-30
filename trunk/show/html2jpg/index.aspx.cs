using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Taobao.Top.Api;
using Common;
using System.Net;
using System.IO;

public partial class show_html2jpg_index : System.Web.UI.Page
{
    private string id = string.Empty;
    private string folderPath = string.Empty;
    private string size = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        id = utils.NewRequest("id", utils.RequestType.QueryString);
        id = id.Replace(".png", "");
        string title = string.Empty;
        string width = string.Empty;
        string height = string.Empty;
        string num = string.Empty;

        //获取广告标题
        string sql = string.Empty;

        //图片缓存判断
        if (1 == 1)
        {
            sql = "SELECT name,size,nick FROM TopIdea WHERE id = " + id;
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                //创建图片临时文件架
                folderPath = Server.MapPath("../folder/" + MD5(dt.Rows[0]["nick"].ToString()));
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                //判断生成好的图片是否过期
                string resultPath = folderPath + "/result_" + id + ".jpg";
                if (File.Exists(resultPath))
                {
                    RecordAndShow(folderPath);
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    private void RecordAndShow(string folderPath)
    {
        //记录广告浏览次数
        string sql = "UPDATE TopIdea SET viewcount = viewcount + 1 WHERE id = " + id;
        utils.ExecuteNonQuery(sql);
        //记录浏览日志
        string url = Request.UrlReferrer == null ? "" : Request.UrlReferrer.ToString();
        sql = "INSERT INTO TopIdeaLog (ideaid, url) VALUES ('" + id + "', '" + url + "')";
        utils.ExecuteNonQuery(sql);

        Response.ClearContent();
        Response.ContentType = "image/png";
        Response.BinaryWrite(File.ReadAllBytes(folderPath + "/result_" + id + ".jpg"));
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }
}
