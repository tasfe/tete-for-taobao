using System;
using System.Collections.Generic;
using System.Text;
using TeteTopApi;
using TeteTopApi.Entity;
using TeteTopApi.Logic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using TeteTopApi.DataContract;
using TeteTopApi.Entity;
using TeteTopApi.TopApi;

namespace WeiboSend
{
    class Program
    {
        static void Main(string[] args)
        {
            //Api top = new Api("12132145", "1fdd2aadd5e2ac2909db2967cbb71e7f", "", "http://stream.api.taobao.com/stream");
            //string result = top.ConnectServerFree();

            string sql = "SELECT * FROM TCS_TaobaoMsgLogBak WHERE nick = 'fxb6417332' AND typ = 'TradeRated'";
            DataTable dt = utils.ExecuteDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    string result = dt.Rows[i]["result"].ToString();

                    ReceiveMessage msg = new ReceiveMessage(result.ToString());
                    msg.ActData();
                }
                catch
                { 
                    
                }
            }


            //Test test = new Test();
            //test.StartTest();


            Console.ReadLine();
        }
    }

    public class Test
    {
        /// <summary>
        /// 获取当前正在使用物流到货短信通知并有短信可发的卖家信息(增加延迟发货短信的卖家，否则无法获取物流状态)
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public List<ShopInfo> GetShopInfoListShippingAlert()
        {
            string sql = "SELECT * FROM TCS_ShopConfig r WITH (NOLOCK) INNER JOIN TCS_ShopSession s WITH (NOLOCK) ON s.nick = r.nick WHERE isdel = 0 AND r.nick = 'zdzzdz12388'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new List<ShopInfo>();
            }
        }


        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<ShopInfo> FormatDataList(DataTable dt)
        {
            List<ShopInfo> infoList = new List<ShopInfo>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ShopInfo info = new ShopInfo();

                info.CouponID = dt.Rows[i]["couponid"].ToString().Trim();
                info.IsKefu = dt.Rows[i]["iskefu"].ToString();
                info.MinDateSelf = dt.Rows[i]["maxdate"].ToString();
                info.MinDateSystem = dt.Rows[i]["mindate"].ToString();
                info.MsgCount = dt.Rows[i]["total"].ToString();
                info.MsgCouponContent = dt.Rows[i]["giftcontent"].ToString();
                info.MsgFahuoContent = dt.Rows[i]["fahuocontent"].ToString();
                info.MsgIsCoupon = dt.Rows[i]["giftflag"].ToString();
                info.MsgIsFahuo = dt.Rows[i]["fahuoflag"].ToString();
                info.MsgIsReview = dt.Rows[i]["reviewflag"].ToString();
                info.MsgReviewTime = dt.Rows[i]["reviewtime"].ToString();
                info.MsgIsShipping = dt.Rows[i]["shippingflag"].ToString();
                info.MsgReviewContent = dt.Rows[i]["reviewcontent"].ToString();
                info.MsgShippingContent = dt.Rows[i]["shippingcontent"].ToString();
                info.MsgShopName = dt.Rows[i]["shopname"].ToString();
                info.Nick = dt.Rows[i]["nick"].ToString();
                info.Session = dt.Rows[i]["session"].ToString();
                info.Version = dt.Rows[i]["version"].ToString();
                info.IsCancelAuto = dt.Rows[i]["IsCancelAuto"].ToString();
                info.Keyword = dt.Rows[i]["Keyword"].ToString();
                info.WordCount = dt.Rows[i]["WordCount"].ToString();
                info.IsCoupon = dt.Rows[i]["IsCoupon"].ToString();
                info.Mobile = dt.Rows[i]["phone"].ToString();

                infoList.Add(info);
            }
            return infoList;
        }

        public void StartTest()
        {
            //获取目前正在使用的卖家(需要改成全部卖家，否则物流状态无法获取)
            List<ShopInfo> list = GetShopInfoListShippingAlert();

            //循环判定这些卖家的订单是否物流到货
            for (int i = 0; i < list.Count; i++)
            {
                ShopInfo shop = list[i];
                //获取那些状态是发货中且没发过短信的订单给予提示
                TradeData dbTrade = new TradeData();
                List<Trade> listTrade = dbTrade.GetShippingTrade(shop);

                Console.Write(listTrade.Count.ToString() + "\r\n");
                for (int j = 0; j < listTrade.Count; j++)
                {
                    Trade trade = listTrade[j];

                    Console.Write(trade.BuyNick + "\r\n");

                    //获取该订单的物流状态
                    TopApiHaoping api = new TopApiHaoping(shop.Session);
                    string status = api.GetShippingStatusByTid(listTrade[j]);

                    Console.Write(status + "\r\n");
                    //如果该物流信息不存在
                    if (status.IndexOf("不存在") != -1)
                    {
                        //如果该物流公司不支持查询则更新为self
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                        continue;
                    }

                    trade = api.GetOrderShippingInfo(trade);

                    if (!dbTrade.IsTaobaoCompany(trade))
                    {
                        //如果不是淘宝合作物流则直接更新
                        dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                    }
                    else
                    {
                        //根据服务器的物流状态进行判断，如果物流状态是已签收
                        if (status == "ACCEPTED_BY_RECEIVER")
                        {
                            string result = api.GetShippingStatusDetailByTid(trade);
                            Console.Write("【" + result + "】\r\n");
                            //如果订单不是服务器错误
                            if (result.IndexOf("company-not-support") != -1)
                            {
                                //如果该物流公司不支持查询则更新为self
                                dbTrade.UpdateTradeShippingStatusSelf(trade, status);
                                continue;
                            }

                            //再根据订单的详细物流信息判断签收的状态
                            if (result.IndexOf("签收人") != -1 || result.IndexOf("正常签收录入扫描") != -1)
                            {
                                //如果物流已经签收了则更新对应订单状态
                                trade.DeliveryEnd = utils.GetShippingEndTime(result); ;
                                trade.DeliveryMsg = result;

                                //如果物流到货时间还是为空
                                if (trade.DeliveryEnd == "")
                                {
                                    LogData dbLog = new LogData();
                                    dbLog.InsertErrorLog(trade.Nick, "deliveryDateNull", "", result, "");
                                    continue;
                                }

                                //更新物流到货时间
                                dbTrade.UpdateTradeShippingStatusSystem(trade, status);

                                //发送短信-上LOCK锁定
                                //lock (padlock1)
                                //{
                                //判断同类型的短信该客户今天是否只收到一条
                                ShopData db = new ShopData();
                                if (!db.IsSendMsgOrder(trade, "shipping"))
                                {
                                    //判断该用户是否开启了发货短信
                                    if (shop.MsgIsShipping == "1" && int.Parse(shop.MsgCount) > 0)
                                    {
                                        //发送短信
                                        string msg = Message.GetMsg(shop.MsgShippingContent, shop.MsgShopName, trade.BuyNick, shop.IsCoupon);
                                        string msgResult = Message.Send(trade.Mobile, msg);

                                        //记录
                                        if (msgResult != "0")
                                        {
                                            db.InsertShopMsgLog(shop, trade, msg, msgResult, "shipping");
                                        }
                                        else
                                        {
                                            db.InsertShopErrMsgLog(shop, trade, msg, msgResult, "shipping");
                                        }

                                        shop.MsgCount = (int.Parse(shop.MsgCount) - 1).ToString();
                                    }
                                }
                                //}
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 执行SQL语句返回DataTable
        /// </summary>
        /// <param name="dbstring">SQL语句</param>
        /// <returns>返回结果的DataTable</returns>
        public static DataTable ExecuteDataTable(string dbstring)
        {
            Database db = DatabaseFactory.CreateDatabase();
            DbCommand dbCommand = db.GetSqlStringCommand(dbstring);
            DataTable dt = null;

            try
            {
                dt = db.ExecuteDataSet(dbCommand).Tables[0];
            }
            catch (Exception e)
            {
                db = null;
            }

            return dt;
        }
    }
}
