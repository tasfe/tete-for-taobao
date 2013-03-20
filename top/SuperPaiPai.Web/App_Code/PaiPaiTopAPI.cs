using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using PaiPaiAPI_2;
using PaiPai.Model;
using System.Text.RegularExpressions;
using DataHelp;

/// <summary>
///从拍拍获取数据
/// </summary>
public class PaiPaiTopAPI
{
    static string strSPID = "29230000ea03ceec2e51cc88c69af1fd";
    static string strSKEY = "azv974zx9mvlvvwj3yl61aej81bn7837";

    static string oauthId = "700104284";
    static string oauthKey = "5xw10WnGZirQP8ZU";

    public static string GetGoodsOrderDetailInfo(string uid, string token, string dealcode)
    {
        OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);

        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("sellerUin", uid);
        dic.Add("charset", "utf-8");
        dic.Add("format", "json");
        dic.Add("dealCode", dealcode);

        string result = client.InvokeOpenApi("http://api.paipai.com/deal/getDealDetail.xhtml", dic, null);
        return result;
    }

    public static IList<GoodsOrderInfo> GetGoodsOrderInfo(string uid, string token)
    {
        OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);
        List<GoodsOrderInfo> list = new List<GoodsOrderInfo>();
        int pageindex = 0;
        while (true)
        {
            pageindex++;
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("sellerUin", uid);
            dic.Add("charset", "utf-8");
            dic.Add("format", "json");
            dic.Add("pageSize", "20");
            dic.Add("pageIndex", pageindex.ToString());

            string result = client.InvokeOpenApi("http://api.paipai.com/deal/sellerSearchDealList.xhtml", dic, null);
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            result = result.Substring(result.IndexOf("\"dealList\":["));
            result = result.Replace("\"dealList\":[", "[");
            result = result.Substring(0, result.Length - 2);
            //Regex regex = new Regex("},\"total_results\":\\d+}}");
            //result = regex.Replace(result, "");
            try
            {
                List<GoodsOrderInfo> newList = js.Deserialize<List<GoodsOrderInfo>>(result);
                if (newList.Count == 0)
                    break;
                list.AddRange(newList);
            }
            catch (Exception ex)
            {
                LogInfo.Add("订单序列化出错", "返回json转化为订单信息集合出错,用户QQ号:" + uid + result + ex.Message);
            }
        }

        return list;
    }

    public static PaiPaiShopInfo GetShopInfo(string uid, string token)
    {
        OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);

        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("sellerUin", uid);
        dic.Add("charset", "utf-8");
        dic.Add("format", "json");
        string result = client.InvokeOpenApi("http://api.paipai.com/shop/getShopInfo.xhtml", dic, null);
        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();

        PaiPaiShopInfo info = null;
        try
        {
            info = js.Deserialize<PaiPaiShopInfo>(result);
        }
        catch (Exception ex)
        {
            LogInfo.Add("店铺信息序列化出错", "返回json转化为店铺信息出错,用户QQ号:" + uid + result + ex.Message);
        }
        return info;
    }
    //public static IList<PaiPaiGoodsInfo> GetGoodsInfo(string uid, string token)
    //{
    //    OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);

    //    List<PaiPaiGoodsInfo> mylist = new List<PaiPaiGoodsInfo>();
    //    int pageindex = 0;
    //    while (true)
    //    {
    //        pageindex++;
    //        Dictionary<string, string> dic = new Dictionary<string, string>();
    //        dic.Add("sellerUin", uid);
    //        dic.Add("charset", "utf-8");
    //        dic.Add("format", "json");
    //        dic.Add("pageSize", "40");
    //        dic.Add("pageIndex", pageindex.ToString());
    //        dic.Add("itemState", "1");

    //        string result = client.InvokeOpenApi("http://api.paipai.com/item/sellerSearchItemList.xhtml", dic, new Dictionary<string, FileItem>());

    //        System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
    //        result = result.Substring(result.IndexOf("\"itemList\":["));
    //        result = result.Replace("\"itemList\":[", "[");
    //        result = result.Substring(0, result.Length - 2);
    //        Regex regex = new Regex("},\"total_results\":\\d+}}");
    //        result = regex.Replace(result, "");
    //        try
    //        {
    //            List<PaiPaiGoodsInfo> newList = js.Deserialize<List<PaiPaiGoodsInfo>>(result);
    //            if (newList.Count == 0)
    //                break;
    //            mylist.AddRange(newList);
    //        }
    //        catch (Exception ex)
    //        {
    //            LogInfo.Add("商品序列化出错", "返回json转化为商品信息集合出错,用户QQ号:" + uid + result + ex.Message);
    //        }
    //    }
    //    return mylist;
    //}

    //public static void GetBuyInfo(string uid, string token)
    //{
    //    OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);

    //    List<PaiPaiGoodsInfo> mylist = new List<PaiPaiGoodsInfo>();
    //    Dictionary<string, string> dic = new Dictionary<string, string>();
    //    dic.Add("userUin", uid);
    //    dic.Add("format", "json");
    //    dic.Add("appId", "229509");
    //    string result = client.InvokeOpenApi("http://api.buy.qq.com/appstore/getSubscribeList.xhtml?charset=gbk", dic, new Dictionary<string, FileItem>());

    //}


    /// <summary>
    /// 拍拍接口暂不能用，何时能用未知
    /// </summary>
    /// <param name="uid"></param>
    /// <param name="token"></param>
    /// <param name="company">物流公司名称</param>
    /// <param name="wuliuCarryId">运单号</param>
    /// <returns></returns>
    public static string GetTrackInfo(string uid, string token, string company, string wuliuCarryId)
    {
        OpenApiOauth client = new OpenApiOauth(oauthId, oauthKey, long.Parse(uid), token);

        Dictionary<string, string> dic = new Dictionary<string, string>();
        dic.Add("sellerUin", uid);
        dic.Add("charset", "utf-8");
        dic.Add("format", "xml");
        dic.Add("company", company);
        dic.Add("wuliuCarryId", wuliuCarryId);

        string result = client.InvokeOpenApi("http://api.paipai.com/wuliu/getTrackInfo.xhtml", dic, null);
        return result;
    }
}
