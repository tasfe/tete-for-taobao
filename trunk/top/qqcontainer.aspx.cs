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

        Response.Write(client.ToString());
    }
}
