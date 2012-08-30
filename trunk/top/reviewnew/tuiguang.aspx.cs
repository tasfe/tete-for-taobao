using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using System.Data;

public partial class top_reviewnew_tuiguang : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string sql = @"SELECT TOP 1000 [nick]
                      ,[adddate]
                      ,[count]
                      ,[ip]
                      ,[laiyuan],TCS_ShopSession.nick AS nickding
                  FROM [TeteCrmSaasNew].[dbo].[TCS_Tui]
                  WHERE nick in
                  (select nick from TCS_ShopSession WHERE version > 1)
                  OR ip in
                   (select ip from TCS_ShopSession where ip is not null AND version > 1)
                  order by adddate desc";

        DataTable dt = utils.ExecuteDataTable(sql);

        rpt.DataSource = dt;
        rpt.DataBind();
    }
}