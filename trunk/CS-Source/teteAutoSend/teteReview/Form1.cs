using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;
using System.Web;
using System.Diagnostics;

namespace teteReview
{
    public partial class Form1 : Form
    {
        public string appkey = string.Empty;
        public string secret = string.Empty;

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            appkey = "12159997";
            secret = "614e40bfdb96e9063031d1a9e56fbed5";

            //获取最新团购相关的订单数据并加入数据库
            Thread newThread = new Thread(DoMyJob);
            newThread.Start();

            //获取订单物流状态并发送短信
            Thread newThread1 = new Thread(CheckOrder);
            newThread1.Start();

            //获取订单完成状态兵判断是否赠送礼品
            Thread newThread2 = new Thread(SendGift);
            newThread2.Start();

            //获取长期未确认订单并给予短信提示
            Thread newThread3 = new Thread(SendOrderMsg);
            newThread3.Start();

            //获取卖家动态评分
            Thread newThread4 = new Thread(GetShopScore);
            newThread4.Start();
        }

        /// <summary>
        /// 获取增量订单
        /// </summary>
        private void DoMyJob()
        {
            //获取超过卖家设置时间还未支付的订单并取消
            string session = string.Empty;

            DBSql db = DBSql.getInstance();
            string sql = "SELECT * FROM TopAutoReview WHERE isdel = 0";
            //textBox1.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //通过接口将该用户加入人群
                    sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                    //textBox1.AppendText("\r\n" + sql);
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0]["sessionblog"].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    //获取相关参数
                    string nick = dt.Rows[i]["nick"].ToString();
                    string fahuoflag = dt.Rows[i]["fahuoflag"].ToString();
                    string fahuocontent = dt.Rows[i]["fahuocontent"].ToString();
                    string shopname = dt.Rows[i]["shopname"].ToString();
                    string iscoupon = dt.Rows[i]["iscoupon"].ToString();
                    string isfree = dt.Rows[i]["isfree"].ToString();

                    DateTime startdate = new DateTime();
                    DateTime enddate = new DateTime();

                    //获取该团购上次更新时间
                    if (dt.Rows[i]["updatetime"] == DBNull.Value)
                    {
                        //如果是第一次检测i
                        startdate = DateTime.Parse(dt.Rows[i]["starttime"].ToString());
                    }
                    else
                    {
                        //如果不是第一次检测
                        startdate = DateTime.Parse(dt.Rows[i]["updatetime"].ToString());
                    }
                    enddate = startdate.AddHours(23);

                    //判断结束时间是否大于现在
                    if (enddate > DateTime.Now.AddMinutes(-2))
                    {
                        enddate = DateTime.Now.AddMinutes(-2);
                    }

                    //更新数据中记录的判断时间
                    sql = "UPDATE TopAutoReview SET updatetime = '" + enddate.ToString() + "' WHERE id = " + dt.Rows[i]["id"].ToString();
                    db.ExecSql(sql);
                    //textBox1.AppendText("\r\n" + sql);

                    //通过接口获取用户下单信息(此处使用查询效率最高的30分钟间距作为查询条件)
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("fields", "buyer_nick,tid,status,created,receiver_mobile,orders.oid,orders.num_iid");
                    param.Add("start_modified", startdate.ToString("yyyy-MM-dd HH:mm:ss"));
                    param.Add("end_modified", enddate.ToString("yyyy-MM-dd HH:mm:ss"));
                    string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trades.sold.increment.get", session, param);

