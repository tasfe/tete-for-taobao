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

/// <summary>
/// 收费分类存取
/// </summary>
public class FeeService
{

    const string SQL_SELECT_ALL_FEE = "SELECT FeeId,Fee,SiteCount,AdsPubuCount,ShowDays,AdsType,Score FROM BangT_Fee";

    const string SQL_SELECT_ALL_BUY = "SELECT Nick,FeeId,BuyTime,IsExpied FROM BangT_Buys";

    public IList<FeeInfo> SelectAllFee()
    {
        IList<FeeInfo> list = new List<FeeInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL_FEE);

        foreach (DataRow dr in dt.Rows)
        {
            FeeInfo info = new FeeInfo();
            info.FeeId = new Guid(dr["FeeId"].ToString());
            info.Fee = decimal.Parse(dr["Fee"].ToString());
            info.SiteCount = int.Parse(dr["SiteCount"].ToString());
            info.AdsPubuCount = int.Parse(dr["AdsPubuCount"].ToString());
            info.ShowDays = int.Parse(dr["ShowDays"].ToString());
            info.AdsType = int.Parse(dr["AdsType"].ToString());
            info.Score = int.Parse(dr["Score"].ToString());

            list.Add(info);
        }

        return list;
    }

    public IList<BuyInfo> SelectAllBuy()
    {
        IList<BuyInfo> list = new List<BuyInfo>();
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL_BUY);

        foreach (DataRow dr in dt.Rows)
        {
            BuyInfo info = new BuyInfo();
            info.Nick = dr["Nick"].ToString();
            info.FeeId = new Guid(dr["FeeId"].ToString());
            info.BuyTime = DateTime.Parse(dr["BuyTime"].ToString());
            info.IsExpied = (bool)dr["IsExpied"];

            list.Add(info);
        }

        return list;
    }
}
