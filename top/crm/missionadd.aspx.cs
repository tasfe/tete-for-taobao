using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web.Security;

public partial class top_crm_missionadd : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;
    public string id = string.Empty;
    public string now = string.Empty;
    public string totalcustomer = string.Empty;

    public string count2 = string.Empty;
    public string count3 = string.Empty;
    public string count4 = string.Empty;
    public string count5 = string.Empty;
    public string count6 = string.Empty;
    public string count7 = string.Empty;
    public string count8 = string.Empty;
    public string count9 = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        Common.Cookie cookie = new Common.Cookie();
        string taobaoNick = cookie.getCookie("nick");
        session = cookie.getCookie("top_sessiongroupbuy");
        string iscrm = cookie.getCookie("iscrm");
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


        //过期判断
        if (!IsBuy(nick))
        {

            string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【客户关系营销】功能，如需继续使用请<a href='http://fuwu.taobao.com/ser/detail.htm?service_code=service-0-22904&frm=haoping' target='_blank'>购买高级会员服务</a>，谢谢！";
            Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
            Response.End();
            return;
        }

        if (!IsPostBack)
        {
            now = DateTime.Now.AddHours(1).ToString();

            string sql = "SELECT COUNT(DISTINCT buynick) FROM TCS_Customer WHERE nick = '" + nick + "' AND mobile <> ''";
            //totalcustomer = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 0 AND mobile <> ''";
            //count2 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount = 1 AND mobile <> ''";
            //count3 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND tradecount > 2 AND mobile <> ''";
            //count4 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 0 AND mobile <> ''";
            //count5 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 1 AND mobile <> ''";
            //count6 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 2 AND mobile <> ''";
            //count7 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 3 AND mobile <> ''";
            //count8 = utils.ExecuteString(sql);

            //sql = "SELECT COUNT(*) FROM TCS_Customer WHERE nick = '" + nick + "' AND grade = 4 AND mobile <> ''";
            //count9 = utils.ExecuteString(sql);

            sql = "SELECT * FROM TCS_Group WHERE nick = '" + nick + "' AND isdel = 0";
            DataTable dt = utils.ExecuteDataTable(sql);

            rptGroup.DataSource = dt;
            rptGroup.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string guid = Guid.NewGuid().ToString();
        string sql = string.Empty;
        string typ = utils.NewRequest("typ", utils.RequestType.Form);
        string group = utils.NewRequest("group", utils.RequestType.Form);
        string groupguid = utils.NewRequest("groupguid", utils.RequestType.Form);
        string cuicontent = utils.NewRequest("cuicontent", utils.RequestType.Form);
        string cuidate = utils.NewRequest("cuidate", utils.RequestType.Form);
        string birthdaycontent = utils.NewRequest("birthdaycontent", utils.RequestType.Form);
        string backdate = utils.NewRequest("backdate", utils.RequestType.Form);
        string backcontent = utils.NewRequest("backcontent", utils.RequestType.Form);
        string actdate = utils.NewRequest("actdate", utils.RequestType.Form);
        string actcontent = utils.NewRequest("actcontent", utils.RequestType.Form);
        string isstop = utils.NewRequest("isstop", utils.RequestType.Form);
        string sendnow = utils.NewRequest("sendnow", utils.RequestType.Form);

        //判断是否有同类型活动
        sql = "SELECT COUNT(*) FROM TCS_Mission WHERE nick = '" + nick + "' AND typ = '" + typ + "' AND grade = '" + groupguid + "' AND isdel = 0";
        string count = utils.ExecuteString(sql);
        if (count != "0")
        {
            Response.Write("<script>alert('已经有同类型的服务在执行中了！');history.go(-1);</script>");
            Response.End();
            return;
        }

        string black = string.Empty;
        //判断是否为黑词
        if (CheckIsBlack(msg, ref black))
        {
            Response.Write("<script>alert('短信内容包含黑词“" + black + "”！');history.go(-1);</script>");
            Response.End();
            return;
        }

        switch (typ)
        {
            case "unpay":
                if (cuicontent.Length == 0)
                {
                    Response.Write("<script>alert('请填写内容！');history.go(-1);</script>");
                    Response.End();
                    return;
                }

                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, grade, timecount,isstop) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + cuicontent + "','" + group + "','" + cuidate + "','" + isstop + "')";
                break;
            case "birthday":
                if (birthdaycontent.Length == 0)
                {
                    Response.Write("<script>alert('请填写内容！');history.go(-1);</script>");
                    Response.End();
                    return;
                }
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, grade,isstop) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + birthdaycontent + "','" + group + "','" + isstop + "')";
                break;
            case "back":
                if (backcontent.Length == 0)
                {
                    Response.Write("<script>alert('请填写内容！');history.go(-1);</script>");
                    Response.End();
                    return;
                }
                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, grade, timecount,isstop) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + backcontent + "','" + group + "','" + backdate + "','" + isstop + "')";
                break;
            case "act":
                if (actcontent.Length == 0)
                {
                    Response.Write("<script>alert('请填写内容！');history.go(-1);</script>");
                    Response.End();
                    return;
                }

                //如果是立即发送
                if (sendnow == "1")
                {
                    actdate = DateTime.Now.ToString();
                }

                sql = "INSERT INTO TCS_Mission (guid, nick, typ, content, grade, senddate,isstop) VALUES ('" + guid + "','" + nick + "','" + typ + "','" + actcontent + "','" + groupguid + "','" + actdate + "','" + isstop + "')";
                //SendMutiMsg(actcontent, groupguid);
                break;
        }

        utils.ExecuteNonQuery(sql);

        //Response.Write(sql);
        Response.Redirect("missionlist.aspx");
    }

    private bool CheckIsBlack(string p, ref string black)
    {
        string sql = "SELECT * FROM TCS_BlackWord";
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (p.IndexOf(dt.Rows[i]["word"].ToString()) != -1)
            {
                black = dt.Rows[i]["word"].ToString();
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 短信群发
    /// </summary>
    /// <param name="actcontent"></param>
    private void SendMutiMsg(string actcontent, string groupguid)
    {
        string sql = string.Empty;
        if (groupguid == "0")
        {
            sql = "SELECT * FROM TCS_Customer WHERE nick = '" + nick + "' AND LEN(mobile) > 0";
        }
        else
        {
            sql = "SELECT * FROM TCS_Customer WHERE nick = '" + nick + "' AND LEN(mobile) > 0 AND groupguid = '" + groupguid + "'";
        }
        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            //判断是否还有短信可发
            sql = "SELECT total FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
            string total = utils.ExecuteString(sql);

            if (int.Parse(total) > 0)
            {
                string buynick = dt.Rows[i]["buynick"].ToString();
                string mobile = dt.Rows[i]["mobile"].ToString();

                sql = "SELECT COUNT(*) FROM TCS_MsgSend WHERE buynick = '" + buynick + "' AND nick = '" + nick + "' AND typ = 'act' AND DATEDIFF(d, adddate, GETDATE()) = 0";
                string count = utils.ExecuteString(sql);
                if (count == "0")
                {
                    //强行截取
                    if (actcontent.Length > 66)
                    {
                        actcontent = actcontent.Substring(0, 66);
                    }

                    string result = SendMessage(mobile, actcontent);
                    if (result != "0")
                    {
                        string number = "1";

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
                                            " '" + mobile + "', " +
                                            " '" + actcontent.Replace("'", "''") + "', " +
                                            " '" + result + "', " +
                                            " '" + number + "', " +
                                            " 'act' " +
                                        ") ";
                        if (mobile.Length != 0)
                        {
                            utils.ExecuteNonQuery(sql);
                        }

                        //更新短信数量
                        sql = "UPDATE TCS_ShopConfig SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                }
            }
        }
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
        if (phone.Length == 0)
        {
            return "0";
        }

        string uid = "ZXHD-SDK-0107-XNYFLX";
        string pass = MD5AAA("WEGXBEPY").ToLower();

        msg = UrlEncode(msg);

        string param = "regcode=" + uid + "&pwd=" + pass + "&phone=" + phone + "&CONTENT=" + msg + "&extnum=11&level=1&schtime=null&reportflag=1&url=&smstype=0&key=aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        byte[] bs = Encoding.ASCII.GetBytes(param);

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com:8082/zqhdServer/sendSMS.jsp" + "?" + param);

        req.Method = "GET";

        using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
        {
            using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
            {
                string content = reader.ReadToEnd();

                if (content.IndexOf("<result>0</result>") == -1)
                {
                    //发送失败
                    return content;
                }
                else
                {
                    //发送成功
                    Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(content);
                    string number = "888888";// match[0].Groups[1].ToString();
                    return number;
                }
            }
        }
    }


    /// <summary>
    /// 判断该用户是否订购了该服务
    /// </summary>
    /// <param name="nick"></param>
    /// <returns></returns>
    private bool IsBuy(string nick)
    {
        string sql = "SELECT plus FROM TCS_ShopSession WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string plus = dt.Rows[0][0].ToString();
            if (plus.IndexOf("crm") != -1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}