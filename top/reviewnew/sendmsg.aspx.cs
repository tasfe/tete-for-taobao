using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

public partial class top_reviewnew_sendmsg : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = "SELECT * FROM TCS_ShopConfig WHERE LEN(phone) = 11 AND isdel = 0";
        DataTable dt = utils.ExecuteDataTable(sql);

        string msg = "好评有礼通知：近期审核优惠券赠送时提示，该买家不是卖家的会员！因淘宝调整了优惠券规则，可能缓存问题导致数据出错。11日我们已提交问题至淘宝，有回复后我们会立刻联系您！";
        //SendMessage("18606297190", msg);
        //return;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            SendMessage(dt.Rows[i]["phone"].ToString(), msg);
        }
    }


    public static string SendMessage(string phone, string msg)
    {
        //有客户没有手机号也发送短信
        if (phone.Length == 0)
        {
            return "0";
        }

        string uid = "terrylv";
        string pass = "123456";

        msg = HttpUtility.UrlEncode(msg);

        string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
        byte[] bs = Encoding.ASCII.GetBytes(param);
        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
        req.Method = "POST";
        req.ContentType = "application/x-www-form-urlencoded";
        req.ContentLength = bs.Length;

        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(bs, 0, bs.Length);
        }

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();

                if (content.IndexOf("<error>0</error>") == -1)
                {
                    //发送失败
                    return content;
                }
                else
                {
                    //发送成功
                    Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = match[0].Groups[1].ToString();
                    return number;
                }
            }
        }
    }
}