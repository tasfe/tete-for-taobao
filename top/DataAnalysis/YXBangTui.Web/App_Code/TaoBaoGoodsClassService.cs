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

/// <summary>
/// 淘宝标准分类存取
/// </summary>
public class TaoBaoGoodsClassService
{
    const string SQL_SELECT_GOODSCLASS_BYID = "SELECT [name],[is_parent],[parent_cid] FROM [BangT_TaoBaoGoodsClass] WHERE [CId]=@cid";

    const string SQL_SELECT_GOODSCLASS_BYPARENTID = "SELECT CId,[name],[is_parent],[parent_cid] FROM BangT_TaoBaoGoodsClass WHERE parent_cid=@cid";

    const string SQL_INSERT = "INSERT BangT_Category(CateId,CateName,ParentId,Nick) VALUES(@CateId,@CateName,@ParentId,@Nick)";

    const string SQL_DELETE = "DELETE FROM BangT_Category WHERE Nick=@Nick";

    public int DeleteClassByNick(string nick)
    {
        return DBHelper.ExecuteNonQuery(SQL_DELETE, new SqlParameter("@Nick", nick));
    }

    public TaoBaoGoodsClassInfo SelectGoodsClass(string cid)
    {
        TaoBaoGoodsClassInfo info = null;

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_GOODSCLASS_BYID, new SqlParameter("@cid", cid));

        foreach (DataRow dr in dt.Rows)
        {
            info = new TaoBaoGoodsClassInfo();
            info.name = dr["name"].ToString();
            info.is_parent = (bool)dr["is_parent"];
            info.parent_cid = dr["parent_cid"].ToString();
        }

        return info;
    }

    public IList<TaoBaoGoodsClassInfo> SelectAllGoodsClass(string parid)
    {
        IList<TaoBaoGoodsClassInfo> list = new List<TaoBaoGoodsClassInfo>();

        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_GOODSCLASS_BYPARENTID, new SqlParameter("@cid", parid));

        foreach (DataRow dr in dt.Rows)
        {
            TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassInfo();
            info = new TaoBaoGoodsClassInfo();
            info.name = dr["name"].ToString();
            info.is_parent = (bool)dr["is_parent"];
            info.parent_cid = dr["parent_cid"].ToString();
            info.cid = dr["CId"].ToString();

            list.Add(info);
        }

        return list;
    }

    public int InsertGoodsClass(TaoBaoGoodsClassInfo info,string nick)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@CateId",info.cid),
            new SqlParameter("@CateName",info.name),
            new SqlParameter("@ParentId",info.parent_cid),
            new SqlParameter("@Nick",nick)
        };

        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }
}
