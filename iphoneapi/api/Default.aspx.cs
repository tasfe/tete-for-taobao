﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Web.Security;

using JdSoft.Apple.Apns.Notifications;
using System.IO;

public partial class api_Default : System.Web.UI.Page
{
    private string act = string.Empty;
    private string uid = string.Empty;
    private string cid = string.Empty;
    private string itemid = string.Empty;
    private string page = string.Empty;
    private string pagesize = string.Empty;
    private string direct = string.Empty;
    private string token = string.Empty;
    private string err = string.Empty;
    private string typ = string.Empty;
    private string msgid = string.Empty;
    private string buynick = string.Empty;
    private string couponid = string.Empty;
    private string qrcode = string.Empty;
    private string mobile = string.Empty;
    private string alerttoken = string.Empty;
    private string isrefresh = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //try
        //{
        act = utils.NewRequest("act", utils.RequestType.QueryString);
        uid = utils.NewRequest("uid", utils.RequestType.QueryString);
        cid = utils.NewRequest("cid", utils.RequestType.QueryString);
        itemid = utils.NewRequest("itemid", utils.RequestType.QueryString);
        page = utils.NewRequest("page", utils.RequestType.QueryString);
        pagesize = utils.NewRequest("pagesize", utils.RequestType.QueryString);
        direct = utils.NewRequest("direct", utils.RequestType.QueryString);
        token = utils.NewRequest("token", utils.RequestType.QueryString);
        typ = utils.NewRequest("typ", utils.RequestType.QueryString);
        msgid = utils.NewRequest("msgid", utils.RequestType.QueryString);
        buynick = utils.NewRequest("buynick", utils.RequestType.QueryString);
        couponid = utils.NewRequest("couponid", utils.RequestType.QueryString);
        mobile = utils.NewRequest("mobile", utils.RequestType.QueryString);
        qrcode = utils.NewRequest("qrcode", utils.RequestType.QueryString);
        alerttoken = utils.NewRequest("alerttoken", utils.RequestType.QueryString);
        isrefresh = utils.NewRequest("isrefresh", utils.RequestType.QueryString);

        err = utils.NewRequest("err", utils.RequestType.Form);

