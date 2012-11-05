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
using System.Data.SqlClient;

/// <summary>
/// 店铺信息数据存取
/// </summary>
public class ShopService
{
    const string SQL_SELECT_BY_NICK = "SELECT sid,cid,title,[desc],pic_path FROM TopTaobaoShop WHERE nick=@nick";

    public TaoBaoShopInfo SelectShopByNick(string nick)
    {
        SqlParameter param = new SqlParameter("@nick",nick);

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_NICK,param);
        TaoBaoShopInfo info = null;

        foreach(DataRow dr in dt.Rows)
        {
            info = new TaoBaoShopInfo();
            info.Description = dr["desc"].ToString();
            info.ShopId = dr["sid"].ToString();
            info.Name = dr["title"].ToString();
            info.ShopLogo = dr["pic_path"].ToString();
            info.CateId = dr["cid"].ToString();
        }

        return info;
    }
}
