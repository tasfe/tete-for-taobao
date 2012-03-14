using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using TaoBaoAPIHelper;
using CusServiceAchievements.DAL;

namespace GetTaoBaoGoodsService
{
    public class TaoBaoGoods
    {
        public void GetTaoBaoGoods()
        {
            NickSessionService nsDal = new NickSessionService();
            IList<TopNickSessionInfo> list = nsDal.GetAllNickSession(Enum.TopTaoBaoService.YingXiaoJueCe);

            GoodsService goodsDal = new GoodsService();

            for (int i = 0; i < list.Count; i++)
            {
                string shopId = TaoBaoAPI.GetShopInfo(list[i].Nick);
                list[i].ShopId = shopId;
                nsDal.UpdateNickShop(list[i].Nick, shopId);
            }

            foreach (TopNickSessionInfo info in list)
            {
                List<GoodsInfo> goodsList = TaoBaoAPI.GetGoodsInfoListByNick(info.Nick, info.Session);

                foreach (GoodsInfo ginfo in goodsList)
                {
                    goodsDal.InsertGoods(ginfo);
                }
            }
        }
    }
}
