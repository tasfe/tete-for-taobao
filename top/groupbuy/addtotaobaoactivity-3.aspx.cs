using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;
using Taobao.Top.Api;
using Taobao.Top.Api.Request;
using Taobao.Top.Api.Domain;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using System.IO;
using System.Text.RegularExpressions;
using System.Text; 
using System.Security.Cryptography;

public partial class top_groupbuy_addtotaobaoactivity_3 : System.Web.UI.Page
{
    public static string logUrl = "D:/groupbuy.7fshop.com/wwwroot/top/groupbuy/ErrLog";
    public string style = string.Empty;
    public string size = string.Empty;
    public string type = string.Empty;
    public string orderby = string.Empty;
    public string query = string.Empty;
    public string shopcat = string.Empty;
    public string name = string.Empty;
    public string items = string.Empty;

    public string nickid = string.Empty;
    public string nickidEncode = string.Empty;
    public string md5nick = string.Empty;
    public string tabletitle = string.Empty;

    public string width = string.Empty;
    public string height = string.Empty;

    public string id = string.Empty;
    public string ads = string.Empty;
    public string myadstemp = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        id = utils.NewRequest("id", Common.utils.RequestType.QueryString);

        if (id != "" && !utils.IsInt32(id))
        {
            Response.Write("非法参数1");
            Response.End();
            return;
        }

        style = utils.NewRequest("style", Common.utils.RequestType.Form);
        size = utils.NewRequest("size", Common.utils.RequestType.Form);
        type = utils.NewRequest("type", Common.utils.RequestType.Form);
        orderby = utils.NewRequest("orderby", Common.utils.RequestType.Form);
        query = utils.NewRequest("query", Common.utils.RequestType.Form);
        shopcat = utils.NewRequest("shopcat", Common.utils.RequestType.Form);
        name = utils.NewRequest("name", Common.utils.RequestType.Form);
        items = utils.NewRequest("items", Common.utils.RequestType.Form);
        ads = utils.NewRequest("ads", Common.utils.RequestType.Form);
        myadstemp = utils.NewRequest("myadstemp", Common.utils.RequestType.Form);

        string missionid = string.Empty;


        int itemcount = 0;

        string act = utils.NewRequest("act", Common.utils.RequestType.Form);

        //创建任务时，判断是否有同类型任务在进行
        if (act == "save" && NoRepeat(id,myadstemp, type))
        {
            //记录该任务
            Response.Write("任务进行");
            missionid = RecordMission();
            Response.Write("任务" + missionid + "进行");
            TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12287381", "d3486dac8198ef01000e7bd4504601a4");
            //提交更新到淘宝商品上去
            if (type != "1")
            {
                for (int j = 1; j <= 500; j++)
                {
                    ItemsOnsaleGetRequest request = new ItemsOnsaleGetRequest();
                    request.Fields = "num_iid,title,price,pic_url";
                    request.PageSize = 200;
                    request.PageNo = j;

                    if (orderby == "new")
                    {
                        request.OrderBy = "list_time:desc";
                    }
                    else if (orderby == "sale")
                    {
                        request.OrderBy = "volume:desc";
                    }

                    if (shopcat != "0")
                    {
                        request.SellerCids = shopcat;
                    }

                    if (query != "0")
                    {
                        request.Q = query;
                    }

                    Cookie cookie = new Cookie();
                    string taobaoNick = cookie.getCookie("nick");
                    string session = cookie.getCookie("top_sessiongroupbuy");
                    PageList<Item> product = client.ItemsOnsaleGet(request, session);

          
                    for (int i = 0; i < product.Content.Count; i++)
                    {
                        RecordMissionDetail(id,myadstemp, product.Content[i].NumIid.ToString(),missionid);
                        itemcount++;
                    }

                    if (product.Content.Count < 200)
                    {
                        break;
                    }
                }
            }
            else
            {
                string[] itemId = items.Split(',');

                for (int i = 0; i < itemId.Length; i++)
                {
                    RecordMissionDetail(id,myadstemp, itemId[i],missionid);
                    itemcount++;
                }
            }


        }


