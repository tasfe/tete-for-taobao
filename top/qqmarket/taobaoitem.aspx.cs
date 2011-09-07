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
using PaiPaiAPI;
using System.Text.RegularExpressions;

public partial class top_market_taobaoitem : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");
        string session = cookie.getCookie("top_session");
        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        //获取参数
        string q = utils.NewRequest("query", utils.RequestType.QueryString);
        string page = utils.NewRequest("p", utils.RequestType.QueryString);
        if (page == "")
        {
            page = "1";
        }
        string catid = utils.NewRequest("catid", utils.RequestType.QueryString);
        string act = utils.NewRequest("act", utils.RequestType.QueryString);
        TopXmlRestClient client = new TopXmlRestClient("http://gw.api.taobao.com/router/rest", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");

        int pageSizeNow = 15;

        if (act == "get")
        {
            string strSPID = "29230000ea039296234e9d74d8d3d5b7";
            string strSKEY = "2dsi35b3fdx050a41jufbnzirrlqd9kl";
            string strUIN = taobaoNick;
            string strTOKEN = session;

            ApiClient clientQQ = new ApiClient(strSPID, strSKEY, Convert.ToInt32(strUIN), strTOKEN);
            //通过以下的接口函数添加这些参数 
            clientQQ.addParamInStringField("sellerUin", strUIN);
            clientQQ.addParamInStringField("itemName", q);
            clientQQ.addParamInStringField("pageIndex", page);
            clientQQ.addParamInStringField("pageSize", pageSizeNow.ToString());
            clientQQ.invokeApi("http://api.paipai.com/item/sellerSearchItemList.xhtml?charset=utf-8");

            string result = clientQQ.ToString();

            Regex reg = new Regex(@"""itemCode"":""([^<""\}]*)"",[\s]*""itemName"":""([^<""\}]*)"",", RegexOptions.IgnoreCase);
            MatchCollection match = reg.Matches(result);

            //输出页面HTML
            for (int i = 0; i < match.Count; i++)
            {
                Response.Write("<input type='checkbox' name='items' id='item_" + match[i].Groups[1].ToString() + "' title='" + match[i].Groups[2].ToString() + "' value='" + match[i].Groups[1].ToString() + "' onclick=\"InitArea(this)\"><label for='item_" + match[i].Groups[1].ToString() + "'>" + match[i].Groups[2].ToString() + "</label><br>");
            }
            Response.Write("<br>");
            long totalnum = long.Parse(new Regex(@"<countTotal>([^<]*)</countTotal>", RegexOptions.IgnoreCase).Match(result).Groups[1].ToString());
            long totalPage = (totalnum % pageSizeNow == 0) ? (totalnum / pageSizeNow) : (totalnum / pageSizeNow + 1);
            //输出分页HTML
            for (int i = 1; i <= totalPage; i++)
            {
                if (page == i.ToString())
                {
                    Response.Write(i.ToString() + " ");
                }
                else
                {
                    Response.Write("<a href=\"javascript:spreadStat(" + i.ToString() + ")\">[" + i.ToString() + "]</a> ");
                }
            }
        }
        else if (act == "getCat")
        {
            //记录店铺分类信息
            string strSPID = "29230000ea039296234e9d74d8d3d5b7";
            string strSKEY = "2dsi35b3fdx050a41jufbnzirrlqd9kl";
            string strUIN = taobaoNick;
            string strTOKEN = session;

            ApiClient clientQQ = new ApiClient(strSPID, strSKEY, Convert.ToInt32(strUIN), strTOKEN);
            //通过以下的接口函数添加这些参数 
            clientQQ.addParamInStringField("sellerUin", strUIN);
            clientQQ.invokeApi("http://api.paipai.com/shop/getShopCategoryList.xhtml?charset=utf-8");

            //正则获取分类信息
            Response.Write(clientQQ.ToString());

            //清除老分类
            string sql = "DELETE FROM TopTaobaoShopCatQQ WHERE nick = '" + taobaoNick + "'";
            utils.ExecuteNonQuery(sql);

            ////插入新分类
            //for (int i = 0; i < cat.Content.Count; i++)
            //{
            //    sql = "INSERT INTO TopTaobaoShopCatQQ (" +
            //                    "cid, " +
            //                    "parent_cid, " +
            //                    "name, " +
            //                    "pic_url, " +
            //                    "sort_order, " +
            //                    "created, " +
            //                    "nick, " +
            //                    "modified " +
            //                " ) VALUES ( " +
            //                    " '" + cat.Content[i].Cid + "', " +
            //                    " '" + cat.Content[i].ParentCid + "', " +
            //                    " '" + cat.Content[i].Name + "', " +
            //                    " '" + cat.Content[i].PicUrl + "', " +
            //                    " '" + cat.Content[i].SortOrder + "', " +
            //                    " '" + cat.Content[i].Created + "', " +
            //                    " '" + taobaoNick + "', " +
            //                    " '" + cat.Content[i].Modified + "' " +
            //              ") ";
            //    utils.ExecuteNonQuery(sql);
            //}

            ////输出页面HTML
            //Response.Write("<select id=\"shopcat\" name=\"shopcat\"><option value=\"0\"></option>");
            //for (int i = 0; i < cat.Content.Count; i++)
            //{
            //    Response.Write("<option value='" + cat.Content[i].Cid + "'>" + cat.Content[i].Name + "</option>");
            //}
            //Response.Write("</select>");
        }
    }
}
