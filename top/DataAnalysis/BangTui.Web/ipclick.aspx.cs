using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Collections.Generic;

public partial class ipclick : System.Web.UI.Page
{
    UserAdsService uasDal = new UserAdsService();
    PasswordParam pp = new PasswordParam();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            IList<AdsInfo> adsList = CacheCollection.GetAllAdsInfo();

            if (CacheCollection.UserADS_Index == null)
            {
                CacheCollection.UserADS_Index = new Dictionary<Guid, int>();
            }
            foreach (AdsInfo info in adsList)
            {
                if (!CacheCollection.UserADS_Index.ContainsKey(info.AdsId))
                    CacheCollection.UserADS_Index.Add(info.AdsId, 0);

                IList<UserAdsInfo> list = uasDal.SelectAllUserAdsByAdsId(info.AdsId, 1);

                //按时间倒序
                list = list.OrderByDescending(o => o.AddTime).ToList();

                //如果是最后一个，结束本次循环
                if (CacheCollection.UserADS_Index[info.AdsId] == list.Count - 1)
                    break;

                //常量下标加1
                CacheCollection.UserADS_Index[info.AdsId]++;
                string param = "id=" + list[CacheCollection.UserADS_Index[info.AdsId] - 1].Id + "&url=" + list[CacheCollection.UserADS_Index[info.AdsId] - 1].AdsUrl;
                Response.Redirect("getclick.aspx?" + pp.Encrypt3DES(param).Replace("+", "[jia]"));

            }

            //清空，开始新一轮的跳转
            CacheCollection.UserADS_Index = null;

        }
    }
}
