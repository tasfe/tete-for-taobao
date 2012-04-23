using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

public class TeteShopItemService
{
    public IList<TeteShopItemInfo> GetAllTeteShopItem(string nick)
    {
        string sql = "select * from TeteShopItem where nick=@nick";
        SqlParameter param = new SqlParameter("@nick", nick);
        return TeteShopItemPropertity(sql, param);
    }
    public int AddTeteShopItem(TeteShopItemInfo teteshopitem)
    {
        string sql = "insert TeteShopItem values(@cateid,@itemid,@itemname,@picurl,@linkurl,@nick,@price)";
        SqlParameter[] param = CreateParameter(teteshopitem);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int ModifyTeteShopItem(TeteShopItemInfo teteshopitem)
    {
        string sql = "update TeteShopItem set cateid=@cateid,itemid=@itemid,itemname=@itemname,picurl=@picurl,linkurl=@linkurl,nick=@nick,price=@price where id=@id";
        SqlParameter[] param = CreateParameter(teteshopitem);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int DeleteTeteShopItem(int teteshopitemId)
    {
        string sql = "delete from TeteShopItem where id=" + teteshopitemId;
        return DBHelper.ExecuteNonQuery(sql);
    }
    public TeteShopItemInfo GetTeteShopItemById(int teteshopitemId)
    {
        string sql = "select * from TeteShopItem where id=" + teteshopitemId;
        IList<TeteShopItemInfo> list = TeteShopItemPropertity(sql);
        return list.Count == 0 ? null : list[0];
    }
    private SqlParameter[] CreateParameter(TeteShopItemInfo teteshopitem)
    {
        SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@id",teteshopitem.Id),
                      new SqlParameter("@cateid",teteshopitem.Cateid),
                      new SqlParameter("@itemid",teteshopitem.Itemid),
                      new SqlParameter("@itemname",teteshopitem.Itemname),
                      new SqlParameter("@picurl",teteshopitem.Picurl),
                      new SqlParameter("@linkurl",teteshopitem.Linkurl),
                      new SqlParameter("@nick",teteshopitem.Nick),
                      new SqlParameter("@price",teteshopitem.Price)
                    };
        return param;
    }
    private IList<TeteShopItemInfo> TeteShopItemPropertity(string sql, params SqlParameter[] param)
    {
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<TeteShopItemInfo> list = new List<TeteShopItemInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TeteShopItemInfo teteshopitem = new TeteShopItemInfo();
            teteshopitem.Id = Convert.ToInt32(dr["id"]);
            teteshopitem.Cateid = Convert.ToString(dr["cateid"]);
            teteshopitem.Itemid = Convert.ToString(dr["itemid"]);
            teteshopitem.Itemname = Convert.ToString(dr["itemname"]);
            teteshopitem.Picurl = Convert.ToString(dr["picurl"]);
            teteshopitem.Linkurl = Convert.ToString(dr["linkurl"]);
            teteshopitem.Nick = Convert.ToString(dr["nick"]);
            teteshopitem.Price = Convert.ToDouble(dr["price"]);
            list.Add(teteshopitem);
        }
        return list;
    }
}