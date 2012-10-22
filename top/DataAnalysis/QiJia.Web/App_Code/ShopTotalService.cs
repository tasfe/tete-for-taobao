using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using DBHelp;

/// <summary>
///订购数据统计商户
/// </summary>
public class ShopTotalService
{
    const string SQL_SELECT_SHOPID = "select ShopId from Jia_TotalShop";

    const string SQL_INSERT = "INSERT Jia_TotalShop(ShopId,AddTime) VALUES(@ShopId,@AddTime)";

    public List<string> GetAllShopIds()
    {
        List<string> tables = new List<string>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_SHOPID);

        foreach (DataRow dr in dt.Rows)
        {
            tables.Add(dr[0].ToString());
        }

        return tables;
    }

    public int InsertShopTotal(ShopTotalInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@ShopId",info.ShopId),
            new SqlParameter("@AddTime",info.AddTime)
        };
        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }
}
