using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace teteactivity
{
    public partial class Form1 : Form
    {
        public static string logUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/ErrLog";
        public static string styleUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew1.html";//默认模板
        public static string styleUrl2 = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew2.html";//第二套模板（一大三小）
        public static string styleUrl2smail = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew2-1.html";//第二套模板（一大三小） 小模板 (团购模板第三套和第二套)
        public static string styleUrl3 = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew3.html";//第三套模板（一排三列）
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
 
            //刷新未开始的服务
            Thread newThread = new Thread(activitynotStart);
            newThread.Start();

             
        }

        /// <summary>
        /// 刷新未开始的服务
        /// </summary>
        private void activitynotStart()
        {
            try
            {
 
                string session = string.Empty;

                DBSql db = DBSql.getInstance();

                string sql1 = "select * from tete_activity where status=0 and  DATEDIFF(s,GETDATE(),startDate) < 0 "; //未开始的活动
                DataTable dt1 = DBSql.getInstance().GetTable(sql1);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        sql1 = "update tete_activity set Status=1 , isok=1 where id=" + dt1.Rows[i]["ID"].ToString(); //更新活动状态
                        WriteLog(sql1,"");
                        DBSql.getInstance().ExecSql(sql1);
                    }
                }
                string sql2 = " select * from tete_activitylist where status=0 and promotionID=0 and  DATEDIFF(s,GETDATE(),startDate) < 0";//未开始的活动列表

                dt1 = DBSql.getInstance().GetTable(sql2); 
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        string sqlstr1 = "SELECT session FROM TopTaobaoShop WHERE nick = '" + dt1.Rows[i]["nick"].ToString() + "'";

                        DataTable dtnick = db.GetTable(sqlstr1);
                        if (dtnick!=null&&dtnick.Rows.Count != 0)
                        {
                            session = dtnick.Rows[0]["session"].ToString();
                        }
                        sqlstr1 = "select * from tete_activitylist where ProductID=" + dt1.Rows[i]["ProductID"].ToString() + " and status=1 and promotionID<>0";
                        dtnick = db.GetTable(sqlstr1);
                        if (dtnick != null && dtnick.Rows.Count != 0)
                        {
                            sqlstr1 = "update tete_activitylist set status=4 where id=" + dt1.Rows[i]["ID"].ToString();
                            db.ExecSql(sqlstr1);
                        }
                        else
                        {
                            //添加活动,现在不需要了,添加的时候就已经添加活动了
                            //addpromotion(dt1.Rows[i]["ProductID"].ToString(), dt1.Rows[i]["discountType"].ToString(), dt1.Rows[i]["discountValue"].ToString(), dt1.Rows[i]["startDate"].ToString(), dt1.Rows[i]["endDate"].ToString(), dt1.Rows[i]["Name"].ToString(), dt1.Rows[i]["decreaseNum"].ToString(), session, dt1.Rows[i]["ActivityID"].ToString());
                        }
                    }
                }

                dt1.Dispose();

                //更新宝贝描述
                DoMyJob();
                //休息后继续循环-默认1分半钟一次
                Thread.Sleep(30000);

                Thread newThread11 = new Thread(activitynotStart);
                newThread11.Start();
            }
            catch (Exception e)
            {

                WriteLog("activity自动取消活动运行错误*****************************************" + e.StackTrace + e.Message+e + "----error!!!", "1");
                //MessageBox.Show("\r\n" + e.StackTrace);
                Thread newThread11 = new Thread(activitynotStart);
                //休息后继续循环-默认1分半钟一次 
                Thread.Sleep(30000);
                newThread11.Start();
            }
        }

        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="iid">商品宝贝ID</param>
        /// <param name="discountType">促销类型</param>
        /// <param name="discountValue">促销值</param>
        /// <param name="sdate">活动开始时间</param>
        /// <param name="edate">活动结束时间</param>
        /// <param name="name">活动标题</param>
        /// <param name="decreaseNum">是否限制</param>
        /// <param name="session"></param>
        /// <param name="actionId">活动ID</param>
        public void addpromotion(string iid, string discountType, string discountValue, string sdate, string edate, string name, string decreaseNum, string session, string actionId)
        {
            //创建活动及相关人群
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";



            //创建活动相关人群
            string guid = Guid.NewGuid().ToString().Substring(0, 4);
            IDictionary<string, string> param = new Dictionary<string, string>();

            string tagid = "1";  
            //创建活动
            param = new Dictionary<string, string>();
            param.Add("num_iids", iid);
            param.Add("discount_type", discountType);
            param.Add("discount_value", discountValue);
            param.Add("start_date", DateTime.Parse(sdate).ToString("yyyy-MM-dd hh:mm:ss"));
            param.Add("end_date", DateTime.Parse(edate).ToString("yyyy-MM-dd hh:mm:ss"));
            param.Add("promotion_title", name);
            param.Add("decrease_num", decreaseNum);


            param.Add("tag_id", tagid);
            string result = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.add", session, param);


            if (result.IndexOf("error_response") != -1)
            {
                string  sql = "delete from    [tete_activitylist]    WHERE ActivityID = " + actionId + " and  ProductID=" + iid;

                DBSql.getInstance().ExecSql(sql);

                string err = new Regex(@"<sub_msg>([^<]*)</sub_msg>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();
                if (err == "")
                {
                    WriteLog("活动创建失败，错误原因：您的session已经失效，需要重新授权" + actionId, "1");

                }
                else {
                    WriteLog("活动创建失败，错误原因：" + err, "1");
                }
 
 
                return;
            }

            string promotionid = new Regex(@"<promotion_id>([^<]*)</promotion_id>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString();

            //更新活动
           string  sql2 = "update  tete_activitylist set Status=1 ,isok=1,promotionID=" + promotionid + "  WHERE ActivityID = " + actionId + " and  ProductID=" + iid;
           DBSql.getInstance().ExecSql(sql2);
 
        }



        /// <summary>
        /// 生成HTML
        /// </summary>
        /// <param name="id">店铺模板ID</param>
        /// <returns></returns>
        private string CreateGroupbuyHtml(string id)
        {
            if (id == "")
            {
                return "";
            }
            string str = string.Empty;
            string sql = "select tete_shoptempletlist.*,templetID,[buttonValue],[scbzvalue],[lpbzvalue],[byvalue],[title],[careteDate],[Sort] from tete_shoptempletlist left join   tete_shoptemplet on tete_shoptempletlist.shoptempletID=tete_shoptemplet.ID  WHERE tete_shoptempletlist.shoptempletID = " + id;
            DBSql db = DBSql.getInstance();
            DataTable dt = db.GetTable(sql);
            if (dt == null || dt.Rows.Count < 1)
            {
                return "";
            }
            string templatehtmlUrl = styleUrl;//默认模板
            string template2htmlUrl = styleUrl2smail;//第二套模板（一大三小） 小模板  (团购模板第三套和第二套)

            //模板生成需要在好好考虑一下。。。。。。。。。。。。。。。。。。。。。。。。。。。。
            //是多商品团购模板
            if (dt.Rows[0]["templetID"].ToString() == "2")
            {
                //第二套模板（一大三小）
                templatehtmlUrl = styleUrl2;
            }
            //是多商品团购模板
            if (dt.Rows[0]["templetID"].ToString() == "3")
            {
                //第三套模板（一排三列）
                templatehtmlUrl = styleUrl3;
            }

           
            string html = File.ReadAllText(templatehtmlUrl);
            string smailtempStr = string.Empty;//小模板
            string btvalue = dt.Rows[0]["buttonValue"].ToString();
            string scbzvalue = dt.Rows[0]["scbzvalue"].ToString();
            string lpbzvalue = dt.Rows[0]["lpbzvalue"].ToString();
            string byvalue = dt.Rows[0]["byvalue"].ToString();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string purl = dt.Rows[i]["ProductUrl"].ToString();
                if (i == 0)
                {
                    str = html;
                    str = tuanhtmlReplace(str, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());

                    //如果模板是第三套模板，追加第一个活动HTML
                    if (templatehtmlUrl == "tpl/stylenew3.html")
                    {
                        html = File.ReadAllText(template2htmlUrl);
                        smailtempStr += html;
                        smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                    }
                }
                else
                {
                    //是多商品团购模板
                    if (dt.Rows[i]["templetID"].ToString() == "2" || dt.Rows[i]["templetID"].ToString() == "3")
                    {
                        html = File.ReadAllText(template2htmlUrl);
                        smailtempStr += html;
                        smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                    }

                    //是多商品团购模板
                    if (dt.Rows[i]["templetID"].ToString() == "1")
                    {
                        html = File.ReadAllText(templatehtmlUrl);
                        str = str + html;
                        str = tuanhtmlReplace(str, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                    }

                }
            }
            str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
            //模板图片替换
            str = tempImgreplace(str, dt.Rows[0]["nick"].ToString(), dt.Rows[0]["templetID"].ToString(), btvalue, scbzvalue, lpbzvalue, byvalue);

            return str;
        }

        /// <summary>
        /// 模板图片替换
        /// </summary>
        /// <param name="str">html</param>
        /// <param name="tid">模板ID</param>
        /// <returns></returns>
        public string tempImgreplace(string str, string taobaoNick, string tid, string btvalue, string scbzvalue, string lpbzvalue, string byvalue)
        {
            DBSql db = DBSql.getInstance();
            string sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and templetID=" + tid;
            DataTable dt = db.GetTable(sqlstr);
            if (dt == null && dt.Rows.Count < 1)
            {
                //如果为空返回HTML
                return str;
            }
            sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='" + btvalue + ".png'";
            DataTable dtr2 = db.GetTable(sqlstr);
            if (dtr2 != null && dtr2.Rows.Count > 0)
            {
                //参团按钮替换
                str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/temp2ct.png", dtr2.Rows[0]["taobaourl"].ToString());
                str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/temp2ct2.png", dtr2.Rows[0]["taobaourl"].ToString());
            }
            if (tid == "1" || tid == "2")
            {
                //如果是团购模板需要替换团购HTML
                //商城图片替换
                if (scbzvalue == "1")
                {
                    sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='mall_protection_icon.png'";
                    dtr2 = db.GetTable(sqlstr);
                    if (dtr2 != null && dtr2.Rows.Count > 0)
                    {
                        str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/mall_protection_icon.png", dtr2.Rows[0]["taobaourl"].ToString());
                    }
                }
                else
                {
                    str = str.Replace("<img src=\"http://groupbuy.7fshop.com/top/groupbuy/images/mall_protection_icon.png\" />", "");
                }
                //良品图片替换
                if (lpbzvalue == "1")
                {
                    sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='lp_protection_icon.png'";
                    dtr2 = db.GetTable(sqlstr);
                    if (dtr2 != null && dtr2.Rows.Count > 0)
                    {
                        str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/lp_protection_icon.png", dtr2.Rows[0]["taobaourl"].ToString());
                    }
                }
                else
                {
                    str = str.Replace("<img src=\"http://groupbuy.7fshop.com/top/groupbuy/images/lp_protection_icon.png\" />", "");
                }
                //包邮标准替换
                if (byvalue != "")
                {
                    string str1 = string.Empty;
                    string str2 = string.Empty;
                    string str3 = string.Empty;
                    string str4 = string.Empty;
                    string str5 = string.Empty;
                    string str6 = string.Empty;
                    string str7 = string.Empty;
                    for (int k = 0; k < byvalue.Split(',').Length; k++)
                    {
                        #region
                        if (k == 0)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str1 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str1 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str1 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str1 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str1 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str1 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str1 = "-198";
                            }

                        }
                        if (k == 1)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str2 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str2 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str2 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str2 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str2 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str2 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str2 = "-198";
                            }
                        }
                        if (k == 2)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str3 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str3 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str3 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str3 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str3 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str3 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str3 = "-198";
                            }
                        }
                        if (k == 3)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str4 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str4 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str4 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str4 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str4 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str4 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str4 = "-198";
                            }
                        }
                        if (k == 4)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str5 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str5 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str5 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str5 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str5 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str5 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str5 = "-198";
                            }
                        }
                        if (k == 5)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str6 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str6 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str6 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str6 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str6 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str6 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str6 = "-198";
                            }
                        }
                        if (k == 6)
                        {
                            if (byvalue.Split(',')[k].ToString() == "0")
                            {
                                str7 = "0";
                            }
                            if (byvalue.Split(',')[k].ToString() == "1")
                            {
                                str7 = "-33";
                            }
                            if (byvalue.Split(',')[k].ToString() == "2")
                            {
                                str7 = "-66";
                            }
                            if (byvalue.Split(',')[k].ToString() == "3")
                            {
                                str7 = "-99";
                            }
                            if (byvalue.Split(',')[k].ToString() == "4")
                            {
                                str7 = "-132";
                            }
                            if (byvalue.Split(',')[k].ToString() == "5")
                            {
                                str7 = "-165";
                            }
                            if (byvalue.Split(',')[k].ToString() == "6")
                            {
                                str7 = "-198";
                            }
                        }
                        #endregion
                    }

                    string byhtml = string.Empty;
                    #region
                    if (byvalue.Split(',').Length == 1)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 2)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 3)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 4)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 5)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 6)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str6 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }
                    if (byvalue.Split(',').Length == 7)
                    {
                        byhtml = "<tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str1 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str2 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str3 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str4 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str5 + "px; padding: 0pt 0pt 0pt 25px;\"> </td><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str6 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr><tr><td width=\"110\" height=\"30\" background=\"http://groupbuy.7fshop.com/top/groupbuy/images/bz.png\" style=\"background-repeat: no-repeat; background-position: 0pt " + str7 + "px; padding: 0pt 0pt 0pt 25px;\"> </td></tr>";
                    }

                    #endregion
                    str = str.Replace("{baoyou}", byhtml);
                }
                else
                {
                    str = str.Replace("{baoyou}", "");
                }


            }
            //替换数据库模板url
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //讲模板本地图片替换成淘宝url
                str = str.Replace("http://groupbuy.7fshop.com/top/groupbuy/images/" + dt.Rows[i]["url"].ToString(), dt.Rows[i]["taobaourl"].ToString());
            }
            return str;
        }

        /// <summary>
        /// 团购模板HTML替换
        /// </summary>
        /// <param name="str">html</param>
        /// <param name="name">商品名</param>
        /// <param name="price">价格</param>
        /// <param name="proprice">促销价格</param>
        /// <param name="rcount">参团人数</param>
        /// <param name="producturl">宝贝地址</param>
        /// <param name="productimg">图片地址</param>
        /// <param name="id"></param>
        /// <param name="tid">模板名</param>
        /// <returns></returns>
        public string tuanhtmlReplace(string str, string name, string price, string proprice, string rcount, string producturl, string productimg, string id, string tid)
        {

            str = str.Replace("{name}", name);
            str = str.Replace("{oldprice}", price);
            if (proprice != "")
            {
                try
                {
                    str = str.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
                }
                catch
                { }
                try
                {
                    str = str.Replace("{leftprice}", proprice.Split('.')[0]);
                }
                catch { }
                try
                {
                    if (proprice.Split('.')[1].Length < 2)
                    {
                        str = str.Replace("{rightprice}", "00");
                    }
                    else
                    {
                        str = str.Replace("{rightprice}", proprice.Split('.')[1]);
                    }
                }
                catch { }
            }
            str = str.Replace("{newprice}", (decimal.Parse(price) - decimal.Parse(proprice)).ToString());
            str = str.Replace("{buycount}", rcount);
            str = str.Replace("{producturl}", producturl);
            str = str.Replace("{productimg}", productimg);
            str = str.Replace("{id}", id);
            str = str.Replace("'", "''");
            return str;
        }

        /// <summary>
        /// 促销模板HTML替换
        /// </summary>
        /// <param name="smailtempStr">html</param>
        /// <param name="name">商品名</param>
        /// <param name="price">价格</param>
        /// <param name="proprice">促销价格</param>
        /// <param name="rcount">参团人数</param>
        /// <param name="producturl">宝贝地址</param>
        /// <param name="productimg">图片地址</param>
        /// <param name="id"></param>
        /// <param name="tid">模板名</param>
        /// <returns></returns>
        public string cxhtmlReplace(string smailtempStr, string name, string price, string proprice, string rcount, string producturl, string productimg, string id, string tid)
        {
            smailtempStr = smailtempStr.Replace("{name}", name);
            smailtempStr = smailtempStr.Replace("{oldprice}", price);
            if (proprice != "")
            {
                try
                {
                    smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
                }
                catch { }
                try
                {
                    smailtempStr = smailtempStr.Replace("{leftprice}", proprice.Split('.')[0]);
                }
                catch { }
                try
                {
                    if (proprice.Split('.')[1].Length < 2)
                    {
                        smailtempStr = smailtempStr.Replace("{rightprice}", "00");
                    }
                    else
                    {
                        smailtempStr = smailtempStr.Replace("{rightprice}", proprice.Split('.')[1]);
                    }
                }
                catch { }
            }
            smailtempStr = smailtempStr.Replace("{newprice}", (decimal.Parse(price) - decimal.Parse(proprice)).ToString());
            smailtempStr = smailtempStr.Replace("{buycount}", rcount);
            smailtempStr = smailtempStr.Replace("{producturl}", producturl);
            smailtempStr = smailtempStr.Replace("{productimg}", productimg);
            smailtempStr = smailtempStr.Replace("{id}", id);
            smailtempStr = smailtempStr.Replace("'", "''");
            return smailtempStr;
        }



        private void DoMyJob()
        {

            //获取正在进行中的宝贝同步任务        
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            string session = string.Empty;
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

            string sql = "SELECT top 100 t.*, s.sessiongroupbuy FROM Tete_ActivityMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'write'  ORDER BY t.id ASC";
            DBSql db = DBSql.getInstance();
            DataTable dt = db.GetTable(sql);
            DataTable dtWrite = null;
            string styleHtml = string.Empty;
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    session = dt.Rows[i]["sessiongroupbuy"].ToString();
                    sql = "SELECT * FROM Tete_ActivityWriteContent WHERE ActivityMissionID = '" + dt.Rows[i]["id"].ToString() + "' AND isok = 0";

                    dtWrite = db.GetTable(sql);
                    if (dtWrite != null)
                    {
                        for (int j = 0; j < dtWrite.Rows.Count; j++)
                        {
                            styleHtml = CreateGroupbuyHtml(dt.Rows[i]["shoptempletID"].ToString());//生成HTML
                            try
                            {
                                //获取原宝贝描述
                                ItemGetRequest requestItem = new ItemGetRequest();
                                requestItem.Fields = "desc";
                                requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                                Item product = client.ItemGet(requestItem, session);
                                string newContent = string.Empty;
                                string ActivityMissionID = dtWrite.Rows[j]["ActivityMissionID"].ToString();
                                //string tetegroupbuyGuid = ActivityMissionID;
                                string tetegroupbuyGuid = dtWrite.Rows[j]["ActivityID"].ToString();
                                string sqltemp = "SELECT * FROM Tete_ActivityMission WHERE id = '" + ActivityMissionID + "'";
                                DataTable dttemp = db.GetTable(sqltemp);
                                if (dttemp == null)
                                {
                                    return;
                                }

                                //WriteLog("html:" + styleHtml.Length.ToString(), "");
                                if (!Regex.IsMatch(product.Desc, @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>"))
                                {
                                    newContent = @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>" + product.Desc;
                                }
                                else
                                {
                                    newContent = Regex.Replace(product.Desc, @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>", @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>");
                                }
                                //WriteLog("html2:" + newContent.Length.ToString(), "");


                                //更新宝贝描述
                                IDictionary<string, string> param = new Dictionary<string, string>();
                                param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                                param.Add("desc", newContent);

                                string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update ", session, param);
                                //插入宝贝错误日志
                                if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                                {
                                    //WriteLog("更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "", dt.Rows[i]["nick"].ToString());
                                    ////插入宝贝错误日志
                                    //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                                    //utils.ExecuteNonQuery(sql);
                                    //更新宝贝错误数
                                    sql = "UPDATE Tete_ActivityMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                                    db.ExecSql(sql);
                                }
                                else
                                {
                                    WriteLog("itemid:" + dtWrite.Rows[j]["itemid"].ToString() + resultpro, "", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                    //更新状态
                                    sql = "UPDATE Tete_ActivityWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                                    db.ExecSql(sql);

                                    //更新状态
                                    sql = "UPDATE Tete_ActivityMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                                    db.ExecSql(sql);
                                }

                            }
                            catch (Exception e)
                            {
                                WriteLog(e.Message, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                WriteLog(e.StackTrace, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                sql = "UPDATE Tete_ActivityMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                                db.ExecSql(sql);
                                continue;
                            }
                        }

                        dtWrite.Dispose();
                    }
                    sql = "UPDATE Tete_ActivityMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                    db.ExecSql(sql);
                }
                dt.Dispose();
            }
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog(string message, string type, string nick, string mid)
        {

            string tempStr = logUrl + "/activity" + nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/activitypromotion" + mid + "" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //if (type == "1")
            //{
            //    tempFile = tempStr + "/activitypromotionErr" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            //}
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
                    // postData.Append(Uri.EscapeDataString(value));
                    postData.Append(GetUriFormate(value));
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 将参数转换成 uri 格式
        /// </summary>
        /// <param name="inputString">string类型的字符串</param>
        /// <returns>编码后的string</returns>
        private static string GetUriFormate(string inputString)
        {
            StringBuilder strBuilder = new StringBuilder();
            string sourceStr = inputString;
            int len = sourceStr.Length;
            do
            {
                if (len - 21766 <= 0)
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr));
                }
                else
                {
                    strBuilder.Append(Uri.EscapeDataString(sourceStr.Substring(0, 21766)));

                    sourceStr = sourceStr.Substring(21766);
                    len = sourceStr.Length;
                    if (len - 21766 < 0)
                    {
                        strBuilder.Append(Uri.EscapeDataString(sourceStr));
                    }
                }
            }
            while (len - 21766 > 0);

            return strBuilder.ToString();
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


        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog2(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog3(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteLog4(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
 

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <returns></returns>
        public static void WriteDeleteLog(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/DeletepromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErrpromotionID" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

      

        /// <summary>
        /// 更新宝贝详细日志
        /// </summary>
        /// <param name="message">返回结果</param>
        /// <param name="type">成功、错误</param>
        /// <param name="groupbuyID">活动ID</param>
        public static void WriteLog(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/promotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/ErrpromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

       
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="value">日志内容</param>
        /// <param name="type">类型 0(成功日志),1(错误日志) 可传空文本默认为0</param>
        /// <param name="groupbuyID">活动ID</param>
        /// <returns></returns>
        public static void WriteDeleteLog(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/DeletepromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErrpromotionID" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
                sr.WriteLine("Encoding: {0}", sr.Encoding.ToString());

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

       
        #endregion



    }
}
