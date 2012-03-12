using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_review_js_isshow : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Cookie cookie = new Cookie();
        string taobaoNick = cookie.getCookie("nick");

        Rijndael_ encode = new Rijndael_("tetesoft");
        taobaoNick = encode.Decrypt(taobaoNick);

        string sql = "SELECT phone,qq FROM TCS_ShopConfig WHERE nick = '" + taobaoNick + "'";
        DataTable dt = utils.ExecuteDataTable(sql);
        if (dt.Rows.Count != 0)
        {
            string phone = dt.Rows[0][0].ToString();
            string qq = dt.Rows[0][1].ToString();

            if (phone.Length == 0 || qq.Length == 0)
            {
                Response.Write("setTimeout('showAreaPhone()', 3000);");
            }
        }
    }
}