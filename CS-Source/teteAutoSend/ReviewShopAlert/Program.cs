using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;
using TeteTopApi;

namespace ReviewShopAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            //while (true)
            //{
                ShopAlert act = new ShopAlert();

                if (1 == 1)
                {
                    //店铺等级和过期检查
                    act.StartDeleteShop();

                    //更新特殊用户
                    string sql = "UPDATE TCS_ShopSession SET version = 3 WHERE nick = '四川中青旅锦华分社'";
                    utils.ExecuteString(sql);
                }

                if (1 == 1)
                {
                    //检查有很多未审核评价的卖家并给出提示  
                    act.StartUnChecked();
                }

                if (1 == 1)
                {
                    //检查优惠券过期的卖家并给出提示
                    act.StartCouponExpired();
                }

                if (1 == 1)
                {
                    //检查优惠券赠送完毕的卖家并给出提示
                    act.StartCouponExpiredMax();
                }

                if (1 == 1)
                {
                    //充值过短信但是现在短信为0的客户清单并给出提示
                    act.StartMsgZero();
                }
                
                if (1 == 1)
                {
                    //给客户发出消息汇总
                    act.StartShopStatusAlert();
                }

            //    Thread.Sleep(86400000);
            //}
        }
    }
}
