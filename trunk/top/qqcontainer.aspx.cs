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
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PaiPaiAPI;

public partial class top_qqcontainer : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
	    Response.AddHeader("P3P", "CP=CURa ADMa DEVa PSAo PSDo OUR BUS UNI PUR INT DEM STA PRE COM NAV OTC NOI DSP COR");

        string strSPID = "29230000ea039296234e9d74d8d3d5b7";
        string strSKEY = "2dsi35b3fdx050a41jufbnzirrlqd9kl";
        string strUIN = utils.NewRequest("useruin", utils.RequestType.QueryString);
        string strTOKEN = utils.NewRequest("usertoken", utils.RequestType.QueryString);

        ApiClient client = new ApiClient(strSPID, strSKEY, Convert.ToInt32(strUIN), strTOKEN);
        //通过以下的接口函数添加这些参数 
        client.addParamInStringField("sellerUin", strUIN);

        client.invokeApi("http://api.paipai.com/shop/getShopInfo.xhtml?charset=utf-8");
        string result = client.ToString();

        //记录到本地数据库
        string sql = "INSERT INTO TopPaipaiShop (" +
                        "sellerUin, " +
                        "shopName, " +
                        "logo, " +
                        "regTime, " +
                        "authenticated, " +
                        "mainBusiness, " +
                        "sellerLocation, " +
                        "cityId, " +
                        "sellerLevelCount, " +
                        "guaranteeCompensation, " +
                        "property, " +
                        "itemCountOnSale, " +
                        "goodDescriptionMatch, " +
                        "attitudeOfService, " +
                        "speedOfDelivery, " +
                        "goodEval, " +
                        "normalEval, " +
                        "badEval, " +
                        "sigTencent, " +
                        "sigPaipai " +
                    " ) VALUES ( " +
                        " '" + getValue(result, "sellerUin") + "', " +
                        " '" + getValue(result, "shopName") + "', " +
                        " '" + getValue(result, "logo") + "', " +
                        " '" + getValue(result, "regTime") + "', " +
                        " '" + getValue(result, "authenticated") + "', " +
                        " '" + getValue(result, "mainBusiness") + "', " +
                        " '" + getValue(result, "sellerLocation") + "', " +
                        " '" + getValue(result, "cityId") + "', " +
                        " '" + getValue(result, "sellerLevelCount") + "', " +
                        " '" + getValue(result, "guaranteeCompensation") + "', " +
                        " '" + getValue(result, "property") + "', " +
                        " '" + getValue(result, "itemCountOnSale") + "', " +
                        " '" + getValue(result, "goodDescriptionMatch") + "', " +
                        " '" + getValue(result, "attitudeOfService") + "', " +
                        " '" + getValue(result, "speedOfDelivery") + "', " +
                        " '" + getValue(result, "goodEval") + "', " +
                        " '" + getValue(result, "normalEval") + "', " +
                        " '" + getValue(result, "badEval") + "', " +
                        " '" + getValue(result, "sigTencent") + "', " +
                        " '" + getValue(result, "sigPaipai") + "' " +
                  ") ";

        Response.Write(sql);

        //utils.ExecuteNonQuery(sql);
        //Response.Redirect("qqindex.html");
    }

    private string getValue(string str, string field)
    {
        string value = string.Empty;
        if (Regex.IsMatch(str, @"""" + field + @""":([""]*)([^<]*)([""]*)", RegexOptions.IgnoreCase))
        {
            value = Regex.Match(str, @"""" + field + @""":([""]*)([^<]*)([""]*)", RegexOptions.IgnoreCase).Groups[2].ToString();
            value = value.Replace("'", "''");
        }
        return value;
    }
}
