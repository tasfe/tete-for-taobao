using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using System.IO;

public partial class top_containerblogfree : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;
    public string versionNo = string.Empty;
    public string isFirst = string.Empty;
    public string sendMsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //签名验证
        string top_appkey = "12690738";
        string top_parameters = utils.NewRequest("top_parameters", utils.RequestType.QueryString).Replace(" ", "+");
        top_session = utils.NewRequest("top_session", utils.RequestType.QueryString).Replace(" ", "+");
        string app_secret = "66d488555b01f7b85f93d33bc2a1c001";
        string top_sign = utils.NewRequest("top_sign", utils.RequestType.QueryString).Replace(" ", "+"); //字符串中的+在获取后会被替换成空格，要再替换回来
        string sign = utils.NewRequest("sign", utils.RequestType.QueryString).Replace(" ", "+");

        versionNo = utils.NewRequest("versionNo", utils.RequestType.QueryString);
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取,也可以固定 
        string timestamp = utils.NewRequest("timestamp", utils.RequestType.QueryString).Replace(" ", "+"); //可以从 QueryString 来获取 
        string agreementsign = utils.NewRequest("agreementsign", utils.RequestType.QueryString).Replace(" ", "+");


        if (!Taobao.Top.Api.Util.TopUtils.VerifyTopResponse(top_parameters, top_session, top_sign, top_appkey, app_secret))
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        nick = Taobao.Top.Api.Util.TopUtils.DecodeTopParams(top_parameters)["visitor_nick"];
        if (nick == null || nick == "")
        {
            Response.Write("top签名验证不通过，请不要非法注入");
            Response.End();
            return;
        }

        ////加密NICK
        //Rijndael_ encode = new Rijndael_("tetesoft");
        //nick = encode.Encrypt(nick);

        //Cookie cookie = new Cookie();
        //cookie.setCookie("top_session", top_session, 999999);
        //cookie.setCookie("top_sessiongroupbuy", top_session, 999999);
        //cookie.setCookie("top_sessionblog", top_session, 999999);
        //cookie.setCookie("nick", nick, 999999);

        //Response.Redirect("indexnew1.html");
        //return;
        File.WriteAllText(Server.MapPath("customer/" + nick + ".txt"), Request.Url.ToString());

        if (nick == "langbow旗舰店")
        {
            Response.Redirect("http://iphone.tetesoft.com/api/container.aspx?nick=" + HttpUtility.UrlEncode(nick) + "&session=" + Session + "&short=langbow");
            return;
        }

        //判断跳转，判断客户是否订购了好评有礼
        string sql = "SELECT * FROM TCS_ShopSession WHERE nick = '" + nick + "' AND version > 1";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            //获取SESSION
            string session = dt.Rows[0]["session"].ToString();
            if (session.Length != 0)
            {
                //加密NICK
                Rijndael_ encode = new Rijndael_("tetesoft");
                nick = encode.Encrypt(nick);

                Cookie cookie = new Cookie();
                cookie.setCookie("top_session", session, 999999);
                cookie.setCookie("top_sessiongroupbuy", session, 999999);
                cookie.setCookie("top_sessionblog", session, 999999);
                cookie.setCookie("nick", nick, 999999);

                Response.Redirect("indexnew.html");
            }
            else
            {
                ShowErrPage();
                return;
            }
        }
        else
        {
            ShowErrPage();
            return;
        }
    }

    private void ShowErrPage()
    {
        //加密NICK
        Rijndael_ encode = new Rijndael_("tetesoft");
        nick = encode.Encrypt(nick);

        Cookie cookie = new Cookie();
        cookie.setCookie("nick", nick, 999999);

        Response.Write(" <b style='font-size:14px;'> 亲，您好！本服务用于：<span style='color:red'>好评有礼-会员营销 的客户做前台展示，没有订购 <a href='http://fuwu.taobao.com/serv/detail.htm?service_id=4545&from=xiuxiu'>好评有礼-会员营销</a> 则不可使用本服务</span>，给您带来的不便深感抱歉！ <br> <a href='reviewnew/html1.aspx?session=" + top_session + "' target=_blank>过期用户清除宝贝描述中好评图片请点这里</a><br> <a href='http://groupbuy.7fshop.com/top/groupbuy/deletegroupbuy.aspx?session=" + top_session + "&nick=" + HttpUtility.UrlEncode(nick) + "' target=_blank>过期用户清除团购宝贝描述中图片请点这里</a> <br></b>");
        // <a href='reviewnew/html1.aspx?session=" + top_session + "'>清除宝贝描述中好评图片请点这里</a>
    }
}