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

namespace tetegroupbuy
{
    public partial class Form1 : Form
    {
        public static string logUrl = "D:/svngroupbuy/website/ErrLog";
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            //获取最新团购相关的订单数据并加入数据库
            Thread newThread = new Thread(DoMyJob);
            newThread.Start();

            ////获取订单清单中未付款的订单并判断是否超时取消
            //Thread newThread1 = new Thread(CheckOrder);
            //newThread1.Start();
        }

        /// <summary>
        /// 获取订单清单中未付款的订单并判断是否超时取消
        /// </summary>
        private void CheckOrder()
        {
            try
            {
                //获取超过卖家设置时间还未支付的订单并取消
                string appkey = "12223169";
                string secret = "ff3d3442ab809930d187623ffad8e91e";
                string session = string.Empty;
                string minute = "8";

                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE DATEDIFF(s,GETDATE(),starttime) < 0 AND DATEDIFF(s,GETDATE(),endtime) > 0 AND isdelete = 0 AND tagid <> 1";
                //textBox2.AppendText("\r\n" + sql);
                DataTable dtGroup = db.GetTable(sql);
                for (int q = 0; q < dtGroup.Rows.Count; q++)
                {
                    //判断如果没有合适的需要检查的订单则下一个
                    sql = "SELECT o.*,d.buynick FROM TopGroupBuyDetailOrder o INNER JOIN TopGroupBuyDetail d ON d.id = o.detailid WHERE o.groupbuyid = " + dtGroup.Rows[q]["id"].ToString() + " AND o.status = 'WAIT_BUYER_PAY' AND DATEDIFF(n,o.addtime,GETDATE()) > " + minute;
                    //textBox2.AppendText("\r\n" + sql);
                    DataTable dt = db.GetTable(sql);
                    if (dt.Rows.Count == 0)
                    {
                        continue;
                    }

                    //通过接口将该用户加入人群
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dtGroup.Rows[q]["nick"].ToString() + "'";
                    //textBox2.AppendText("\r\n" + sql);
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //查询该订单的状态
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("fields", "status,pay_time");
                        param.Add("tid", dt.Rows[i]["orderid"].ToString());
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trade.fullinfo.get", session, param);
                        //textBox2.AppendText("\r\n" + resultpro);
                        Regex reg = new Regex(@"<status>([^<]*)</status>", RegexOptions.IgnoreCase);
                        MatchCollection match = reg.Matches(resultpro);

                        if (match[0].Groups[1].ToString() == "WAIT_SELLER_SEND_GOODS" || match[0].Groups[1].ToString() == "WAIT_BUYER_CONFIRM_GOODS" || match[0].Groups[1].ToString() == "TRADE_FINISHED")
                        {
                            string paytime = Regex.Match(resultpro, @"<pay_time>([^<]*)</pay_time>").Groups[1].ToString();
                            //如果支付则更改订单状态并记录支付成功时间
                            sql = "UPDATE TopGroupBuyDetailOrder SET status = '" + match[0].Groups[1].ToString() + "',paytime = '" + paytime + "' WHERE id = " + dt.Rows[i]["id"].ToString();
                            //textBox2.AppendText("\r\n" + sql);
                            db.ExecSql(sql);
                        }
                        else
                        {
                            //向接口发送取消请求
                            param = new Dictionary<string, string>();
                            param.Add("tid", dt.Rows[i]["orderid"].ToString());
                            param.Add("close_reason", "拍下团购商品后在指定时间内没有付款");
                            resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trade.close", session, param);
                            //textBox2.AppendText("\r\n" + resultpro);

                            //取消改用户购买资格
                            param = new Dictionary<string, string>();
                            param.Add("tag_id", dtGroup.Rows[q]["tagid"].ToString());
                            param.Add("nick", dt.Rows[i]["buynick"].ToString());
                            resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.taguser.delete", session, param);
                            textBox2.AppendText("\r\n" + resultpro);

                            //如果没有支付则取消该订单并减少团购数量
                            sql = "UPDATE TopGroupBuyDetailOrder SET status = 'TRADE_CLOSED_BY_TAOBAO',canceltime = GETDATE() WHERE id = " + dt.Rows[i]["id"].ToString();
                            //textBox2.AppendText("\r\n" + sql);
                            db.ExecSql(sql);

                            //减少团购详细里面记录的数量
                            sql = "UPDATE TopGroupBuyDetail SET count = count - " + dt.Rows[i]["count"].ToString() + " WHERE id = " + dt.Rows[i]["detailid"].ToString();
                            //textBox2.AppendText("\r\n" + sql);
                            db.ExecSql(sql);

                            //减少团购总表里面记录的数量
                            sql = "UPDATE TopGroupBuy SET buycount = buycount - " + dt.Rows[i]["count"].ToString() + " WHERE id = " + dt.Rows[i]["groupbuyid"].ToString();
                            //textBox2.AppendText("\r\n" + sql);
                            db.ExecSql(sql);

                            //更新团购加入记录并置为不可用
                            sql = "UPDATE TopGroupBuyDetail SET iscancel = 1 WHERE id = " + dt.Rows[i]["detailid"].ToString();
                            //textBox2.AppendText("\r\n" + sql);
                            db.ExecSql(sql);
                        }
                    }
                }
                //textBox2.AppendText("\r\n\r\n**********************************************************");
                //休息后继续循环-默认10分钟一次
                Thread.Sleep(60000);

                Thread newThread1 = new Thread(CheckOrder);
                newThread1.Start();
            }
            catch (Exception e)
            {
                MessageBox.Show("\r\n" + e.StackTrace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void DoMyJob()
        {
            try
            {
                //获取正在进行中的团购项目        
                string appkey = "12287381";//"12287381";
                string secret = "d3486dac8198ef01000e7bd4504601a4";//d3486dac8198ef01000e7bd4504601a4";
                string session = string.Empty;

                //获取已经结束的团购活动并取消
                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE DATEDIFF(s,GETDATE(),endtime) < 0 AND promotionid <> 0 AND isdelete = 0";

                #region 取消活动 删除该活动关联的用户群 将该团购标志为已结束
                //WriteLog(sql, "");
                DataTable enddt = db.GetTable(sql);
                //通过接口将该用户加入人群
                for (int y = 0; y < enddt.Rows.Count; y++)
                {
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";

                    WriteLog(sql, "");
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    //取消该活动
                    IDictionary<string, string> paramnew = new Dictionary<string, string>();
                    paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                    string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);

                    WriteLog(resultnew, "");

                    //删除该活动关联的用户群
                    paramnew = new Dictionary<string, string>();
                    if (enddt.Rows[y]["tagid"].ToString() != "1")
                    {
                        paramnew.Add("tag_id", enddt.Rows[y]["tagid"].ToString());
                    }
                    resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.tag.delete", session, paramnew);

                    WriteLog(resultnew, "");

                    //将该团购标志为已结束
                    sql = "UPDATE TopGroupBuy SET isdelete = 1 WHERE id = " + enddt.Rows[y]["id"].ToString();
                    //textBox2.AppendText("\r\n" + sql);
                    db.ExecSql(sql);
                }
                #endregion

                //获取正在进行中的团购活动
                sql = "SELECT * FROM TopGroupBuy WHERE DATEDIFF(s,GETDATE(),starttime) < 0 AND DATEDIFF(s,GETDATE(),endtime) > 0 AND promotionid <> 0 AND isdelete = 0";

                WriteLog(sql, "");
                DataTable dt = db.GetTable(sql);
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    //获取买家团购记录信息表
                    sql = "SELECT * FROM TopGroupBuyDetail WHERE groupbuyid = " + dt.Rows[k]["id"].ToString();

                    WriteLog(sql, "");
                    DataTable dtdetail = db.GetTable(sql);
                    if (dtdetail.Rows.Count == 0)
                    {
                        //如果还没有购买记录则继续-为了保持卖家的SESION长期有效，所以必须使用订单查询接口进行查询
                        //continue;
                    }

                    //通过接口将该用户加入人群
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dt.Rows[k]["nick"].ToString() + "'";

                    WriteLog(sql, "");
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    else
                    {
                        //如果查不到记录则继续
                        continue;
                    }

                    //变量赋值
                    string groupproductid = dt.Rows[k]["productid"].ToString();
                    DateTime startdate = new DateTime();
                    DateTime enddate = new DateTime();

                    //获取该团购上次更新时间
                    if (dt.Rows[k]["updatedate"] == DBNull.Value)
                    {
                        //如果是第一次检测
                        startdate = DateTime.Parse(dt.Rows[k]["starttime"].ToString());
                    }
                    else
                    {
                        //如果不是第一次检测
                        startdate = DateTime.Parse(dt.Rows[k]["updatedate"].ToString());
                    }
                    enddate = startdate.AddHours(23);

                    //判断结束时间是否大于现在
                    if (enddate > DateTime.Now.AddMinutes(-2))
                    {
                        enddate = DateTime.Now.AddMinutes(-2);
                    }

                    WriteLog(startdate.ToString("yyyy-MM-dd HH:mm:ss") + "-" + enddate.ToString("yyyy-MM-dd HH:mm:ss"), "");

                    //更新数据中记录的判断时间
                    sql = "UPDATE TopGroupBuy SET updatedate = '" + enddate.ToString() + "' WHERE id = " + dt.Rows[k]["id"].ToString();
                    db.ExecSql(sql);

                    WriteLog(sql, "");

                    //通过接口获取用户下单信息(此处使用查询效率最高的30分钟间距作为查询条件)
                    IDictionary<string, string> param = new Dictionary<string, string>();
                    param.Add("fields", "buyer_nick,tid,status,created");
                    param.Add("start_modified", startdate.ToString("yyyy-MM-dd HH:mm:ss"));
                    param.Add("end_modified", enddate.ToString("yyyy-MM-dd HH:mm:ss"));
                    string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trades.sold.increment.get", session, param);

                    WriteLog(result, "");
                    Regex reg = new Regex(@"<buyer_nick>([^<]*)</buyer_nick><created>([^<]*)</created><status>([^<]*)</status><tid>([^<]*)</tid>", RegexOptions.IgnoreCase);
                    MatchCollection match = reg.Matches(result);


                    WriteLog(result, "");
                    WriteLog(match.Count.ToString(), "");
                    for (int i = 0; i < match.Count; i++)
                    {

                        //判断该用户的订单是否包含该团购商品
                        param = new Dictionary<string, string>();
                        param.Add("fields", "orders.num_iid,orders.num");
                        param.Add("tid", match[i].Groups[4].ToString());
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.trade.fullinfo.get", session, param);

                        //判断该订单里面是否包含团购商品并统计订购数量
                        Regex regpro = new Regex(@"<num>([^<]*)</num><num_iid>([^<]*)</num_iid>", RegexOptions.IgnoreCase);
                        MatchCollection matchpro = regpro.Matches(resultpro);

                        WriteLog(resultpro, "");
                        for (int j = 0; j < matchpro.Count; j++)
                        {
                            WriteLog(matchpro[j].Groups[2].ToString() + "-" + groupproductid, "");
                            //判断该订单是否包含团购商品
                            if (matchpro[j].Groups[2].ToString() == groupproductid)
                            {
                                //如果包含则记录到本地数据库
                                RecordBuyData(match[i].Groups[1].ToString(), dt.Rows[k]["id"].ToString(), match[i].Groups[3].ToString(), matchpro[j].Groups[1].ToString(), match[i].Groups[4].ToString(), "0", match[i].Groups[2].ToString());
                            }
                            else
                            {
                                //取消 
                                continue;
                            }
                        }
                    }
                }
                dt.Dispose();

                WriteLog("**********************************************************", "");
                //休息后继续循环-默认15分钟一次
                Thread.Sleep(900000);

                Thread newThread = new Thread(DoMyJob);
                newThread.Start();
            }
            catch (Exception e)
            {

                WriteLog("**********************************************************" + e.StackTrace + "----error!!!", "1");
                //MessageBox.Show("\r\n" + e.StackTrace);
                Thread newThread = new Thread(DoMyJob);
                //休息后继续循环-默认10分钟一次
                Thread.Sleep(600000);
                newThread.Start();
            }
        }

