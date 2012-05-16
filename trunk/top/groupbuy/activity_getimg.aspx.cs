using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Text;
using System.Net;

public partial class top_groupbuy_activity_getimg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string id = string.Empty;
            string dateStr = string.Empty;
            string type = string.Empty;
            if (Request.QueryString["id"] == null || Request.QueryString["typ"] == null)
            {
                Response.Write("");
                Response.End();
            }

            id = Request.QueryString["id"].ToString();
            dateStr = DateTime.Now.ToString();//传当前时间
            type = Request.QueryString["typ"].ToString();

            // 输出时间图片
            GetDateImage(id, dateStr, type);
        }
    }

    /// <summary>
    /// 返回时间图片
    /// </summary>
    /// <param name="id">团购ID</param>
    /// <param name="date">时间  （用来计算当前时间与团购结束时间的时间差）</param>
    /// <param name="type">返回图片类型（用来计算是小时或分钟 0:毫秒图片,1:天,2小时,3分,4秒）</param>
    public void GetDateImage(string id, string date, string type)
    {
        string imageName = string.Empty;
        string sql = string.Empty;
        DateTime endDate;
        DateTime startDate;
        //计算时间 判断团购时间是否结束 
        sql = "select top 1 tete_activitylist.enddate,* from tete_shoptemplet left join tete_shoptempletlist on tete_shoptemplet.id=tete_shoptempletlist.shoptempletID left join tete_activitylist on tete_shoptempletlist.productid=tete_activitylist.productid  where tete_shoptempletlist.shoptempletID="+id+"    order by sort asc ,tete_activitylist.enddate desc ";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            try
            {
                endDate = DateTime.Parse(dt.Rows[0]["enddate"].ToString());
                startDate = DateTime.Parse(date);
                TimeSpan tdays = endDate - startDate;  //得到时间差 当前时间与团购结束时间
                string d = tdays.Days.ToString();//剩余时间:小时
                string h = tdays.Hours.ToString();//剩余时间:分钟
                string m = tdays.Minutes.ToString();//剩余时间:秒
                if (type == "1")
                {
                    imageName = d;
                }
                else if (type == "2")
                {
                    imageName = h;
                }
                else
                {
                    imageName = m;
                }
                Response.Write(imageName + "-" + type);
                OutPutImage(imageName, type);
                ////淘宝图片地址
                //sql = "SELECT imageName,taobaoImageUrl,Type FROM taobaoImageUrl WHERE Type = " + type + " AND imageName='" + imageName + "'";
                //dt = utils.ExecuteDataTable(sql);
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    //输出淘宝图片地址
                //    Response.Write(dt.Rows[0]["taobaoImageUrl"].ToString());
                //}
            }
            catch
            {
                Response.Write("");
                Response.End();
            }

        }
    }

    private void OutPutImage(string imageName, string type)
    {
        if (imageName.IndexOf("-") != -1)
        {
            imageName = "0";
        }

        //淘宝图片地址
        string url = string.Empty;
        string sql = "SELECT imageName,taobaoImageUrl,Type FROM taobaoImageUrl WHERE Type = 1 AND imageName='" + imageName + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            //输出淘宝图片地址
            url = dt.Rows[0]["taobaoImageUrl"].ToString();
        }

        if (url == "")
        {
            Response.Write(sql);
            Response.End();
        }
        else
        {
            Response.Redirect(url);
            Response.End();
        }

        string strHtml = string.Empty;

        StreamReader sr = null; //用来读取流
        StreamWriter sw = null; //用来写文件
        Encoding code = Encoding.GetEncoding("utf-8"); //定义编码 
        ASCIIEncoding encoding = new ASCIIEncoding();
        //构造web请求，发送请求，获取响应
        WebRequest HttpWebRequest = null;
        HttpWebRequest = WebRequest.Create(url);

        WebResponse HttpWebResponse = null;
        HttpWebResponse = HttpWebRequest.GetResponse();
        //获得流
        //sr = new StreamReader(HttpWebResponse.GetResponseStream(), code);
        //strHtml = sr.ReadToEnd();
        byte[] arrayByte;

        Stream stream = HttpWebResponse.GetResponseStream();

        //保存图片
        int imgLong = (int)HttpWebResponse.ContentLength;
        arrayByte = new byte[imgLong];
        int l = 0;
        while (l < imgLong)
        {
            int i = stream.Read(arrayByte, 0, imgLong);
            l += i;
        }
        stream.Close();
        HttpWebResponse.Close();

        Response.ClearContent();
        Response.ContentType = "image/gif";
        Response.BinaryWrite(arrayByte);
        Response.End();
    }

    /// <summary>
    /// 更新图片地址
    /// </summary>
    /// <param name="id">团购ID</param>
    /// <param name="imageUrl">图片地址</param>
    public void UpdateGroupbuyHtml(string id, string imageUrl)
    {

    }
}