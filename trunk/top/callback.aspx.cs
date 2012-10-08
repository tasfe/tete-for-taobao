using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Text.RegularExpressions;
using System.Data;
using System.IO;

public partial class top_callback : System.Web.UI.Page
{
    public string nick = string.Empty;
    public string buynick = string.Empty;
    public string coupon = string.Empty;
    public string alipay = string.Empty;
    public string freecard = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString);
        nick = utils.NewRequest("seller_nick", utils.RequestType.QueryString);

        string result = Base64Decode(top_parameters);
        buynick = Regex.Match(result, "visitor_nick=([^&]*)").Groups[1].ToString();

        string laiyuan = utils.NewRequest("laiyuan", utils.RequestType.QueryString);
        string act = utils.NewRequest("action", utils.RequestType.QueryString);
        string newnick = utils.NewRequest("newnick", utils.RequestType.QueryString);
        string ip = Request.UserHostAddress;

        if (laiyuan != "")
        {
            //记录
            string sql = string.Empty;
            sql = "SELECT COUNT(*) FROM TCS_Tui WHERE nick = '" + buynick + "' AND ip = '" + ip + "' AND DATEDIFF(D,adddate, GETDATE()) = 0";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                sql = "INSERT INTO TCS_Tui (nick, ip, laiyuan) VALUES ('" + buynick + "', '" + ip + "','" + laiyuan + "')";
                utils.ExecuteNonQuery(sql);
            }
            else
            {
                sql = "UPDATE TCS_Tui SET count = count + 1 WHERE nick = '" + buynick + "' AND ip = '" + ip + "' AND DATEDIFF(D,adddate, GETDATE()) = 0";
                utils.ExecuteNonQuery(sql);
            }

            Response.Redirect("http://fuwu.taobao.com/service/service.htm?service_code=service-0-22904&laiyuan=" + laiyuan);
            return;
        }


        if (act == "freecard")
        {
            //包邮卡查询
            Response.Redirect("reviewnew/freesearch.aspx?nick=" + HttpUtility.UrlEncode(newnick) + "&buynick=" + HttpUtility.UrlEncode(buynick) + "");
        }
        else
        {
            //File.WriteAllText(Server.MapPath("aaaaa.txt"), Request.Url.ToString());
            string module_width = utils.NewRequest("module_width", utils.RequestType.QueryString);
            if (module_width == "950")
            {
                Response.Redirect("reviewnew/haopingshow_950_1.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&buynick=" + HttpUtility.UrlEncode(buynick) + "");
            }
            else if (module_width == "750")
            {
                if (nick == "美杜莎之心")
                {
                    Response.Redirect("reviewnew/haopingshow_750_1.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&buynick=" + HttpUtility.UrlEncode(buynick) + "");
                }
            }
            else
            {
                Response.Redirect("reviewnew/haopingshow_190_1.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&buynick=" + HttpUtility.UrlEncode(buynick) + "");
            }
        }

        return;

        //test

        BindData();
    }

    private void BindData()
    {
        string sql = "SELECT COUNT(*) FROM TCS_CouponSend WHERE nick = '" + nick + "' AND buynick = '" + buynick + "'";
        coupon = utils.ExecuteString(sql);


        sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE nick = '" + nick + "' AND buynick = '" + buynick + "'";
        alipay = utils.ExecuteString(sql);


        sql = "SELECT COUNT(*) FROM TCS_Freecard WHERE nick = '" + nick + "' AND buynick = '" + buynick + "'";
        freecard = utils.ExecuteString(sql);


        sql = "SELECT TOP 10 * FROM TCS_CouponSend WHERE nick = '" + nick + "' ORDER BY taobaonumber DESC";
        DataTable dt = utils.ExecuteDataTable(sql);

        rptTradeRate.DataSource = dt;
        rptTradeRate.DataBind();


        sql = "SELECT TOP 2 * FROM TCS_TradeRate WHERE nick = '" + nick + "' AND isshow = 1 ORDER BY reviewdate DESC";
        dt = utils.ExecuteDataTable(sql);

        Repeater1.DataSource = dt;
        Repeater1.DataBind();
    }

    /// <summary>
    /// Base64解码
    /// </summary>
    /// <param name="Message"></param>
    /// <returns></returns>
    public static string Base64Decode(string Message)
    {
        if ((Message.Length % 4) != 0)
        {
            throw new ArgumentException("不是正确的BASE64编码，请检查。", "Message");
        }
        if (!System.Text.RegularExpressions.Regex.IsMatch(Message, "^[A-Z0-9/+=]*$", System.Text.RegularExpressions.RegexOptions.IgnoreCase))
        {
            throw new ArgumentException("包含不正确的BASE64编码，请检查。", "Message");
        }
        string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
        int page = Message.Length / 4;
        System.Collections.ArrayList outMessage = new System.Collections.ArrayList(page * 3);
        char[] message = Message.ToCharArray();
        for (int i = 0; i < page; i++)
        {
            byte[] instr = new byte[4];
            instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
            instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
            instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
            instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
            byte[] outstr = new byte[3];
            outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
            if (instr[2] != 64)
            {
                outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
            }
            else
            {
                outstr[2] = 0;
            }
            if (instr[3] != 64)
            {
                outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
            }
            else
            {
                outstr[2] = 0;
            }
            outMessage.Add(outstr[0]);
            if (outstr[1] != 0)
                outMessage.Add(outstr[1]);
            if (outstr[2] != 0)
                outMessage.Add(outstr[2]);
        }
        byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
        return System.Text.Encoding.Default.GetString(outbyte);
    }
}