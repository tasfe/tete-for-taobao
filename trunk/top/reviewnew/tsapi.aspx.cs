using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_reviewnew_tsapi : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = utils.NewRequest("act", utils.RequestType.QueryString);

        switch (act)
        {
            case "giftmsg":
                OutGiftMsg("gift");
                break;
            case "actmsg":
                OutGiftMsg("act");
                break;
            case "alipaymsg":
                OutGiftMsg("alipay");
                break;
            case "cuimsg":
                OutGiftMsg("cui");
                break;
            case "fahuomsg":
                OutGiftMsg("fahuo");
                break;
            case "reviewmsg":
                OutGiftMsg("review");
                break;
            case "shippingmsg":
                OutGiftMsg("shipping");
                break;
            case "testmsg":
                OutGiftMsg("test");
                break;
            case "taobaomsg":
                OutTaobaoMsg();
                break;
        }
    }

    private void OutTaobaoMsg()
    {
        string sql = string.Empty;
        string str = string.Empty;
        
        sql = "SELECT COUNT(*) FROM TCS_TaobaoMsgLog WHERE typ = 'TradeSellerShip' AND isok = 0";
        str = utils.ExecuteString(sql);

        sql = "SELECT COUNT(*) FROM TCS_TaobaoMsgLog WHERE typ = 'TradeCreate' AND isok = 0";
        str += ","+utils.ExecuteString(sql);

        sql = "SELECT COUNT(*) FROM TCS_TaobaoMsgLog WHERE typ = 'TradeRated' AND isok = 0";
        str += "," + utils.ExecuteString(sql);

        sql = "SELECT COUNT(*) FROM TCS_TaobaoMsgLog WHERE typ = 'TradeBuyerPay' AND isok = 0";
        str += "," + utils.ExecuteString(sql);

        Response.Write(str);
        Response.End();
    }

    private void OutGiftMsg(string typ)
    {
        string str = string.Empty;
        string sql = "SELECT COUNT(*) FROM TCS_MsgSend WHERE typ='" + typ + "' AND DATEDIFF(D,adddate,GETDATE()) = 0";
        str = utils.ExecuteString(sql);

        sql = "SELECT TOP 1 * FROM TCS_MsgSend WHERE typ='" + typ + "' AND DATEDIFF(D,adddate,GETDATE()) = 0 ORDER BY adddate DESC";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            str += "|" + dt.Rows[0]["adddate"].ToString() + "|" + dt.Rows[0]["content"].ToString();
        }
        else
        {
            str += "||";
        }

        Response.Write(str);
        Response.End();
    }
}