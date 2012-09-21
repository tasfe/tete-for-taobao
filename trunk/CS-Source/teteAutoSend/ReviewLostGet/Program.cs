using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeteTopApi.Logic;

namespace ReviewLostGet
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime startDate = DateTime.Now.AddHours(-1);
            DateTime endDate = DateTime.Now;
            string start = startDate.ToString("yyyy-MM-dd HH:00:00");
            string end = endDate.ToString("yyyy-MM-dd HH:00:00");

            Console.Write(start + "\r\n");
            Console.Write(end + "\r\n");
            //Console.ReadLine();

            LostMessageGet lostGet = new LostMessageGet();
            lostGet.Start(start, end);

            //Console.ReadLine();
        }
    }
}
