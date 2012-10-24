using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;

namespace TeteTopApi.Logic
{
    public class CustomerNotice
    {
        public CustomerNotice() { }

        /// <summary>
        /// 长时间未消费客户回访
        /// </summary>
        public void CallMemberBack()
        {
            //获取在执行中的回访任务卖家
            string typ = "back";
            ShopData data = new ShopData();
            CustomerData cusData = new CustomerData();
            MessageData dbMessage = new MessageData();
            List<ShopInfo> shoplist = data.GetActMissionShop(typ);

            for (int i = 0; i < shoplist.Count; i++)
            {
                ShopInfo shop = shoplist[i];

                try
                {
                    //获取该卖家符合条件的买家清单
                    List<Customer> cusList = cusData.GetBackCustomer(shop);

                    //每天最多发送100条提醒
                    int total = 0;

                    for (int j = 0; j < cusList.Count; j++)
                    {
                        Customer customer = cusList[j];
                        //发送短信
                        if (customer.Mobile != "")
                        {
                            if (!dbMessage.IsSendMsgNearDays(customer, typ, shop.MissionBackDay))
                            {
                                string msgResult = Message.SendGuodu(customer.Mobile, shop.MissionContent);
                                data.InsertShopMsgLog(shop, customer, shop.MissionContent, msgResult, typ);
                                Console.Write(shop.MissionContent + "--" + customer.Nick + "--" + customer.BuyNick + "[" + shop.MissionContent.Length.ToString() + "]\r\n");
                                total++;

                                //每天最多发送100条
                                if (total > 100)
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine(shop.Nick + "------------err!!");
                }

                //Console.ReadLine();
            }
        }

        /// <summary>
        /// 生日关怀
        /// </summary>
        public void BirthDayCall()
        {
            //获取在执行中的回访任务卖家
            string typ = "birthday";
            ShopData data = new ShopData();
            CustomerData cusData = new CustomerData();
            MessageData dbMessage = new MessageData();
            List<ShopInfo> shoplist = data.GetActMissionShop(typ);

            for (int i = 0; i < shoplist.Count; i++)
            {
                ShopInfo shop = shoplist[i];
                //获取该卖家符合条件的买家清单
                try
                {
                    List<Customer> cusList = cusData.GetBirthdayCustomer(shop);
                    for (int j = 0; j < cusList.Count; j++)
                    {
                        Customer customer = cusList[j];
                        //发送短信
                        if (customer.Mobile != "")
                        {
                            if (!dbMessage.IsSendMsgNearDays(customer, typ, "30"))
                            {
                                string msgResult = Message.SendGuodu(customer.Mobile, shop.MissionContent);
                                data.InsertShopMsgLog(shop, customer, shop.MissionContent, msgResult, typ);
                                Console.Write(shop.MissionContent + "--" + customer.Nick + "--" + customer.BuyNick + "[" + shop.MissionContent.Length.ToString() + "]\r\n");
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine(shop.Nick + "------------err!!");
                }
            }
        }
    }
}