        switch (act)
        {
            case "ads":
                ShowAdsInfo();
                break;
            case "adslist":
                ShowAdsListInfo();
                break;
            case "cate":
                ShowCateInfo();
                break;
            case "cateindex":
                ShowCateInfoIndex();
                break;
            case "catechild":
                ShowCateInfoChild();
                break;
            case "list":
                ShowListInfo();
                break;
            case "listindex":
                ShowListInfoIndex();
                break;
            case "listindexcate":
                ShowListInfoIndexCate();
                break;
            case "special":
                ShowSpecialListInfo();
                break;
            case "hot":
                ShowHotListInfo();
                break;
            case "detail":
                ShowDetailInfo();
                break;
            case "near":
                ShowNearInfo();
                break;
            case "token":
                RecordTokenInfo();
                break;
            case "err":
                RecordErrInfo();
                break;
            case "msgcount":
                ShowMsgCountInfo();
                break;
            case "msglist":
                ShowMsgListInfo();
                break;
            case "msgdetail":
                ShowMsgDetailInfo();
                break;
            case "getcoupon":
                GetCouponInfo();
                break;
            case "couponlist":
                ShowCouponListInfo();
                break;
            case "getcount":
                GetCountInfo();
                break;
            case "sendmsg":
                SendMsgInfo();
                break;
            case "getmsglist":
                GetMsgListInfo();
                break;
            case "cancel":
                CancelInfo();
                break;
            case "buy":
                BuyInfo();
                break;
            case "checkstatus":
                GetCheckStatus();
                break;
            //case "bindtoken":
            //    BindToken();
            //    break;
        }
        //}
        //catch
        //{
        //    string str = "{\"error_response\":\"service_error\"}";
        //    Response.Write(str);
        //}
        ////File.WriteAllText(Server.MapPath(DateTime.Now.Ticks.ToString() + ".txt"), Request.Url.ToString());
    }

    private void GetCheckStatus()
    {
        string str = "{\"status\":0}";

        Response.Write(str);
        Response.End();
    }

    private void ShowAdsListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT logo,url,cateid,title FROM TeteShopAds WHERE nick = '" + uid + "' AND typ = '" + typ + "' ORDER BY adddate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"adslist\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"logo\":\"" + dt.Rows[i][0].ToString() + "\",\"url\":\"" + dt.Rows[i][1].ToString() + "\",\"cateid\":\"" + dt.Rows[i][2].ToString() + "\",\"title\":\"" + dt.Rows[i][3].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"adslist\":\"\",\"adsurllist\":\"\"}";
        }

        Response.Write(str);
    }

    /// <summary>
    /// 购买接口
    /// </summary>
    private void BuyInfo()
    {
        string data = utils.NewRequest("data", utils.RequestType.Form);
        data = utils.NewRequest("data", utils.RequestType.Form);
        //File.WriteAllText(Server.MapPath(DateTime.Now.Ticks.ToString() + ".txt"), data);
        //data = @"{""receipt-data"":""ewoJInNpZ25hdHVyZSIgPSAiQXNQOVNQRThhdUFTV2lwMkhvL1lYaHZua1VTMmRXMUxmdHlTcmdyTzh6TmZ6a3QyNFZTNHVqQ2VvOHpsS2s0MCtUOTR6TXpBaE4xTVNOUnA4ZlFzYloxSDhzSG0yaXNsMXZlNkZIVUhVL3RCdGNNbE8zbHBOVzlGYjNZT3oyRXFESnVLYkhlZzI5cEM1c3VId053Mi9ObDc4dVp3K21HNkNKWUF2dmVSdjY0K0FBQURWekNDQTFNd2dnSTdvQU1DQVFJQ0NHVVVrVTNaV0FTMU1BMEdDU3FHU0liM0RRRUJCUVVBTUg4eEN6QUpCZ05WQkFZVEFsVlRNUk13RVFZRFZRUUtEQXBCY0hCc1pTQkpibU11TVNZd0pBWURWUVFMREIxQmNIQnNaU0JEWlhKMGFXWnBZMkYwYVc5dUlFRjFkR2h2Y21sMGVURXpNREVHQTFVRUF3d3FRWEJ3YkdVZ2FWUjFibVZ6SUZOMGIzSmxJRU5sY25ScFptbGpZWFJwYjI0Z1FYVjBhRzl5YVhSNU1CNFhEVEE1TURZeE5USXlNRFUxTmxvWERURTBNRFl4TkRJeU1EVTFObG93WkRFak1DRUdBMVVFQXd3YVVIVnlZMmhoYzJWU1pXTmxhWEIwUTJWeWRHbG1hV05oZEdVeEd6QVpCZ05WQkFzTUVrRndjR3hsSUdsVWRXNWxjeUJUZEc5eVpURVRNQkVHQTFVRUNnd0tRWEJ3YkdVZ1NXNWpMakVMTUFrR0ExVUVCaE1DVlZNd2daOHdEUVlKS29aSWh2Y05BUUVCQlFBRGdZMEFNSUdKQW9HQkFNclJqRjJjdDRJclNkaVRDaGFJMGc4cHd2L2NtSHM4cC9Sd1YvcnQvOTFYS1ZoTmw0WElCaW1LalFRTmZnSHNEczZ5anUrK0RyS0pFN3VLc3BoTWRkS1lmRkU1ckdYc0FkQkVqQndSSXhleFRldngzSExFRkdBdDFtb0t4NTA5ZGh4dGlJZERnSnYyWWFWczQ5QjB1SnZOZHk2U01xTk5MSHNETHpEUzlvWkhBZ01CQUFHamNqQndNQXdHQTFVZEV3RUIvd1FDTUFBd0h3WURWUjBqQkJnd0ZvQVVOaDNvNHAyQzBnRVl0VEpyRHRkREM1RllRem93RGdZRFZSMFBBUUgvQkFRREFnZUFNQjBHQTFVZERnUVdCQlNwZzRQeUdVakZQaEpYQ0JUTXphTittVjhrOVRBUUJnb3Foa2lHOTJOa0JnVUJCQUlGQURBTkJna3Foa2lHOXcwQkFRVUZBQU9DQVFFQUVhU2JQanRtTjRDL0lCM1FFcEszMlJ4YWNDRFhkVlhBZVZSZVM1RmFaeGMrdDg4cFFQOTNCaUF4dmRXLzNlVFNNR1k1RmJlQVlMM2V0cVA1Z204d3JGb2pYMGlreVZSU3RRKy9BUTBLRWp0cUIwN2tMczlRVWU4Y3pSOFVHZmRNMUV1bVYvVWd2RGQ0TndOWXhMUU1nNFdUUWZna1FRVnk4R1had1ZIZ2JFL1VDNlk3MDUzcEdYQms1MU5QTTN3b3hoZDNnU1JMdlhqK2xvSHNTdGNURXFlOXBCRHBtRzUrc2s0dHcrR0szR01lRU41LytlMVFUOW5wL0tsMW5qK2FCdzdDMHhzeTBiRm5hQWQxY1NTNnhkb3J5L0NVdk02Z3RLc21uT09kcVRlc2JwMGJzOHNuNldxczBDOWRnY3hSSHVPTVoydG04bnBMVW03YXJnT1N6UT09IjsKCSJwdXJjaGFzZS1pbmZvIiA9ICJld29KSW05eWFXZHBibUZzTFhCMWNtTm9ZWE5sTFdSaGRHVXRjSE4wSWlBOUlDSXlNREV5TFRBM0xUSTJJREExT2pVd09qQTRJRUZ0WlhKcFkyRXZURzl6WDBGdVoyVnNaWE1pT3dvSkluVnVhWEYxWlMxcFpHVnVkR2xtYVdWeUlpQTlJQ0kxTURjM09URTFPVFJrWVRrME5qWTBaREUzTmpreE5HUmlNVEU0TVdZMFlUUm1OV0k1TTJVeklqc0tDU0p2Y21sbmFXNWhiQzEwY21GdWMyRmpkR2x2YmkxcFpDSWdQU0FpTVRBd01EQXdNREExTXpRNE56UXhPQ0k3Q2draVluWnljeUlnUFNBaU1TNHdJanNLQ1NKMGNtRnVjMkZqZEdsdmJpMXBaQ0lnUFNBaU1UQXdNREF3TURBMU16UTROelF4T0NJN0Nna2ljWFZoYm5ScGRIa2lJRDBnSWpFaU93b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGJYTWlJRDBnSWpFek5ETXpNRGN3TURnek5ERWlPd29KSW5CeWIyUjFZM1F0YVdRaUlEMGdJbU52YlM1amIyTnZMbk50YzE4eE1DSTdDZ2tpYVhSbGJTMXBaQ0lnUFNBaU5UUTJOVGN6TXpBeElqc0tDU0ppYVdRaUlEMGdJbU52YlM1amIyTnZMblJwYldsdVoxTk5VeUk3Q2draWNIVnlZMmhoYzJVdFpHRjBaUzF0Y3lJZ1BTQWlNVE0wTXpNd056QXdPRE0wTVNJN0Nna2ljSFZ5WTJoaGMyVXRaR0YwWlNJZ1BTQWlNakF4TWkwd055MHlOaUF4TWpvMU1Eb3dPQ0JGZEdNdlIwMVVJanNLQ1NKd2RYSmphR0Z6WlMxa1lYUmxMWEJ6ZENJZ1BTQWlNakF4TWkwd055MHlOaUF3TlRvMU1Eb3dPQ0JCYldWeWFXTmhMMHh2YzE5QmJtZGxiR1Z6SWpzS0NTSnZjbWxuYVc1aGJDMXdkWEpqYUdGelpTMWtZWFJsSWlBOUlDSXlNREV5TFRBM0xUSTJJREV5T2pVd09qQTRJRVYwWXk5SFRWUWlPd3A5IjsKCSJlbnZpcm9ubWVudCIgPSAiU2FuZGJveCI7CgkicG9kIiA9ICIxMDAiOwoJInNpZ25pbmctc3RhdHVzIiA9ICIwIjsKfQ==""}";

        string sql = string.Empty;
        string str = string.Empty;
        string msgCount = string.Empty;
        string url = "https://sandbox.itunes.apple.com/verifyReceipt";
        //url = "https://buy.itunes.apple.com/verifyReceipt";
        string result = SendPostData(url, data);
        string orderid = Regex.Match(result, @"""original_transaction_id"":""([^""]*)""").Groups[1].ToString();
        string typ = Regex.Match(result, @"""product_id"":""([^""]*)""").Groups[1].ToString();
        string status = Regex.Match(result, @"""status"":([0-9]*)").Groups[1].ToString();


        if (status == "0")
        {
            if (uid == "huli")
            {
                if (typ == "com.leon.sms_1_15")
                {
                    msgCount = "15";
                }
                if (typ == "com.leon.sms_3_60")
                {
                    msgCount = "60";
                }
                if (typ == "com.leon.sms_5_110")
                {
                    msgCount = "110";
                }
                if (typ == "com.leon.sms_10_240")
                {
                    msgCount = "240";
                }
            }
            else
            {
                if (typ == "com.leon.sms_1_15")
                {
                    msgCount = "15";
                }
                if (typ == "com.leontt.sms_3_60")
                {
                    msgCount = "60";
                }
                if (typ == "com.leon.sms_5_110")
                {
                    msgCount = "110";
                }
                if (typ == "com.leon.sms_10_240")
                {
                    msgCount = "240";
                }
            }

            sql = "SELECT COUNT(*) FROM HuliBuyLog WHERE orderid = '" + orderid + "'";
            string count = utils.ExecuteString(sql);
            if (count == "0")
            {
                sql = "INSERT INTO HuliBuyLog (token, adddate, typ, orderid, count) VALUES ('" + token + "',GETDATE(),'" + typ + "','" + orderid + "','" + msgCount + "')";
                utils.ExecuteNonQuery(sql);

                //加短信
                sql = "UPDATE [TeteUserToken] SET total = total + " + msgCount + " WHERE token = '" + token + "' AND nick = '" + uid + "'";
                //File.WriteAllText(Server.MapPath("aaaaa.txt"), result + "\r\n\r\n" + sql);
                utils.ExecuteNonQuery(sql);

                str = "{\"result\":\"" + msgCount + "\",\"orderid\":\"" + orderid + "\"}";
                Response.Write(str);
            }
            else
            {
                str = "{\"result\":\"0\"}";
                Response.Write(str);
            }
        }
        else
        {
            str = "{\"result\":\"-1\"}";
            Response.Write(str);
        }
    }

    private void CancelInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string guid = utils.NewRequest("guid", utils.RequestType.QueryString);

        sql = "UPDATE HuliUserMsg SET iscancel = 1 WHERE guid = '" + guid + "'";
        utils.ExecuteNonQuery(sql);
        //Response.Write(sql);

        str = "{\"guid\":\"" + guid + "\"}";
        Response.Write(str);
    }



    private void GetMsgListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string guid = string.Empty;
        string mobile = utils.NewRequest("mobile", utils.RequestType.QueryString);
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        string content = utils.NewRequest("content", utils.RequestType.QueryString);
        string delay = utils.NewRequest("delay", utils.RequestType.QueryString);

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }
        int pageCount = 20;
        int dataCount = (pageNow - 1) * pageCount;

        sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY adddate DESC) AS rownumber FROM HuliUserMsg WHERE token='" + token + "' AND nick = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY adddate DESC";

        DataTable dt = utils.ExecuteDataTable(sql);
        str = "{\"msg\":[";
        if (dt.Rows.Count != 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"guid\":\"" + dt.Rows[i]["guid"].ToString() + "\",\"content\":\"" + dt.Rows[i]["content"].ToString() + "\",\"mobile\":\"" + dt.Rows[i]["mobile"].ToString() + "\",\"adddate\":\"" + dt.Rows[i]["adddate"].ToString() + "\",\"ispass\":\"" + dt.Rows[i]["ispass"].ToString() + "\",\"delay\":\"" + dt.Rows[i]["delay"].ToString() + "\",\"issend\":\"" + dt.Rows[i]["issend"].ToString() + "\",\"senddate\":\"" + dt.Rows[i]["senddate"].ToString() + "\",\"iscancel\":\"" + dt.Rows[i]["iscancel"].ToString() + "\"}";
            }
        }
        str += "]}";

        Response.Write(str);
    }


    private void SendMsgInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string guid = string.Empty;
        string mobile = utils.NewRequest("mobile", utils.RequestType.QueryString);
        string content = utils.NewRequest("content", utils.RequestType.QueryString);
        string delay = utils.NewRequest("delay", utils.RequestType.QueryString);


        //str = "{\"err\":\"短信服务器安全维护，定时短信暂时无法提交，非常抱歉！\"}";
        //Response.Write(str);
        //Response.End();
        //return;

        sql = "SELECT total FROM TeteUserToken WHERE token = '" + token + "' AND nick = '" + uid + "'";
        guid = Guid.NewGuid().ToString();
        string total = utils.ExecuteString(sql);
        if (int.Parse(total) > 0)
        {
            //判定是否发过
            sql = "SELECT COUNT(*) FROM HuliUserMsg WHERE content = '" + content.Replace("'", "''") + "' AND ispass = 1";
            string count = utils.ExecuteString(sql);

            if (count != "0")
            {
                sql = "INSERT INTO HuliUserMsg (guid, nick, token, mobile, content, delay, ispass) VALUES ('" + guid + "','" + uid + "','" + token + "','" + mobile.Replace("'", "''") + "','" + content.Replace("'", "''") + "','" + delay + "','1')";
                utils.ExecuteNonQuery(sql);
                str = "{\"guid\":\"" + guid + "\",\"status\":1}";
            }
            else
            {
                sql = "INSERT INTO HuliUserMsg (guid, nick, token, mobile, content, delay, ispass) VALUES ('" + guid + "','" + uid + "','" + token + "','" + mobile.Replace("'", "''") + "','" + content + "','" + delay + "','0')";
                utils.ExecuteNonQuery(sql);
                str = "{\"guid\":\"" + guid + "\",\"status\":0}";
            }
        }
        else
        {
            str = "{\"guid\":\"" + guid + "\",\"status\":-1}";
        }

        Response.Write(str);
    }

    private void GetCountInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "UPDATE [TeteUserToken] SET updatedate = GETDATE() WHERE token = '" + token + "'";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT * FROM TeteUserToken WHERE token = '" + token + "' AND nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"count\":" + dt.Rows[0]["total"].ToString() + "}";
        }
        else
        {
            str = "{\"count\":0}";
        }

        Response.Write(str);
    }

    /// <summary>
    /// 
    /// </summary>
    private void ShowCouponListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserCoupon WHERE token = '" + token + "' AND nick = '" + uid + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"coupon\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\",\"date\":\"" + dt.Rows[i]["adddate"].ToString() + "\",\"isread\":\"" + dt.Rows[i]["isread"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    ///// <summary>
    ///// 绑定会员名到TOKEN上
    ///// </summary>
    //private void BindToken()
    //{
    //    string sql = string.Empty;
    //    string str = string.Empty;

    //    //绑定淘宝会员和该手机
    //}

    private void GetCouponInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        //判断该2唯码是否有效,如果无效给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"errNumber\"}");
            return;
        }

        //如果有效则判断对应的2唯码是否过期，如果过期给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"expireNumber\"}");
            return;
        }

        //判断该会员是否领取过该优惠券，如果领过给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"alreadyGet\"}");
            return;
        }

        //如果没过期则赠送给该淘宝会员，如果该会员没在此店铺购买过东西给出提示
        if (1 == 2)
        {
            Response.Write("{\"result\":\"notUser\"}");
            return;
        }

        couponid = qrcode.Replace("{\"coupon\":\"", "").Replace("\"}", "");
        /*
                //给客户赠送优惠券并给出提示
                string number = "110";
                sql = "SELECT taobaocouponid FROM TeteCoupon WHERE guid = '" + couponid + "'";
        
                string taobaocouponid = utils.ExecuteString(sql);
                //执行优惠券赠送行为
                string appkey = "12159997";
                string secret = "614e40bfdb96e9063031d1a9e56fbed5";
                string session = "610261249120700fcda21f569d424328537a3d82a101d7c204200856";
                //buynick = "叶儿随清风";

                IDictionary<string, string> param = new Dictionary<string, string>();
                param.Add("coupon_id", taobaocouponid);
                param.Add("buyer_nick", buynick);

                string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
                number = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                //Response.Write(result);
                //Response.Write("<br>");
                //Response.Write(number);
        */

        if (couponid == "tetesoft-getcoupon")
        {
            Response.Write("{\"url\":\"http://tacera.m.tmall.com/\",\"message\":\"恭喜您获得本店送出的满100减20优惠券!\"}");
        }
        else if (couponid == "tetesoft-promotion")
        {
            Response.Write("{\"url\":\"http://tacera.m.tmall.com/shop/a-5-5-40-487083979-42-698457170.htm?sid=b6daf44b5655714a\",\"message\":\"店铺特价风暴正在进行中,赶快去抢!\"}");
        }
        else if (couponid == "tetesoft-choujiang")
        {
            Response.Write("{\"url\":\"http://tr.isv8.com/?15001\",\"message\":\"请问是否马上要去抽奖?\"}");
        }
        else if (couponid == "tetesoft-detail")
        {
            Response.Write("{\"url\":\"http://a.m.tmall.com/i10169621941.htm?sid=b6daf44b5655714a\",\"message\":\"秒杀商品进行中,赶快去抢!\"}");
        }
        else
        {
            Response.Write("{\"url\":\"http://tacera.m.tmall.com/\",\"message\":\"恭喜您获得本店送出的满100减50优惠券!\"}");
        }


    }

    private void ShowMsgDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE nick = '" + uid + "' AND id = " + msgid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str += "{\"id\":\"" + dt.Rows[0]["id"].ToString() + "\",\"html\":\"" + dt.Rows[0]["html"].ToString() + "\"}";

            //更新标记为已读
            sql = "UPDATE TeteUserMsg SET isread = 1 WHERE nick = '" + uid + "' AND id = " + msgid + "";
            utils.ExecuteNonQuery(sql);
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' ORDER BY id DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"msg\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"id\":\"" + dt.Rows[i]["id"].ToString() + "\",\"title\":\"" + dt.Rows[i]["title"].ToString() + "\",\"date\":\"" + DateTime.Parse(dt.Rows[i]["adddate"].ToString()).ToString("yyyy-MM-dd") + "\",\"isread\":\"" + dt.Rows[i]["isread"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowMsgCountInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserMsg WHERE token = '" + token + "' AND nick = '" + uid + "' AND isread = 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"count\":\"" + dt.Rows[0][0].ToString() + "\"}";
        }
        else
        {
            str = "{\"count\":\"\",\"0\":\"\"}";
        }

        Response.Write(str);
    }


    private void RecordErrInfo()
    {
        string sql = string.Empty;


        sql = "INSERT INTO TeteUserErr (err, token, nick) VALUES ('" + err + "', '" + token + "', '" + uid + "')";
        utils.ExecuteNonQuery(sql);


        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }

    private void RecordTokenInfo()
    {
        string sql = string.Empty;

        sql = "SELECT COUNT(*) FROM TeteUserToken WHERE nick = '" + uid + "' AND token = '" + token + "'";
        //Response.Write(sql + "<br>");
        string count = utils.ExecuteString(sql);

        if (count == "0")
        {
            if (uid == "huli")
            {
                sql = "INSERT INTO TeteUserToken (nick, token, mobile, total) VALUES ('" + uid + "', '" + token + "', '" + mobile + "', 0)";
                //Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
            else if (uid == "huli1")
            {
                sql = "INSERT INTO TeteUserToken (nick, token, mobile, total) VALUES ('" + uid + "', '" + token + "', '" + mobile + "', 1)";
                //Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
            else
            {
                sql = "INSERT INTO TeteUserToken (nick, token, mobile) VALUES ('" + uid + "', '" + token + "', '" + mobile + "')";
                //Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }
        }
        else
        {
            string verify = string.Empty;
            if (token.Length > 0)
            {
                verify = token.Substring(0, 4);
            }

            if (alerttoken.Length != 0)
            {
                sql = "UPDATE TeteUserToken SET mobile = '" + mobile + "',alerttoken='" + alerttoken + "',updatedate = GETDATE(),logintimes = logintimes + 1,verify='" + verify + "' WHERE token = '" + token + "' AND nick = '" + uid + "'";
                //Response.Write(sql + "<br>");
                utils.ExecuteNonQuery(sql);
            }

            sql = "SELECT * FROM TeteUserToken WHERE token = '" + token + "' AND nick = '" + uid + "' AND issend = 0";
            //Response.Write(sql + "<br>");
            DataTable dt = utils.ExecuteDataTable(sql);

            string filePath = string.Empty;
            string pass = "3561402";

            if (uid == "huli")
            {
                filePath = Server.MapPath("../p12/sms_huli.p12");
                SendAlert(dt, "亲，感谢您对定时短信的支持，为了回馈客户，现在您给我们5星好评即可获赠2条免费短信，评价时请带上您的标志【" + verify + "】！", filePath, pass);
            }
            else if (uid == "huli1")
            {
                filePath = Server.MapPath("../p12/sms_huli1.p12");
                SendAlert(dt, "亲，感谢您对定时短信的支持，为了回馈客户，现在您给我们5星好评即可获赠20条免费短信，评价时请带上您的标志【" + verify + "】！", filePath, pass);
            }
        }
        ////File.WriteAllText(Server.MapPath("aaa.txt"), Request.Url.ToString());
        string str = "{\"result\":\"ok\"}";
        Response.Write(str);
    }


    private void SendAlert(DataTable dt, string msg, string file, string pass)
    {
        bool sandbox = false;
        string p12File = file;
        string p12FilePassword = pass;

        string p12Filename = file;

        NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

        service.SendRetries = 5; //5 retries before generating notificationfailed event
        service.ReconnectDelay = 2000; //5 seconds

        for (int i = 1; i < dt.Rows.Count + 1; i++)
        {
            //Create a new notification to send
            Notification alertNotification = new Notification(dt.Rows[i - 1]["alerttoken"].ToString());
            //Response.Write(dt.Rows[i - 1]["alerttoken"].ToString());

            alertNotification.Payload.Alert.Body = string.Format(msg, i);
            alertNotification.Payload.Sound = "default";
            alertNotification.Payload.Badge = 1;

            service.QueueNotification(alertNotification);
        }
    }

    private void ShowNearInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        if (direct == "left")
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id < (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id DESC";
        }
        else
        {
            sql = "SELECT TOP 1 * FROM TeteShopItem WHERE nick = '" + uid + "' AND id > (SELECT id FROM TeteShopItem WHERE itemid = " + itemid + ") AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY id ASC";
        }

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowDetailInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND itemid = " + itemid + "";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }


    private void ShowSpecialListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string con = string.Empty;

        if (uid == "taozhe")
        {
            //直接查询淘宝客
            string appkey = "21088121";
            string secret = "f115a6790148d314cf3214aa029f7eda";

            if (page == "")
            {
                page = "1";
            }

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "num_iid,title,pic_url,click_url,price");

            if (int.Parse(page) > 10 && int.Parse(page) <= 20)
            {
                param.Add("keyword", "潮女");
                param.Add("page_no", (int.Parse(page) - 10).ToString());
            }
            else if (int.Parse(page) > 20 && int.Parse(page) <= 30)
            {
                param.Add("keyword", "女包");
                param.Add("page_no", (int.Parse(page) - 20).ToString());
            }
            else if (int.Parse(page) > 30 && int.Parse(page) <= 40)
            {
                param.Add("keyword", "女鞋");
                param.Add("page_no", (int.Parse(page) - 30).ToString());
            }
            if (int.Parse(page) > 40 && int.Parse(page) <= 50)
            {
                param.Add("keyword", "首饰");
                param.Add("page_no", (int.Parse(page) - 40).ToString());
            }
            else
            {
                param.Add("keyword", "淑女");
                param.Add("page_no", page);
            }

            param.Add("sort", "commissionNum_desc");
            param.Add("is_mobile", "true");
            param.Add("page_size", "20");

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.taobaoke.items.get", "", param);

            Regex reg = new Regex(@"<click_url>([^<]*)</click_url><num_iid>([^<]*)</num_iid><pic_url>([^<]*)</pic_url><price>([^<]*)</price><title>([^<]*)</title>", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(result);


            str = "{\"item\":[";
            for (int i = 0; i < match.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                if (uid == "taozhe" || uid == "taobao1")
                {
                    string fileName = Server.MapPath("tmpimg/" + strMD5(match[i].Groups[3].ToString() + "_100x100.jpg"));
                    
                    //保存临时图片获取图片尺寸
                    if (!File.Exists(fileName))
                    {
                        WebClient c = new WebClient();
                        c.DownloadFile(match[i].Groups[3].ToString() + "_100x100.jpg", fileName);
                    }
                    System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

                    str += "{\"itemid\":\"" + match[i].Groups[2].ToString() + "\",\"pic_url\":\"" + match[i].Groups[3].ToString() + "\",\"name\":\"" + ReplaceTitleHtml(match[i].Groups[5].ToString()) + "\",\"detail_url\":\"" + match[i].Groups[1].ToString() + "\",\"width\":" + img.Width.ToString() + ",\"height\":" + img.Height.ToString() + ",\"price\":" + match[i].Groups[4].ToString() + "}";
                }
                else
                {
                    str += "{\"itemid\":\"" + match[i].Groups[2].ToString() + "\",\"pic_url\":\"" + match[i].Groups[3].ToString() + "_240x240.jpg\",\"name\":\"" + ReplaceTitleHtml(match[i].Groups[5].ToString()) + "\",\"detail_url\":\"" + match[i].Groups[1].ToString() + "\"}";
                }

            }
            str += "],\"pagenow\":" + page + ",\"total\":100,\"isrefresh\":0}";
        }
        else
        {
            int pageNow = 1;
            if (page == "")
            {
                pageNow = 1;
            }
            else
            {
                pageNow = int.Parse(page);
            }

            int pageSizeNow = 20;
            if (pagesize == "")
            {
                pageSizeNow = 20;
            }
            else
            {
                pageSizeNow = int.Parse(pagesize);
            }
            int pageCount = pageSizeNow;
            int dataCount = (pageNow - 1) * pageCount;


            if (cid.Length == 0)
            {
                sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1";
            }
            else
            {

                sql = "SELECT * FROM TeteShopCategory WHERE parentid = '" + cid + "'";
                DataTable dt1 = utils.ExecuteDataTable(sql);
                con = "(1 = 2";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    con += " OR CHARINDEX('" + dt1.Rows[i]["cateid"].ToString() + "', cateid) > 0";
                }
                con += " OR CHARINDEX('" + cid + "', cateid) > 0)";

                sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1 AND " + con + "";
            }
            int totalCount = int.Parse(utils.ExecuteString(sql));
            int totalPageCount = 1;

            if (totalCount % pageCount == 0)
            {
                totalPageCount = totalCount / pageCount;
            }
            else
            {
                totalPageCount = totalCount / pageCount + 1;
            }

            if (cid.Length == 0)
            {
                if (uid.IndexOf("taobao") != -1)
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY adddate desc,NEWID) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY adddate desc,NEWID";
                }
                else
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY orderid) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY orderid";
                }
            }
            else
            {
                if (uid.IndexOf("taobao") != -1)
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY adddate desc,NEWID) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1 AND " + con + ") AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY adddate desc,NEWID";
                }
                else
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY orderid) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND isnew = 1 AND " + con + ") AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY orderid";
                }
            }

            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"item\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }

                    if (uid == "hishop" || uid == "tacera")
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                    else
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\",\"width\":" + dt.Rows[i]["width"].ToString() + ",\"height\":" + dt.Rows[i]["height"].ToString() + ",\"price\":" + dt.Rows[i]["price"].ToString() + "}";
                    }
                }
                str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + ",\"isrefresh\":0}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }

        Response.Write(str);
    }


    public static string strMD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5");
    }

    private void ShowHotListInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string con = string.Empty;

        if (uid == "taobao111")
        {
            //直接查询淘宝客
            string appkey = "21088121";
            string secret = "f115a6790148d314cf3214aa029f7eda";

            if (page == "")
            {
                page = "1";
            }

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "num_iid,title,pic_url,click_url,price");
            param.Add("keyword", "淑女 新品 服饰");
            param.Add("sort", "commissionNum_desc");
            param.Add("is_mobile", "true");
            param.Add("page_no", page);
            param.Add("page_size", "20");

            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.taobaoke.items.get", "", param);

            Regex reg = new Regex(@"<click_url>([^<]*)</click_url><num_iid>([^<]*)</num_iid><pic_url>([^<]*)</pic_url><price>([^<]*)</price><title>([^<]*)</title>", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(result);


            str = "{\"item\":[";
            for (int i = 0; i < match.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                string fileName = Server.MapPath("tmpimg/" + strMD5(match[i].Groups[3].ToString() + "_100x100.jpg"));
                    
                //保存临时图片获取图片尺寸
                if (!File.Exists(fileName))
                {
                    WebClient c = new WebClient();
                    c.DownloadFile(match[i].Groups[3].ToString() + "_100x100.jpg", fileName);
                }
                System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

                str += "{\"itemid\":\"" + match[i].Groups[2].ToString() + "\",\"pic_url\":\"" + match[i].Groups[3].ToString() + "\",\"name\":\"" + ReplaceTitleHtml(match[i].Groups[5].ToString()) + "\",\"detail_url\":\"" + match[i].Groups[1].ToString() + "\",\"width\":" + img.Width.ToString() + ",\"height\":" + img.Height.ToString() + ",\"price\":" + match[i].Groups[4].ToString() + "}";

                //str += "{\"itemid\":\"" + match[i].Groups[2].ToString() + "\",\"pic_url\":\"" + match[i].Groups[3].ToString() + "\",\"name\":\"" + ReplaceTitleHtml(match[i].Groups[5].ToString()) + "\",\"detail_url\":\"" + match[i].Groups[1].ToString() + "\",\"price\":" + match[i].Groups[4].ToString() + "}";

            }
            str += "],\"pagenow\":" + page + ",\"total\":100,\"isrefresh\":0}";
        }
        else
        {
            int pageNow = 1;
            if (page == "")
            {
                pageNow = 1;
            }
            else
            {
                pageNow = int.Parse(page);
            }

            int pageSizeNow = 20;
            if (pagesize == "")
            {
                pageSizeNow = 20;
            }
            else
            {
                pageSizeNow = int.Parse(pagesize);
            }
            int pageCount = pageSizeNow;
            int dataCount = (pageNow - 1) * pageCount;

            if (cid.Length == 0)
            {
                sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "'";
            }
            else
            {
                sql = "SELECT * FROM TeteShopCategory WHERE parentid = '" + cid + "'";
                DataTable dt1 = utils.ExecuteDataTable(sql);
                con = "(1 = 2";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    con += " OR CHARINDEX('" + dt1.Rows[i]["cateid"].ToString() + "', cateid) > 0";
                }
                con += " OR CHARINDEX('" + cid + "', cateid) > 0)";

                sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND " + con + "";
            }


            int totalCount = int.Parse(utils.ExecuteString(sql));
            int totalPageCount = 1;

            if (totalCount % pageCount == 0)
            {
                totalPageCount = totalCount / pageCount;
            }
            else
            {
                totalPageCount = totalCount / pageCount + 1;
            }

            if (cid.Length == 0)
            {
                if (uid.IndexOf("taobao") != -1)
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY adddate desc,NEWID DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND ishot = 1) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY adddate desc,NEWID DESC";
                }
                else
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY price DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY NEWID()";
                }
            }
            else
            {
                if (uid.IndexOf("taobao") != -1)
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY adddate desc,NEWID DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND ishot = 1) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY adddate desc,NEWID DESC";
                }
                else
                {
                    sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY price DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "'  AND " + con + ") AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY NEWID()";
                }
            }

            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"item\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }
                    if (uid == "hishop" || uid == "tacera")
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                    else
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\",\"width\":" + dt.Rows[i]["width"].ToString() + ",\"height\":" + dt.Rows[i]["height"].ToString() + ",\"price\":" + dt.Rows[i]["price"].ToString() + "}";
                    }
                }
                str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }

        Response.Write(str);
    }


    private void ShowListInfoIndex()
    {
        string sql = string.Empty;
        string str = string.Empty;

        if (uid.IndexOf("taobao") != -1)
        {
            sql = "SELECT TOP 5 * FROM TeteShopItem WHERE isindex = 1 AND nick = '" + uid + "' AND cateid = '5' ORDER BY adddate DESC,orderid";
            //Response.Write(sql);
            //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
            //File.WriteAllText(Server.MapPath("aaa111.txt"), sql + "-" + Request.Url.ToString());
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"item\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }

                    str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                }
                str += "]}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }
        else
        {

            sql = "SELECT TOP 5 * FROM TeteShopItem WHERE isnew = 1 AND nick = '" + uid + "' ORDER BY orderid";
            //Response.Write(sql);
            //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
            //File.WriteAllText(Server.MapPath("aaa111.txt"), sql + "-" + Request.Url.ToString());
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"item\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }

                    if (uid == "hishop" || uid == "tacera")
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                    else
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                }
                str += "]}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }

        Response.Write(str);
    }





    private void ShowListInfoIndexCate()
    {
        string sql = string.Empty;
        string str = string.Empty;
        string con = string.Empty;

        sql = "SELECT * FROM TeteShopCategory WHERE parentid = '" + cid + "'";
        DataTable dt1 = utils.ExecuteDataTable(sql);
        con = "(1 = 2";
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            con += " OR CHARINDEX('" + dt1.Rows[i]["cateid"].ToString() + "', cateid) > 0";
        }
        con += " OR CHARINDEX('" + cid + "', cateid) > 0)";


        if (uid.IndexOf("taobao") != -1)
        {
            sql = "SELECT TOP 5 * FROM TeteShopItem WHERE isindex = 1 AND nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY adddate DESC,orderid";

            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"new\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }

                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    
                }
                str += "]";

                str += ",\"hot\":[";

                for (int i = 3; i < dt.Rows.Count; i++)
                {
                    if (i != 3)
                    {
                        str += ",";
                    }

                    str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                }

                str += "]}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }
        else
        {
            //sql = "SELECT TOP 5 * FROM TeteShopItem WHERE isnew = 1 AND nick = '" + uid + "' ORDER BY orderid";
            //Response.Write(sql);
            sql = "SELECT TOP 5 * FROM TeteShopItem WHERE isnew = 1 AND nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0 ORDER BY orderid";
            //File.WriteAllText(Server.MapPath(DateTime.Now.Ticks.ToString() + ".txt"), sql);
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                str = "{\"new\":[";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i != 0)
                    {
                        str += ",";
                    }

                    if (uid == "hishop" || uid == "tacera")
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                    else
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                }
                str += "]";

                str += ",\"hot\":[";

                for (int i = 3; i < dt.Rows.Count; i++)
                {
                    if (i != 3)
                    {
                        str += ",";
                    }

                    if (uid == "hishop" || uid == "tacera")
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                    else
                    {
                        str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "_240x240.jpg\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                    }
                }

                str += "]}";
            }
            else
            {
                str = "{\"count\":\"0\"}";
            }
        }

        Response.Write(str);
    }

    private string ReplaceTitleHtml(string title)
    {
        string str = Regex.Replace(title, @"&lt;[^&]*&gt;", "");

        return str;
    }

    private void ShowListInfo()
    {
        if (typ == "hot")
        {
            ShowHotListInfo();
            return;
        }

        string sql = string.Empty;
        string str = string.Empty;

        int pageNow = 1;
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        int pageSizeNow = 20;
        if (pagesize == "")
        {
            pageSizeNow = 20;
        }
        else
        {
            pageSizeNow = int.Parse(pagesize);
        }
        int pageCount = pageSizeNow;
        int dataCount = (pageNow - 1) * pageCount;


        if (cid.Length == 0)
        {
            sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "'";
        }
        else
        {
            sql = "SELECT COUNT(*) FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        }


        int totalCount = int.Parse(utils.ExecuteString(sql));
        int totalPageCount = 1;

        if (totalCount % pageCount == 0)
        {
            totalPageCount = totalCount / pageCount;
        }
        else
        {
            totalPageCount = totalCount / pageCount + 1;
        }

        if (cid.Length == 0)
        {
            sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY itemname DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "') AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY itemname DESC";
        }
        else
        {
            sql = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT *,ROW_NUMBER() OVER (ORDER BY itemname DESC) AS rownumber FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY itemname DESC";
        }
        //Response.Write(sql);
        //sql = "SELECT * FROM TeteShopItem WHERE nick = '" + uid + "' AND CHARINDEX('" + cid + "', cateid) > 0";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"item\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                if (uid == "hishop" || uid == "tacera")
                {
                    str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\"}";
                }
                else
                {
                    str += "{\"itemid\":\"" + dt.Rows[i]["itemid"].ToString() + "\",\"pic_url\":\"" + dt.Rows[i]["picurl"].ToString() + "\",\"name\":\"" + dt.Rows[i]["itemname"].ToString() + "\",\"detail_url\":\"" + dt.Rows[i]["linkurl"].ToString() + "\",\"width\":" + dt.Rows[i]["width"].ToString() + ",\"height\":" + dt.Rows[i]["height"].ToString() + ",\"price\":" + dt.Rows[i]["price"].ToString() + "}";
                }
            }
            str += "],\"pagenow\":" + page + ",\"total\":" + totalPageCount + "}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }


    private void ShowCateInfoIndex()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT TOP 5 * FROM TeteShopCategory WHERE nick = '" + uid + "' AND parentid=0 ORDER BY orderid";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"cate\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString().Substring(0, 2) + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\",\"catepicurl\":\"" + dt.Rows[i]["catepicurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }


    private void ShowCateInfoChild()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + uid + "' AND parentid=" + cid + " ORDER BY orderid";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"cate\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString().Substring(0, 2) + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\",\"catepicurl\":\"" + dt.Rows[i]["catepicurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    private void ShowCateInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT * FROM TeteShopCategory WHERE nick = '" + uid + "' AND catename <> '' ORDER BY orderid1";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"cate\":[";
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i != 0)
                {
                    str += ",";
                }

                str += "{\"cid\":\"" + dt.Rows[i]["cateid"].ToString() + "\",\"parent_cid\":\"" + dt.Rows[i]["parentid"].ToString() + "\",\"name\":\"" + dt.Rows[i]["catename"].ToString() + "\",\"count\":\"" + dt.Rows[i]["catecount"].ToString() + "\",\"catepicurl\":\"" + dt.Rows[i]["catepicurl"].ToString() + "\"}";
            }
            str += "]}";
        }
        else
        {
            str = "{\"count\":\"0\"}";
        }

        Response.Write(str);
    }

    /// <summary>
    /// 将客户设置的广告图片地址发送给客户端
    /// </summary>
    private void ShowAdsInfo()
    {
        string sql = string.Empty;
        string str = string.Empty;

        sql = "SELECT logo,ads FROM TeteShop WHERE nick = '" + uid + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str = "{\"logo\":\"" + dt.Rows[0][0].ToString() + "\",\"adsurl\":\"" + dt.Rows[0][1].ToString() + "\"}";
        }
        else
        {
            str = "{\"logo\":\"\",\"adsurl\":\"\"}";
        }

        Response.Write(str);
    }




    /// <summary> 
    /// 给TOP请求签名 API v2.0 
    /// </summary> 
    /// <param name="parameters">所有字符型的TOP请求参数</param> 
    /// <param name="secret">签名密钥</param> 
    /// <returns>签名</returns> 
    protected static string CreateSign(IDictionary<string, string> parameters, string secret)
    {
        parameters.Remove("sign");
        IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
        IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();
        StringBuilder query = new StringBuilder(secret);
        while (dem.MoveNext())
        {
            string key = dem.Current.Key;
            string value = dem.Current.Value;
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                query.Append(key).Append(value);
            }
        }
        query.Append(secret);
        MD5 md5 = MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(query.ToString()));
        StringBuilder result = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            string hex = bytes[i].ToString("X");
            if (hex.Length == 1)
            {
                result.Append("0");
            }
            result.Append(hex);
        }
        return result.ToString();
    }
    /// <summary> 
    /// 组装普通文本请求参数。 
    /// </summary> 
    /// <param name="parameters">Key-Value形式请求参数字典</param> 
    /// <returns>URL编码后的请求数据</returns> 
    protected static string PostData(IDictionary<string, string> parameters)
    {
        StringBuilder postData = new StringBuilder();
        bool hasParam = false;
        IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
        while (dem.MoveNext())
        {
            string name = dem.Current.Key;
            string value = dem.Current.Value;
            // 忽略参数名或参数值为空的参数 
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
            {
                if (hasParam)
                {
                    postData.Append("&");
                }
                postData.Append(name);
                postData.Append("=");
                postData.Append(Uri.EscapeDataString(value));
                hasParam = true;
            }
        }
        return postData.ToString();
    }
    /// <summary> 
    /// TOP API POST 请求 
    /// </summary> 
    /// <param name="url">请求容器URL</param> 
    /// <param name="appkey">AppKey</param> 
    /// <param name="appSecret">AppSecret</param> 
    /// <param name="method">API接口方法名</param> 
    /// <param name="session">调用私有的sessionkey</param> 
    /// <param name="param">请求参数</param> 
    /// <returns>返回字符串</returns> 
    public static string Post(string url, string appkey, string appSecret, string method, string session,
    IDictionary<string, string> param)
    {
        #region -----API系统参数----
        param.Add("app_key", appkey);
        param.Add("method", method);
        param.Add("session", session);
        param.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        param.Add("format", "xml");
        param.Add("v", "2.0");
        param.Add("sign_method", "md5");
        param.Add("sign", CreateSign(param, appSecret));
        #endregion
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return Regex.Replace(result, @"[\x00-\x08\x0b-\x0c\x0e-\x1f]", "");
    }

    public static string SendPostData(string url, string data)
    {
        string result = string.Empty;
        #region ---- 完成 HTTP POST 请求----
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(data);
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
        Encoding encoding = Encoding.UTF8;
        Stream stream = null;
        StreamReader reader = null;
        stream = rsp.GetResponseStream();
        reader = new StreamReader(stream, encoding);
        result = reader.ReadToEnd();
        if (reader != null) reader.Close();
        if (stream != null) stream.Close();
        if (rsp != null) rsp.Close();
        #endregion
        return result;
    }
}