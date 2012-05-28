using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;

namespace TeteTopApi.TopApi
{
    public class TopApiFree
    {
        public TopApiFree(string session)
        {
            this.AppKey = "12132145";
            this.Secret = "1fdd2aadd5e2ac2909db2967cbb71e7f";
            this.Session = session;
            this.Url = "http://gw.api.taobao.com/router/rest";
        }

        public Item GetItemInfo(Item item)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            Console.Write(Session + "\r\n");
            Console.Write(item.ID + "\r\n");

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "title,price,pic_url");
            param.Add("num_iid", item.ID);

            string result = top.CommonTopApi("taobao.item.get", param, Session);
            Console.Write(result + "\r\n");

            item.Name = utils.GetValueByProperty(result, "title");
            item.Price = utils.GetValueByProperty(result, "price");
            item.ImageUrl = utils.GetValueByProperty(result, "pic_url").Replace("\\/", "/");
            item.ItemUrl = "http://item.taobao.com/item.htm?id=" + item.ID;

            return item;
        }

        public string GetTradeItemByTid(string tradeId)
        {
            Api top = new Api(AppKey, Secret, Session, Url);

            IDictionary<string, string> param = new Dictionary<string, string>();
            param.Add("fields", "orders.num_iid");
            param.Add("tid", tradeId);

            string result = top.CommonTopApi("taobao.trade.get", param, Session);
            string itemId = utils.GetValueByPropertyNum(result, "num_iid");

            return itemId;
        }

        public string AppKey { get; set; }
        public string Secret { get; set; }
        public string Session { get; set; }
        public string Url { get; set; }
    }
}
