using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.IO;
using System.Web;
using System.Net;
using System.Security.Cryptography;

namespace teteWriteItem
{

    public partial class Form1 : Form 
    {

        public static string logUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/ErrLog";
        public static string styleUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/style1.html";//默认模板
        public static string styleUrl2 = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew2.html";//第二套模板（一大三小）
        public static string styleUrl2smail = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/stylenew2-1.html";//第二套模板（一大三小） 小模板 (团购模板第三套和第二套)
        public static string styleUrl3 = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/tpl/style3.html";//第三套模板（一排三列）
 
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;

            //更新宝贝详细
            Thread newThread = new Thread(DoMyJob);
            newThread.Start();

            //删除宝贝活动详细
            Thread newThread5 = new Thread(DeleteTaobao);
            newThread5.Start();

            //删除宝贝活动详细
            Thread newThread3 = new Thread(DeleteTaobao2);
            newThread3.Start();

            //删除宝贝活动详细
            Thread newThread4 = new Thread(DeleteTaobao3);
            newThread4.Start();
        }

        /// <summary>
        /// 创建团购HTML
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateGroupbuyHtml(string id)
        {
            DBSql db = DBSql.getInstance();
            string str = string.Empty;
            string templatehtmlUrl = styleUrl;
          
            string sql = "SELECT * FROM TopGroupBuy WHERE id = '" + id + "'";
            DataTable dt = db.GetTable(sql);
            if (dt.Rows[0]["ismuch"].ToString() == "1")
            {
                //是多商品团购模板
                if (dt.Rows[0]["template"].ToString() == "2")
                {
                    //第二套模板（一大三小）
                    templatehtmlUrl = styleUrl2;
                }
                //是多商品团购模板
                if (dt.Rows[0]["template"].ToString() == "3")
                {
                    //第三套模板（一排三列）
                    templatehtmlUrl = styleUrl3;
                }
                if (dt.Rows[0]["groupbuyGuid"].ToString() != "")
                {
                    //根据多商品团购标示，检索商品列表
                    sql = "SELECT * FROM TopGroupBuy WHERE groupbuyGuid = '" + dt.Rows[0]["groupbuyGuid"].ToString() + "'";
                    dt = db.GetTable(sql);
                    if (dt == null || dt.Rows.Count < 1)
                    {
                        return "";
                    }
                }
            }

            string html = File.ReadAllText(templatehtmlUrl); 
            string smailtempStr = string.Empty;//小模板
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i == 0)
                {
                    str = html;
                    str = str.Replace("{name}", dt.Rows[i]["name"].ToString());
                    str = str.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                    str = str.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                    str = str.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                    str = str.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                    str = str.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                    str = str.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                    str = str.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                    str = str.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                    str = str.Replace("{id}", id);
                    str = str.Replace("'", "''");
                }
                else
                {
                    //是多商品团购模板
                    if (dt.Rows[i]["template"].ToString() == "2" || dt.Rows[i]["template"].ToString() == "3")
                    {
                        html = File.ReadAllText(styleUrl2smail);
                        smailtempStr += html;
                        smailtempStr = smailtempStr.Replace("{name}", dt.Rows[i]["name"].ToString());
                        smailtempStr = smailtempStr.Replace("{oldprice}", dt.Rows[i]["productprice"].ToString());
                        smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round((decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())) / decimal.Parse(dt.Rows[i]["productprice"].ToString()) * 10, 1).ToString());
                        smailtempStr = smailtempStr.Replace("{leftprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[0]);
                        smailtempStr = smailtempStr.Replace("{rightprice}", (decimal.Parse(dt.Rows[i]["productprice"].ToString()) - decimal.Parse(dt.Rows[i]["zhekou"].ToString())).ToString().Split('.')[1]);
                        smailtempStr = smailtempStr.Replace("{newprice}", dt.Rows[i]["zhekou"].ToString());
                        smailtempStr = smailtempStr.Replace("{buycount}", dt.Rows[i]["buycount"].ToString());
                        smailtempStr = smailtempStr.Replace("{producturl}", dt.Rows[i]["producturl"].ToString());
                        smailtempStr = smailtempStr.Replace("{productimg}", dt.Rows[i]["productimg"].ToString());
                        smailtempStr = smailtempStr.Replace("{id}", id);
                        smailtempStr = smailtempStr.Replace("'", "''");
                    }

                }
            }
            str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
            return str;
        }
         


        /// <summary>
        /// 更新宝贝活动详细
        /// </summary>
        private void DoMyJob()
        {

            //获取正在进行中的宝贝同步任务        
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            string session = string.Empty;
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

            DBSql db = DBSql.getInstance();
            string sql = "SELECT top 400  t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'write' ORDER BY t.id ASC";
             
            DataTable dt = db.GetTable(sql);
            DataTable dtWrite = null;
            string styleHtml = string.Empty;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                sql = "SELECT * FROM TopWriteContent WHERE missionid = '" + dt.Rows[i]["id"].ToString() + "' AND isok = 0";
                dtWrite = db.GetTable(sql);
                for (int j = 0; j < dtWrite.Rows.Count; j++)
                {
                    styleHtml = CreateGroupbuyHtml(dtWrite.Rows[j]["groupbuyid"].ToString());
                    try
                    {
                        //获取原宝贝描述
                        ItemGetRequest requestItem = new ItemGetRequest();
                        requestItem.Fields = "desc";
                        requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                        Item product = client.ItemGet(requestItem, session);
                        string newContent = string.Empty;
                        string groupid = dtWrite.Rows[j]["groupbuyid"].ToString();
                        string tetegroupbuyGuid = groupid;
                        string sqltemp = "SELECT * FROM TopGroupBuy WHERE id = '" + groupid + "'";
                        DataTable dttemp = db.GetTable(sqltemp);
                        if (dttemp == null)
                        {
                            continue;
                        }
                        //判断团购是多模板
                        if (dttemp.Rows[0]["groupbuyGuid"].ToString() != "")
                        {
                            tetegroupbuyGuid = dttemp.Rows[0]["groupbuyGuid"].ToString();
                        }


                        if (!Regex.IsMatch(product.Desc, @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>"))
                        {
                            newContent = @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>" + product.Desc;
                        }
                        else
                        {
                            newContent = Regex.Replace(product.Desc, @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>([\s\S]*)<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>", @"<div><a name=""tetesoft-area-start-" + tetegroupbuyGuid + @"""></a></div>" + styleHtml + @"<div><a name=""tetesoft-area-end-" + tetegroupbuyGuid + @"""></a></div>");
                        }

                        //更新宝贝描述
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                        param.Add("desc", newContent);
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                        string resultpro2 = resultpro;

                        //插入宝贝错误日志
                        if (resultpro2.ToLower().IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {
                            WriteLog("更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "1", groupid);
                        }
                        else {
                            WriteLog("更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的成功信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "", groupid);  
                        }
                        if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {
                            //WriteLog("更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "");

                            ////插入宝贝错误日志
                            //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                            //db.ExecSql(sql);

                            //更新宝贝错误数
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                        else
                        {
                            //更新状态
                            sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                            db.ExecSql(sql);

                            //更新状态
                            sql = "UPDATE TopMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteLog(e.Message, "1");
                        WriteLog(e.StackTrace, "1");
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                }

                dtWrite.Dispose();

                sql = "UPDATE TopMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                db.ExecSql(sql);
            }

            dt.Dispose();

            WriteLog("**********************************************************", "");
            //休息后继续循环-默认10分钟一次
            Thread.Sleep(90000);

            Thread newThread = new Thread(DoMyJob);
            newThread.Start();

        }

        /// <summary>
        /// 记录该任务的详细关联商品
        /// </summary>
        private void RecordMissionDetail(string groupbuyid, string missionid, string itemid, string html)
        {
            string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html,isok) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '" + html + "',1)";
            DBSql.getInstance().ExecSql(sql);
        }

        /// <summary>
        /// 记录该任务的详细关联商品
        /// </summary>
        private void RecordMissionDetail2(string groupbuyid, string missionid, string itemid, string html)
        {
            string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html,isok) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '" + html + "',1)";
            DBSql.getInstance().ExecSql(sql);
        }

        /// <summary>
        /// 记录该任务的详细关联商品
        /// </summary>
        private void RecordMissionDetail3(string groupbuyid, string missionid, string itemid, string html)
        {
            string sql = "INSERT INTO TopWriteContent (groupbuyid, missionid, itemid, html,isok) VALUES ('" + groupbuyid + "', '" + missionid + "', '" + itemid + "', '" + html + "',1)";
            DBSql.getInstance().ExecSql(sql);
        }

 

        /// <summary>
        /// 清除代码
        /// </summary>
        /// <param name="id"></param>
        public void clearGroupbuy(string id)
        {
            try
            {
                string appkey = "12287381";
                string secret = "d3486dac8198ef01000e7bd4504601a4";
                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE ID=" + id;
                string session = string.Empty;
                #region 取消活动 删除该活动关联的用户群 将该团购标志为已结束
              
                DataTable enddt = db.GetTable(sql);
                //通过接口将该用户加入人群
                for (int y = 0; y < enddt.Rows.Count; y++)
                {
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";
                     
                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    //取消该活动
                    IDictionary<string, string> paramnew = new Dictionary<string, string>();
                    paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                    string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);

                   
                }
            }catch{}

            #endregion
        }


        /// <summary>
        /// 清除代码
        /// </summary>
        /// <param name="id"></param>
        public void clearGroupbuy2(string id)
        {
            try
            {
                string appkey = "12287381";
                string secret = "d3486dac8198ef01000e7bd4504601a4";
                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE ID=" + id;
                string session = string.Empty;
                #region 取消活动 删除该活动关联的用户群 将该团购标志为已结束

                DataTable enddt = db.GetTable(sql);
                //通过接口将该用户加入人群
                for (int y = 0; y < enddt.Rows.Count; y++)
                {
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";

                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    //取消该活动
                    IDictionary<string, string> paramnew = new Dictionary<string, string>();
                    paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                    string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);


                }
            }
            catch { }

                #endregion
        }

        /// <summary>
        /// 清除代码
        /// </summary>
        /// <param name="id"></param>
        public void clearGroupbuy3(string id)
        {
            try
            {
                string appkey = "12287381";
                string secret = "d3486dac8198ef01000e7bd4504601a4";
                DBSql db = DBSql.getInstance();
                string sql = "SELECT * FROM TopGroupBuy WHERE ID=" + id;
                string session = string.Empty;
                #region 取消活动 删除该活动关联的用户群 将该团购标志为已结束

                DataTable enddt = db.GetTable(sql);
                //通过接口将该用户加入人群
                for (int y = 0; y < enddt.Rows.Count; y++)
                {
                    sql = "SELECT session FROM TopTaobaoShop WHERE nick = '" + enddt.Rows[y]["nick"].ToString() + "'";

                    DataTable dtnick = db.GetTable(sql);
                    if (dtnick.Rows.Count != 0)
                    {
                        session = db.GetTable(sql).Rows[0][0].ToString();
                    }
                    //取消该活动
                    IDictionary<string, string> paramnew = new Dictionary<string, string>();
                    paramnew.Add("promotion_id", enddt.Rows[y]["promotionid"].ToString());
                    string resultnew = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.marketing.promotion.delete", session, paramnew);


                }
            }
            catch { }

                #endregion
        }
        /// <summary>
        /// 删除宝贝活动详细
        /// </summary>
        private void DeleteTaobao()
        {
            //try
            //{
            //获取正在进行中的宝贝同步任务        
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            string session = string.Empty;
            string id = string.Empty;
            string missionid = string.Empty;
            string html = string.Empty;
            string shopcat = "0";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

            DBSql db = DBSql.getInstance();
            string sql = "SELECT  top 300  t.*, s.sessiongroupbuy,s.sid FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'delete' and  s.sid%3=0 ORDER BY t.id ASC";

            DataTable dt = db.GetTable(sql);
            DataTable dtWrite = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                id = dt.Rows[i]["groupbuyid"].ToString();
                clearGroupbuy(id);
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                missionid = dt.Rows[i]["id"].ToString();
                html = "";

                sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                dtWrite = db.GetTable(sql); 
                if (dtWrite == null || dtWrite.Rows.Count < 1)
                {
                    for (int j = 1; j <= 500; j++)
                    {
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 200;
                        request.PageNo = j;
 
                        Cookie cookie = new Cookie();
                        string taobaoNick = dt.Rows[i]["nick"].ToString();
                        try
                        {
                            PageList<Item> product = client.ItemsOnsaleGet(request, session);

                            WriteDeleteLog(taobaoNick+"INGCount：" + product.Content.Count.ToString(), "1");
                            for (int num = 0; num < product.Content.Count; num++)
                            {
                                RecordMissionDetail(id, missionid, product.Content[num].NumIid.ToString(), html);
                            }

                            if (product.Content.Count < 200)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            WriteDeleteLog(e.StackTrace, "1");
                            WriteDeleteLog(e.Message, "1");
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            break;
                        }
                    }

                    sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                    dtWrite = db.GetTable(sql);
                }
                WriteDeleteLog("ING：" + dtWrite.Rows.Count.ToString(), "1");
                for (int j = 0; j < dtWrite.Rows.Count; j++)
                {
                    try
                    {
                        //获取原宝贝描述
                        ItemGetRequest requestItem = new ItemGetRequest();
                        requestItem.Fields = "desc";
                        requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                        Item product = client.ItemGet(requestItem, session);
                        string newContent2 = string.Empty;
                        string groupid = dt.Rows[i]["groupbuyid"].ToString();
                        string tetegroupbuyGuid = groupid;
                        string sqltemp = "SELECT * FROM TopGroupBuy WHERE id = '" + groupid + "'";
                        DataTable dttemp = db.GetTable(sqltemp);
                        if (dttemp == null)
                        {
                            continue;

                        }
                        //判断团购是多模板
                        if (dttemp.Rows[0]["groupbuyGuid"].ToString() != "")
                        {
                            tetegroupbuyGuid = dttemp.Rows[0]["groupbuyGuid"].ToString();
                        }

                        if (!Regex.IsMatch(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>"))
                        {
                            //更新状态

                            WriteDeleteLog("http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码", "");
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            continue;
                        }
                        else
                        {

                            newContent2 = Regex.Replace(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>", @"");
                        
                        }

                        //更新宝贝描述
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                        param.Add("desc", newContent2);
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);


                        //插入宝贝错误日志
                        if (resultpro.ToLower().IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {

                            WriteDeleteLog("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>",""), "1", groupid);              
                        }
                        else
                        {
                            WriteDeleteLog("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的成功信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "", groupid);
                        }

                        if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {
                            //WriteDeleteLog("删除宝贝更新宝贝描述结果：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "");

                            ////插入宝贝错误日志
                            //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                            //db.ExecSql(sql);

                            //更新宝贝错误数
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                        else
                        {

                            //更新状态
                            sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["itemid"].ToString();
                            db.ExecSql(sql);


                            //更新状态
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteDeleteLog("删除宝贝" + e.StackTrace, "1");
                        WriteDeleteLog("删除宝贝" + e.Message, "1");
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                }
                dtWrite.Dispose();


            }
            dt.Dispose();


            WriteDeleteLog("**********************************************************", "");
            //休息后继续循环-默认10分钟一次
            Thread.Sleep(91000);

            Thread newThread = new Thread(DeleteTaobao);
            newThread.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("\r\n" + e.StackTrace);
            //}

            return;
        }

        /// <summary>
        /// 删除宝贝2活动详细
        /// </summary>
        private void DeleteTaobao2()
        {
            //try
            //{
            //获取正在进行中的宝贝同步任务        
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            string session = string.Empty;
            string id = string.Empty;
            string missionid = string.Empty;
            string html = string.Empty;
            string shopcat = "0";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

            DBSql db = DBSql.getInstance();
            string sql = "SELECT  top 300   t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'delete' and  s.sid%3=1 ORDER BY t.id ASC";

            DataTable dt = db.GetTable(sql);
            DataTable dtWrite = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                id = dt.Rows[i]["groupbuyid"].ToString();
                clearGroupbuy2(id);
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                missionid = dt.Rows[i]["id"].ToString();
                html = "";

                sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                dtWrite = db.GetTable(sql);
                if (dtWrite == null || dtWrite.Rows.Count < 1)
                {
                    for (int j = 1; j <= 500; j++)
                    {
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 200;
                        request.PageNo = j;

                        Cookie cookie = new Cookie();
                        string taobaoNick = dt.Rows[i]["nick"].ToString();
                        try
                        {
                            PageList<Item> product = client.ItemsOnsaleGet(request, session);

                            WriteDeleteLog2(taobaoNick + "INGCount：" + product.Content.Count.ToString(), "1");
                            for (int num = 0; num < product.Content.Count; num++)
                            {
                                RecordMissionDetail2(id, missionid, product.Content[num].NumIid.ToString(), html);
                            }

                            if (product.Content.Count < 200)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            WriteDeleteLog2(e.StackTrace, "1");
                            WriteDeleteLog2(e.Message, "1");
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            break;
                        }
                    }

                    sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                    dtWrite = db.GetTable(sql);
                }
                WriteDeleteLog2("ING：" + dtWrite.Rows.Count.ToString(), "1");
                for (int j = 0; j < dtWrite.Rows.Count; j++)
                {
                    try
                    {
                        //获取原宝贝描述
                        ItemGetRequest requestItem = new ItemGetRequest();
                        requestItem.Fields = "desc";
                        requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                        Item product = client.ItemGet(requestItem, session);
                        string newContent2 = string.Empty;
                        string groupid = dt.Rows[i]["groupbuyid"].ToString();
                        string tetegroupbuyGuid = groupid;
                        string sqltemp = "SELECT * FROM TopGroupBuy WHERE id = '" + groupid + "'";
                        DataTable dttemp = db.GetTable(sqltemp);
                        if (dttemp == null)
                        {
                            continue;

                        }
                        //判断团购是多模板
                        if (dttemp.Rows[0]["groupbuyGuid"].ToString() != "")
                        {
                            tetegroupbuyGuid = dttemp.Rows[0]["groupbuyGuid"].ToString();
                        }

                        if (!Regex.IsMatch(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>"))
                        {
                            //更新状态
                            WriteDeleteLog2("http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码", "");
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            continue;
                        }
                        else
                        {
                            newContent2 = Regex.Replace(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>", @"");

                        }

                        //更新宝贝描述
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                        param.Add("desc", newContent2);
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                        //插入宝贝错误日志
                        if (resultpro.ToLower().IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {

                            WriteDeleteLog2("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "1", groupid);
                        }
                        else
                        {
                            WriteDeleteLog2("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的成功信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "", groupid);
                        }

                        if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {
                            //WriteDeleteLog("删除宝贝更新宝贝描述结果：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "");

                            ////插入宝贝错误日志
                            //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                            //db.ExecSql(sql);

                            //更新宝贝错误数
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                        else
                        {

                            //更新状态
                            sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["itemid"].ToString();
                            db.ExecSql(sql);


                            //更新状态
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteDeleteLog2("删除宝贝" + e.StackTrace, "1");
                        WriteDeleteLog2("删除宝贝" + e.Message, "1");
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                }
                dtWrite.Dispose();


            }
            dt.Dispose();


            WriteDeleteLog2("**********************************************************", "");
            //休息后继续循环-默认10分钟一次
            Thread.Sleep(92000);

            Thread newThread2 = new Thread(DeleteTaobao2);
            newThread2.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("\r\n" + e.StackTrace);
            //}

            return;
        }

        /// <summary>
        /// 删除宝贝3活动详细
        /// </summary>
        private void DeleteTaobao3()
        {
            //try
            //{
            //获取正在进行中的宝贝同步任务        
            string appkey = "12287381";
            string secret = "d3486dac8198ef01000e7bd4504601a4";
            string session = string.Empty;
            string id = string.Empty;
            string missionid = string.Empty;
            string html = string.Empty;
            string shopcat = "0";
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

            DBSql db = DBSql.getInstance();
            string sql = "SELECT  top 300   t.*, s.sessiongroupbuy FROM TopMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'delete' and  s.sid%3=2 ORDER BY t.id ASC";

            DataTable dt = db.GetTable(sql);
            DataTable dtWrite = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                id = dt.Rows[i]["groupbuyid"].ToString();
                clearGroupbuy3(id);
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                missionid = dt.Rows[i]["id"].ToString();
                html = "";

                sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                dtWrite = db.GetTable(sql);
                if (dtWrite == null || dtWrite.Rows.Count < 1)
                {
                    for (int j = 1; j <= 500; j++)
                    {
                        ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                        request.Fields = "num_iid,title,price,pic_url";
                        request.PageSize = 200;
                        request.PageNo = j;

                        Cookie cookie = new Cookie();
                        string taobaoNick = dt.Rows[i]["nick"].ToString();
                        try
                        {
                            PageList<Item> product = client.ItemsOnsaleGet(request, session);

                            WriteDeleteLog3(taobaoNick + "INGCount：" + product.Content.Count.ToString(), "1");
                            for (int num = 0; num < product.Content.Count; num++)
                            {
                                RecordMissionDetail3(id, missionid, product.Content[num].NumIid.ToString(), html);
                            }

                            if (product.Content.Count < 200)
                            {
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            WriteDeleteLog3(e.StackTrace, "1");
                            WriteDeleteLog3(e.Message, "1");
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            break;
                        }
                    }

                    sql = "SELECT DISTINCT itemid FROM TopWriteContent WHERE groupbuyid = '" + dt.Rows[i]["groupbuyid"].ToString() + "' AND isok = 1";
                    dtWrite = db.GetTable(sql);
                }
                WriteDeleteLog3("ING：" + dtWrite.Rows.Count.ToString(), "1");
                for (int j = 0; j < dtWrite.Rows.Count; j++)
                {
                    try
                    {
                        //获取原宝贝描述
                        ItemGetRequest requestItem = new ItemGetRequest();
                        requestItem.Fields = "desc";
                        requestItem.NumIid = long.Parse(dtWrite.Rows[j]["itemid"].ToString());
                        Item product = client.ItemGet(requestItem, session);
                        string newContent2 = string.Empty;
                        string groupid = dt.Rows[i]["groupbuyid"].ToString();
                        string tetegroupbuyGuid = groupid;
                        string sqltemp = "SELECT * FROM TopGroupBuy WHERE id = '" + groupid + "'";
                        DataTable dttemp = db.GetTable(sqltemp);
                        if (dttemp == null)
                        {
                            continue;

                        }
                        //判断团购是多模板
                        if (dttemp.Rows[0]["groupbuyGuid"].ToString() != "")
                        {
                            tetegroupbuyGuid = dttemp.Rows[0]["groupbuyGuid"].ToString();
                        }

                        if (!Regex.IsMatch(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>"))
                        {
                            //更新状态

                            WriteDeleteLog3("http://item.taobao.com/item.htm?id=" + dtWrite.Rows[j]["itemid"].ToString() + " 不含需要清除的代码", "");
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                            continue;
                        }
                        else
                        {

                            newContent2 = Regex.Replace(product.Desc, @"<div>[\s]*<a name=""tetesoft-area-start-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>[\s]*([\s\S]*)<div>[\s]*<a name=""tetesoft-area-end-" + tetegroupbuyGuid + @""">[\s]*</a>[\s]*</div>", @"");

                        }

                        //更新宝贝描述
                        IDictionary<string, string> param = new Dictionary<string, string>();
                        param.Add("num_iid", dtWrite.Rows[j]["itemid"].ToString());
                        param.Add("desc", newContent2);
                        string resultpro = Post("http://gw.api.taobao.com/router/rest", appkey, secret, "taobao.item.update", session, param);
                        //插入宝贝错误日志
                        if (resultpro.ToLower().IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {

                            WriteDeleteLog3("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "1", groupid);
                        }
                        else
                        {
                            WriteDeleteLog3("删除活动更新宝贝描述：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的成功信息" + resultpro.Replace("<?xml version=\"1.0\" encoding=\"utf-8\" ?>", ""), "", groupid);
                        }

                        if (resultpro.IndexOf("ITEM_PROPERTIES_ERROR") != -1)
                        {
                            //WriteDeleteLog("删除宝贝更新宝贝描述结果：宝贝ID：" + dtWrite.Rows[j]["itemid"].ToString() + "返回的错误信息" + resultpro, "");

                            ////插入宝贝错误日志
                            //sql = "insert TopMissionErrDetail (TopMissionID,itemid,nick,ErrDetail) values('" + dt.Rows[i]["id"].ToString() + "','" + dtWrite.Rows[j]["itemid"].ToString() + "','" + dt.Rows[i]["nick"].ToString() + "','" + resultpro + "')";
                            //db.ExecSql(sql);

                            //更新宝贝错误数
                            sql = "UPDATE TopMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                        else
                        {

                            //更新状态
                            sql = "UPDATE TopWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["itemid"].ToString();
                            db.ExecSql(sql);


                            //更新状态
                            sql = "UPDATE TopMission SET success = success + 1,isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                            db.ExecSql(sql);
                        }
                    }
                    catch (Exception e)
                    {
                        WriteDeleteLog3("删除宝贝" + e.StackTrace, "1");
                        WriteDeleteLog3("删除宝贝" + e.Message, "1");
                        sql = "UPDATE TopMission SET fail = fail + 1,isok = -1 WHERE id = " + dt.Rows[i]["id"].ToString();
                        db.ExecSql(sql);
                        continue;
                    }
                }
                dtWrite.Dispose();


            }
            dt.Dispose();


            WriteDeleteLog3("**********************************************************", "");
            //休息后继续循环-默认10分钟一次
            Thread.Sleep(93000);

            Thread newThread3 = new Thread(DeleteTaobao3);
            newThread3.Start();
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("\r\n" + e.StackTrace);
            //}

            return;
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
            string tempFile = tempStr + "/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/2" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err2" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/3" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err3" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/Delete" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        public static void WriteDeleteLog2(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Delete2" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr2" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        public static void WriteDeleteLog3(string message, string type)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Delete3" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr3" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        public static void WriteLog2(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/2" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err2" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        public static void WriteLog3(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/3" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/Err3" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
            string tempFile = tempStr + "/Delete" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        public static void WriteDeleteLog2(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Delete2" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr2" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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

        public static void WriteDeleteLog3(string message, string type, string groupbuyID)
        {
            string tempStr = logUrl + "/Write" + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
            string tempFile = tempStr + "/Delete3" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            if (type == "1")
            {
                tempFile = tempStr + "/DeleteErr3" + groupbuyID + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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