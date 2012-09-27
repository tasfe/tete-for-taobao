using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using System.Web.Security;
using System.Text;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

public partial class top_reviewnew_lostsend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string nick = "奥美姿旗舰店";
        string buynick = string.Empty;
        string sql = "SELECT * FROM Tmp_FailedList";
        int index = 0;

        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            try
            {
                string phone = dt.Rows[i]["mobile"].ToString();
                string content = dt.Rows[i]["content"].ToString();

                sql = "SELECT buynick FROM TCS_Customer WHERE mobile = '" + phone + "'";
                buynick = utils.ExecuteString(sql);

                string result = SendMessage(phone, content);
                //记录短信发送记录
                sql = "INSERT INTO TCS_MsgSend (" +
                                    "nick, " +
                                    "buynick, " +
                                    "mobile, " +
                                    "[content], " +
                                    "yiweiid, " +
                                    "num, " +
                                    "typ " +
                                " ) VALUES ( " +
                                    " '" + nick + "', " +
                                    " '" + buynick + "', " +
                                    " '" + phone + "', " +
                                    " '" + content + "', " +
                                    " '" + result + "', " +
                                    " '1', " +
                                    " 'act' " +
                                ") ";

                if (phone.Length > 10)
                {
                    utils.ExecuteNonQuery(sql);

                    //更新短信数量
                    sql = "UPDATE TCS_ShopConfig SET used = used + 1,total = total-1 WHERE nick = '" + nick + "'";
                    utils.ExecuteNonQuery(sql);
                }
                index++;
            }
            catch { }
        }

        //插入充值记录并更新短信条数
        sql = "INSERT INTO TCS_PayLog (" +
                        "typ, " +
                        "enddate, " +
                        "nick, " +
                        "count " +
                    " ) VALUES ( " +
                        " '短信漏发重发补偿', " +
                        " GETDATE(), " +
                        " '" + nick + "', " +
                        " '" + index.ToString() + "' " +
                  ") ";
        utils.ExecuteNonQuery(sql);

        //加短信
        sql = "UPDATE TCS_ShopConfig SET total = total + " + index.ToString() + " WHERE nick = '" + nick + "'";
        utils.ExecuteNonQuery(sql);

        Response.Write("重发成功，数量【" + index.ToString() + "】条!!");
    }



    public static string UrlEncode(string str)
    {
        StringBuilder sb = new StringBuilder();
        byte[] byStr = System.Text.Encoding.Default.GetBytes(str);
        for (int i = 0; i < byStr.Length; i++)
        {
            sb.Append(@"%" + Convert.ToString(byStr[i], 16));
        }

        return (sb.ToString());
    }

    public static string MD5AAA(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }

    public string SendMessage(string phone, string msg)
    {
        //有客户没有手机号也发送短信
        if (phone.Length < 11)
        {
            return "0";
        }

        string uid = "ZXHD-SDK-0107-XNYFLX";
        string pass = MD5AAA("WEGXBEPY").ToLower();

        msg = UrlEncode(msg);

        string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=0&url=&smstype=0&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] bs = Encoding.ASCII.GetBytes(param);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com/zqhdServer/sendSMS.jsp" + "?" + param);

        req.Method = "GET";

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();
                //File.WriteAllText(Server.MapPath("aaa.txt"), content);

                if (content.IndexOf("<result>0</result>") == -1)
                {
                    Regex reg = new Regex(@"<result>([^<]*)</result>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = string.Empty;
                    if (reg.IsMatch(content))
                    {
                        number = match[0].Groups[1].ToString(); // match[0].Groups[1].ToString();
                    }
                    else
                    {
                        number = "888888";
                    }

                    if (number.Length > 50)
                    {
                        number = content.Substring(0, 50);
                    }
                    return number;
                }
                else
                {
                    if (content.Length > 50)
                    {
                        content = content.Substring(0, 50);
                    }

                    return content;
                }
            }
        }
    }
}