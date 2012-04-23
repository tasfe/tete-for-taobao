using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

public partial class AddUserMsg : BasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Btn_AddAll_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Tb_Title.Text.Trim()))
        {
            Page.RegisterStartupScript("提示", "<script>alert('请输入标题!');</script>");
            return;
        }

        if (string.IsNullOrEmpty(Tt_Html.Value.Trim()))
        {
            Page.RegisterStartupScript("提示", "<script>alert('请输入信息内容!');</script>");
            return;
        }

        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

        TeteUserTokenService tutDal = new TeteUserTokenService();

        TeteUserMsgService tumDal = new TeteUserMsgService();

        IList<TeteUserTokenInfo> list = tutDal.GetAllTeteUserToken(nick);
        IList<TeteUserMsgInfo> msgList = new List<TeteUserMsgInfo>();

        DateTime now = DateTime.Now;

        foreach (TeteUserTokenInfo info in list)
        {
            TeteUserMsgInfo msg = new TeteUserMsgInfo();
            msg.Adddate = now;
            msg.Html = Tt_Html.Value.Trim();
            msg.Isread = 0;
            msg.Nick = nick;
            msg.Title = Tb_Title.Text.Trim();
            msg.Token = info.Token;

            msgList.Add(msg);
        }

        foreach (TeteUserMsgInfo info in msgList)
        {
            tumDal.AddTeteUserMsg(info);
        }

        Page.RegisterStartupScript("提示", "<script>alert('推送信息成功!');</script>");
    }
}
