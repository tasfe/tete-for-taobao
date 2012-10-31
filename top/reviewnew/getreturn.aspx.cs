using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Common;

public partial class top_reviewnew_getreturn : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string guoduid = utils.NewRequest("Content", utils.RequestType.QueryString);
        string status = utils.NewRequest("Status", utils.RequestType.QueryString);

        //UPDATE TCS_MsgSend SET status = '0' WHERE guoduid LIKE '%20121031144351571563%'
        //<?xml version="1.0" encoding="gbk" ?><response><code>03</code><message><desmobile>18858008354</desmobile><msgid>20121026183555199900</msgid></message></response>
        string sql = "UPDATE TCS_MsgSend SET status = '" + status + "' WHERE guoduid = '" + guoduid + "'";
        utils.ExecuteNonQuery(sql);

        File.WriteAllText(Server.MapPath("111url.txt"), Request.Url.ToString());
    }
}