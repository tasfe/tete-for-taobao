using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;

namespace ReviewShippingCheckNormal
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //如果是晚上10点到早上9点这个时间端内，不发送物流提醒短信
                if (DateTime.Now.Hour > 8 && DateTime.Now.Hour < 22)
                {
                    ShippingSuccess act = new ShippingSuccess();
                    act.StartNormal();
                }
            }
        }
    }
}
