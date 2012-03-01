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
/// Summary description for TaoBaoGoodsServivr
/// </summary>
public class TaoBaoGoodsServive
{
    const string SQL_INSERT = "INSERT TopTaoBaoGoodsInfo(GoodsID,GoosName,NickNo) VALUES(@GoodsID,@GoosName,@NickNo)";

    public void InsertTaoBaoGoodsInfo(GoodsInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("GoodsID",info.num_iid),
            new SqlParameter("GoosName",info.title),
            new SqlParameter("NickNo",info.nick)
        };
        DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }
}
