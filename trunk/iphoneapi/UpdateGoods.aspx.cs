using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

public partial class UpdateGoods : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //IList<GoodsClassInfo> classList = TaoBaoAPI.GetGoodsClassInfoList("luckyfish8800", "6101312587cbdace711a1e26d5877064329a4dd05d9c96326907498", "12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f");
        }
    }
    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        string nick = HttpUtility.UrlDecode(Request.Cookies["nick"].Value);
        string session = Request.Cookies["nicksession"].Value;

        TeteShopCategoryService cateDal = new TeteShopCategoryService();

        IList<TeteShopInfo> list = CacheCollection.GetNickSessionList().Where(o => o.Nick == nick && o.Session == session).ToList();

        TeteShopInfo info = null;
        if (list.Count > 0)
        {
            info = list[0];
        }
        if (info == null)
        {
            Page.RegisterStartupScript("错误", "<script>alert('您的身份不合法，请确定您已购买!');</script>");
            return;
        }

        IList<TeteShopCategoryInfo> cateList = cateDal.GetAllTeteShopCategory(nick);
        IList<GoodsClassInfo> classList = new List<GoodsClassInfo>();
        try
        {
            classList = TaoBaoAPI.GetGoodsClassInfoList(nick, session, info.Appkey, info.Appsecret);
        }
        catch (Exception ex)
        {
            Page.RegisterStartupScript("错误", "<script>alert('" + ex.Message + "');</script>");
            return;
        }

        List<TeteShopCategoryInfo> addList = new List<TeteShopCategoryInfo>();

        List<TeteShopCategoryInfo> upList = new List<TeteShopCategoryInfo>();

        foreach (GoodsClassInfo cinfo in classList)
        {
            List<TeteShopCategoryInfo> clist = cateList.Where(o => o.Cateid == cinfo.cid).ToList();
            if (clist.Count > 0)
            {
                InitCate(nick, cinfo, clist[0]);
                clist[0].Catecount = classList.Count(o => o.parent_cid == cinfo.cid);
                upList.Add(clist[0]);
            }

            else
            {
                TeteShopCategoryInfo ainfo = new TeteShopCategoryInfo();
                InitCate(nick, cinfo, ainfo);
                ainfo.Catecount = classList.Count(o => o.parent_cid == cinfo.cid);

                addList.Add(ainfo);
            }
        }

        //添加
        foreach (TeteShopCategoryInfo cinfo in addList)
        {
            cateDal.AddTeteShopCategory(cinfo);
        }

        //修改
        foreach (TeteShopCategoryInfo cinfo in upList)
        {
            cateDal.ModifyTeteShopCategory(cinfo);
        }

        //删除
        List<TeteShopCategoryInfo> delList = new List<TeteShopCategoryInfo>();
        foreach (TeteShopCategoryInfo cinfo in cateList)
        {
            if (upList.Where(o => o.Cateid == cinfo.Cateid).ToList().Count == 0)
            {
                delList.Add(cinfo);
            }
        }

        foreach (TeteShopCategoryInfo cinfo in upList)
        {
            cateDal.DeleteTeteShopCategory(cinfo.Id);
        }

        //更新商品
        ActionGoods(nick, session, info);

        Page.RegisterStartupScript("更新提示", "<script>alert('更新成功!');</script>");
    }

    private static void ActionGoods(string nick,string session,TeteShopInfo info)
    {
        TeteShopItemService itemDal = new TeteShopItemService();

        List<GoodsInfo> glist = TaoBaoAPI.GetGoodsInfoListByNick(nick, session, info.Appkey, info.Appsecret);
        IList<TeteShopItemInfo> itemList = itemDal.GetAllTeteShopItem(nick);

        List<TeteShopItemInfo> addList = new List<TeteShopItemInfo>();
        List<TeteShopItemInfo> upList = new List<TeteShopItemInfo>();

        foreach (GoodsInfo cinfo in glist)
        {
            List<TeteShopItemInfo> clist = itemList.Where(o => o.Itemid == cinfo.num_iid).ToList();
            if (clist.Count > 0)
            {
                InitItem(nick, cinfo, clist[0]);
                upList.Add(clist[0]);
            }

            else
            {
                TeteShopItemInfo ainfo = new TeteShopItemInfo();
                InitItem(nick, cinfo, ainfo);

                addList.Add(ainfo);
            }
        }

        //添加
        foreach (TeteShopItemInfo cinfo in addList)
        {
            itemDal.AddTeteShopItem(cinfo);
        }

        //修改
        foreach (TeteShopItemInfo cinfo in upList)
        {
            itemDal.ModifyTeteShopItem(cinfo);
        }

        //删除
        List<TeteShopItemInfo> delList = new List<TeteShopItemInfo>();
        foreach (TeteShopItemInfo cinfo in itemList)
        {
            if (upList.Where(o => o.Itemid == cinfo.Itemid).ToList().Count == 0)
            {
                delList.Add(cinfo);
            }
        }

        foreach (TeteShopItemInfo cinfo in upList)
        {
            itemDal.DeleteTeteShopItem(cinfo.Id);
        }
    }

    private static void InitItem(string nick, GoodsInfo cinfo, TeteShopItemInfo ainfo)
    {
        ainfo.Itemid = cinfo.num_iid;
        ainfo.Nick = nick;
        ainfo.Price = (double)cinfo.price;
        ainfo.Picurl = cinfo.pic_url;
        ainfo.Itemname = cinfo.title;
        ainfo.Cateid = cinfo.cid;
        ainfo.Linkurl = "http://item.taobao.com/item.htm?id=" + ainfo.Itemid;
    }

    private static void InitCate(string nick, GoodsClassInfo cinfo, TeteShopCategoryInfo ainfo)
    {
        ainfo.Cateid = cinfo.cid;
        ainfo.Parentid = cinfo.parent_cid;
        ainfo.Catename = cinfo.name;
        ainfo.Catepicurl = cinfo.pic_url;
        ainfo.Nick = nick;
    }
}
