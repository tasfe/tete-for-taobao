using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;
using JdSoft.Apple.Apns.Notifications;

public partial class iphoneapi_msgcheck : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string ispass = utils.NewRequest("ispass", utils.RequestType.QueryString);

        string sql = "SELECT * FROM HuliUserMsg WHERE ispass = 0 ORDER BY adddate DESC";

        if (ispass == "0")
            sql = "SELECT * FROM HuliUserMsg WHERE ispass <> 0 ORDER BY adddate DESC";

        DataTable dt = utils.ExecuteDataTable(sql);

        rptList.DataSource = dt;
        rptList.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string ids = utils.NewRequest("ids", utils.RequestType.Form);

        string sql = "UPDATE HuliUserMsg SET ispass = 1 WHERE CHARINDEX(guid, '"+ids+"') > 0";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT DISTINCT alerttoken FROM TeteUserToken WHERE token IN (SELECT DISTINCT token FROM HuliUserMsg WHERE CHARINDEX(guid, '" + ids + "') > 0 AND token IS NOT NULL) AND LEN(alerttoken) = 64";
        DataTable dt = utils.ExecuteDataTable(sql);

        string filePath = Server.MapPath("p12/定时短信证书.p12");
        string pass = "3561402";

        SendAlert(dt, "亲，您的短信审核已经通过，进入等待发送队列中！", filePath, pass);

        Response.Redirect("msgcheck.aspx");
    }

    private void SendAlert(DataTable dt, string msg, string file, string pass)
    {
        bool sandbox = true;
        string p12File = file;
        string p12FilePassword = pass;

        string p12Filename = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p12File);

        NotificationService service = new NotificationService(sandbox, p12Filename, p12FilePassword, 1);

        service.SendRetries = 5; //5 retries before generating notificationfailed event
        service.ReconnectDelay = 2000; //5 seconds

        //The notifications will be sent like this:
        //		Testing: 1...
        //		Testing: 2...
        //		Testing: 3...
        // etc...
        for (int i = 0; i <= dt.Rows.Count; i++)
        {
            //Create a new notification to send
            Notification alertNotification = new Notification(dt.Rows[i]["alerttoken"].ToString());

            alertNotification.Payload.Alert.Body = string.Format(msg, i);
            alertNotification.Payload.Sound = "default";
            alertNotification.Payload.Badge = i;

            service.QueueNotification(alertNotification);
        }

        service.Close();

        //Clean up
        service.Dispose();
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        string ids = utils.NewRequest("ids", utils.RequestType.Form);

        string sql = "UPDATE HuliUserMsg SET ispass = 2 WHERE CHARINDEX(guid, '" + ids + "') > 0";
        utils.ExecuteNonQuery(sql);

        sql = "SELECT DISTINCT token FROM HuliUserMsg WHERE CHARINDEX(guid, '" + ids + "') > 0 AND token IS NOT NULL";
        DataTable dt = utils.ExecuteDataTable(sql);

        string filePath = Server.MapPath("p12/定时短信证书.p12");
        string pass = "3561402";

        SendAlert(dt, "亲，您的短信含有非法信息，审核不通过无法正常发送！", filePath, pass);

        Response.Redirect("msgcheck.aspx");
    }

    public static string check(string str)
    {
        if (str == "0")
        {
            return "等待审核";
        }
        else if (str == "1")
        {
            return "<span style=color:green>审核通过<span>";
        }
        else
        {
            return "<span style=color:red>审核不通过<span>";
        }
    }
}