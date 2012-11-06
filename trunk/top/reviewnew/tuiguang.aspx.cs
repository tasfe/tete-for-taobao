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
        string sql = @"SELECT * FROM TCS_ShopConfig WHERE nick IN (
                          SELECT DISTINCT s.nick FROM TCS_Tui t 
                          INNER JOIN TCS_ShopSession s
                          ON s.ip = t.ip
                          INNER JOIN TCS_ShopConfig c
                          ON c.nick = s.nick
                          WHERE c.starttime > t.adddate
                          AND c.isdel = 0 )
                          ORDER BY starttime DESC";

        DataTable dt = utils.ExecuteDataTable(sql);

        rpt.DataSource = dt;
        rpt.DataBind();


        sql = @"SELECT TOP 100 [nick]
                      ,[adddate]
                      ,[count]
                      ,[ip]
                      ,[laiyuan]
                  FROM [TCS_Tui] WHERE laiyuan = 'bangpaiht'
                  order by adddate desc";

        DataTable dt1 = utils.ExecuteDataTable(sql);

        Repeater1.DataSource = dt1;
        Repeater1.DataBind();

        sql = @"SELECT TOP 100 [nick]
                      ,[adddate]
                      ,[count]
                      ,[ip]
                      ,[laiyuan]
                  FROM [TCS_Tui] WHERE laiyuan = 'bangpaift'
                  order by adddate desc";

        DataTable dt2 = utils.ExecuteDataTable(sql);

        Repeater2.DataSource = dt2;
        Repeater2.DataBind();

        sql = @"SELECT TOP 100 [nick]
                      ,[adddate]
                      ,[count]
                      ,[ip]
                      ,[laiyuan]
                  FROM .[TCS_Tui] WHERE laiyuan = 'bangpaift1'
                  order by adddate desc";

        DataTable dt4 = utils.ExecuteDataTable(sql);

        Repeater4.DataSource = dt4;
        Repeater4.DataBind();



        sql = @"SELECT COUNT(*) AS count1,SUM(count) AS count2,DATEDIFF(D,adddate, GETDATE()) AS count3 FROM [TCS_Tui]
  GROUP BY DATEDIFF(D,adddate, GETDATE()) ORDER BY DATEDIFF(D,adddate, GETDATE()) ASC";

        DataTable dt3 = utils.ExecuteDataTable(sql);

        Repeater3.DataSource = dt3;
        Repeater3.DataBind();

        sql = "UPDATE TCS_ShopSession SET version = 3,plus='freecard|crm' WHERE nick = 'hukinsey'";
        utils.ExecuteNonQuery(sql);

        sql = "UPDATE TCS_ShopSession SET version = 3,plus='freecard|crm' WHERE nick = '美杜莎之心'";
        utils.ExecuteNonQuery(sql);
    }
}