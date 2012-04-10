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
using TaoBaoAPIHelper;
using System.Collections.Generic;
using CusServiceAchievements.DAL;

/// <summary>
/// Summary description for DataHelper
/// </summary>
public class DataHelper
{

    public static void GetAllGoods(string nick,string session)
    {
        GoodsService goodsDal = new GoodsService();
        List<GoodsInfo> goodsList = TaoBaoAPI.GetGoodsInfoListByNick(nick, session);

        List<GoodsInfo> allGoods = goodsDal.GetAllGoods(nick);

        foreach (GoodsInfo ginfo in goodsList)
        {
            if (allGoods.Contains(ginfo))
                goodsDal.UpdateGoodsInfo(ginfo);
            else
                goodsDal.InsertGoods(ginfo, nick);
        }
    }
}
