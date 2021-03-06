﻿using System;
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
/// Summary description for TeteShopService
/// </summary>
public class TeteShopService
{
    const string SQL_SELECT = "SELECT nick,sid,appkey,appsecret,session,short FROM TeteShop";

    const string SQL_SELECT_BY_NICK = "SELECT guid,ads,logo,appkey,appsecret FROM TeteShop WHERE nick=@nick";

    const string SQL_UPDATE = "UPDATE TeteShop SET ads=@ads,logo=@logo WHERE guid=@guid";

    const string SQL_INSERT = "INSERT TeteShop([guid],nick,appkey,appsecret,session,adddate,short) VALUES(@guid,@nick,@appkey,@appsecret,@session,@adddate,@short)";

    public int InsertShop(TeteShopInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@guid",Guid.NewGuid()),
            new SqlParameter("@nick",info.Nick),
            new SqlParameter("@short",info.Short),
            new SqlParameter("@appkey",info.Appkey),
            new SqlParameter("@appsecret",info.Appsecret),
            new SqlParameter("@session",info.Session),
            new SqlParameter("@adddate",info.Adddate)
        };

        return DBHelper.ExecuteNonQuery(SQL_INSERT, param);
    }

    public IList<TeteShopInfo> GetAllShopInfo()
    {
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT);

        IList<TeteShopInfo> list = new List<TeteShopInfo>();

        foreach (DataRow dr in dt.Rows)
        {
            TeteShopInfo info = new TeteShopInfo();
            info.Nick = dr["nick"].ToString();
            info.Appkey = dr["appkey"].ToString();
            info.Appsecret = dr["appsecret"].ToString();
            info.Session = dr["session"].ToString();
            info.Short = dr["short"].ToString();

            list.Add(info);
        }

        return list;
    }

    public TeteShopInfo GetShopInfo(string nick)
    {
        TeteShopInfo info = null;
        SqlParameter param = new SqlParameter("@nick", nick);
        DataTable dt = DBHelper.ExecuteDataTable(SQL_SELECT_BY_NICK,param);

        foreach (DataRow dr in dt.Rows)
        {
            info = new TeteShopInfo();
            info.Guid = new Guid(dr["guid"].ToString());
            info.Logo = dr["logo"] == DBNull.Value ? "" : dr["logo"].ToString();
            info.Ads = dr["ads"] == DBNull.Value ? "" : dr["ads"].ToString();
            info.Appsecret = dr["appsecret"].ToString();
            info.Appkey = dr["appkey"].ToString();
        }

        return info;
    }

    public int UpdateShopInfo(TeteShopInfo info)
    {
        SqlParameter[] param = new[]
        {
            new SqlParameter("@ads",info.Ads),
            new SqlParameter("@logo",info.Logo),
            new SqlParameter("@guid",info.Guid)
        };

        return DBHelper.ExecuteNonQuery(SQL_UPDATE, param);

    }
    
}