        /// <summary>
        /// 记录数据库
        /// </summary>
        /// <param name="buynick"></param>
        /// <param name="groupbuyid"></param>
        /// <param name="status"></param>
        /// <param name="num"></param>
        /// <param name="orderid"></param>
        private void RecordBuyData(string buynick, string groupbuyid, string status, string num, string orderid, string detailid, string created)
        {
            DBSql db = DBSql.getInstance();
            //判断该订单是否插入过
            string sql = "SELECT COUNT(*) FROM TopGroupBuyDetailOrder WHERE orderid = '" + orderid + "'";

            WriteLog(sql, "");
            string count = db.GetTable(sql).Rows[0][0].ToString();
            if (count != "0")
            {
                return;
            }

            //判断该用户是否有订购记录
            if (detailid == "0")
            {
                sql = "SELECT COUNT(*) FROM TopGroupBuyDetail WHERE buynick = '" + buynick + "' AND groupbuyid = " + groupbuyid;

                WriteLog(sql, "");
                count = db.GetTable(sql).Rows[0][0].ToString();
                if (count == "0")
                {
                    //插入记录
                    sql = "INSERT INTO TopGroupBuyDetail (" +
                                   "groupbuyid," +
                                   "buynick" +
                               " ) VALUES ( " +
                                   " '" + groupbuyid + "'," +
                                   " '" + buynick + "'" +
                            ") ";

                    WriteLog(sql, "");
                    db.ExecSql(sql);

                    //返回ID
                    sql = "SELECT TOP 1 id FROM TopGroupBuyDetail ORDER BY id DESC";

                    WriteLog(sql, "");
                    detailid = db.GetTable(sql).Rows[0][0].ToString();
                }
                else
                {
                    sql = "SELECT id FROM TopGroupBuyDetail WHERE buynick = '" + buynick + "' AND groupbuyid = " + groupbuyid;
                    detailid = db.GetTable(sql).Rows[0][0].ToString();
                }
            }

            //更新详细购买记录
            sql = "UPDATE TopGroupBuyDetail SET count = count + " + num + " WHERE id = " + detailid + "";

            WriteLog(sql, "");
            db.ExecSql(sql);

            //更新团购总购买记录
            sql = "UPDATE TopGroupBuy SET buycount = buycount + " + num + " WHERE id = " + groupbuyid + "";

            WriteLog(sql, "");
            db.ExecSql(sql);

            //插入团购关联淘宝订单表数据
            sql = "INSERT INTO TopGroupBuyDetailOrder (detailid,orderid,status,addtime,count,groupbuyid) VALUES ('" + detailid + "','" + orderid + "','" + status + "','" + created + "','" + num + "','" + groupbuyid + "')";

            WriteLog(sql, "");
            db.ExecSql(sql);
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



        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog(string message, string type)
        {
            string tempStr = logUrl + "/Groupby" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Groupby" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/GroupbyErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            }
            if (!Directory.Exists(tempStr))
            {
                Directory.CreateDirectory(tempStr);
            }

            if (System.IO.File.Exists(tempFile))
            {
                ///如果日志文件已经存在，则直接写入日志文件
                StreamWriter sr = System.IO.File.AppendText(tempFile);
                sr.WriteLine("\n");
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }
            else
            {
                ///创建日志文件
                StreamWriter sr = System.IO.File.CreateText(tempFile);
                sr.WriteLine(DateTime.Now + "\n" + message);
                sr.Close();
            }

        }
    }
}