                    Regex reg = new Regex(@"<buyer_nick>([^<]*)</buyer_nick><created>([^<]*)</created><orders list=""true"">([\s\S]*?)</orders><receiver_mobile>([^<]*)</receiver_mobile><status>([^<]*)</status><tid>([^<]*)</tid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(result);
                    //textBox1.AppendText("\r\n" + result);


                    for (int j = 0; j < match.Count; j++)
                    {
                        string tid = match[j].Groups[6].ToString();
                        string orderstatus = match[j].Groups[5].ToString();
                        string created = match[j].Groups[2].ToString();
                        string receiver_mobile = match[j].Groups[4].ToString();
                        string buynick = match[j].Groups[1].ToString();

                        //判断该订单是否存在
                        sql = "SELECT COUNT(*) FROM TopOrder WHERE orderid = '"+tid+"'";
                        string orderCount = db.GetTable(sql).Rows[0][0].ToString();
                        if (orderCount == "0")
                        {
                            //记录用户产生的订单
                            sql = "INSERT INTO TopOrder (" +
                                        "nick, " +
                                        "orderid, " +
                                        "orderstatus, " +
                                        "addtime, " +
                                        "buynick, " +
                                        "receiver_mobile " +
                                    " ) VALUES ( " +
                                        " '" + nick + "', " +
                                        " '" + tid + "', " +
                                        " '" + orderstatus + "', " +
                                        " '" + created + "', " +
                                        " '" + buynick + "', " +
                                        " '" + receiver_mobile + "' " +
                                    ") ";
                            //textBox1.AppendText("\r\n" + sql);
                            db.ExecSql(sql);

                            //记录订单关联的交易子订单
                            Regex regChild = new Regex(@"<order><num_iid>([^<]*)</num_iid><oid>([^<]*)</oid></order>", RegexOptions.IgnoreCase);
                            //textBox1.AppendText("\r\n" + match[j].Groups[3].ToString());
                            MatchCollection matchChild = regChild.Matches(match[j].Groups[3].ToString());
                            for (int k = 0; k < matchChild.Count; k++)
                            {
                                string oid = matchChild[k].Groups[2].ToString();
                                string num_iid = matchChild[k].Groups[1].ToString();

                                sql = "INSERT INTO TopOrderList (" +
                                        "nick, " +
                                        "tid, " +
                                        "oid, " +
                                        "itemid " +
                                    " ) VALUES ( " +
                                        " '" + nick + "', " +
                                        " '" + tid + "', " +
                                        " '" + oid + "', " +
                                        " '" + num_iid + "'" +
                                    ") ";
                                //textBox1.AppendText("\r\n" + sql);
                                db.ExecSql(sql);
                            }
                        }
                        else
                        { 
                            //如果还没有获取到物流状态则不能更新为完成
                            if (orderstatus == "TRADE_FINISHED")
                            {
                                //更新订单状态
                                sql = "UPDATE TopOrder SET orderstatus='" + orderstatus + "',receiver_mobile='" + receiver_mobile + "' WHERE orderid = '" + tid + "' AND typ IS NOT NULL";
                            }
                            else
                            {
                                sql = "UPDATE TopOrder SET orderstatus='" + orderstatus + "',receiver_mobile='" + receiver_mobile + "' WHERE orderid = '" + tid + "'";
                            }
                            db.ExecSql(sql);
                        }


                        //判断订单是否为发货状态
                        if (orderstatus == "WAIT_BUYER_CONFIRM_GOODS")
                        { 
                            //判断是否开启发货通知短信
                            if (fahuoflag == "1")
                            {
                                //如果今天发过提醒短信就不发了
                                sql = "SELECT COUNT(*) FROM TopMsg WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND sendto = '" + buynick + "' AND typ = 'fahuo'";
                                string msgcount = db.GetTable(sql).Rows[0][0].ToString();
                                if (msgcount != "0")
                                {
                                    //更新状态
                                    sql = "UPDATE TopOrder SET isfahuomsg = 1 WHERE orderid = '" + tid + "'";
                                    db.ExecSql(sql);
                                    continue;
                                }

                                //判断是否还有短信可发
                                sql = "SELECT total FROM TopAutoReview WHERE nick = '" + nick + "'";
                                string total = db.GetTable(sql).Rows[0][0].ToString();
                                //textBox2.AppendText("\r\n" + total);
                                if (int.Parse(total) > 0)
                                {
                                    string phone = receiver_mobile;
                                    //每张物流订单最多提示一次
                                    sql = "SELECT COUNT(*) FROM TopMsg WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND sendto = '" + buynick + "' AND typ = 'fahuo'";
                                    string fahuoCount = db.GetTable(sql).Rows[0][0].ToString();
                                    //textBox2.AppendText("\r\nshippingCount-" + fahuocontent);
                                    if (fahuoCount == "0")
                                    {
                                        string msg = GetMsg(fahuocontent, shopname, buynick, iscoupon, isfree);

                                        string resultmsg = SendMessage(phone, msg);
                                        textBox1.AppendText("\r\n" + nick);
                                        textBox1.AppendText("\r\n" + msg);
                                        textBox1.AppendText("\r\n" + resultmsg);
                                        textBox1.AppendText("\r\n\r\n");


                                        //如果内容超过70个字则算2条
                                        string number = "1";
                                        if (msg.Length > 70)
                                        {
                                            number = "2";
                                        }

                                        if (resultmsg != "0")
                                        {
                                            //记录短信发送记录
                                            sql = "INSERT INTO TopMsg (" +
                                                                "nick, " +
                                                                "sendto, " +
                                                                "phone, " +
                                                                "[content], " +
                                                                "yiweiid, " +
                                                                "orderid, " +
                                                                "num, " +
                                                                "typ " +
                                                            " ) VALUES ( " +
                                                                " '" + nick + "', " +
                                                                " '" + buynick + "', " +
                                                                " '" + phone + "', " +
                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                " '" + resultmsg + "', " +
                                                                " '" + tid + "', " +
                                                                " '" + number + "', " +
                                                                " 'fahuo' " +
                                                            ") ";
                                            db.ExecSql(sql);

                                            //更新状态
                                            sql = "UPDATE TopOrder SET isfahuomsg = 1 WHERE orderid = '" + tid + "'";
                                            db.ExecSql(sql);

                                            //更新短信数量
                                            sql = "UPDATE TopAutoReview SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                                            db.ExecSql(sql);
                                        }
                                        else
                                        {
                                            //记录短信发送记录-另外的服务重发
                                            sql = "INSERT INTO TopMsgBak (" +
                                                                "nick, " +
                                                                "sendto, " +
                                                                "phone, " +
                                                                "[content], " +
                                                                "yiweiid, " +
                                                                "orderid, " +
                                                                "typ " +
                                                            " ) VALUES ( " +
                                                                " '" + nick + "', " +
                                                                " '" + buynick + "', " +
                                                                " '" + phone + "', " +
                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                " '" + resultmsg + "', " +
                                                                " '" + tid + "', " +
                                                                " 'fahuo' " +
                                                            ") ";
                                            db.ExecSql(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    textBox1.AppendText("\r\n\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n************************************************");
                    continue;
                }
            }

            Thread.Sleep(100000);

            Thread newThread = new Thread(DoMyJob);
            newThread.Start();
        }

        private void CheckOrder()
        {
            //获取超过卖家设置时间还未支付的订单并取消
            string session = string.Empty;
            string debugOrder = string.Empty;
            string debugResult = string.Empty;
            string debugSql = string.Empty;

            DBSql db = DBSql.getInstance();
            string sql = "SELECT * FROM TopAutoReview WHERE isdel = 0";
            //textBox2.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //获取基本设置参数
                    string mindate = dt.Rows[i]["mindate"].ToString();
                    string maxdate = dt.Rows[i]["mindate"].ToString();

                    string iscoupon = dt.Rows[i]["iscoupon"].ToString();
                    string isfree = dt.Rows[i]["isfree"].ToString();

                    string shippingflag = dt.Rows[i]["shippingflag"].ToString();
                    string shippingcontent = dt.Rows[i]["shippingcontent"].ToString();
                    string shopname = dt.Rows[i]["shopname"].ToString();

                    //通过接口将该用户加入人群
                    sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                    //textBox2.AppendText("\r\n" + sql);
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = dtnick.Rows[0]["sessionblog"].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    //获取相关参数
                    string nick = dt.Rows[i]["nick"].ToString();

                    //获取改用户设置的短信提醒的订单
                    sql = "SELECT * FROM TopOrder WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "' AND isok = 0 AND orderstatus <> 'WAIT_SELLER_SEND_GOODS' AND orderstatus <> 'WAIT_BUYER_PAY'";
                    DataTable dtOrder = db.GetTable(sql);
                    textBox2.AppendText("\r\n\r\n" + nick + "-" + dtOrder.Rows.Count);

                    //循环订单获取物流状态并判断是否发送短信
                    for (int j = 0; j < dtOrder.Rows.Count; j++)
                    {
                        //获取该用户的物流状态
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("fields", "delivery_start,delivery_end,status");
                        param.Add("tid", dtOrder.Rows[j]["orderid"].ToString());
                        textBox2.AppendText("\r\n" + j.ToString() + "、" + dtOrder.Rows[j]["orderid"].ToString());
                        //textBox2.AppendText("\r\nreview-----------" + dtOrder.Rows[j]["orderstatus"].ToString());

                        //物流接口暂时停用，因为会影响错误率
                        string result = string.Empty;
                        string isself = dtOrder.Rows[j]["typ"].ToString();
                        string buynick = dtOrder.Rows[j]["buynick"].ToString();
                        string isdeliverymsg = dtOrder.Rows[j]["isdeliverymsg"].ToString();
                        debugOrder = dtOrder.Rows[j]["orderid"].ToString();

                        if (isself != "self")
                        {
                            result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.logistics.orders.get", session, param);
                        }
                        Regex reg = new Regex(@"<status>([^<]*)</status>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(result);
                        //textBox2.AppendText("\r\n" + result);

                        //如果订单状态为已取消
                        if (dtOrder.Rows[j]["orderstatus"].ToString() == "TRADE_CLOSED_BY_TAOBAO" || dtOrder.Rows[j]["orderstatus"].ToString() == "TRADE_CLOSED")
                        {
                            textBox2.AppendText("\r\n" + dtOrder.Rows[j]["orderid"].ToString() + "-订单为取消状态");
                            sql = "UPDATE TopOrder SET isok = 1 WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                            db.ExecSql(sql);
                            continue;
                        }


                        //如果订单状态为已结束则获取订单评论
                        if (dtOrder.Rows[j]["orderstatus"].ToString() == "TRADE_FINISHED")
                        {
                            string orderid = dtOrder.Rows[j]["orderid"].ToString();
                            //获取该订单的子订单
                            sql = "SELECT * FROM TopOrderList WHERE tid = '" + orderid + "'";
                            //textBox2.AppendText("\r\nTRADE_FINISHED----" + sql);
                            DataTable dtOrderList = db.GetTable(sql);
                            for (int l = 0; l < dtOrderList.Rows.Count; l++)
                            {
                                //获取该订单的好评信息 taobao.traderates.get
                                param = new Dictionary<string, string>();
                                param.Add("fields", "content,created,nick,result");
                                param.Add("rate_type", "get");
                                param.Add("role", "buyer");
                                param.Add("tid", dtOrderList.Rows[l]["oid"].ToString());

                                //textBox2.AppendText("\r\n[" + dtOrderList.Rows[l]["oid"].ToString() + "]");
                                string resultNew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.traderates.get", session, param);
                                //textBox2.AppendText("\r\n[" + resultNew + "]");
                                //如果还没有评论则跳过
                                if (resultNew.IndexOf("<total_results>0</total_results>") != -1)
                                {
                                    continue;
                                }

                                Regex regReview = new Regex(@"<content>([^<]*)</content><created>([^<]*)</created><nick>([^<]*)</nick><result>([^<]*)</result>", RegexOptions.IgnoreCase);
                                MatchCollection matchReview = regReview.Matches(resultNew);

                                //防止插入重复的评论判断
                                sql = "SELECT COUNT(*) FROM TopTradeRate WHERE tid = '" + orderid + "' AND nick = '" + dtOrder.Rows[j]["buynick"].ToString() + "' AND itemid = '" + dtOrderList.Rows[l]["itemid"].ToString() + "'";
                                debugSql = sql;
                                //textBox2.AppendText("\r\n" + sql);
                                string reviewCount = db.GetTable(sql).Rows[0][0].ToString();

                                //在多次插入的问题没有找到前先用此方法解决
                                if (reviewCount == "0")
                                {
                                    //插入数据库
                                    sql = "INSERT INTO TopTradeRate (" +
                                                        "tid, " +
                                                        "content, " +
                                                        "created, " +
                                                        "nick, " +
                                                        "owner, " +
                                                        "itemid, " +
                                                        "result " +
                                                    " ) VALUES ( " +
                                                        " '" + orderid + "', " +
                                                        " '" + matchReview[0].Groups[1].ToString() + "', " +
                                                        " '" + matchReview[0].Groups[2].ToString() + "', " +
                                                        " '" + dtOrder.Rows[j]["buynick"].ToString() + "', " +
                                                        " '" + nick + "', " +
                                                        " '" + dtOrderList.Rows[l]["itemid"].ToString() + "', " +
                                                        " '" + matchReview[0].Groups[4].ToString() + "' " +
                                                    ") ";
                                    //textBox2.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);
                                }

                                //更新该订单为已评价
                                sql = "UPDATE TopOrder SET isok = 1, reviewtime='" + matchReview[0].Groups[2].ToString() + "',result='" + matchReview[0].Groups[4].ToString() + "',content='" + matchReview[0].Groups[1].ToString() + "' WHERE orderid = '" + orderid + "'";
                                //textBox2.AppendText("\r\n" + sql);
                                db.ExecSql(sql);
                            }
                        }






                        string phone = dtOrder.Rows[j]["receiver_mobile"].ToString();
                        string sendnick = dtOrder.Rows[j]["buynick"].ToString();
                        string id = dtOrder.Rows[j]["orderid"].ToString();
                        string status = string.Empty;

                        //如果没有获取过物流状态则获取物流状态
                        if (dtOrder.Rows[j]["delivery_end"].ToString().Trim().Length == 0)
                        {
                            //如果是不可查询物流状态
                            if (isself != "self")
                            {
                                //如果不是淘宝可查物流
                                if (result.IndexOf("不存在") != -1)
                                {
                                    //记录该订单物流状态
                                    sql = "UPDATE TopOrder SET typ = 'self' WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                    //textBox2.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);
                                }
                                else
                                {
                                    //param.Add("fields", "delivery_start,delivery_end,status");
                                    debugResult = result;

                                    status = match[0].Groups[1].ToString();
                                    //记录该订单物流状态
                                    sql = "UPDATE TopOrder SET typ = 'system', shippingstatus = '" + status + "' WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                    //textBox2.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);

                                    //如果货物已派件
                                    if (status == "ACCEPTED_BY_RECEIVER")
                                    {
                                        //使用获取物流状态接口
                                        param = new Dictionary<string, string>();
                                        param.Add("seller_nick", nick);
                                        param.Add("tid", dtOrder.Rows[j]["orderid"].ToString());

                                        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.logistics.trace.search", session, param);
                                        reg = new Regex(@"<status_time>([^<]*)</status_time></transit_step_info></trace_list></logistics_trace_search_response>", RegexOptions.IgnoreCase);
                                        match = reg.Matches(result);
                                        //textBox2.AppendText("\r\n" + result);

                                        //reg = new Regex(@"<delivery_end>([^<]*)</delivery_end><delivery_start>([^<]*)</delivery_start>", RegexOptions.IgnoreCase);
                                        //match = reg.Matches(result);
                                        //如果没有配送时间状态
                                        if (!reg.IsMatch(result))
                                        {
                                            //如果为逻辑错误哦，查不到物流公司
                                            if (result.IndexOf("company-not-support") != -1)
                                            {
                                                //记录该订单物流状态
                                                sql = "UPDATE TopOrder SET typ = 'self' WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                                textBox2.AppendText("\r\n逻辑错误哦，不到物流公司-" + sql);
                                                db.ExecSql(sql);
                                            }
                                            continue;
                                        }

                                        //如果对方还没有签收
                                        if (result.IndexOf("签收人") == -1 && result.IndexOf("正常签收录入扫描") == -1)
                                        {
                                            textBox2.AppendText("\r\n对方还没有签收");
                                            continue;
                                        }

                                        string delivery_end = match[0].Groups[1].ToString();

                                        sql = "UPDATE TopOrder SET typ = 'system', shippingstatus = '" + status + "', delivery_start = '" + delivery_end + "', delivery_end = '" + delivery_end + "', deliverymsg = '" + result + "' WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                        textBox2.AppendText("\r\n" + sql);
                                        db.ExecSql(sql);

                                        //如果获取物流状态时订单已经好评过了则不发送
                                        sql = "SELECT reviewtime FROM TopOrder WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                        string reviewtime = db.GetTable(sql).Rows[0][0].ToString();
                                        if (reviewtime.Trim().Length != 0)
                                        {
                                            textBox2.AppendText("\r\n如果获取物流状态时订单已经好评过了则不发送");
                                            continue;
                                        }

                                        //如果物流状态为可发送且没发送过则发送短信，且必须为VIP版本
                                        textBox2.AppendText("\r\n" + isdeliverymsg);
                                        if (isdeliverymsg != "1")
                                        {
                                            textBox2.AppendText("\r\n" + shippingflag);
                                            //判断是否开启该短信发送节点-物流到达判断
                                            if (shippingflag == "1")
                                            {
                                                //判断是否还有短信可发
                                                sql = "SELECT total FROM TopAutoReview WHERE nick = '" + nick + "'";
                                                string total = db.GetTable(sql).Rows[0][0].ToString();
                                                textBox2.AppendText("\r\n" + total);
                                                if (int.Parse(total) > 0)
                                                {
                                                    //每张物流订单最多提示一次
                                                    sql = "SELECT COUNT(*) FROM TopMsg WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND sendto = '" + buynick + "' AND typ = 'shipping'";
                                                    //sql = "SELECT COUNT(*) FROM TopMsg WHERE sendto = '" + buynick + "' AND typ = 'shipping' AND orderid = " + dtOrder.Rows[j]["orderid"].ToString();
                                                    textBox2.AppendText("\r\n" + sql);
                                                    string shippingCount = db.GetTable(sql).Rows[0][0].ToString();
                                                    textBox2.AppendText("\r\nshippingCount-" + shippingCount);
                                                    if (shippingCount == "0")
                                                    {
                                                        string msg = GetMsg(shippingcontent, shopname, buynick, iscoupon, isfree);
                                                        string resultmsg = SendMessage(phone, msg);
                                                        textBox2.AppendText("\r\n" + nick);
                                                        textBox2.AppendText("\r\n" + msg);
                                                        textBox2.AppendText("\r\n" + resultmsg);
                                                        textBox2.AppendText("\r\n\r\n");

                                                        //如果内容超过70个字则算2条
                                                        string number = "1";
                                                        if (msg.Length > 70)
                                                        {
                                                            number = "2";
                                                        }

                                                        if (resultmsg != "0")
                                                        {
                                                            //记录短信发送记录
                                                            sql = "INSERT INTO TopMsg (" +
                                                                                "nick, " +
                                                                                "sendto, " +
                                                                                "phone, " +
                                                                                "[content], " +
                                                                                "yiweiid, " +
                                                                                "orderid, " +
                                                                                "num, " +
                                                                                "typ " +
                                                                            " ) VALUES ( " +
                                                                                " '" + nick + "', " +
                                                                                " '" + buynick + "', " +
                                                                                " '" + phone + "', " +
                                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                                " '" + resultmsg + "', " +
                                                                                " '" + dtOrder.Rows[j]["orderid"].ToString() + "', " +
                                                                                " '" + number + "', " +
                                                                                " 'shipping' " +
                                                                            ") ";
                                                            db.ExecSql(sql);

                                                            //更新状态
                                                            sql = "UPDATE TopOrder SET isdeliverymsg = 1 WHERE orderid = '" + id + "'";
                                                            db.ExecSql(sql);

                                                            //更新短信数量
                                                            sql = "UPDATE TopAutoReview SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                                                            db.ExecSql(sql);
                                                        }
                                                        else
                                                        {
                                                            //记录短信发送记录-另外的服务重发
                                                            sql = "INSERT INTO TopMsgBak (" +
                                                                                "nick, " +
                                                                                "sendto, " +
                                                                                "phone, " +
                                                                                "[content], " +
                                                                                "yiweiid, " +
                                                                                "orderid, " +
                                                                                "typ " +
                                                                            " ) VALUES ( " +
                                                                                " '" + nick + "', " +
                                                                                " '" + buynick + "', " +
                                                                                " '" + phone + "', " +
                                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                                " '" + resultmsg + "', " +
                                                                                " '" + dtOrder.Rows[j]["orderid"].ToString() + "', " +
                                                                                " 'shipping' " +
                                                                            ") ";
                                                            db.ExecSql(sql);
                                                        }
                                                    }
                                                }
                                            }
                                            ////记录到数据库
                                            //sql = "INSERT INTO TopMsg (" +
                                            //                    "nick, " +
                                            //                    "sendto, " +
                                            //                    "phone, " +
                                            //                    "[content], " +
                                            //                    "typ " +
                                            //                " ) VALUES ( " +
                                            //                    " '" + nick + "', " +
                                            //                    " '" + sendnick + "', " +
                                            //                    " '" + phone + "', " +
                                            //                    " '" + content + "', " +
                                            //                    " 'message' " +
                                            //                ") ";
                                            ////textBox2.AppendText("\r\n" + sql);
                                            //db.ExecSql(sql);

                                            ////增加该物流信息的发送次数
                                            //sql = "UPDATE TopOrder SET sendcount = sendcount + 1 WHERE orderid = " + dtOrder.Rows[j]["orderid"].ToString();
                                            ////textBox2.AppendText("\r\n" + sql);
                                            //db.ExecSql(sql);
                                        }
                                    }
                                }
                            }

                        }
                        //textBox2.AppendText("\r\n\r\n**********************************************************");debugResult
                    }
                }
                catch(Exception e)
                {
                    System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(e, true);

                    textBox2.AppendText("\r\n\r\n" + debugOrder + "\r\n" + debugSql + "\r\n" + debugResult + "\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n************************************************");
                    continue;
                }
            }

            //每半个小时检查一次
            Thread.Sleep(100000);

            Thread newThread = new Thread(CheckOrder);
            newThread.Start();
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

            //强行截取
            if (giftcontent.Length > 66)
            {
                giftcontent = giftcontent.Substring(0, 66);
            }

            return giftcontent;
        }


        /// <summary>
        /// 判断是否赠送礼品
        /// </summary>
        private void SendGift()
        {
            //获取超过卖家设置时间还未支付的订单并取消
            string session = string.Empty;

            DBSql db = DBSql.getInstance();
            string sql = "SELECT * FROM TopAutoReview WHERE isdel = 0";
            //textBox3.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //获取基本设置参数
                    string mindate = (int.Parse(dt.Rows[i]["mindate"].ToString())).ToString();
                    string maxdate = (int.Parse(dt.Rows[i]["maxdate"].ToString())).ToString();
                    string promotionid = dt.Rows[i]["promotionid"].ToString();
                    string couponid = dt.Rows[i]["couponid"].ToString();
                    string iscoupon = dt.Rows[i]["iscoupon"].ToString();
                    string isfree = dt.Rows[i]["isfree"].ToString();
                    string tagid = dt.Rows[i]["tagid"].ToString();
                    string iskefu = dt.Rows[i]["iskefu"].ToString();
                    string itemid = dt.Rows[i]["itemid"].ToString();


                    //通过接口将该用户加入人群
                    sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                    //textBox3.AppendText("\r\n" + sql);
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = dtnick.Rows[0]["sessionblog"].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    //获取相关参数
                    string nick = dt.Rows[i]["nick"].ToString();

                    if (iskefu == "0")
                    {
                        //获取使用淘宝物流并在规定时间内给与好评的完成状态订单
                        sql = "SELECT * FROM TopOrder WHERE ((nick = '" + dt.Rows[i]["nick"].ToString() + "' AND typ = 'system' AND delivery_start IS NOT NULL AND orderstatus = 'TRADE_FINISHED' AND DATEDIFF(d, delivery_start, reviewtime) <= " + mindate + ") OR (nick = '" + dt.Rows[i]["nick"].ToString() + "' AND (typ = 'self' OR delivery_start IS NULL) AND orderstatus = 'TRADE_FINISHED' AND DATEDIFF(d, addtime, reviewtime) <= " + maxdate + ")) AND issend = 0 AND result IS NOT NULL";
                    }
                    else
                    {
                        sql = "SELECT * FROM TopOrder WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "' AND orderstatus = 'TRADE_FINISHED' AND result IS NOT NULL AND issend = 0";
                    }
                    DataTable dtOrder = db.GetTable(sql);
                    //textBox3.AppendText("\r\n" + sql);
                    textBox3.AppendText("\r\n" + dt.Rows[i]["nick"].ToString() + "-" + dtOrder.Rows.Count);
                    //return;
                    //循环订单完成周期兵判断是否赠送礼品
                    for (int j = 0; j < dtOrder.Rows.Count; j++)
                    {
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        string orderid = dtOrder.Rows[j]["orderid"].ToString();
                        string result = string.Empty;
                        string buynick = dtOrder.Rows[j]["buynick"].ToString();

                        //根据用户等级判断赠送个数，如果超出赠送个数则跳出

                        //判断给的评价是否为好评
                        sql = "SELECT result FROM TopTradeRate WHERE tid = '" + orderid + "'";
                        DataTable dtping = db.GetTable(sql);
                        string pingjia = string.Empty;

                        if (dtping.Rows.Count != 0)
                        {
                            pingjia = dtping.Rows[0][0].ToString();
                        }
                        else
                        {
                            textBox3.AppendText("\r\n找不到评价记录-" + orderid); 
                        }

                        //如果是中差评则直接结束
                        if (pingjia == "neutral" || pingjia == "bad")
                        {
                            sql = "UPDATE TopOrder SET issend = 1 WHERE orderid = '" + orderid + "'";
                            db.ExecSql(sql);
                            continue;
                        }

                        //测试中，不开启赠送功能
                        if (pingjia == "good")
                        {
                            textBox3.AppendText("\r\n赠送礼品-" + orderid);

                            ////赠送礼品
                            //if (isfree == "1")
                            //{
                            //    if (iskefu == "0")
                            //    {
                            //        //赠送免费礼品
                            //        param = new Dictionary<string, string>();
                            //        param.Add("tag_id", tagid);
                            //        param.Add("nick", buynick);

                            //        result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.taguser.add", session, param);

                            //        //记录到数据库
                            //        sql = "INSERT INTO TopItemSend (" +
                            //                            "nick, " +
                            //                            "promotionid, " +
                            //                            "itemid, " +
                            //                            "sendto, " +
                            //                            "count " +
                            //                        " ) VALUES ( " +
                            //                            " '" + nick + "', " +
                            //                            " '" + promotionid + "', " +
                            //                            " '" + itemid + "', " +
                            //                            " '" + buynick + "', " +
                            //                            " '1' " +
                            //                        ") ";
                            //        db.ExecSql(sql);
                            //    }
                            //    else
                            //    {
                            //        //更新订单状态-需要审核
                            //        sql = "UPDATE TopOrder SET issend = 2 WHERE orderid = '" + orderid + "'";
                            //        //textBox3.AppendText("\r\n" + sql);
                            //        db.ExecSql(sql);
                            //    }
                            //}

                            //赠送优惠券
                            if (iscoupon == "1" && couponid.Trim() != "")
                            {
                                //判断优惠券是否过期
                                sql = "SELECT COUNT(*) FROM TopCoupon WHERE nick= '" + nick + "' AND coupon_id = '" + couponid + "' AND GETDATE() > end_time";
                                string isCouponOver = db.GetTable(sql).Rows[0][0].ToString();
                                if (isCouponOver == "1")
                                {
                                    textBox3.AppendText("\r\n该优惠券【" + couponid + "】已经过期"); ;
                                    continue;
                                }

                                //测试用
                                //buynick = "美杜莎之心";
                                if (iskefu == "0")
                                {
                                    param = new Dictionary<string, string>();
                                    param.Add("coupon_id", couponid);
                                    param.Add("buyer_nick", buynick);

                                    result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.promotion.coupon.send", session, param);
                                    //<coupon_number>1323930538</coupon_number>
                                    Regex reg = new Regex(@"<coupon_number>([^<]*)</coupon_number>", RegexOptions.IgnoreCase);
                                    MatchCollection match = reg.Matches(result);
                                    //如果失败
                                    if (!reg.IsMatch(result))
                                    {
                                        //如果为买家获得的优惠券已经到上限了
                                        if (result.IndexOf("买家只能得到最多") != -1)
                                        {
                                            textBox3.AppendText("\r\n买家只能得到最多-取消该订单赠送机会" + result);
                                            sql = "UPDATE TopOrder SET issend = 1 WHERE orderid = '" + orderid + "'";
                                            db.ExecSql(sql);
                                        }

                                        textBox3.AppendText("\r\n" + result);
                                        continue;
                                    }
                                    //textBox3.AppendText("\r\n" + result);

                                    string number = match[0].Groups[1].ToString();

                                    //赠送优惠券
                                    sql = "INSERT INTO TopCouponSend (" +
                                                        "nick, " +
                                                        "couponid, " +
                                                        "sendto, " +
                                                        "number, " +
                                                        "count " +
                                                    " ) VALUES ( " +
                                                        " '" + nick + "', " +
                                                        " '" + couponid + "', " +
                                                        " '" + buynick + "', " +
                                                        " '" + number + "', " +
                                                        " '1' " +
                                                    ") ";
                                    //textBox3.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);

                                    //更新优惠券已经赠送数量
                                    sql = "UPDATE TopCoupon SET used = used + 1 WHERE coupon_id = " + couponid;
                                    //textBox3.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);


                                    //更新订单状态-不需要审核
                                    sql = "UPDATE TopOrder SET issend = 1 WHERE orderid = '" + orderid + "'";
                                    //textBox3.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);
                                }
                                else
                                {
                                    //更新订单状态-需要审核
                                    sql = "UPDATE TopOrder SET issend = 2 WHERE orderid = '" + orderid + "'";
                                    //textBox3.AppendText("\r\n" + sql);
                                    db.ExecSql(sql);
                                }
                            }

                            if (iskefu == "0")
                            {
                                //啥赠送都不开的也算结束
                                sql = "UPDATE TopOrder SET issend = 1 WHERE orderid = '" + orderid + "'";
                                //textBox3.AppendText("\r\n" + sql);
                                db.ExecSql(sql);
                            }
                            else
                            {
                                sql = "UPDATE TopOrder SET issend = 2 WHERE orderid = '" + orderid + "'";
                                //textBox3.AppendText("\r\n" + sql);
                                db.ExecSql(sql);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    textBox3.AppendText("\r\n\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n************************************************");
                    continue;
                }
            }

            Thread.Sleep(100000);

            Thread newThread = new Thread(SendGift);
            newThread.Start();
        }

        /// <summary>
        /// 获取长期未确认订单并给予短信提示
        /// </summary>
        public void SendOrderMsg()
        {
            //获取超过卖家设置时间还未支付的订单并取消
            string session = string.Empty;

            DBSql db = DBSql.getInstance();
            string sql = "SELECT * FROM TopAutoReview WHERE isdel = 0";
            //textBox1.AppendText("\r\n" + sql);
            DataTable dt = db.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //获取基本设置参数
                    string mindate = (int.Parse(dt.Rows[i]["mindate"].ToString())).ToString();
                    string maxdate = (int.Parse(dt.Rows[i]["maxdate"].ToString())).ToString();
                    string reviewflag = dt.Rows[i]["reviewflag"].ToString();
                    string reviewtime = dt.Rows[i]["reviewtime"].ToString();
                    string reviewcontent = dt.Rows[i]["reviewcontent"].ToString();

                    string shopname = dt.Rows[i]["shopname"].ToString();
                    string iscoupon = dt.Rows[i]["iscoupon"].ToString();
                    string isfree = dt.Rows[i]["isfree"].ToString();

                    //如果客户开启了而且现在在设置发送时间内
                    //textBox4.AppendText("\r\n" + reviewflag);
                    //textBox4.AppendText("\r\n" + DateTime.Now.Hour.ToString());
                    //textBox4.AppendText("\r\n" + reviewtime);
                    if (reviewflag == "1" && DateTime.Now.Hour.ToString() == reviewtime)
                    { }
                    else
                    {
                        continue;
                    }

                    //通过接口将该用户加入人群
                    sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                    //textBox3.AppendText("\r\n" + sql);
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = dtnick.Rows[0]["sessionblog"].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    //获取相关参数
                    string nick = dt.Rows[i]["nick"].ToString();


                     //获取使用淘宝物流并在规定时间内给与好评的完成状态订单
                    sql = "SELECT * FROM TopOrder WHERE (nick = '" + nick + "' AND typ = 'system' AND delivery_start IS NOT NULL AND orderstatus = 'WAIT_BUYER_CONFIRM_GOODS' AND DATEDIFF(d, delivery_start, GETDATE()) = " + mindate + ") OR  (nick = '" + nick + "' AND (typ = 'self' OR delivery_start IS NULL) AND orderstatus = 'WAIT_BUYER_CONFIRM_GOODS' AND DATEDIFF(d, addtime, GETDATE()) = " + maxdate + ") AND istellmsg = 0";

                    DataTable dtOrder = db.GetTable(sql);
                    //textBox4.AppendText("\r\n\r\n" + nick + "-" + sql);

                    //循环订单完成周期兵判断是否赠送礼品
                    for (int j = 0; j < dtOrder.Rows.Count; j++)
                    {
                        string phone = dtOrder.Rows[j]["receiver_mobile"].ToString();
                        string buynick = dtOrder.Rows[j]["buynick"].ToString();
                        string istellmsg = dtOrder.Rows[j]["istellmsg"].ToString();

                        //如果物流状态为可发送且没发送过则发送短信，且必须为VIP版本
                        if (istellmsg != "1")
                        {
                            //判断是否开启该短信发送节点-物流到达判断
                            if (reviewflag == "1")
                            {
                                //如果今天发过提醒短信就不发了
                                sql = "SELECT COUNT(*) FROM TopMsg WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND sendto = '" + buynick + "' AND typ = 'review'";
                                string msgcount = db.GetTable(sql).Rows[0][0].ToString();
                                if (msgcount != "0")
                                {
                                    //更新状态
                                    sql = "UPDATE TopOrder SET istellmsg = 1 WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                    db.ExecSql(sql);
                                    continue;
                                }

                                //判断是否还有短信可发
                                //textBox2.AppendText("\r\n" + sql);
                                sql = "SELECT total FROM TopAutoReview WHERE nick = '" + nick + "'";
                                string total = db.GetTable(sql).Rows[0][0].ToString();
                                //textBox2.AppendText("\r\n" + total);
                                if (int.Parse(total) > 0)
                                {
                                    //每张物流订单最多提示一次
                                    sql = "SELECT COUNT(*) FROM TopMsg WHERE DATEDIFF(d, adddate, GETDATE()) = 0 AND sendto = '" + buynick + "' AND typ = 'review'";// AND orderid = " + dtOrder.Rows[j]["orderid"].ToString();
                                    textBox4.AppendText("\r\n" + sql);
                                    string shippingCount = db.GetTable(sql).Rows[0][0].ToString();
                                    textBox4.AppendText("\r\nshippingCount-" + shippingCount);
                                    if (shippingCount == "0")
                                    {
                                        string msg = GetMsg(reviewcontent, shopname, buynick, iscoupon, isfree);
                                        string resultmsg = SendMessage(phone, msg);
                                        textBox4.AppendText("\r\n" + nick);
                                        textBox4.AppendText("\r\n" + msg);
                                        textBox4.AppendText("\r\n" + resultmsg);
                                        textBox4.AppendText("\r\n\r\n");

                                        //如果内容超过70个字则算2条
                                        string number = "1";
                                        if (msg.Length > 70)
                                        {
                                            number = "2";
                                        }

                                        if (resultmsg != "0")
                                        {
                                            //记录短信发送记录
                                            sql = "INSERT INTO TopMsg (" +
                                                                "nick, " +
                                                                "sendto, " +
                                                                "phone, " +
                                                                "[content], " +
                                                                "yiweiid, " +
                                                                "orderid, " +
                                                                "num, " +
                                                                "typ " +
                                                            " ) VALUES ( " +
                                                                " '" + nick + "', " +
                                                                " '" + buynick + "', " +
                                                                " '" + phone + "', " +
                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                " '" + resultmsg + "', " +
                                                                " '" + dtOrder.Rows[j]["orderid"].ToString() + "', " +
                                                                " '" + number + "', " +
                                                                " 'review' " +
                                                            ") ";
                                            db.ExecSql(sql);

                                            //更新状态
                                            sql = "UPDATE TopOrder SET istellmsg = 1 WHERE orderid = '" + dtOrder.Rows[j]["orderid"].ToString() + "'";
                                            db.ExecSql(sql);

                                            //更新短信数量
                                            sql = "UPDATE TopAutoReview SET used = used + " + number + ",total = total-" + number + " WHERE nick = '" + nick + "'";
                                            db.ExecSql(sql);
                                        }
                                        else
                                        {
                                            //记录短信发送记录
                                            sql = "INSERT INTO TopMsgBak (" +
                                                                "nick, " +
                                                                "sendto, " +
                                                                "phone, " +
                                                                "[content], " +
                                                                "yiweiid, " +
                                                                "orderid, " +
                                                                "typ " +
                                                            " ) VALUES ( " +
                                                                " '" + nick + "', " +
                                                                " '" + buynick + "', " +
                                                                " '" + phone + "', " +
                                                                " '" + msg.Replace("'", "''") + "', " +
                                                                " '" + resultmsg + "', " +
                                                                " '" + dtOrder.Rows[j]["orderid"].ToString() + "', " +
                                                                " 'review' " +
                                                            ") ";
                                            db.ExecSql(sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    textBox4.AppendText("\r\n\r\n" + e.Message + "\r\n" + e.InnerException + "\r\n" + e.Source + "\r\n" + e.StackTrace + "\r\n************************************************");
                    continue;
                }
            }

            
            Thread.Sleep(100000);

            Thread newThread = new Thread(SendOrderMsg);
            newThread.Start();
        }




        /// <summary>
        /// 获取卖家动态评分信息1天1次
        /// </summary>
        public void GetShopScore()
        { 
            //获取超过卖家设置时间还未支付的订单并取消
            string session = string.Empty;

            DBSql db = DBSql.getInstance();
            string sql = "SELECT * FROM TopAutoReview WHERE isdel = 0";
            DataTable dt = db.GetTable(sql);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    //通过接口将该用户加入人群
                    sql = "SELECT * FROM TopTaobaoShop WHERE nick = '" + dt.Rows[i]["nick"].ToString() + "'";
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = dtnick.Rows[0]["sessionblog"].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    Dictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("fields", "shop_score");
                    param.Add("nick", dt.Rows[i]["nick"].ToString());

                    string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.shop.get", session, param);
                    Regex regReview = new Regex(@"<delivery_score>([^<]*)</delivery_score><item_score>([^<]*)</item_score><service_score>([^<]*)</service_score>", RegexOptions.IgnoreCase);
                    Match matchReview = regReview.Match(result);

                    //插入数据库TopShopScore
                    sql = "INSERT INTO TopShopScore (" +
                                        "delivery_score, " +
                                        "item_score, " +
                                        "service_score, " +
                                        "nick " +
                                    " ) VALUES ( " +
                                        " '" + matchReview.Groups[1].ToString() + "', " +
                                        " '" + matchReview.Groups[2].ToString() + "', " +
                                        " '" + matchReview.Groups[3].ToString() + "', " +
                                        " '" + dt.Rows[i]["nick"].ToString() + "' " +
                                    ") ";
                    db.ExecSql(sql);
                }
                catch { }
            }

            //一天运行一次
            Thread.Sleep(86400000);

            Thread newThread = new Thread(GetShopScore);
            newThread.Start();
        }






         
        /// <summary>
        /// 通过借口发送短信
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="content"></param>
        private string SendMessage(string phone, string msg)
        {
            string uid = "terrylv";
            string pass = "123456";

            msg = HttpUtility.UrlEncode(msg);

            string param = "username=" + uid + "&password=" + pass + "&method=sendsms&mobile=" + phone + "&msg=" + msg;
            byte[] bs = Encoding.ASCII.GetBytes(param);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://sms2.eachwe.com/api.php");
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
                        textBox3.AppendText("\r\n" + phone + "-" + content);
                        return "0";
                    }
                    else
                    {
                        //发送成功
                        Regex reg = new Regex(@"<sid>([^<]*)</sid>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(content);
                        textBox3.AppendText("\r\n" + content);
                        string number = match[0].Groups[1].ToString();
                        return number;
                    }
                }
            }
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
}
