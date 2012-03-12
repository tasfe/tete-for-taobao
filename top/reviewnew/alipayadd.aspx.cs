using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using Common;

public partial class top_reviewnew_alipayadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Decrypt(taobaoNick);

        //过期判断
        if (string.IsNullOrEmpty(taobaoNick))
        {
            string msg = "尊敬的淘宝卖家，非常抱歉的告诉您，您还没有购买此服务或者登录信息已失效，如需继续使用请<a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545' target='_blank'>进入该服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        //先判断支付宝红包格式是否合法
        string guid = Guid.NewGuid().ToString();

        if (fuAlipay.PostedFile.ContentType != "text/plain" && fuAlipay.PostedFile.FileName.IndexOf(".txt") == -1)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string filename = Server.MapPath("alipay/" + guid + ".txt");
        fuAlipay.PostedFile.SaveAs(filename);

        string content = File.ReadAllText(filename);
        if (content.IndexOf("cardno          ,password") == -1)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string[] arr = Regex.Split(content, "\r\n");
        if (arr.Length == 2)
        {
            Response.Write("<script>alert('您上传的文件格式不正确！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string sql = "INSERT INTO TCS_Alipay (guid, nick, name, count, num, enddate, per) VALUES ('" + guid + "','" + nick + "','" + utils.NewRequest("name", utils.RequestType.Form) + "','" + (arr.Length -2).ToString() + "','" + utils.NewRequest("num", utils.RequestType.Form) + "','" + DateTime.Now.AddDays(int.Parse(utils.NewRequest("end_time", utils.RequestType.Form))) + "','" + utils.NewRequest("per", utils.RequestType.Form) + "')";
        utils.ExecuteNonQuery(sql);

        for (int i = 1; i < arr.Length - 1; i++)
        {
            string[] arrDetail = arr[i].Split(',');

            sql = "INSERT INTO TCS_AlipayDetail (guid,nick,card,pass) VALUES ('" + guid + "','" + nick + "','" + arrDetail[0] + "','" + arrDetail[1] + "')";
            utils.ExecuteNonQuery(sql);
        }

        Response.Redirect("alipay.aspx");
        Response.End();
    }
}