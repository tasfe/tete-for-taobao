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
/// 商品类目存取类
/// </summary>
public class CateService
{
    const string SQL_SELECT_ALL_CATE = "SELECT CateId,CateName,ParentId FROM BangT_Category WHERE Nick=@Nick";

    public IList<CateInfo> SelectAllCateByNick(string nick)
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_ALL_CATE, new SqlParameter("@Nick", nick));

        IList<CateInfo> list = new List<CateInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            CateInfo info =new CateInfo();
            info.CateId = dr["CateId"].ToString();
            info.CateName = dr["CateName"].ToString();
            info.ParentId = dr["ParentId"].ToString();

            list.Add(info);
        }

        return list;
    }
}
