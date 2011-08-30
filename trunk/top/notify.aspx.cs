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
using System.IO;

public partial class top_notify : System.Web.UI.Page
{
    public string top_session = string.Empty;
    public string nick = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string userId = utils.NewRequest("userId", utils.RequestType.Form);
        string nick = utils.NewRequest("nick", utils.RequestType.Form);
        string leaseId = utils.NewRequest("leaseId", utils.RequestType.Form);
        string validateDate = utils.NewRequest("validateDate", utils.RequestType.Form);
        string invalidateDate = utils.NewRequest("invalidateDate", utils.RequestType.Form);
        string factMoney = utils.NewRequest("factMoney", utils.RequestType.Form);
        string subscType = utils.NewRequest("subscType", utils.RequestType.Form);
        string versionNo = utils.NewRequest("versionNo", utils.RequestType.Form);
        string oldVersionNo = utils.NewRequest("oldVersionNo", utils.RequestType.Form);
        string status = utils.NewRequest("status", utils.RequestType.Form);
        string sign = utils.NewRequest("sign", utils.RequestType.Form);
        string gmtCreateDate = utils.NewRequest("gmtCreateDate", utils.RequestType.Form);
        string tadgetCode = utils.NewRequest("tadgetCode", utils.RequestType.Form);


        //记录到本地数据库
        string sql = "INSERT INTO TopNotify (" +
                        "userId, " +
                        "nick, " +
                        "leaseId, " +
                        "validateDate, " +
                        "invalidateDate, " +
                        "factMoney, " +
                        "subscType, " +
                        "versionNo, " +
                        "oldVersionNo, " +
                        "status, " +
                        "sign, " +
                        "gmtCreateDate, " +
                        "tadgetCode " +
                    " ) VALUES ( " +
                        " '" + userId + "', " +
                        " '" + nick + "', " +
                        " '" + leaseId + "', " +
                        " '" + validateDate + "', " +
                        " '" + invalidateDate + "', " +
                        " '" + factMoney + "', " +
                        " '" + subscType + "', " +
                        " '" + versionNo + "', " +
                        " '" + oldVersionNo + "', " +
                        " '" + status + "', " +
                        " '" + sign + "', " +
                        " '" + gmtCreateDate + "', " +
                        " '" + tadgetCode + "' " +
                  ") ";


        //File.WriteAllText(Server.MapPath("notify.txt"), sql);

        utils.ExecuteNonQuery(sql);
    }
}
