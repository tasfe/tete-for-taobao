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
            LogHelper.ServiceLog.RecodeLog("执行了" + list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                LogHelper.ServiceLog.RecodeLog("更新了" + i);
                string shopId = TaoBaoAPI.GetShopInfo(list[i].Nick);
                list[i].ShopId = shopId;
                nsDal.UpdateNickShop(list[i].Nick, shopId);
            }
            LogHelper.ServiceLog.RecodeLog("更新完了");


            LogHelper.ServiceLog.RecodeLog("删除商品表");
            goodsDal.DropTable("TopTaoBaoGoodsInfo");
            LogHelper.ServiceLog.RecodeLog("删除商品表成功");
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
