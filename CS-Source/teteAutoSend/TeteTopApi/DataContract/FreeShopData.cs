using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using System.Data;

namespace TeteTopApi.DataContract
{
    public class FreeShopData
    {
        /// <summary>
        /// 根据卖家ID获取免费版的相关设置
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public ShopInfo ShopInfoGetByNick(string nick)
        {
            string sql = "SELECT sessionmarket FROM TopTaobaoShop WHERE nick = '" + nick + "'";
            Console.Write(sql + "\r\n");
            DataTable dt = utils.ExecuteDataTable(sql);
            if (dt.Rows.Count != 0)
            {
                return FormatData(dt);
            }
            else
            {
                return new ShopInfo();
            }
        }

        /// <summary>
        /// 生成新格式的店铺基本设置数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private ShopInfo FormatData(DataTable dt)
        {
            ShopInfo info = new ShopInfo();

            info.Session = dt.Rows[0]["sessionmarket"].ToString();

            return info;
        }
    }
}
