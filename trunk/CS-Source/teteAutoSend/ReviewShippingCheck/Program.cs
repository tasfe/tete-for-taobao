using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;

namespace ReviewShippingCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                ShippingSuccess act = new ShippingSuccess();
                act.Start();

                //30分钟检查一次
                Thread.Sleep(3600000);
            }
        }
    }
}
