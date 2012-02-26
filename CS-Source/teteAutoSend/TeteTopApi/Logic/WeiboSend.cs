using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;

namespace TeteTopApi.Logic
{
    public class WeiboSend
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="trade"></param>
        public WeiboSend(Item item, string status, string nick, string msg) 
        {
            this.ItemInfo = item;
            this.Status = status;
            this.Nick = nick;
            this.Msg = msg;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            WeiboData data = new WeiboData();
            Weibo weibo = data.GetUserWeiboData(this.Nick);
            string content = string.Empty;
            string index = string.Empty;

            //获取订单编号
            string tradeId = utils.GetValueByPropertyNum(this.Msg, "tid");

            switch (this.Status)
            {
                case "ItemUpshelf":
                    content = weibo.ContentUp;
                    index = "1";
                    break;
                case "TradeCreate":
                    content = weibo.ContentSell;
                    ItemInfo.ID = GetItemId(tradeId);
                    index = "2";
                    break;
                case "TradeRated":
                    content = weibo.ContentReview;
                    ItemInfo.ID = GetItemId(tradeId);
                    index = "3";
                    break;
                case "ItemRecommendAdd":
                    content = weibo.ContentRecommend;
                    index = "4";
                    break;
            }

            string imgUrl = string.Empty;

            //如果卖家没有设置则退出
            if (content.Length == 0)
            {
                Console.Write(Nick + "-还没有设置微博自动发送的信息...\r\n");
                return;
            }

            content = ChangeTag(content, this.ItemInfo, ref imgUrl);

            SendWeiboMsg(Nick, content, imgUrl, index);
        }

        /// <summary>
        /// 获取订单中的宝贝ID
        /// </summary>
        /// <param name="tradeId"></param>
        /// <returns></returns>
        private string GetItemId(string tradeId)
        {
            FreeShopData data = new FreeShopData();
            ShopInfo shop = data.ShopInfoGetByNick(Nick);

            TopApiFree api = new TopApiFree(shop.Session);
            string numiid = api.GetTradeItemByTid(tradeId);
            //Console.Write(numiid + "__\r\n");
            return numiid;
        }

        /// <summary>
        /// 替换内容中的动态链接
        /// </summary>
        /// <param name="content"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private string ChangeTag(string content, Item item, ref string imgUrl)
        {
            FreeShopData data = new FreeShopData();
            ShopInfo shop = data.ShopInfoGetByNick(Nick);

            TopApiFree api = new TopApiFree(shop.Session);
            item = api.GetItemInfo(item);

            content = content.Replace("[宝贝标题]", item.Name);
            content = content.Replace("[宝贝价格]", item.Price);
            content = content.Replace("[宝贝链接]", item.ItemUrl);

            imgUrl = item.ImageUrl;

            return content;
        }

        /// <summary>
        /// 发送微博消息
        /// </summary>
        /// <param name="Nick"></param>
        /// <param name="content"></param>
        private void SendWeiboMsg(string nick, string content, string imgUrl, string index)
        {
            WeiboData data = new WeiboData();
            List<WeiboID> weiboids = data.GetUserWeiboIDS(nick);
            for (int i = 0; i < weiboids.Count; i++)
            {
                if (data.IsCanSendMsg(nick, weiboids[i], index))
                {
                    utils uti = new utils();
                    uti.SendMicroBlog(nick, content, imgUrl, weiboids[i].Key, weiboids[i].Secret);
                    //记录发送数量
                    data.UpdateWeiboNum(nick, index);
                    //记录发送日志
                    data.InsertWeiboSendLog(nick, weiboids[i], content, index);
                }
            }
        }

        /// <summary>
        /// 商品消息
        /// </summary>
        private Item ItemInfo { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        private string Status { get; set; }

        /// <summary>
        /// 淘宝用户ID
        /// </summary>
        private string Nick { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        private string Msg { get; set; }
    }
}
