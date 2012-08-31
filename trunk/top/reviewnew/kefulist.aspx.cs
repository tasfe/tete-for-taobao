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

public partial class top_review_kefulist : System.Web.UI.Page
{
    public string session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
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

        //判断VIP版本，只有VIP才能使用此功能
        string sql = "SELECT * FROM TCS_ShopSession WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string flag = dt.Rows[0]["version"].ToString();

            if (flag == "0")
            {
                Response.Redirect("xufei.aspx");
                Response.End();
                return;
            }

            if (flag != "3")
            {
                string msg = "尊敬的" + nick + "，非常抱歉的告诉您，只有VIP版本才能使用【评价手动审核】功能，如需继续使用请<a href='http://fuwu.taobao.com/item/subsc.htm?items=service-0-22904-3:1;' target='_blank'>购买高级会员服务</a>，谢谢！";
                Response.Redirect("buy.aspx?msg=" + HttpUtility.UrlEncode(msg));
                Response.End();
                return;
            }
        }

        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        string t = utils.NewRequest("t", utils.RequestType.Form);
        string ids = utils.NewRequest("id", utils.RequestType.Form);

        if (act == "send")
        {
            Delete();
            return;
        }

        //批量操作评论
        if (t != "")
        {
            MultiConfirm(t, ids);
        }

        if (!IsPostBack)
        {
            BindData();
        }
    }

    /// <summary>
    /// 是否为黑名单
    /// </summary>
    /// <param name="trade"></param>
    /// <returns></returns>
    private bool IsBlack(string nick, string mobile)
    {
        string sql = "SELECT COUNT(*) FROM TCS_BlackList WHERE nick = '" + nick + "' AND mobile = '" + mobile + "'";
        string count = utils.ExecuteString(sql);
        if (count == "0")
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// 批量操作
    /// </summary>
    private void MultiConfirm(string t, string ids)
    {
        if (t == "ok")
        {
            string[] adsArray = ids.Split(',');
            for (int i = 0; i < adsArray.Length; i++)
            {
                ActOrder(adsArray[i]);
            }
            Response.Write("<script>alert('选中的订单【" + ids + "】已成功赠送！');window.location.href='kefulist.aspx';</script>");
        }
        else if (t == "no")
        {
            //不赠送礼品
            string sql = string.Empty;
            string[] adsArray = ids.Split(',');
            for (int i = 0; i < adsArray.Length; i++)
            {
                sql = "UPDATE TCS_TradeRateCheck SET issend = 2,ischeck = 1,checkdate = GETDATE() WHERE orderid = '" + adsArray[i] + "'";
                utils.ExecuteNonQuery(sql);
            }
            Response.Write("<script>alert('选中的订单【" + ids + "】已经设置为不赠送！');window.location.href='kefulist.aspx';</script>");
        }
        else if (t == "pass")
        {
            string sql = "SELECT * FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                //判断客户是否开启了内容自动判定
                if (dt.Rows[0]["iskeyword"].ToString() == "1")
                {
                    StartPassCheck(dt.Rows[0]["keywordisbad"].ToString(), dt.Rows[0]["keyword"].ToString(), dt.Rows[0]["badkeyword"].ToString(), dt.Rows[0]["wordcount"].ToString());
                    Response.Write("<script>alert('过滤成功，不符合判定条件的评价已经被设置为不赠送！');window.location.href='kefulist.aspx';</script>");
                }
                else
                {
                    Response.Write("<script>alert('请先在基本设置里面开启并设置“好评自动判定”！');window.location.href='kefulist.aspx';</script>");
                }
            }
        }
    }

    private void StartPassCheck(string isbad, string goodkey, string badkey, string len)
    {
        string sql = string.Empty;
        string content = string.Empty;

        sql = "SELECT * FROM TCS_TradeRateCheck WHERE nick = '" + nick + "' AND ischeck = 0";

        DataTable dt = utils.ExecuteDataTable(sql);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            content = dt.Rows[i]["content"].ToString();

            if (CheckContent(isbad, goodkey, badkey, len, content))
            {
                sql = "UPDATE TCS_TradeRateCheck SET issend = 2,ischeck = 1,checkdate = GETDATE() WHERE orderid = '" + dt.Rows[i]["orderid"].ToString() + "'";
                utils.ExecuteNonQuery(sql);
            }
        }
    }

    /// <summary>
    /// 是否不满足条件
    /// </summary>
    /// <param name="isbad"></param>
    /// <param name="keyword"></param>
    /// <param name="len"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    private bool CheckContent(string isbad, string goodkey, string badkey, string len, string content)
    {
        //长度判断
        if (content.Length < int.Parse(len))
        {
            return true;
        }

        //内容判断
        if (isbad == "1")
        {
            string[] keyArray = badkey.Split('|');
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (keyArray[i] != "")
                {
                    if (content.IndexOf(keyArray[i].Trim()) != -1)
                    {
                        return true;
                    }
                }
            }
        }
        else
        {
            string[] keyArray = goodkey.Split('|');
            for (int i = 0; i < keyArray.Length; i++)
            {
                if (keyArray[i] != "")
                {
                    if (content.IndexOf(keyArray[i].Trim()) != -1)
                    {
                        return false;
                    }
                }
            }
        }

        return false;
    }


    /// <summary>
    /// 处理该评价
    /// </summary>
    private void ActOrder(string id)
    {
        string sql = string.Empty;
        string couponid = string.Empty;
        string buynick = string.Empty;
        string iscoupon = string.Empty;
        string isfree = string.Empty;
        string promotionid = string.Empty;
        string tagid = string.Empty;
        string itemid = string.Empty;
        string phone = string.Empty;

        string giftflag = string.Empty;
        string giftcontent = string.Empty;
        string shopname = string.Empty;

        string isalipay = string.Empty;
        string alipayid = string.Empty;

        string isfreecard = string.Empty;
        string freecardid = string.Empty;

        //获取优惠券信息
        sql = "SELECT * FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            iscoupon = dt.Rows[0]["iscoupon"].ToString();
            couponid = dt.Rows[0]["couponid"].ToString();
            giftflag = dt.Rows[0]["giftflag"].ToString();
            giftcontent = dt.Rows[0]["giftcontent"].ToString();
            shopname = dt.Rows[0]["shopname"].ToString();

            isalipay = dt.Rows[0]["isalipay"].ToString();
            alipayid = dt.Rows[0]["alipayid"].ToString();

            isfreecard = dt.Rows[0]["isfree"].ToString();
            freecardid = dt.Rows[0]["freeid"].ToString();
        }
        else
        {
            Response.Write("<script>alert('系统错误，请重新登录！');window.location.href='kefulist.aspx';</script>");
            return;
        }

        //如果没有赠送优惠券或者优惠券为空则放弃
        if ((iscoupon == "0" || iscoupon.Trim() == "") && (isalipay == "0" || alipayid.Trim() == ""))
        {
            Response.Write("<script>alert('您没有设置赠送优惠券或者支付宝红包！');window.location.href='kefulist.aspx';</script>");
            return;
        }
        else
        {

            //获取该订单关联会员
            sql = "SELECT * FROM TCS_Trade WITH (NOLOCK) WHERE nick = '" + nick + "' AND orderid = '" + id + "'";
            dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                buynick = dt.Rows[0]["buynick"].ToString();
                phone = dt.Rows[0]["mobile"].ToString();
            }
            else
            {
                sql = "SELECT * FROM TCS_TradeRateCheck WITH (NOLOCK) WHERE nick = '" + nick + "' AND orderid = '" + id + "'";
                dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    buynick = dt.Rows[0]["buynick"].ToString();
                    //phone = dt.Rows[0]["mobile"].ToString();
                }
                else
                {
                    //Response.Write("<script>alert('【系统错误】：找不到该订单【" + id + "】关联的淘宝会员，请联系客服人员！');window.location.href='kefulist.aspx';</script>");
                    return;
                }
            }


            #region 赠送包邮卡
            if (isfreecard == "0" || freecardid.Trim() == "")
            {
            }
            else
            {
                //赠送包邮卡
                sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + freecardid + "'";
                dt = utils.ExecuteDataTable(sql);

                if (dt.Rows.Count != 0)
                {
                    string startdate = DateTime.Now.ToString();
                    string enddate = DateTime.Now.ToString();
                    string usecountlimit = dt.Rows[0]["usecount"].ToString();
                    string carddate = dt.Rows[0]["carddate"].ToString();

                    //赠送包邮卡
                    sql = "INSERT INTO TCS_FreeCard (nick,buynick,cardid,startdate,enddate,carddate,usecountlimit) VALUES ('" + nick + "', '" + buynick + "','" + freecardid + "','" + startdate + "','" + enddate + "','" + carddate + "','" + usecountlimit + "')";
                    utils.ExecuteNonQuery(sql);

                    //更新包邮卡赠送数量
                    sql = "UPDATE TCS_FreeCardAction SET sendcount = sendcount + 1 WHERE guid = '" + freecardid + "'";
                    utils.ExecuteNonQuery(sql);
                }
            }
            #endregion





            #region 赠送支付宝红包
            //先看还有没有短信了
            sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
            string totalAlipay = utils.ExecuteString(sql);
            if (int.Parse(totalAlipay) > 0)
            {
                //如果有短信再开始判断
                sql = "SELECT isalipay,alipayid FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
                DataTable dtAlipay = utils.ExecuteDataTable(sql);
                if (dtAlipay.Rows.Count != 0)
                {
                    //看看卖家是否开启了支付宝红包赠送
                    if (dtAlipay.Rows[0][0].ToString() == "1")
                    {
                        //判断红包是否有效
                        sql = "SELECT * FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND DATEDIFF(d, GETDATE(), enddate) > 0 AND used < count";
                        DataTable dtAlipayDetail = utils.ExecuteDataTable(sql);
                        if (dtAlipayDetail.Rows.Count != 0)
                        {
                            //判断用户获取的优惠券是否超过了每人的最大领取数量
                            sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND buynick = '" + buynick + "'";
                            string alipayCount = utils.ExecuteString(sql);
                            if (int.Parse(alipayCount) < int.Parse(dtAlipayDetail.Rows[0]["per"].ToString()))
                            {
                                //赠送支付宝红包
                                sql = "SELECT TOP 1 * FROM TCS_AlipayDetail WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND issend = 0";
                                DataTable dtAlipayDetailList = utils.ExecuteDataTable(sql);
                                if (dtAlipayDetailList.Rows.Count != 0)
                                {
                                    string msgAlipay = "亲，" + shopname + "赠送您支付宝红包，卡号" + dtAlipayDetailList.Rows[0]["card"].ToString() + "密码" + dtAlipayDetailList.Rows[0]["pass"].ToString() + "，您可以到支付宝绑定使用。";
                                    //强行截取
                                    if (msgAlipay.Length > 66)
                                    {
                                        msgAlipay = msgAlipay.Substring(0, 66);
                                    }



                                    //黑名单判断
                                    if (!IsBlack(nick, phone))
                                    {
                                        string result = SendMessage(phone, msgAlipay);
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
                                                            " '" + msgAlipay + "', " +
                                                            " '" + result + "', " +
                                                            " '1', " +
                                                            " 'alipay' " +
                                                        ") ";

                                        if (phone.Length != 0)
                                        {
                                            //记录支付宝红包发送成功
                                            utils.ExecuteNonQuery(sql);

                                            sql = "UPDATE TCS_Alipay SET used = used + 1 WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "'";
                                            utils.ExecuteNonQuery(sql);

                                            //更新优惠券已经赠送数量
                                            sql = "UPDATE TCS_AlipayDetail SET issend = 1,buynick = '" + buynick + "',senddate = GETDATE(), orderid = '" + id + "' WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND card = '" + dtAlipayDetailList.Rows[0]["card"].ToString() + "'";
                                            utils.ExecuteNonQuery(sql);


                                            //更新短信数量
                                            sql = "UPDATE TCS_ShopConfig SET used = used + 1,total = total-1 WHERE nick = '" + nick + "'";
                                            utils.ExecuteNonQuery(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion





            if (iscoupon == "0" || iscoupon.Trim() == "")
            {
            }
            else
            {
                //获取淘宝优惠券ID
                sql = "SELECT taobaocouponid FROM TCS_Coupon WHERE guid = '" + couponid + "'";
                string taobaocouponid = utils.ExecuteString(sql);

                //判断优惠券赠送限制
                sql = "SELECT per FROM TCS_Coupon WITH (NOLOCK) WHERE guid = '" + couponid + "' ";
                string max = utils.ExecuteString(sql);

                //判断该用户是否超过了最大赠送
                sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + buynick + "' AND guid = '" + couponid + "' AND taobaonumber NOT IN (SELECT couponnumber FROM TCS_Trade WHERE nick = '" + nick + "' AND iscoupon = 1)";
                DataTable dtCoupon = utils.ExecuteDataTable(sql);
                if (dtCoupon.Rows.Count >= int.Parse(max))
                {
                    //退出
                }
                else
                {
                    //执行优惠券赠送行为
                    string appkey = "12159997";
                    string secret = "614e40bfdb96e9063031d1a9e56fbed5";
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("coupon_id", taobaocouponid);
                    param.Add("buyer_nick", buynick);

                    string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
                    Regex reg = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(result);
                    //如果失败
                    if (!reg.IsMatch(result))
                    {
                        string err = new Regex(@"<msg>([^<]*)</msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                        string errnew = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                        string reason = new Regex(@"<reason>([^<]*)</reason>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                        Response.Write("<script>alert('【系统错误】：" + err + " - " + errnew + " - " + reason + "，请稍后再试或者联系客服人员！');window.location.href='kefulist.aspx';</script>");
                        Response.End();
                        return;
                    }
                    else
                    {
                        string number = match[0].Groups[1].ToString();

                        //赠送优惠券
                        sql = "INSERT INTO TCS_CouponSend (" +
                                            "nick, " +
                                            "guid, " +
                                            "buynick, " +
                                            "orderid, " +
                                            "taobaonumber " +
                                        " ) VALUES ( " +
                                            " '" + nick + "', " +
                                            " '" + couponid + "', " +
                                            " '" + buynick + "', " +
                                            " '" + id + "', " +
                                            " '" + number + "'" +
                                        ") ";
                        utils.ExecuteNonQuery(sql);

                        //更新优惠券已经赠送数量
                        sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = '" + couponid + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                }
            }
        }



        //发送短信
        if (1 == 1) //短信测试中
        {
            if (iscoupon == "1" || isfree == "1")
            {
                //判断是否开启该短信发送节点
                if (giftflag == "1")
                {
                    //判断是否还有短信可发
                    sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
                    string total = utils.ExecuteString(sql);

                    if (int.Parse(total) > 0)
                    {
                        //每张物流订单最多提示一次
                        sql = "SELECT COUNT(*) FROM TCS_MsgSend WITH (NOLOCK) WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND  buynick = '" + buynick + "' AND typ = 'gift'";
                        string giftCount = utils.ExecuteString(sql);

                        if (giftCount == "0")
                        {
                            //开始发送
                            string msg = GetMsg(giftcontent, shopname, buynick, iscoupon, isfree);

                            //强行截取
                            if (msg.Length > 66)
                            {
                                msg = msg.Substring(0, 66);
                            }

                            //黑名单判断
                            if (!IsBlack(nick, phone))
                            {
                                string result = SendMessage(phone, msg);

                                if (result != "0")
                                {
                                    string number = "1";

                                    //如果内容超过70个字则算2条
                                    if (msg.Length > 66)
                                    {
                                        number = "2";
                                    }

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
                                                        " '" + msg.Replace("'", "''") + "', " +
                                                        " '" + result + "', " +
                                                        " '" + number + "', " +
                                                        " 'gift' " +
                                                    ") ";

                                    if (phone.Length != 0)
                                    {
                                        utils.ExecuteNonQuery(sql);
                                    }

                                    ////更新状态
                                    //sql = "UPDATE TopOrder SET isgiftmsg = 1 WHERE orderid = '" + id + "'";
                                    //utils.ExecuteNonQuery(sql);

                                    //更新短信数量
                                    sql = "UPDATE TCS_ShopConfig SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                                    utils.ExecuteNonQuery(sql);
                                }
                                else
                                {
                                    ////记录短信发送记录
                                    //sql = "INSERT INTO TopMsgBak (" +
                                    //                    "nick, " +
                                    //                    "sendto, " +
                                    //                    "phone, " +
                                    //                    "[content], " +
                                    //                    "yiweiid, " +
                                    //                    "typ " +
                                    //                " ) VALUES ( " +
                                    //                    " '" + nick + "', " +
                                    //                    " '" + buynick + "', " +
                                    //                    " '" + phone + "', " +
                                    //                    " '" + msg + "', " +
                                    //                    " '" + result + "', " +
                                    //                    " 'gift' " +
                                    //                ") ";
                                    //utils.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }
            }
        }

        //更新订单状态-不需要审核
        sql = "UPDATE TCS_TradeRateCheck SET issend = 1,ischeck = 1,checkdate = GETDATE() WHERE orderid = '" + id + "'";
        utils.ExecuteNonQuery(sql);
    }



    /// <summary>
    /// 审核处理
    /// </summary>
    private void Delete()
    {
        string id = utils.NewRequest("id", utils.RequestType.QueryString);
        string send = utils.NewRequest("send", utils.RequestType.QueryString);
        string sql = string.Empty;
        string couponid = string.Empty;
        string buynick = string.Empty;
        string iscoupon = string.Empty;
        string isfree = string.Empty;
        string promotionid = string.Empty;
        string tagid = string.Empty;
        string itemid = string.Empty;
        string phone = string.Empty;

        string giftflag = string.Empty;
        string giftcontent = string.Empty;
        string shopname = string.Empty;

        string isalipay = string.Empty;
        string alipayid = string.Empty;
        
        string isfreecard = string.Empty;
        string freecardid = string.Empty;

        if (send == "1")
        {
            //获取优惠券信息
            sql = "SELECT * FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                iscoupon = dt.Rows[0]["iscoupon"].ToString();
                couponid = dt.Rows[0]["couponid"].ToString();
                giftflag = dt.Rows[0]["giftflag"].ToString();
                giftcontent = dt.Rows[0]["giftcontent"].ToString();
                shopname = dt.Rows[0]["shopname"].ToString();

                isalipay = dt.Rows[0]["isalipay"].ToString();
                alipayid = dt.Rows[0]["alipayid"].ToString();

                isfreecard = dt.Rows[0]["isfree"].ToString();
                freecardid = dt.Rows[0]["freeid"].ToString();
            }
            else
            {
                Response.Write("<script>alert('系统错误，请重新登录！');window.location.href='kefulist.aspx';</script>");
                return;
            }

            //如果没有赠送优惠券或者优惠券为空则放弃
            if ((iscoupon == "0" || iscoupon.Trim() == "") && (isalipay == "0" || alipayid.Trim() == ""))
            {
                Response.Write("<script>alert('您没有设置赠送优惠券或者礼品！');window.location.href='kefulist.aspx';</script>");
                return;
            }
            else
            {



                //获取该订单关联会员
                sql = "SELECT * FROM TCS_Trade WITH (NOLOCK) WHERE nick = '" + nick + "' AND orderid = '" + id + "'";
                dt = utils.ExecuteDataTable(sql);
                if (dt.Rows.Count != 0)
                {
                    buynick = dt.Rows[0]["buynick"].ToString();
                    phone = dt.Rows[0]["mobile"].ToString();
                }
                else
                {
                    sql = "SELECT * FROM TCS_TradeRateCheck WITH (NOLOCK) WHERE nick = '" + nick + "' AND orderid = '" + id + "'";
                    dt = utils.ExecuteDataTable(sql);
                    if (dt.Rows.Count != 0)
                    {
                        buynick = dt.Rows[0]["buynick"].ToString();
                        //phone = dt.Rows[0]["mobile"].ToString();
                    }
                    else
                    {
                        Response.Write("<script>alert('【系统错误】：找不到该订单【" + id + "】关联的淘宝会员，请联系客服人员！');window.location.href='kefulist.aspx';</script>");
                        return;
                    }
                }



                #region 赠送包邮卡
                if (isfreecard == "0" || freecardid.Trim() == "")
                {
                }
                else
                {
                    //赠送包邮卡
                    sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + freecardid + "'";
                    dt = utils.ExecuteDataTable(sql);

                    if (dt.Rows.Count != 0)
                    {
                        string startdate = DateTime.Now.ToString();
                        string enddate = DateTime.Now.ToString();
                        string usecountlimit = dt.Rows[0]["usecount"].ToString();
                        string carddate = dt.Rows[0]["carddate"].ToString();

                        //赠送包邮卡
                        sql = "INSERT INTO TCS_FreeCard (nick,buynick,cardid,startdate,enddate,carddate,usecountlimit) VALUES ('" + nick + "', '" + buynick + "','" + freecardid + "','" + startdate + "','" + enddate + "','" + carddate + "','" + usecountlimit + "')";
                        utils.ExecuteNonQuery(sql);

                        //更新包邮卡赠送数量
                        sql = "UPDATE TCS_FreeCardAction SET sendcount = sendcount + 1 WHERE guid = '" + freecardid + "'";
                        utils.ExecuteNonQuery(sql);
                    }
                }
                #endregion

                #region 赠送支付宝红包
                //先看还有没有短信了
                sql = "SELECT total FROM TCS_ShopConfig WHERE nick = '" + nick + "'";
                string totalAlipay = utils.ExecuteString(sql);
                if (int.Parse(totalAlipay) > 0)
                {
                    //如果有短信再开始判断
                    sql = "SELECT isalipay,alipayid FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
                    DataTable dtAlipay = utils.ExecuteDataTable(sql);
                    if (dtAlipay.Rows.Count != 0)
                    {
                        //看看卖家是否开启了支付宝红包赠送
                        if (dtAlipay.Rows[0][0].ToString() == "1")
                        {
                            //判断红包是否有效
                            sql = "SELECT * FROM TCS_Alipay WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND DATEDIFF(d, GETDATE(), enddate) > 0 AND used < count";
                            DataTable dtAlipayDetail = utils.ExecuteDataTable(sql);
                            if (dtAlipayDetail.Rows.Count != 0)
                            {
                                //判断用户获取的优惠券是否超过了每人的最大领取数量
                                sql = "SELECT COUNT(*) FROM TCS_AlipayDetail WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND buynick = '" + buynick + "'";
                                string alipayCount = utils.ExecuteString(sql);
                                if (int.Parse(alipayCount) < int.Parse(dtAlipayDetail.Rows[0]["per"].ToString()))
                                {
                                    //赠送支付宝红包
                                    sql = "SELECT TOP 1 * FROM TCS_AlipayDetail WITH (NOLOCK) WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND issend = 0";
                                    DataTable dtAlipayDetailList = utils.ExecuteDataTable(sql);
                                    if (dtAlipayDetailList.Rows.Count != 0)
                                    {
                                        string msgAlipay = "亲，" + shopname + "赠送您支付宝红包，卡号" + dtAlipayDetailList.Rows[0]["card"].ToString() + "密码" + dtAlipayDetailList.Rows[0]["pass"].ToString() + "，您可以到支付宝绑定使用。";
                                        //强行截取
                                        if (msgAlipay.Length > 66)
                                        {
                                            msgAlipay = msgAlipay.Substring(0, 66);
                                        }

                                        string result = SendMessage(phone, msgAlipay);
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
                                                            " '" + msgAlipay + "', " +
                                                            " '" + result + "', " +
                                                            " '1', " +
                                                            " 'alipay' " +
                                                        ") ";

                                        if (phone.Length != 0)
                                        {
                                            //记录支付宝红包发送成功
                                            utils.ExecuteNonQuery(sql);

                                            sql = "UPDATE TCS_Alipay SET used = used + 1 WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "'";
                                            utils.ExecuteNonQuery(sql);

                                            //更新优惠券已经赠送数量
                                            sql = "UPDATE TCS_AlipayDetail SET issend = 1,buynick = '" + buynick + "',senddate = GETDATE(), orderid = '" + id + "' WHERE guid = '" + dtAlipay.Rows[0][1].ToString() + "' AND card = '" + dtAlipayDetailList.Rows[0]["card"].ToString() + "'";
                                            utils.ExecuteNonQuery(sql);


                                            //更新短信数量
                                            sql = "UPDATE TCS_ShopConfig SET used = used + 1,total = total-1 WHERE nick = '" + nick + "'";
                                            utils.ExecuteNonQuery(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                if (iscoupon == "0" || iscoupon.Trim() == "")
                {
                }
                else
                {
                    //获取淘宝优惠券ID
                    sql = "SELECT taobaocouponid FROM TCS_Coupon WHERE guid = '" + couponid + "'";
                    string taobaocouponid = utils.ExecuteString(sql);

                    //判断优惠券赠送限制
                    sql = "SELECT per FROM TCS_Coupon WITH (NOLOCK) WHERE guid = '" + couponid + "' ";
                    string max = utils.ExecuteString(sql);

                    //判断该用户是否超过了最大赠送
                    sql = "SELECT guid FROM TCS_CouponSend WITH (NOLOCK) WHERE buynick= '" + buynick + "' AND guid = '" + couponid + "' AND taobaonumber NOT IN (SELECT couponnumber FROM TCS_Trade WHERE nick = '" + nick + "' AND iscoupon = 1)";
                    DataTable dtCoupon = utils.ExecuteDataTable(sql);
                    if (dtCoupon.Rows.Count >= int.Parse(max))
                    {
                        //退出
                    }
                    else
                    {

                        //执行优惠券赠送行为
                        string appkey = "12159997";
                        string secret = "614e40bfdb96e9063031d1a9e56fbed5";
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("coupon_id", taobaocouponid);
                        param.Add("buyer_nick", buynick);

                        string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
                        Regex reg = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(result);
                        //如果失败
                        if (!reg.IsMatch(result))
                        {
                            string err = new Regex(@"<msg>([^<]*)</msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                            string errnew = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                            string reason = new Regex(@"<reason>([^<]*)</reason>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                            Response.Write("<script>alert('【系统错误】：" + err + " - " + errnew + " - " + reason + "，请稍后再试或者联系客服人员！');window.location.href='kefulist.aspx';</script>");
                            Response.End();
                            return;
                        }
                        else
                        {
                            string number = match[0].Groups[1].ToString();

                            //赠送优惠券
                            sql = "INSERT INTO TCS_CouponSend (" +
                                                "nick, " +
                                                "guid, " +
                                                "buynick, " +
                                                "orderid, " +
                                                "taobaonumber " +
                                            " ) VALUES ( " +
                                                " '" + nick + "', " +
                                                " '" + couponid + "', " +
                                                " '" + buynick + "', " +
                                                " '" + id + "', " +
                                                " '" + number + "'" +
                                            ") ";
                            utils.ExecuteNonQuery(sql);

                            //更新优惠券已经赠送数量
                            sql = "UPDATE TCS_Coupon SET used = used + 1 WHERE guid = '" + couponid + "'";
                            utils.ExecuteNonQuery(sql);
                        }
                    }
                }
            }

            //发送短信
            if (1 == 1) //短信测试中
            {
                //Response.Write(iscoupon + "<br>");
                //Response.Write(isfree + "<br>");
                if (iscoupon == "1" || isfree == "1")
                {
                    Response.Write(giftflag + "<br>");

                    //判断是否开启该短信发送节点
                    if (giftflag == "1")
                    {
                        //判断是否还有短信可发
                        sql = "SELECT total FROM TCS_ShopConfig WITH (NOLOCK) WHERE nick = '" + nick + "'";
                        string total = utils.ExecuteString(sql);

                        if (int.Parse(total) > 0)
                        {
                            sql = "SELECT COUNT(*) FROM TCS_MsgSend WHERE buynick = '" + buynick + "' AND nick = '" + nick + "' AND typ = 'gift' AND DATEDIFF(d, adddate, GETDATE()) = 0";
                            string count = utils.ExecuteString(sql);
                            if (count == "0")
                            {
                                //开始发送
                                string msg = GetMsg(giftcontent, shopname, buynick, iscoupon, isfree);

                                //强行截取
                                if (msg.Length > 66)
                                {
                                    msg = msg.Substring(0, 66);
                                }

                                //Response.Write(msg + "<br>");
                                string result = SendMessage(phone, msg);

                                //Response.Write(result + "<br>");
                                if (result != "0")
                                {
                                    string number = "1";

                                    //如果内容超过70个字则算2条
                                    if (msg.Length > 70)
                                    {
                                        number = "2";
                                    }

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
                                                        " '" + msg.Replace("'", "''") + "', " +
                                                        " '" + result + "', " +
                                                        " '" + number + "', " +
                                                        " 'gift' " +
                                                    ") ";
                                    //Response.Write(sql + "<br>");
                                    if (phone.Length != 0)
                                    {
                                        utils.ExecuteNonQuery(sql);
                                    }
                                    ////更新状态
                                    //sql = "UPDATE TopOrder SET isgiftmsg = 1 WHERE orderid = '" + id + "'";
                                    //utils.ExecuteNonQuery(sql);

                                    //更新短信数量
                                    sql = "UPDATE TCS_ShopConfig SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                                    utils.ExecuteNonQuery(sql);
                                }
                                else
                                {
                                    ////记录短信发送记录
                                    //sql = "INSERT INTO TopMsgBak (" +
                                    //                    "nick, " +
                                    //                    "sendto, " +
                                    //                    "phone, " +
                                    //                    "[content], " +
                                    //                    "yiweiid, " +
                                    //                    "typ " +
                                    //                " ) VALUES ( " +
                                    //                    " '" + nick + "', " +
                                    //                    " '" + buynick + "', " +
                                    //                    " '" + phone + "', " +
                                    //                    " '" + msg + "', " +
                                    //                    " '" + result + "', " +
                                    //                    " 'gift' " +
                                    //                ") ";
                                    //utils.ExecuteNonQuery(sql);
                                }
                            }
                        }
                    }
                }
            }

            //更新订单状态-不需要审核
            sql = "UPDATE TCS_TradeRateCheck SET issend = 1,ischeck = 1,checkdate = GETDATE() WHERE orderid = '" + id + "'";
            utils.ExecuteNonQuery(sql);

            Response.Write("<script>alert('该订单已成功赠送！');window.location.href='kefulist.aspx';</script>");
        }
        else if (send == "2")
        {
            //不赠送礼品
            sql = "UPDATE TCS_TradeRateCheck SET issend = 2,ischeck = 1,checkdate = GETDATE() WHERE orderid = '" + id + "'";
            utils.ExecuteNonQuery(sql);
            Response.Write("<script>alert('设置成功，该订单不赠送！');window.location.href='kefulist.aspx';</script>");
        }
    }


    private string GetMsg(string giftcontent, string shopname, string buynick, string iscoupon, string isfree)
    {
        string giftstr = "";
        if (iscoupon == "1")
        {
            giftstr = "优惠券";
        }

        if (iscoupon == "1" && isfree == "1")
        {
            giftstr += "+";
        }

        if (isfree == "1")
        {
            giftstr += "精美礼品";
        }

        giftcontent = giftcontent.Replace("[shopname]", shopname);
        giftcontent = giftcontent.Replace("[buynick]", buynick);
        giftcontent = giftcontent.Replace("[gift]", giftstr);//.Replace("[buynick]", buynick);

        return giftcontent;
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

    //public static string SendMessage(string phone, string msg)
    //{
    //    //有客户没有手机号也发送短信
    //    if (phone.Length == 0)
    //    {
    //        return "0";
    //    }

    //    string uid = "terrylv";
    //    string pass = "123456";

    //    msg = HttpUtility.UrlEncode(msg);

    //    string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
    //    byte[] bs = Encoding.ASCII.GetBytes(param);
    //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
    //    req.Method = "POST";
    //    req.ContentType = "application/x-www-form-urlencoded";
    //    req.ContentLength = bs.Length;

    //    using (Stream reqStream = req.GetRequestStream())
    //    {
    //        reqStream.Write(bs, 0, bs.Length);
    //    }

    //    using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
    //    {
    //        using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
    //        {
    //            string content = reader.ReadToEnd();

    //            if (content.IndexOf("<error>0</error>") == -1)
    //            {
    //                //发送失败
    //                return content;
    //            }
    //            else
    //            {
    //                //发送成功
    //                Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
    //                MatchCollection match = reg.Matches(content);
    //                string number = match[0].Groups[1].ToString();
    //                return number;
    //            }
    //        }
    //    }
    //}

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

        HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms.pica.com/zqhdServer/sendSMS.jsp" + "?" + param);

        req.Method = "GET";
        //req.ContentType = "application/x-www-form-urlencoded";
        //req.ContentLength = bs.Length;

        //using (Stream reqStream = req.GetRequestStream())
        //{
        //    reqStream.Write(bs, 0, bs.Length);
        //}

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

    ///// <summary>
    ///// 通过借口发送短信
    ///// </summary>
    ///// <param name="phone"></param>
    ///// <param name="content"></param>
    //private string SendMessage(string phone, string msg)
    //{
    //    string uid = "terrylv";
    //    string pass = "123456";

    //    msg = HttpUtility.UrlEncode(msg);

    //    string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
    //    byte[] bs = Encoding.ASCII.GetBytes(param);
    //    HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms3.eachwe.com/api.php");
    //    req.Method = "POST";
    //    req.ContentType = "application/x-www-form-urlencoded";
    //    req.ContentLength = bs.Length;

    //    using (Stream reqStream = req.GetRequestStream())
    //    {
    //        reqStream.Write(bs, 0, bs.Length);
    //    }

    //    using (HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse())
    //    {
    //        using (StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.GetEncoding("GB2312")))
    //        {
    //            string content = reader.ReadToEnd();

    //            if (content.IndexOf("<error>0</error>") == -1)
    //            {
    //                //发送失败
    //                //Response.Write(content);
    //                //Response.End();
    //                return "0";
    //            }
    //            else
    //            {
    //                //发送成功
    //                Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
    //                MatchCollection match = reg.Matches(content);
    //                string number = match[0].Groups[1].ToString();
    //                return number;
    //            }
    //        }
    //    }
    //}

    public static string left(string str)
    {
        string newstr = string.Empty;
        if (str.Length < 25)
        {
            newstr = str;
        }
        else
        {
            newstr = "<span title='" + str + "'>" + str.Substring(0, 25) + "..</span>";
        }
        return newstr;
    }

    public static string getimg(string str)
    {
        return "images/" + str + ".jpg";
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (search.Text.Trim() == "")
        {
            Response.Redirect("kefulist.aspx");
            return;
        }

        string sqlNew = "SELECT b.*,c.giftcount,c.couponcount,t.totalprice FROM TCS_TradeRateCheck b INNER JOIN TCS_Customer c ON c.buynick = b.buynick AND c.nick = '" + nick + "' LEFT JOIN TCS_Trade t ON t.orderid = b.orderid WHERE b.buynick = '" + search.Text.Trim().Replace("'", "''") + "' AND b.ischeck = 0 ORDER BY reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        lbPage.Text = "";
    }

    private void BindData()
    {
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
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
        
//        //SELECT TOP 20 * FROM 
//(SELECT b.*,c.*,ROW_NUMBER() OVER (ORDER BY b.reviewdate DESC) d
//AS rownumber FROM TCS_TradeRateCheck b LEFT JOIN TCS_Customer c ON c.buynick = b.buynick 
//WHERE b.nick = '魔女茶花' AND b.ischeck = 0 ) AS a 
//WHERE a.rownumber > 0 ORDER BY reviewdate DESC

        string sqlNew = "SELECT TOP " + pageCount.ToString() + " * FROM (SELECT b.*,c.giftcount,c.couponcount,t.totalprice,ROW_NUMBER() OVER (ORDER BY b.reviewdate DESC) AS rownumber FROM TCS_TradeRateCheck b LEFT JOIN TCS_Customer c ON c.buynick = b.buynick AND c.nick = '" + nick + "' LEFT JOIN TCS_Trade t ON t.orderid = b.orderid WHERE b.nick = '" + nick + "' AND b.ischeck = 0 ) AS a WHERE a.rownumber > " + dataCount.ToString() + " ORDER BY reviewdate DESC";
        DataTable dt = utils.ExecuteDataTable(sqlNew);

        rptArticle.DataSource = dt;
        rptArticle.DataBind();

        //分页数据初始化
        sqlNew = "SELECT COUNT(*) FROM TCS_TradeRateCheck WHERE nick = '" + nick + "' AND ischeck = 0";
        int totalCount = int.Parse(utils.ExecuteString(sqlNew));

        lbPage.Text = InitPageStr(totalCount, "kefulist.aspx");
    }

    private string InitPageStr(int total, string url)
    {
        //分页数据初始化
        string str = string.Empty;
        int pageCount = 20;
        int pageSize = 0;
        int pageNow = 1;
        string page = utils.NewRequest("page", utils.RequestType.QueryString);
        if (page == "")
        {
            pageNow = 1;
        }
        else
        {
            pageNow = int.Parse(page);
        }

        //取总分页数
        if (total % pageCount == 0)
        {
            pageSize = total / pageCount;
        }
        else
        {
            pageSize = total / pageCount + 1;
        }

        //如果总页面大于20，则最大页面差不超过20
        int start = 1;
        int end = 20;

        if (pageSize < end)
        {
            end = pageSize;
        }
        else
        {
            if (pageNow > 15)
            {
                start = pageNow - 10;

                if (pageNow < (total - 10))
                {
                    end = pageNow + 10;
                }
                else
                {
                    end = total;
                }
            }
        }

        for (int i = start; i <= end; i++)
        {
            if (i.ToString() == pageNow.ToString())
            {
                str += i.ToString() + " ";
            }
            else
            {
                str += "<a href='" + url + "?page=" + i.ToString() + "'>[" + i.ToString() + "]</a> ";
            }
        }

        return str;
    }







    #region TOP API
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
        req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
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
    #endregion
}