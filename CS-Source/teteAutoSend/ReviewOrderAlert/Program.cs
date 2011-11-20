using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;
using System.Threading;

namespace ReviewOrderAlert
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                WaitSellerConfirm act = new WaitSellerConfirm();
                act.Start();

                //30分钟检查一次
                Thread.Sleep(1800000);
            }
        }
    }
}
