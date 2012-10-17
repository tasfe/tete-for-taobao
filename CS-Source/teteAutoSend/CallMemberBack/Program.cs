using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;

namespace CallMemberBack
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomerNotice customer = new CustomerNotice();

            try
            {
                Console.WriteLine("call back start!!");
                customer.CallMemberBack();
            }
            catch
            {
                Console.WriteLine("call back err!!!!");
            }

            try
            {
                Console.WriteLine("call birthday start!!");
                customer.BirthDayCall();
            }
            catch
            {
                Console.WriteLine("call birthday err!!!!");
            }
        }
    }
}
