using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CusServiceAchievements.DAL;
using System.Collections.Generic;
using TaoBaoAPIHelper;

public partial class AllTalkContent : System.Web.UI.Page
{

    TalkRecodService talkDal = new TalkRecodService();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //TaoBaoAPIHelper.TaoBaoAPI.GetChildNick("美杜莎之心", "6102b061e6fe4c1b437274d442350197c9fb5846db06ca8204200856");
            DateTime[] dateArr = DBHelp.DataHelper.GetDateTime(DateTime.Now, 1);
            string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);

            ViewState["start"] = dateArr[0];
            ViewState["end"] = dateArr[1];
            //ViewState["nick"] = nick;

            BindUser(nick, dateArr[0], dateArr[1]);
            BindCustomer(nick, dateArr[0], dateArr[1]);
        }
    }

    private void BindUser(string nick, DateTime start, DateTime end)
    {
        List<TalkContent> list = talkDal.GetTalkUser(start, end, nick, Enum.TalkObjType.All);
        Rp_KefuList.DataSource = list;
        Rp_KefuList.DataBind();
    }

    private void BindCustomer(string nick, DateTime start, DateTime end)
    {
        List<TalkContent> list = talkDal.GetTalkCustomer(start, end, nick, Enum.TalkObjType.All);
        Rpt_CustomerList.DataSource = list;
        Rpt_CustomerList.DataBind();
    }

    private void BindTalkContent(string fromNick, string toNick, DateTime start, DateTime end)
    {
        List<TalkContent> list = talkDal.GetAllContentByFromId(fromNick, toNick, start, end);
        Rpt_TalkList.DataSource = list;
        Rpt_TalkList.DataBind();
    }

    protected void ShowCus(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        string fnick = lb.Text;

        List<TalkContent> list = talkDal.GetTalkCustomer(DateTime.Parse(ViewState["start"].ToString()), DateTime.Parse(ViewState["end"].ToString()), fnick, Enum.TalkObjType.All);
        Rpt_CustomerList.DataSource = list;
        Rpt_CustomerList.DataBind();
        Hf_User.Value = fnick;
    }

    protected void ShowTalk(object sender, EventArgs e)
    {
        LinkButton lb = (LinkButton)sender;

        string tnick = lb.Text;

        List<TalkContent> list = talkDal.GetAllContentByFromId(Hf_User.Value, tnick, DateTime.Parse(ViewState["start"].ToString()), DateTime.Parse(ViewState["end"].ToString()));
        Rpt_TalkList.DataSource = list;
        Rpt_TalkList.DataBind();
    }

    protected string GetNick(string dire, string fnic, string tnick)
    {
        if (dire == "1")
            return tnick;
        return fnic;
    }

}
