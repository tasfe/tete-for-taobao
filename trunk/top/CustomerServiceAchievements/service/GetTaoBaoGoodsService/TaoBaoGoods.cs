using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaoBaoAPIHelper;
using CusServiceAchievements.DAL;

namespace GetTaoBaoGoodsService
{
    public class TaoBaoGoods
    {
        public void GetTaoBaoGoods()
        {
            NickSessionService nsDal = new NickSessionService();
            IList<Model.TopNickSessionInfo> list = nsDal.GetAllNickSession();

            GoodsService goodsDal = new GoodsService();
            for (int i = 0; i < list.Count; i++)
            {
                string shopId = TaoBaoAPI.GetShopInfo(list[i].Nick, list[i].Session);
                list[i].ShopId = shopId;
                nsDal.UpdateNickShop(list[i].Nick, shopId);
            }

            foreach (Model.TopNickSessionInfo info in list)
            {
                List<GoodsInfo> goodsList = TaoBaoAPI.GetGoodsInfoListByNick(info.Nick, info.Session);

                List<GoodsInfo> allGoods = goodsDal.GetAllGoods(info.Nick);

                foreach (GoodsInfo ginfo in goodsList)
                {
                    if (allGoods.Contains(ginfo))
                        goodsDal.UpdateGoodsInfo(ginfo);
                    else
                        goodsDal.InsertGoods(ginfo, info.Nick);
                }
            }
        }
    }
}
