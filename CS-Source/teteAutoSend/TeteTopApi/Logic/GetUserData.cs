using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Entity;
using TeteTopApi.DataContract;
using TeteTopApi.TopApi;

namespace TeteTopApi.Logic
{
    public class GetUserData
    {
        public void Get(Trade trade)
        {
            //获取店铺的基础数据
            ShopData data = new ShopData();
            ShopInfo shop = data.ShopInfoGetByNick(trade.Nick);

            //通过TOP接口查询该会员的详细数据并记录到数据库中
            TopApiHaoping api = new TopApiHaoping(shop.Session);
            Customer customer = api.GetUserInfoByNick(trade);

            //先判断数据库里面是否该会员数据
            CustomerData cdata = new CustomerData();
            //把好评有礼的评价和优惠券数据加入顾客表
            customer = cdata.InitHaopingData(customer);
            if (!cdata.IsHaveThisCustomer(trade))
            {
                //如果有则通过会员接口插入会员基础数据
                cdata.InsertCustomerData(trade, customer);
            }
            else
            {
                //根据订单数据更新会员数据
                cdata.UpdateCustomerData(trade, customer);
            }
        }
    }
}
