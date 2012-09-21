using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class FreeCardData
    {
        /// <summary>
        /// 更新包邮卡使用次数
        /// </summary>
        /// <param name="guid"></param>
        public void RecordFreeCardLog(string guid, Trade trade)
        {
            string sql = "UPDATE TCS_FreeCard SET usecount = usecount + 1 WHERE guid = '" + guid + "'";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);

            sql = "INSERT INTO TCS_FreeCardLog(freecardid,nick,buynick,orderid) VALUES ('" + guid + "','" + trade.Nick + "','" + trade.BuyNick + "','" + trade.Tid + "')";
            Console.Write(sql + "\r\n");
            utils.ExecuteNonQuery(sql);
        }

        /// <summary>
        /// 获取买家是否用可用的包邮卡，有就返回包邮卡ID，没有返回空
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public string GetUserFreeCard(Trade trade)
        {
            string sql = "SELECT guid FROM TCS_FreeCard WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "' AND (Enddate > GETDATE() OR StartDate = Enddate) AND (usecount < usecountlimit OR usecountlimit = 0) AND isdel = 0";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public FreeCard GetUserFreeCardById(string guid)
        {
            string sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = (SELECT cardid FROM TCS_FreeCard WHERE guid = '" + guid + "')";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new FreeCard();
            }
        }

        public FreeCard GetFreeCardById(string guid)
        {
            string sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + guid + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatDataList(dt);
            }
            else
            {
                return new FreeCard();
            }
        }


        private FreeCard FormatDataList(DataTable dt)
        {
            FreeCard info = new FreeCard();

            info.AreaList = dt.Rows[0]["arealist"].ToString();
            info.IsFreeAreaList = dt.Rows[0]["areaisfree"].ToString();
            info.Name = dt.Rows[0]["name"].ToString();

            return info;
        }

        /// <summary>
        /// 赠送包邮卡
        /// </summary>
        /// <returns></returns>
        public void SendFreeCard(ShopInfo shop, Trade trade)
        {
            string sql = "SELECT * FROM TCS_FreeCardAction WHERE guid = '" + shop.FreeCardId + "'";
            DataTable dt = utils.ExecuteDataTable(sql);

            if (dt.Rows.Count != 0)
            {
                string startdate = DateTime.Now.ToString();
                string enddate = DateTime.Now.ToString();
                string usecountlimit = dt.Rows[0]["usecount"].ToString();
                string carddate = dt.Rows[0]["carddate"].ToString();

                //赠送包邮卡
                sql = "INSERT INTO TCS_FreeCard (nick,buynick,cardid,startdate,enddate,carddate,usecountlimit) VALUES ('" + trade.Nick + "', '" + trade.BuyNick + "','" + shop.FreeCardId + "','" + startdate + "','" + enddate + "','" + carddate + "','" + usecountlimit + "')";
                Console.WriteLine(sql);
                utils.ExecuteNonQuery(sql);

                //更新包邮卡赠送数量
                sql = "UPDATE TCS_FreeCardAction SET sendcount = sendcount + 1 WHERE guid = '" + shop.FreeCardId + "'";
                Console.WriteLine(sql);
                utils.ExecuteNonQuery(sql);
            }
        }

        /// <summary>
        /// 获取该包邮卡设定的包邮金额
        /// </summary>
        /// <param name="trade"></param>
        /// <returns></returns>
        public string GetFreeCardPrice(string guid)
        {
            string sql = "SELECT price FROM TCS_FreeCardAction WHERE guid = (SELECT cardid FROM TCS_FreeCard WHERE guid = '" + guid + "')";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return "";
            }
        }

        public bool CheckOrderIsFree(Trade trade)
        {
            string sql = "SELECT COUNT(*) FROM TCS_FreeCardLog WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "' AND orderid = '" + trade.Tid + "'";
            Console.Write(sql + "\r\n");
            string count = utils.ExecuteString(sql);

            if (count == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool CheckFreeCardCanSend(Trade trade, ShopInfo shop)
        {
            string sql = "SELECT COUNT(*) FROM TCS_FreeCard WHERE nick = '" + trade.Nick + "' AND buynick = '" + trade.BuyNick + "' AND cardid = '" + shop.FreeCardId + "'";
            Console.Write(sql + "\r\n");
            string count = utils.ExecuteString(sql);

            if (count == "0")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
