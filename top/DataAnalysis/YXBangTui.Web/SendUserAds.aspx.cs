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

public partial class SendUserAds : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Btn_AddAds_Click(object sender, EventArgs e)
    {
        if (TB_Password.Text == "tuituiguang!(**gansibangtuiguang")
        {
            IList<BuyInfo> list = CacheCollection.GetAllBuyInfo();
            if (list.Where(o => o.Nick == TB_Nick.Text.Trim()).ToList().Count > 0)
            {
                IList<GoodsInfo> glist = new GoodsService().SelectAllGoodsByNick(TB_Nick.Text.Trim());
                if (glist.Count != 0)
                {
                    GoodsInfo ginfo = null;
                    if (glist.Count == 1)
                        ginfo = glist[0];
                    else
                        ginfo = glist[new Random().Next(0, glist.Count)];
                    UserAdsInfo info = new UserAdsInfo();
                    info.AdsTitle = ginfo.GoodsName;
                    info.Id = Guid.NewGuid();
                    info.UserAdsState = 1;
                    info.AdsUrl = "http://item.taobao.com/item.htm?id=" + ginfo.GoodsId;
                    string cateId = ginfo.CateId;

                    TaoBaoGoodsClassInfo tcinfo = new TaoBaoGoodsClassService().SelectGoodsClass(cateId);
                    info.CateIds = ginfo.TaoBaoCId;
                    string cname = tcinfo == null ? "" : tcinfo.name;
                    info.SellCateName = GetTaoBaoCName(info.CateIds, cname);
                    info.AliWang = TB_Nick.Text.Trim();
                    info.Nick = TB_Nick.Text.Trim();
                    info.AdsPic = ginfo.GoodsPic;

                    info.AddTime = DateTime.Now;
                    info.AdsId = GetRand(CacheCollection.GetAllAdsInfo().Where(o => o.AdsType == 1).ToList());
                    info.FeeId = list[0].FeeId;
                    info.AdsShowStartTime = DateTime.Now;
                    info.AdsShowFinishTime = DateTime.Now.AddDays(7);
                    info.UserAdsState = 1;
                    string taoId = info.CateIds;
                    info.CateIds = GetTaoBaoCId(taoId, taoId);

                    new UserAdsService().InsertUserAds(info);
                    Page.RegisterStartupScript("恭喜", "<script>alert('赠送广告成功');</script>");

                }
            }
            else
            {
                Page.RegisterStartupScript("Error", "<script>alert('没有购买记录');</script>");
            }
        }
    }

    private string GetTaoBaoCName(string taoId, string cname)
    {
        string returnName = cname;
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                returnName = info.name + (cname == "" ? "" : ("->" + cname));
                GetTaoBaoCName(info.parent_cid, returnName);
            }
            return returnName;

        }
        return returnName;
    }

    private string GetTaoBaoCId(string taoId, string cids)
    {
        string returncids = cids;
        TaoBaoGoodsClassInfo info = new TaoBaoGoodsClassService().SelectGoodsClass(taoId);
        if (info != null)
        {
            if (info.parent_cid != "0")
            {
                returncids += "," + info.parent_cid;
                GetTaoBaoCId(info.parent_cid, returncids);
            }
            return returncids;

        }
        return returncids;
    }

    /// <summary>
    /// 获取随机数
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private Guid GetRand(IList<AdsInfo> list)
    {
        if (list.Count == 1)
            return list[0].AdsId;
        Random rand = new Random();
        return list[rand.Next(list.Count)].AdsId;
    }
}