        //更新总数量
        string sql = "UPDATE tete_ActivityMission SET total = '" + itemcount + "' WHERE id = " + missionid;
        utils.ExecuteNonQuery(sql);


        //更新任务
        DoMyJob(missionid);
    }


    /// <summary>
    /// 判断是否有同类型任务进行中
    /// </summary>
    /// <param name="id">团购ID</param>
    /// <param name="mytemp">团购模板ID</param>
    /// <param name="type">任务类型</param>
    /// <returns></returns>
    private bool NoRepeat(string id,string mytemp, string type)
    {
        string sql = "SELECT * FROM Tete_shoptemplet WHERE id = '" + mytemp + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt == null)
        {
            Response.Write("<script>alert('创建任务失败，该活动模板不存在！');window.location.href='activitymissionlist.aspx';</script>");
            Response.End();
            return false;
        }

       
        //根据活动模板ID，检索模板商品列表
        sql = "SELECT * FROM Tete_shoptempletlist WHERE shoptempletID = '" + mytemp + "'";
        dt = utils.ExecuteDataTable(sql);
        if (dt == null || dt.Rows.Count < 1)
        {
            return false;
        }

        sql = "SELECT COUNT(*) FROM Tete_ActivityMission WHERE shoptempletID ="+mytemp+" AND typ='write' AND isok = 0";
        string count = utils.ExecuteString(sql);

        if (count != "0")
        {
            Response.Write("<script>alert('创建任务失败，有同类型的任务正在执行中，请等待其完成后再创建新的任务！');window.location.href='activitymissionlist.aspx';</script>");
            Response.End();
            return false;
        }

        return true;
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

        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt == null || dt.Rows.Count < 1)
        {
            Response.Write("数据为空");
            Response.End();
        }
        string templatehtmlUrl = "tpl/stylenew1.html";//默认模板
        string template2htmlUrl = "tpl/stylenew2-1.html";//第二套模板（一大三小） 小模板  (团购模板第三套和第二套)

        //模板生成需要在好好考虑一下。。。。。。。。。。。。。。。。。。。。。。。。。。。。
        //是多商品团购模板
        if (dt.Rows[0]["templetID"].ToString() == "2")
        {
            //第二套模板（一大三小）
            templatehtmlUrl = "tpl/stylenew2.html";
        }
        //是多商品团购模板
        if (dt.Rows[0]["templetID"].ToString() == "3")
        {
            //第三套模板（一排三列）
            templatehtmlUrl = "tpl/stylenew3.html";
        }


        string html = File.ReadAllText(Server.MapPath(templatehtmlUrl));
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
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                }
            }
            else
            {
                //是多商品团购模板
                if (dt.Rows[i]["templetID"].ToString() == "2" || dt.Rows[i]["templetID"].ToString() == "3")
                {
                    html = File.ReadAllText(Server.MapPath(template2htmlUrl));
                    smailtempStr += html;
                    smailtempStr = cxhtmlReplace(smailtempStr, dt.Rows[i]["name"].ToString(), dt.Rows[i]["price"].ToString(), dt.Rows[i]["proprice"].ToString(), dt.Rows[i]["rcount"].ToString(), dt.Rows[i]["producturl"].ToString(), dt.Rows[i]["productimg"].ToString(), id, dt.Rows[0]["templetID"].ToString());
                }

            }
        }
        str = str.Replace("{productlist}", smailtempStr);//一大三小模板，商品列表替换
        //模板图片替换
        str = tempImgreplace(str, dt.Rows[0]["templetID"].ToString(), btvalue, scbzvalue, lpbzvalue, byvalue);

        return str;
    }


    /// <summary>
    /// 模板图片替换
    /// </summary>
    /// <param name="str">html</param>
    /// <param name="tid">模板ID</param>
    /// <returns></returns>
    public string tempImgreplace(string str, string tid, string btvalue, string scbzvalue, string lpbzvalue, string byvalue)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);
        string sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and templetID=" + tid;
        DataTable dt = utils.ExecuteDataTable(sqlstr);
        if (dt == null && dt.Rows.Count < 1)
        {
            //如果为空返回HTML
            return str;
        }
        sqlstr = "select * from  tete_shoptempletimg where nick='" + taobaoNick + "' and url='" + btvalue + ".png'";
        DataTable dtr2 = utils.ExecuteDataTable(sqlstr);
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
                dtr2 = utils.ExecuteDataTable(sqlstr);
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
                dtr2 = utils.ExecuteDataTable(sqlstr);
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
        str = str.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
        str = str.Replace("{leftprice}", proprice.Split('.')[0]);
        str = str.Replace("{rightprice}", proprice.Split('.')[1]);
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
        smailtempStr = smailtempStr.Replace("{zhekou}", Math.Round(decimal.Parse(proprice) / decimal.Parse(price) * 10, 1).ToString());
        smailtempStr = smailtempStr.Replace("{leftprice}", proprice.Split('.')[0]);
        smailtempStr = smailtempStr.Replace("{rightprice}", proprice.Split('.')[1]);
        smailtempStr = smailtempStr.Replace("{newprice}", (decimal.Parse(price) - decimal.Parse(proprice)).ToString());
        smailtempStr = smailtempStr.Replace("{buycount}", rcount);
        smailtempStr = smailtempStr.Replace("{producturl}", producturl);
        smailtempStr = smailtempStr.Replace("{productimg}", productimg);
        smailtempStr = smailtempStr.Replace("{id}", id);
        smailtempStr = smailtempStr.Replace("'", "''");
        return smailtempStr;
    }

 
    /// <summary>
    /// 记录该任务的详细关联商品
    /// </summary>
    /// <param name="ActivityID">活动ID</param>
    /// <param name="missionid">任务ID</param>
    /// <param name="itemid">宝贝ID</param>
    /// <param name="html">html</param>
    private void RecordMissionDetail(string ActivityID, string shoptempletID, string itemid, string ActivityMissionID)
    {
        string sql = "INSERT INTO tete_ActivityWriteContent (ActivityID, shoptempletID, itemid,ActivityMissionID,Actdate,Isok) VALUES ('" + ActivityID + "', '" + shoptempletID + "', '" + itemid + "'," + ActivityMissionID + ",'" + DateTime.Now.ToString() + "',0)";
   
        utils.ExecuteNonQuery(sql);
    }

    /// <summary>
    /// 记录该任务并返回任务ID
    /// </summary>
    /// <returns></returns>
    private string RecordMission()
    {
        Cookie cookie = new Cookie();
        string missionid = string.Empty;
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_sessiongroupbuy");

        //COOKIE过期判断
        if (taobaoNick == "")
        {
            //SESSION超期 跳转到登录页
            Response.Write("<script>parent.location.href='http://container.open.taobao.com/container?appkey=12287381'</script>");
            Response.End();
        }

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "INSERT INTO Tete_ActivityMission (typ, nick, ActivityID,IsOK,shoptempletID,Success,Fail,Startdate) VALUES ('write','" + taobaoNick + "','" + id + "',0,'" + myadstemp + "',0,0,'"+DateTime.Now.ToString()+"')";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT TOP 1 ID FROM Tete_ActivityMission ORDER BY ID DESC";
        missionid = utils.ExecuteString(sql);

 
        return missionid;
    }

    private string EncodeStr(string[] parmArray)
    {
        string newStr = string.Empty;
        for (int i = 0; i < parmArray.Length; i++)
        {
            if (i == 0)
            {
                newStr = parmArray[i];
            }
            else
            {
                newStr += "|" + parmArray[i];
            }
        }

        Rijndael_ encode = new Rijndael_("tetesoftstr");
        newStr = encode.Encrypt(newStr);
        newStr = HttpUtility.UrlEncode(newStr);
        return newStr;
    }

    /// <summary> 
    /// MD5 加密函数 
    /// </summary> 
    /// <param name="str"></param> 
    /// <param name="code"></param> 
    /// <returns></returns> 
    public static string MD5(string str)
    {
        return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").Substring(8, 16);
    }


    private void DoMyJob(string topMissionID)
    {

        //获取正在进行中的宝贝同步任务        
        string appkey = "12287381";
        string secret = "d3486dac8198ef01000e7bd4504601a4";
        string session = string.Empty;
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", appkey, secret);

        string sql = "SELECT t.*, s.sessiongroupbuy FROM Tete_ActivityMission t INNER JOIN TopTaobaoShop s ON s.nick = t.nick WHERE t.isok = 0 AND t.typ = 'write' AND t.id=" + topMissionID + "  ORDER BY t.id ASC";

        DataTable dt = utils.ExecuteDataTable(sql);
        DataTable dtWrite = null;
        string styleHtml = string.Empty;
        if (dt != null)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                session = dt.Rows[i]["sessiongroupbuy"].ToString();
                sql = "SELECT * FROM Tete_ActivityWriteContent WHERE ActivityMissionID = '" + dt.Rows[i]["id"].ToString() + "' AND isok = 0";

                dtWrite = utils.ExecuteDataTable(sql);
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
                            string tetegroupbuyGuid = ActivityMissionID;
                            string sqltemp = "SELECT * FROM Tete_ActivityMission WHERE id = '" + ActivityMissionID + "'";
                            DataTable dttemp = utils.ExecuteDataTable(sqltemp);
                            if (dttemp == null)
                            {
                                Response.Write("<script>alert('更新宝贝描述失败，该活动模板不存在！');window.location.href='activitymissionlist.aspx';</script>");
                                Response.End();

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
                                utils.ExecuteNonQuery(sql);
                            }
                            else
                            {
                                WriteLog("itemid:" + dtWrite.Rows[j]["itemid"].ToString() + resultpro, "", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                                //更新状态
                                sql = "UPDATE Tete_ActivityWriteContent SET isok = 1 WHERE id = " + dtWrite.Rows[j]["id"].ToString();
                                utils.ExecuteNonQuery(sql);

                                //更新状态
                                sql = "UPDATE Tete_ActivityMission SET success = success + 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                                utils.ExecuteNonQuery(sql);
                            }

                        }
                        catch (Exception e)
                        {
                            WriteLog(e.Message, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                            WriteLog(e.StackTrace, "1", dt.Rows[i]["nick"].ToString(), dtWrite.Rows[j]["ActivityMissionID"].ToString());
                            sql = "UPDATE Tete_ActivityMission SET fail = fail + 1,isok = -1  WHERE id = " + dt.Rows[i]["id"].ToString();
                            utils.ExecuteNonQuery(sql);
                            continue;
                        }
                    }

                    dtWrite.Dispose();
                }
                sql = "UPDATE Tete_ActivityMission SET isok = 1 WHERE id = " + dt.Rows[i]["id"].ToString();
                utils.ExecuteNonQuery(sql);
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
    public static void WriteLog(string message, string type, string nick,string mid)
    {

        string tempStr = logUrl + "/activity" + nick + DateTime.Now.ToString("yyyyMMdd");//文件夹路径
        string tempFile = tempStr + "/activitypromotion"+mid+"" + nick + DateTime.Now.ToString("yyyyMMdd") + ".txt";
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
        MD5 md5 = System.Security.Cryptography.MD5.Create();

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
        System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
        req.Method = "POST";
        req.KeepAlive = true;
        req.Timeout = 300000;
        req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
        byte[] postData = Encoding.UTF8.GetBytes(PostData(param));
        Stream reqStream = req.GetRequestStream();
        reqStream.Write(postData, 0, postData.Length);
        reqStream.Close();
        System.Net.HttpWebResponse rsp = (System.Net.HttpWebResponse)req.GetResponse();
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
}