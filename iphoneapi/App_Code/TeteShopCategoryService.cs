using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
public class TeteShopCategoryService
{
    public IList<TeteShopCategoryInfo> GetAllTeteShopCategory(string nick)
    {
        string sql = "select * from TeteShopCategory where nick=@nick";
        SqlParameter param = new SqlParameter("@nick", nick);

        return TeteShopCategoryPropertity(sql, param);
    }
    public int AddTeteShopCategory(TeteShopCategoryInfo teteshopcategory)
    {
        string sql = "insert TeteShopCategory values(@cateid,@catename,@catecount,@parentid,@nick,@catepicurl)";
        SqlParameter[] param = CreateParameter(teteshopcategory);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int ModifyTeteShopCategory(TeteShopCategoryInfo teteshopcategory)
    {
        string sql = "update TeteShopCategory set cateid=@cateid,catename=@catename,catecount=@catecount,parentid=@parentid,nick=@nick,catepicurl=@catepicurl where id=@id";
        SqlParameter[] param = CreateParameter(teteshopcategory);
        return DBHelper.ExecuteNonQuery(sql, param);
    }
    public int DeleteTeteShopCategory(int teteshopcategoryId)
    {
        string sql = "delete from TeteShopCategory where id=" + teteshopcategoryId;
        return DBHelper.ExecuteNonQuery(sql);
    }
    public TeteShopCategoryInfo GetTeteShopCategoryById(int teteshopcategoryId)
    {
        string sql = "select * from TeteShopCategory where id=" + teteshopcategoryId;
        IList<TeteShopCategoryInfo> list = TeteShopCategoryPropertity(sql);
        return list.Count == 0 ? null : list[0];
    }
    private SqlParameter[] CreateParameter(TeteShopCategoryInfo teteshopcategory)
    {
        SqlParameter[] param = new SqlParameter[]
                    {
                      new SqlParameter("@id",teteshopcategory.Id),
                      new SqlParameter("@cateid",teteshopcategory.Cateid),
                      new SqlParameter("@catename",teteshopcategory.Catename),
                      new SqlParameter("@catecount",teteshopcategory.Catecount),
                      new SqlParameter("@parentid",teteshopcategory.Parentid),
                      new SqlParameter("@nick",teteshopcategory.Nick),
                      new SqlParameter("@catepicurl",teteshopcategory.Catepicurl)
                    };
        return param;
    }
    private IList<TeteShopCategoryInfo> TeteShopCategoryPropertity(string sql, params SqlParameter[] param)
    {
        DataTable dt = DBHelper.ExecuteDataTable(sql, param);
        IList<TeteShopCategoryInfo> list = new List<TeteShopCategoryInfo>();
        foreach (DataRow dr in dt.Rows)
        {
            TeteShopCategoryInfo teteshopcategory = new TeteShopCategoryInfo();
            teteshopcategory.Id = Convert.ToInt32(dr["id"]);
            teteshopcategory.Cateid = Convert.ToString(dr["cateid"]);
            teteshopcategory.Catename = Convert.ToString(dr["catename"]);
            teteshopcategory.Catecount = Convert.ToInt32(dr["catecount"]);
            teteshopcategory.Parentid = Convert.ToString(dr["parentid"]);
            teteshopcategory.Nick = Convert.ToString(dr["nick"]);
            teteshopcategory.Catepicurl = Convert.ToString(dr["catepicurl"]);
            list.Add(teteshopcategory);
        }
        return list;
    }
}